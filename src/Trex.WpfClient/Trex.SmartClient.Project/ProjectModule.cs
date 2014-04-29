using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Project.ProjectAdministration;
using Trex.SmartClient.Project.ProjectMasterScreen;
using Trex.SmartClient.Project.TaskDisposition;

namespace Trex.SmartClient.Project
{
    [Module(ModuleName = "ProjectModule")]
    public class ProjectModule : IModule
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IMenuRegistry _menuRegistry;
        private readonly IScreenFactoryRegistry _screenFactoryRegistry;
        private readonly ProjectScreenFactory _projectScreenFactory;
        private MenuInfo _adminScreenInfo;

        public ProjectModule(IUnityContainer unityContainer
            , IMenuRegistry menuRegistry
            , IScreenFactoryRegistry screenFactoryRegistry
            , ProjectScreenFactory projectScreenFactory)
        {
            _unityContainer = unityContainer;
            _menuRegistry = menuRegistry;
            _screenFactoryRegistry = screenFactoryRegistry;
            _projectScreenFactory = projectScreenFactory;

            RegisterServices();
            RegisterViews();
            RegisterViewModels();
        }

        public void Initialize()
        {
            // Main module
            _adminScreenInfo = MenuInfo.Create(1, "Project", true, false, false, false);
            _screenFactoryRegistry.RegisterFactory(_adminScreenInfo.ScreenGuid, _projectScreenFactory);
            _menuRegistry.RegisterMenuInfo(_adminScreenInfo);

            // Sub module Administration
            var subProjectAdministration = SubMenuInfo.Create("Administration", typeof(ProjectAdministrationView), _adminScreenInfo);
            subProjectAdministration.IsActive = true;
            _adminScreenInfo.AddSubMenu(subProjectAdministration);

            // Sub module Disposition
            var subTaskDisposition = SubMenuInfo.Create("Disposition", typeof(TaskDispositionView), _adminScreenInfo);
            _adminScreenInfo.AddSubMenu(subTaskDisposition);
        }

        private void RegisterViewModels()
        {
            _unityContainer.RegisterType<IProjectMasterViewModel, ProjectMasterViewModel>();
            _unityContainer.RegisterType<IProjectAdministrationViewModel, ProjectAdministrationViewModel>();
            _unityContainer.RegisterType<ITaskDispositionViewModel, TaskDispositionViewModel>();
        }

        private void RegisterViews()
        {
            _unityContainer.RegisterType<IProjectMasterView, ProjectMasterView>();
            _unityContainer.RegisterType<IProjectAdministrationView, ProjectAdministrationView>();
            _unityContainer.RegisterType<ITaskDispositionView, TaskDispositionView>();
        }

        private void RegisterServices()
        {
            
        }
    }
}