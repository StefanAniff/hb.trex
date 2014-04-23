using System;
using Microsoft.Practices.Unity;
using Trex.Core.Attributes;
using Trex.Core.Implemented;
using Trex.Core.Services;

namespace Trex.Invoices.InvoiceTemplatesScreen
{
    [Screen(Name = "InvoiceTemplates", CanBeDeactivated = true)]
    public class InvoiceTemplatesScreen : ScreenBase
    {
        private IUnityContainer _unityContainer;
        public InvoiceTemplatesScreen(Guid guid, IUnityContainer unityContainer) : base(guid, unityContainer)
        {
            _unityContainer = unityContainer;
        }

        protected override void Initialize()
        {

            var dataService = Container.Resolve<IDataService>();
            var userSession = Container.Resolve<IUserSession>();
            var invoiceTemplatesListView = new InvoiceTemplatesListView.InvoiceTemplatesListView();
            var viewModel = new InvoiceTemplatesListView.InvoiceTemplatesListViewModel(dataService,userSession);
            invoiceTemplatesListView.ViewModel = viewModel;
            MasterView = invoiceTemplatesListView;
        }
    }
}
