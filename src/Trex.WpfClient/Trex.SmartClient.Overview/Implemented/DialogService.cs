using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.Dialog.SelectTask.Viewmodels;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Implemented;
using Trex.SmartClient.Overview.Interfaces;

namespace Trex.SmartClient.Overview.Implemented
{
    public class DialogService : DialogManagerBase, IDialogService
    {
        private readonly IAppSettings _appSettings;
        private readonly IUnityContainer _unityContainer;
        private const string SaveTaskRegionName = "SelectTask";
        private bool _screenIsOpen;

        public DialogService(IRegionManager regionManager, IAppSettings appSettings, IUnityContainer unityContainer)
            : base(regionManager, appSettings)
        {
            _appSettings = appSettings;
            _unityContainer = unityContainer;

            OverviewCommands.AddNewTask.RegisterCommand(new DelegateCommand<object>(SelectTaskStart));
            TaskCommands.TaskSelectCompleted.RegisterCommand(new DelegateCommand<object>(TaskSelectCompleted));
            TaskCommands.SaveTaskCancelled.RegisterCommand(new DelegateCommand<object>(TaskSelectCompleted));
        }

        private void TaskSelectCompleted(object obj)
        {
            if (_screenIsOpen)
            {
                CloseDialog(SaveTaskRegionName);
            }
        }

        private void SelectTaskStart(object obj)
        {
            var taskSearchService = _unityContainer.Resolve<ITaskSearchService>();
            var userSession = _unityContainer.Resolve<IUserSession>();

            var dialog = new SelectTaskBox.SelectTaskBox();
            ITaskRepository taskRepository = _unityContainer.Resolve<ITaskRepository>();
            var viewModel = new SelectTaskViewModel(taskSearchService, userSession, _appSettings, taskRepository);

            dialog.ApplyViewModel(viewModel);

            OpenDialog(dialog, SaveTaskRegionName);
            _screenIsOpen = true;
        }

        // IVA: Placer popups her!
    }
}