#region

using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using StructureMap;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;

#endregion

namespace TrexSL.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CustomerService
    {
        [OperationContract]
        public bool ExistsApplicationName(string customerId)
        {
            try
            {
                var context = ObjectFactory.GetInstance<ITrexBaseContextProvider>();
                var entityContext = context.TrexBaseEntityContext;

                var customer =
                    entityContext.TrexCustomers.SingleOrDefault(c => c.CustomerId.ToLower() == customerId.ToLower());

                return customer != null;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public TrexCustomer GetCustomerByCustomerId(string customerId)
        {
            var context = ObjectFactory.GetInstance<ITrexBaseContextProvider>();
            var entityContext = context.TrexBaseEntityContext;

            var customer =
                entityContext.TrexCustomers.SingleOrDefault(c => c.CustomerId.ToLower() == customerId.ToLower());

            return customer;
        }

        [OperationContract]
        public TrexCustomer SaveCustomer(TrexCustomer customer)
        {
            try
            {
                var context = ObjectFactory.GetInstance<ITrexBaseContextProvider>();
                var entityContext = context.TrexBaseEntityContext;
                if (customer.Id != 0)
                {
                    entityContext.Attach(customer);
                    entityContext.DetectChanges();
                    entityContext.SaveChanges();
                }
                else
                {
                    entityContext.TrexCustomers.AddObject(customer);
                    entityContext.SaveChanges();
                }

                return customer;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public UserCreationResponse RegisterNewUser(string applicationName, string userName, string password,
                                                    string creatorContactName, string email, string language)
        {
            throw new Exception("Online activation is no longer supported");

            // No longer possible since scripts are no longer updated.
            //try
            //{
            //    var trexRegistrator = ObjectFactory.GetInstance<ITrexRegistrator>();
            //    return trexRegistrator.RegisterNewUser(applicationName, userName, password, creatorContactName, email,
            //                                           language);
            //}
            //catch (Exception ex)
            //{
            //    OnError(ex);
            //    throw;
            //}
        }

        [OperationContract]
        public void ActivateCustomer(string activationId, string language)
        {
            try
            {
                var trexActivator = ObjectFactory.GetInstance<ITrexActivator>();
                trexActivator.Activate(activationId, language);
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        private void OnError(Exception ex)
        {
            Logger.LogError("An error occured",ex);
        }
    }
}