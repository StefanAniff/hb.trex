#region

using System.Web;
using StructureMap.Configuration.DSL;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;
using Trex.Server.Infrastructure.ServiceBehavior;
using Trex.ServiceContracts;

#endregion

namespace TrexSL.Web.StructureMap
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            For<IRoleManagementService>().Use<RoleManagementService>();
            For<IMembershipService>().Use<MembershipService>();
            For<IAppSettings>().Use<ApplicationSettings>();
            For<IEmailService>().Use<EmailService>();
            For<IMailServiceSettings>().Use<ApplicationSettings>();
            For<IPermissionService>().Use<PermissionService>();
            For<ITrexContextProvider>().Use<TrexContextProvider>();
            For<ITrexBaseContextProvider>().Use<TrexBaseContextProvider>();

            For<IConnectionStringProvider>().Use<EFConnectionStringProvider>();


            For<ITrexRegistrator>().Use<TrexRegistrator>();
            For<ITrexActivator>().Use<TrexActivator>();
            For<IDatabaseCreator>().Use<DatabaseCreator>();
            For<IUserManagementService>().Use<UserManagementService>();
            For<IDatabaseConnectionStringProvider>().Use<DatabaseConnectionStringProvider>();
            For<ITaskService>().Use<TaskService>();
            For<IInvoiceWorker>().Use<InvoiceWorker>();
            For<IInvoiceService>().Use<InvoiceService>();
            For<ITemplateService>().Use<TemplateService>();
            For<IGatherData>().Use<GatherData>();
            For<IClock>().Use<Clock>();
            For<IGenerateInvoices>().Use<GenerateInvoices>();
            For<IRepository>().Use<Repository>();
            For<ICustomerService>().Use<CustomerServices>();
            //Pdf generators
            For<ISavePDF>().Use<SaveToDB>();
            For<IProjectService>().Use<ProjectService>();
            For<ISpecificationBuilder>().Use<SpecificationBuilder>();
            For<IInvoiceBuilder>().Use<InvoiceBuilder>();
            For<ITimeEntryService>().Use<TimeEntryService>();
            For<ISpecificationComposer>().Use<SpecificationComposer>();
            For<IInvoiceCompose>().Use<InvoiceComposer>();
            For<ICreditNoteCompose>().Use<CreditNoteComposer>();

            For<IInvoiceSender>().Use<InvoiceSender>();

            For<ICustomerInvoiceGroupService>().Use<CustomerInvoiceGroupService>();
            For<ICustomerInvoiceGroupRepository>().Use<CustomerInvoiceGroupRepository>();
            For<IFileDownloadService>().Use<FileDownloadService>();
        }
    }
}
