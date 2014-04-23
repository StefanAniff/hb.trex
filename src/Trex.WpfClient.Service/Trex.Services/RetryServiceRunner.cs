using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Security;
using NHibernate;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using Trex.Common.ServiceStack;
using Trex.Server.Core;
using Trex.Server.Core.Model;
using Trex.Server.Infrastructure.UnitOfWork;
using TrexSL.Web;
using TrexSL.Web.DataContracts;
using VMDe.Core;

namespace VMDe
{
    public class RetryServiceRunner<T> : ServiceRunner<T>
    {
        private const int MaxAttempts = 10;
        private Exception _lastException;
        private readonly IActiveSessionManager _sessionManager;
        public RetryServiceRunner(Global.TrexAppHost appHost, ActionContext actionContext, IActiveSessionManager sessionManager)
            : base(appHost, actionContext)
        {
            _sessionManager = sessionManager;
        }

        private static string GetCustomerConnectionString(string customerID)
        {
            string customerConnectionString = null;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TrexBase"].ConnectionString))
            {
                using (var cmd = new SqlCommand("spGetConnection", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CustomerID", SqlDbType.VarChar, 100).Value = customerID;

                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customerConnectionString = reader.GetString(0);
                        }
                    }
                }
            }
            return customerConnectionString;
        }


        public override object Execute(IRequestContext requestContext, object instance, T request)
        {
            for (var attempt = 1; attempt <= MaxAttempts; attempt++)
            {
                try
                {
                    try
                    {
                        var customerId = requestContext.GetHeader("CustomerId");
                        var environment = ConfigurationManager.AppSettings["environment"];
                        if (environment != "Debug")
                        {
                            Membership.ApplicationName = customerId;
                            Roles.ApplicationName = customerId;
                        }
                        if (instance.GetType() != typeof(AuthService))
                        {
                            var clientApplicationType = requestContext.GetHeader("ClientApplicationType");

                            string conn;
                            if ((conn = GetCustomerConnectionString(customerId)) == null)
                            {
                                throw new Exception("Invalid customer: " + customerId);
                            }

                            TenantConnectionProvider.DynamicString = conn;
                            TenantConnectionProvider.ApplicationType = Int32.Parse(clientApplicationType);
                        }
                        else
                        {
                            TenantConnectionProvider.DynamicString = ConfigurationManager.ConnectionStrings["TrexBase"].ConnectionString;
                        }

                        var watch = Stopwatch.StartNew();
                        var response = base.Execute(requestContext, instance, request);
                        if (attempt > 1)
                        {
                            Log.InfoFormat("Succeded after at least one retry exception. Attempts: {0}. Last exception: {1}",
                                           attempt, GetExceptionInfo(_lastException));
                        }
                        watch.Stop();
                        Log.InfoFormat("Request={1};ElapsedMs={0}", watch.ElapsedMilliseconds, request.GetType());
                        return response;
                    }
                    catch (InvalidStateException ex)
                    {
                        var invalidValues = ex.GetInvalidValues().GroupBy(i => i.Entity);
                        throw new DomainException(@"Could not save an entity because of the following invalid values: {0}", string.Join(Environment.NewLine, invalidValues.Select(GenerateEntityErrorDescriptions)));
                    }
                }
                catch (Exception ex)
                {
                    _sessionManager.ErrorHappened();
                    Log.DebugFormat("Got exception when in Handle() (exception was handled): {0}", GetExceptionInfo(ex));

                    if (attempt == MaxAttempts)
                    {
                        Log.ErrorFormat("Giving up after {0} attempts. ExceptionInfo: {1}", attempt, GetExceptionInfo(ex));
                        return FinalExceptionHandling(requestContext, request, ex);
                    }

                    if (!IsRetryException(ex))
                    {
                        if (IsAuthenticationError(ex))
                        {
                            Log.Info("Got authentification denail", ex);   
                        }                        
                        else if (!IsDomainException(ex)) // only log non-domain exceptions as errors
                        {
                            Log.ErrorFormat("Got exception that should not be retried and is rethrown: {0}", GetExceptionInfo(ex));
                        }
                        else
                        {
                            Log.WarnFormat("Got domain exception: {0}", GetExceptionInfo(ex));
                        }   
                                                
                        return FinalExceptionHandling(requestContext, request, ex);
                    }
                    _lastException = ex;
                    var sleeptime = SleeptimeMs();
                    Log.DebugFormat("Retrying in {0} ms", sleeptime);
                    Thread.Sleep(sleeptime); // Sleep a bit before trying again. Raises the chances of success next time.
                }
                finally
                {
                    _sessionManager.Close();
                }
            }
            throw new Exception("Reached point that should be unreachable!!");
        }

