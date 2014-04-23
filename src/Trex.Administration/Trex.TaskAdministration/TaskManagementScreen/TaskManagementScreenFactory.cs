using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.Core.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen
{
    public class TaskManagementScreenFactory : IScreenFactory
    {
        private readonly IUnityContainer _unityContainer;

        public TaskManagementScreenFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        #region IScreenFactory Members

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var screen = new TaskManagementScreen(_unityContainer, guid);

            var screenAttribute = screen.GetScreenAttribute();
            if (screenAttribute != null)
            {
                var localRegionManager = region.Add(screen.MasterView, screen.Guid.ToString(), true);

                var treeViewRegion = localRegionManager.Regions["TreeViewRegion"];
                treeViewRegion.Add(screen.TaskTreeView);

                var taskListRegion = localRegionManager.Regions["TaskListRegion"];
                taskListRegion.Add(screen.TaskGridView);

                var filterRegion = localRegionManager.Regions["FilterRegion"];
                filterRegion.Add(screen.FilterView);

                var panelButtonView = localRegionManager.Regions["ActionPanelRegion"];
                panelButtonView.Add(screen.ButtonPanelView);

                //var searchViewRegion = localRegionManager.Regions["SearchRegion"];
                //searchViewRegion.Add(screen.SearchView);

                return screen;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}