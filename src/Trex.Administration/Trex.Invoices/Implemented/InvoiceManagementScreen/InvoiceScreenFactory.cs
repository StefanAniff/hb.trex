using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.Core.Interfaces;

namespace Trex.Invoices.InvoiceManagementScreen
{
    public class InvoiceScreenFactory : IScreenFactory
    {

        private readonly IUnityContainer _unityContainer;


        public InvoiceScreenFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }


        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var screen = new InvoiceManagementScreen(_unityContainer, guid);

            var screenAttribute = screen.GetScreenAttribute();
            if (screenAttribute != null)
            {
                var localRegionManager = region.Add(screen.MasterView, screen.Guid.ToString(), true);

                var invoiceViewRegion = localRegionManager.Regions["InvoiceRegion"];
                invoiceViewRegion.Add(screen.InvoiceView);

                var treeViewRegion = localRegionManager.Regions["TreeViewRegion"];
                treeViewRegion.Add(screen.CustomerListView);

                var filterViewRegion = localRegionManager.Regions["FilterRegion"];
                filterViewRegion.Add(screen.FilterView);

                var panelButtonView = localRegionManager.Regions["ActionPanelRegion"];
                panelButtonView.Add(screen.ButtonPanelView);
            }
            return screen;
        }


    }
}
