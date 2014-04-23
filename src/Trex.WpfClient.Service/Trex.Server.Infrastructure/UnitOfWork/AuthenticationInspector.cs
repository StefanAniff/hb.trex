using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web.Security;
using Microsoft.Practices.Unity;
using Trex.Server.Core;
using Trex.Server.Core.Services;
using Trex.Server.Core.Unity;
using VMDe.Core.Unity;
using log4net;

namespace Trex.Server.Infrastructure.UnitOfWork
{
    public class AuthenticationInspector : IDispatchMessageInspector
    {
        private readonly bool _validateUserLogin;
        private static readonly ILog Log = LogManager.GetLogger("TRex." + typeof(AuthenticationInspector).Name);
        private UnityInstanceProvider InstanceProvider;

        public AuthenticationInspector(bool validateUserLogin)
        {
            _validateUserLogin = validateUserLogin;
            InstanceProvider = new UnityInstanceProvider { Container = UnityContainerManager.GetInstance };
        }

        public object AfterReceiveRequest(ref Message request,
            System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            try
            {
                var membershipProvider = InstanceProvider.Container.Resolve<IMembershipProvider>();

                int userNameHeaderIndex = request.Headers.FindHeader("UserName", "Authentication");
                var passwordHeaderIndex = request.Headers.FindHeader("UserPassword", "Authentication");
                var customerIdHeader = request.Headers.FindHeader("CustomerId", "Authentication");
                var clientApplicationTypeHeader = request.Headers.FindHeader("ClientApplicationType", "Authentication");
                var hasLoginInfo = userNameHeaderIndex >= 1 && passwordHeaderIndex >= 1;
                var customerId = request.Headers.GetHeader<string>(customerIdHeader);


                if ((!hasLoginInfo && _validateUserLogin) || !(customerIdHeader >= 1) || !(clientApplicationTypeHeader >= 1))
                {
                    throw new ArgumentException("Invalid headers");
                }

#if !DEBUG
                Membership.ApplicationName = customerId;
                Roles.ApplicationName = customerId;
#endif

                if (_validateUserLogin)
                {
                    var userName = request.Headers.GetHeader<string>(userNameHeaderIndex);
                    var userPassword = request.Headers.GetHeader<string>(passwordHeaderIndex);
                    if (!membershipProvider.ValidateUser(userName, userPassword))
                    {
                        throw new Exception("Invalid user: " + userName);
                    }
                }
                var clientApplicationType = request.Headers.GetHeader<string>(clientApplicationTypeHeader);

                string conn;
                if ((conn = GetCustomerConnectionString(customerId)) == null)
                {
                    throw new Exception("Invalid customer: " + customerId);
                }

                TenantConnectionProvider.DynamicString = conn;
                TenantConnectionProvider.ApplicationType = Int32.Parse(clientApplicationType);
            }
            catch (Exception ex)
            {
                Log.Info(ex);
                request = null;
                throw;
            }

            return null;
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


        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (reply.IsFault)
            {
                var property = new HttpResponseMessageProperty { StatusCode = System.Net.HttpStatusCode.OK };
                reply.Properties[HttpResponseMessageProperty.Name] = property;
            }
        }
    }
}