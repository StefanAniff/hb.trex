using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Invoices.Implemented;
using Trex.Invoices.Interfaces;
using Trex.Invoices.InvoiceManagementScreen;
using Trex.Invoices.InvoiceManagementScreen.ButtonPanelView;
using Trex.Invoices.InvoiceManagementScreen.CustomerTreeView;
using Trex.Invoices.InvoiceManagementScreen.FilterView;
using Trex.Invoices.InvoiceManagementScreen.Interfaces;
using Trex.Invoices.InvoiceManagementScreen.InvoiceView;
using Trex.Invoices.InvoiceTemplatesScreen;
using Trex.ServiceContracts;

namespace Trex.Invoices
{
    public class InvoiceModule : IModule
    {

        private readonly IUnityContainer _container;
        private readonly IScreenFactoryRegistry _screenFactoryRegistry;
        private readonly IMenuRegistry _menuRegistry;


        public InvoiceModule(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IMenuRegistry menuRegistry)
        {
            _container = container;
            _screenFactoryRegistry = screenFactoryRegistry;
            _menuRegistry = menuRegistry;
        }


        public void Initialize()
        {
            RegisterViewModels();

            var invoiceScreenInfo = MenuInfo.CreateMenuItem("Manage invoices", true, false, Permissions.InvoicePermission);
            _menuRegistry.RegisterMenuInfo(invoiceScreenInfo, "Invoices", 0);
            _screenFactoryRegistry.RegisterFactory(invoiceScreenInfo.ScreenGuid, new InvoiceScreenFactory(_container));

            var invoiceTemplateInfo = MenuInfo.CreateMenuItem("Manage templates", true, false, Permissions.TemplateManagementPermission);
            _menuRegistry.RegisterMenuInfo(invoiceTemplateInfo, "Invoices", 1);
            _screenFactoryRegistry.RegisterFactory(invoiceTemplateInfo.ScreenGuid, new InvoiceTemplatesScreenFactory(_container));
        }


        private void RegisterViewModels()
        {
            _container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IInvoiceService, InvoiceService>();
            _container.RegisterType<IFilterView, FilterView>();
            _container.RegisterType<IInvoiceView, InvoiceView>();
            _container.RegisterType<IInvoiceViewModel, InvoiceViewModel>();
            _container.RegisterType<ICustomerListView, CustomerListView>();
            _container.RegisterType<ICustomerListViewModel, CustomerListViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IButtonPanelViewModel, ButtonPanelViewModel>();
           
        }

    }
}
