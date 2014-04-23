using System;
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;
using NHibernate;
using Trex.Server.Core;
using Trex.Server.Core.Unity;
using Trex.Server.Infrastructure.UnitOfWork;

namespace TrexSL.Web.Intercepts
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class UnitOfWorkAttribute : Attribute, IOperationBehavior, IParameterInspector, IErrorHandler
    {
        private readonly bool _isReadonly;
        private readonly bool _usingBase;

        public void Validate(OperationDescription operationDescription)
        {
        }

        public UnitOfWorkAttribute()
        {

        }

        public UnitOfWorkAttribute(bool isReadonly = false, bool usingBase = false)
        {
            _isReadonly = isReadonly;
            _usingBase = usingBase;
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(this);
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            if (_usingBase)
            {
                TenantConnectionProvider.DynamicString = ConfigurationManager.ConnectionStrings["TrexBase"].ConnectionString;                
            }
            var resolve = UnityContainerManager.GetInstance.Resolve<IUnitOfWork>();
            if (_isReadonly)
            {
                return resolve.CreatReadonlyTransaction();
            }
            return resolve.CreateTransaction();
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            var unitOfWork = UnityContainerManager.GetInstance.Resolve<IUnitOfWork>();
            try
            {
                using (var transaction = ((ITransaction)correlationState))
                {
                    if (!transaction.WasRolledBack)
                    {
                        transaction.Commit();
                    }
                }
            }
            catch (Exception)
            {
                unitOfWork.RollBack();
                throw;
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {

        }

        public bool HandleError(Exception error)
        {
            UnityContainerManager.GetInstance.Resolve<IUnitOfWork>().RollBack();
            return false;
        }
    }
}