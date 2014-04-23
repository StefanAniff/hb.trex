using System.Linq;
using Microsoft.Practices.Prism.Regions;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public abstract class DialogManagerBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IRegionNames _regionNames;

        protected DialogManagerBase(IRegionManager regionManager, IAppSettings appSettings)
        {
            _regionManager = regionManager;
            _regionNames = appSettings.RegionNames;
        }

        protected void OpenDialog(IView dialog, string viewName)
        {
            var region = _regionManager.Regions[_regionNames.DialogRegion];
            if (region.GetView(viewName) != null)
            {
                return;
            }

            if (region.ActiveViews.Any())
            {
                var view = region.ActiveViews.FirstOrDefault();
                region.Remove(view);
                DisposeView(view);
            }

            region.Add(dialog, viewName);


        }

        protected void CloseDialog(string viewName)
        {
            var region = _regionManager.Regions.ToList().SingleOrDefault(r => r.Name == _regionNames.DialogRegion);
            if (region == null)
            {
                return;
            }

            var view = region.GetView(viewName);
            if (view != null)
            {
                region.Remove(view);

                DisposeView(view);
            }
        }

        private static void DisposeView(object view)
        {
            var viewInterface = view as IView;
            if (viewInterface != null)
            {
                var viewModel = viewInterface.DataContext as ViewModelBase;
                if (viewModel != null) viewModel.Dispose();
            }
        }
    }
}