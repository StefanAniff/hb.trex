using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.Infrastructure.Implemented;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Implemented;
using Trex.TaskAdministration.Interfaces;
using Trex.TaskAdministration.TaskManagementScreen;
using Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView;
using Trex.TaskAdministration.TaskManagementScreen.FilterView;
using Trex.TaskAdministration.TaskManagementScreen.RightPanelView;
using Trex.TaskAdministration.TaskManagementScreen.TaskAdministrationMasterView;
using Trex.TaskAdministration.TaskManagementScreen.TaskTreeView;
using Trex.TaskAdministration.TimeEntryTypeScreen;

namespace Trex.TaskAdministration
{
    public class TaskAdministrationModule : IModule
    {
        private readonly IMenuRegistry _menuRegistry;
        private readonly IScreenFactoryRegistry _screenFactoryRegistry;
        private readonly IUnityContainer _unityContainer;

        public TaskAdministrationModule(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IMenuRegistry menuRegistry)
        {
            _unityContainer = container;
            _screenFactoryRegistry = screenFactoryRegistry;
            _menuRegistry = menuRegistry;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewModels();
            var taskAdminScreenInfo = MenuInfo.CreateMenuItem("Task Management", true, true, Permissions.TaskManagementPermission);

            _screenFactoryRegistry.RegisterFactory(taskAdminScreenInfo.ScreenGuid, new TaskManagementScreenFactory(_unityContainer));
            _menuRegistry.RegisterMenuInfo(taskAdminScreenInfo, null, 0);

            var timeEntryTypeScreenInfo = MenuInfo.CreateMenuItem("TimeEntry types", true, false, Permissions.TimeEntryTypesPermission);
            _screenFactoryRegistry.RegisterFactory(timeEntryTypeScreenInfo.ScreenGuid, new TimeEntryTypeScreenFactory(_unityContainer));
            _menuRegistry.RegisterMenuInfo(timeEntryTypeScreenInfo, "Administration", 0);
        }

        #endregion

        private void RegisterViewModels()
        {
            _unityContainer.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<IEstimateService, EstimateService>();
            _unityContainer.RegisterType<ITaskAdministrationMasterViewModel, TaskAdministrationMasterViewModel>();
            _unityContainer.RegisterType<ITaskTreeViewModel, TaskTreeViewModel>();
            _unityContainer.RegisterType<ITaskTreeView, TaskTreeView>();
            _unityContainer.RegisterType<ICustomerViewModel, TreeCustomerViewModel>();
            _unityContainer.RegisterType<IProjectViewModel, TreeProjectViewModel>();
            _unityContainer.RegisterType<ITaskGridViewModel, MainListViewModel>();
            _unityContainer.RegisterType<IFilterViewModel, FilterViewModel>();
            _unityContainer.RegisterType<IButtonPanelViewModel, ButtonPanelViewModel>();
        }
    }
}