using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Implemented;
using Trex.SmartClient.TaskModule.Dialogs;
using Trex.SmartClient.TaskModule.Dialogs.Viewmodels;
using IDialogService = Trex.SmartClient.TaskModule.Interfaces.IDialogService;

namespace Trex.SmartClient.TaskModule.Implemented
{
    public class DialogService : DialogManagerBase, IDialogService
    {
        private readonly IUnityContainer _unityContainer;
        private const string SaveTaskRegionName = "SaveTask";

        HashSet<string> _activeDialogs = new HashSet<string>();
        private bool _screenIsOpen;


        public DialogService(IRegionManager regionManager, IAppSettings appSettings, IUnityContainer unityContainer)
            : base(regionManager, appSettings)
        {
            _unityContainer = unityContainer;

            TaskCommands.SaveTaskStart.RegisterCommand(new DelegateCommand<TimeEntry>(o => SaveTaskStart(o, false)));
            TaskCommands.EditTaskStart.RegisterCommand(new DelegateCommand<TimeEntry>(o => SaveTaskStart(o, true)));
            TaskCommands.SaveTaskCompleted.RegisterCommand(new DelegateCommand<TimeEntry>(SaveTaskCompleted));
            TaskCommands.SaveTaskCancelled.RegisterCommand(new DelegateCommand<TimeEntry>(SaveTaskCancelled));
        }

        private void SaveTaskCancelled(TimeEntry timeEntry)
        {
            if (_screenIsOpen)
            {
                CloseDialog(SaveTaskRegionName);
            }
        }


        private void SaveTaskCompleted(TimeEntry timeEntry)
        {
            var viewName = CreateViewName(timeEntry);
            CloseDialog(SaveTaskRegionName);
        }

        private void SaveTaskStart(TimeEntry timeEntry, bool editing)
        {
            var timeEntryTypeRepository = _unityContainer.Resolve<ITimeEntryTypeRepository>();
            var timeEntryRepository = _unityContainer.Resolve<ITimeEntryRepository>();
            var taskSearchService = _unityContainer.Resolve<ITaskSearchService>();
            var userSession = _unityContainer.Resolve<IUserSession>();
            var appsettings = _unityContainer.Resolve<IAppSettings>();
            var userWlanSettingsService = _unityContainer.Resolve<IUserWlanSettingsService>();

            var dialog = new SaveTaskDialog1();
            ITaskRepository taskRepository = _unityContainer.Resolve<ITaskRepository>();
            var viewModel = new SaveTaskDialogViewModel(timeEntry, timeEntryTypeRepository, timeEntryRepository,
                                                        taskSearchService, userSession, appsettings,
                                                        userWlanSettingsService, editing, taskRepository);

            dialog.ApplyViewModel(viewModel);

            var viewName = CreateViewName(timeEntry);
            if (!_activeDialogs.Contains(viewName))
            {
                _activeDialogs.Add(viewName);
            }
            OpenDialog(dialog, SaveTaskRegionName);
            _screenIsOpen = true;
        }

        private static string CreateViewName(TimeEntry timeEntry)
        {
            var viewName = timeEntry == null ? SaveTaskRegionName : SaveTaskRegionName + timeEntry.Guid;
            return viewName;
        }
    }
}