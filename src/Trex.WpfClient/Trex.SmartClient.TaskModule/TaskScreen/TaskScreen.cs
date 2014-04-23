using System;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Attributes;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.TaskModule.Interfaces;

namespace Trex.SmartClient.TaskModule.TaskScreen
{
    [Screen(IsStartScreen = true,CanBeDeactivated = true)]
    public class TaskScreen:ScreenBase
    {
        private readonly IUnityContainer _unityContainer;

        public TaskScreen(IUnityContainer unityContainer,Guid guid) : base(guid)
        {
            _unityContainer = unityContainer;
            Initialize();
        }

        private void Initialize()
        {
            var masterView = new TaskScreenMasterView.TaskScreenMasterView();
            var masterViewModel = _unityContainer.Resolve<ITaskScreenMasterViewModel>();

            masterView.ApplyViewModel(masterViewModel);

            MasterView = masterView;


        }
    }

   
}
