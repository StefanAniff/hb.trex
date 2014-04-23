using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Configuration.DSL;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;
using Trex.Server.Infrastructure.ServiceBehavior;
using Trex.ServiceContracts;

namespace TrexSL.Web.StructureMap
{
    public class RequestRegistry : Registry
    {
        public RequestRegistry()
        {
            For<IInvoiceSender>().Use<InvoiceSender>();
            For<IFileDownloadService>().Use<FileDownloadService>();
            For<IConnectionStringProvider>().Use<RequestConnectionStringProvider>();
            For<ITrexContextProvider>().Use<TrexContextProvider>();
            For<ITrexBaseContextProvider>().Use<TrexBaseContextProvider>();
            For<IDatabaseConnectionStringProvider>().Use<DatabaseConnectionStringProvider>();
            For<IDatabaseCreator>().Use<DatabaseCreator>();
            For<IInvoiceWorker>().Use<InvoiceWorker>();
            For<ITemplateService>().Use<TemplateService>();
            For<ISpecificationComposer>().Use<SpecificationComposer>();
            For<IInvoiceCompose>().Use<InvoiceComposer>();
            For<ICreditNoteCompose>().Use<CreditNoteComposer>();
            For<ISpecificationBuilder>().Use<SpecificationBuilder>();
            For<IInvoiceBuilder>().Use<InvoiceBuilder>();
            For<ISavePDF>().Use<SaveToDB>();
            For<IInvoiceService>().Use<InvoiceService>();
            For<ITemplateService>().Use<TemplateService>();
            For<IGatherData>().Use<GatherData>();
            For<IAppSettings>().Use<ApplicationSettings>();
            For<IEmailService>().Use<EmailService>();
            For<IMailServiceSettings>().Use<ApplicationSettings>();
        }

    }
}