        private bool IsAuthenticationError(Exception exception)
        {
            var httpError = exception as HttpError;
            if (httpError == null)
                return false;

            return httpError.StatusCode == HttpStatusCode.Unauthorized;
        }

        static object GenerateEntityDescription(object entity)
        {
            var entityBase = entity as EntityBase;

            if (entityBase != null)
            {
                return string.Format("{0} ({1})", entityBase.GetType().Name, entityBase.EntityId);
            }

            return string.Format("{0} (?)", entity.GetType().Name);
        }

        static string FormatSingleInvalidValue(InvalidValue invalidValue)
        {
            return "        " + (string.IsNullOrEmpty(invalidValue.PropertyName)
                                     ? string.Format("{0}: {1}", invalidValue.Message, invalidValue.Value)
                                     : string.Format("{0}: {1} = {2}", invalidValue.Message,
                                                     invalidValue.PropertyName,
                                                     invalidValue.Value));
        }

        static string GenerateEntityErrorDescriptions(IGrouping<object, InvalidValue> entityWithInvalidValues)
        {
            var entity = entityWithInvalidValues.Key;

            return string.Format(@"    {0}:{1}", GenerateEntityDescription(entity), string.Join(Environment.NewLine, entityWithInvalidValues.Select(FormatSingleInvalidValue)));
        }

        private bool IsDomainException(Exception exception)
        {
            return exception is DomainException;
        }

        /// <summary>
        /// For now only returns true when a Deadlock SQL exception occurs.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool IsRetryException(Exception ex)
        {
            string message = null;
            try
            {
                var adoException = ex as ADOException;
                if (adoException == null) return false;

                var sqlException = adoException.InnerException as SqlException;
                if (sqlException == null) return false;

                if (sqlException.Number == 1205)
                {
                    message = "Deadlock exception";
                    return true;
                }
            }
            finally
            {
                if (message != null)
                    Log.DebugFormat("Exception is retry-exception. Reason: {0}. Exception details: {1}", message, GetExceptionInfo(ex));
            }
            return false;
        }

        private string GetExceptionInfo(Exception ex)
        {
            IList<string> info = new List<string>();
            info.Add("Outer type: " + ex.GetType().Name);
            info.Add("Outer msg: " + ex.Message);

            var innerEx = ex.InnerException;
            if (innerEx != null)
            {
                info.Add("Inner type: " + innerEx.GetType().Name);
                info.Add("Inner msg: " + innerEx.Message);

                var sqlEx = innerEx as SqlException;
                if (sqlEx != null)
                    info.Add("SqlException errorNo:" + sqlEx.Number);
            }
            info.Add("Stacktrace: " + ex.StackTrace);
            return string.Join(", ", info);
        }

        private long GetElapsedTimeMs(long startTick)
        {
            return (DateTime.UtcNow.Ticks - startTick) / 10000;
        }

        private int SleeptimeMs()
        {
            return 300;
        }

        private object FinalExceptionHandling(IRequestContext requestContext, T request, Exception ex)
        {
            return base.HandleException(requestContext, request, ex);
        }

        public override void OnBeforeExecute(IRequestContext requestContext, T request)
        {
            // Called just before any Action is executed
            var session = _sessionManager.OpenSession();
            var isReadonlyRequest = typeof (T).BaseType == typeof (ReadonlyRequest);
            if (!requestContext.EndpointAttributes.HasFlag(EndpointAttributes.HttpGet) &&
                 !isReadonlyRequest)
            {
                session.FlushMode = FlushMode.Auto;
            }
            _sessionManager.BeginTransaction();
        }

        public override object OnAfterExecute(IRequestContext requestContext, object response)
        {
            _sessionManager.Commit();
            return response;
            // Called just after any Action is executed, you can modify the response returned here as well
        }

        public override object HandleException(IRequestContext requestContext, T request, Exception ex)
        {
            return null; //to allow retrys, dont handle!
            //return base.HandleException(requestContext, request, ex);
            // Called whenever an exception is thrown in your Services Action
        }
    }
}