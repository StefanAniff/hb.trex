using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web.Security;
using Trex.Server.Core.Exceptions;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.Implemented;

namespace Trex.Server.Infrastructure.ServiceBehavior
{
    public class CustomInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            try
            {

                var userNameIndex = request.Headers.FindHeader("UserName", "Authentication");
                if (!(userNameIndex >= 1))
                    throw new MissingHeaderException("Username is missing");

                var passwordIndex = request.Headers.FindHeader("UserPassword", "Authentication");
                if (!(passwordIndex >= 1))
                    throw new MissingHeaderException("Password is missing");

                var customerIdIndex = request.Headers.FindHeader("CustomerId", "Authentication");
                if (!(customerIdIndex >= 1))
                    throw new MissingHeaderException("Customerid is missing");

                var clientAppTypeIndex = request.Headers.FindHeader("ClientApplicationType", "Authentication");
                if (!(clientAppTypeIndex >= 1))
                    throw new MissingHeaderException("Clientapplicationtype is missing");



                var userName = request.Headers.GetHeader<string>(userNameIndex);
                var userPassword = request.Headers.GetHeader<string>(passwordIndex);
                var customerId = request.Headers.GetHeader<string>(customerIdIndex);
                var clientApplicationType = request.Headers.GetHeader<string>(clientAppTypeIndex);

                string conn;



                Membership.ApplicationName = customerId;
                Roles.ApplicationName = customerId;

                if (!Membership.ValidateUser(userName, userPassword))
                    throw new FaultException<ExceptionDetail>(new ExceptionDetail(new UserValidationException("Credentials were incorrect.")));


                if ((conn = GetConnectionString(customerId)) == null)
                    throw new ConnectionStringException("Could not obtain a connectionstring for the given customer id: " + customerId);

                var connectionContext = new ConnectionContext()
                {
                    ConnectionString = conn,
                    CustomerId = customerId,
                    UserName = userName,
                    ClientApplicationType = Int32.Parse(clientApplicationType)
                };
                OperationContext.Current.Extensions.Add(connectionContext);


            }
            catch (Exception e)
            {
                Logger.LogError("Authentication error", e);
                throw;
            }

            return null;
        }


        public static string GetConnectionString(string customerID)
        {
            var trexBaseEntities = new TrexBaseEntities();
            var customer = trexBaseEntities.TrexCustomers.SingleOrDefault(c => c.CustomerId == customerID);
            return customer.ConnectionString;
        }


        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var context = OperationContext.Current.Extensions.Find<ConnectionContext>();
            OperationContext.Current.Extensions.Remove(context);

            if (reply.IsFault)
            {
                var property = new HttpResponseMessageProperty();
                property.StatusCode = System.Net.HttpStatusCode.OK;
                reply.Properties[HttpResponseMessageProperty.Name] = property;
            }
        }
    }
}