using System;
using Microsoft.Practices.Unity;
using Trex.Core.Attributes;
using Trex.Core.Implemented;
using Trex.Invoices.Interfaces;
using Trex.Invoices.InvoiceManagementScreen.Interfaces;

namespace Trex.Invoices.InvoiceManagementScreen
{
    [Screen(Name = "InvoiceAdministration", CanBeDeactivated = true)]
    public class InvoiceManagementScreen : ScreenBase
    {
        private readonly IUnityContainer _unityContainer;

        public IInvoiceView InvoiceView { get; private set; }
        public ICustomerListView CustomerListView { get; private set; }
        public IFilterView FilterView { get; private set; }
        public ButtonPanelView.ButtonPanelView ButtonPanelView { get; private set; }


        public InvoiceManagementScreen(IUnityContainer unityContainer, Guid guid)
            : base(guid, unityContainer)
        {
            _unityContainer = unityContainer;
        }


        protected override void Initialize()
        {
            var invoiceAdministrationView = new InvoiceAdministrationMasterView.InvoiceAdministrationMasterView();
            var invoiceAdministrationViewModel =
                new InvoiceAdministrationMasterView.InvoiceAdministrationMasterViewModel();
            invoiceAdministrationView.ViewModel = invoiceAdministrationViewModel;

            MasterView = invoiceAdministrationView;


            var invoiceView = Container.Resolve<IInvoiceView>();
            var invoiceViewModel = Container.Resolve<IInvoiceViewModel>();
            invoiceView.ViewModel =invoiceViewModel;

            InvoiceView = invoiceView;


            var customerListView = Container.Resolve<ICustomerListView>();
            var customerListViewModel = Container.Resolve<ICustomerListViewModel>();
            customerListView.ViewModel = customerListViewModel;

            CustomerListView = customerListView;

            var filterView = Container.Resolve<IFilterView>();

            filterView.ViewModel = customerListViewModel;

            FilterView = filterView;

            var buttonPanelViewModel = Container.Resolve<IButtonPanelViewModel>();
            var buttonPanelView = new ButtonPanelView.ButtonPanelView();
            buttonPanelView.ViewModel = buttonPanelViewModel;
            ButtonPanelView = buttonPanelView;

            Container.Resolve<IDialogService>();

        }


    }
}
