using StructureMap;
using Trex.Server.Core.Services;

namespace TrexSL.Web.Helpers
{
    public static class RepositoryHelper
    {
        public static ICustomerRepository CustomerRepository
        {
            get { return ObjectFactory.GetInstance<ICustomerRepository>(); }
        }

        public static IProjectRepository ProjectRepository
        {
            get { return ObjectFactory.GetInstance<IProjectRepository>(); }
        }

        public static ITaskRepository TaskRepository
        {
            get { return ObjectFactory.GetInstance<ITaskRepository>(); }
        }

        public static ITimeEntryRepository TimeEntryRepository
        {
            get { return ObjectFactory.GetInstance<ITimeEntryRepository>(); }
        }

        public static IUserRepository UserRepository
        {
            get { return ObjectFactory.GetInstance<IUserRepository>(); }
        }

        public static IInvoiceRepository InvoiceRepository
        {
            get { return ObjectFactory.GetInstance<IInvoiceRepository>(); }
        }
    }
}