using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.Core.Interfaces;

namespace Trex.Invoices.InvoiceTemplatesScreen
{
    public class InvoiceTemplatesScreenFactory:IScreenFactory
    {
        private readonly IUnityContainer _unityContainer;

        public InvoiceTemplatesScreenFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var screen = new InvoiceTemplatesScreen(guid, _unityContainer);
            region.Add(screen.MasterView,screen.Guid.ToString());
            return screen;
        }
    }
}
