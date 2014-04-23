using StructureMap;
using Trex.Server.Core.Services;

namespace TrexSL.Web.Helpers
{
    public static class FactoryHelper
    {
        public static ICustomerFactory CustomerFactory
        {
            get { return ObjectFactory.GetInstance<ICustomerFactory>(); }
        }

        public static ITaskFactory TaskFactory
        {
            get { return ObjectFactory.GetInstance<ITaskFactory>(); }
        }

        public static ITimeEntryFactory TimeEntryFactory
        {
            get { return ObjectFactory.GetInstance<ITimeEntryFactory>(); }
        }

        public static IProjectFactory ProjectFactory
        {
            get { return ObjectFactory.GetInstance<IProjectFactory>(); }
        }

        public static IUserCustomerInfoFactory UserCustomerInfoFactory
        {
            get { return ObjectFactory.GetInstance<IUserCustomerInfoFactory>(); }
        }

        public static IInvoiceFactory InvoiceFactory
        {
            get { return ObjectFactory.GetInstance<IInvoiceFactory>(); }
        }

        public static IInvoiceLineFactory InvoiceLineFactory
        {
            get { return ObjectFactory.GetInstance<IInvoiceLineFactory>(); }
        }
    }
}