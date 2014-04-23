using Microsoft.Practices.Prism.Commands;
using Trex.Core.Services;
using Trex.Roles.Commands;
using Trex.Roles.Dialogs.CreateNewRoleDialog;
using Trex.Roles.Dialogs.DeleteRoleDialog;
using Trex.Roles.Interfaces;
using Trex.ServiceContracts;

namespace Trex.Roles.Implemented
{
    public class DialogService : IDialogService
    {
        
        private readonly IDataService _dataService;
        private readonly IExceptionHandlerService _exceptionHandlerService;


        public DialogService( IDataService dataService,IExceptionHandlerService exceptionHandlerService)
        {
            _dataService = dataService;
            _exceptionHandlerService = exceptionHandlerService;

            InternalCommands.CreateNewRoleStart.RegisterCommand(new DelegateCommand<object>(CreateNewRoleStart));
            InternalCommands.DeleteRoleStart.RegisterCommand(new DelegateCommand<Role>(DeleteRoleStart));
        }

        private void DeleteRoleStart(Role obj)
        {
            var deleteRoleDialog = new DeleteRoleDialog();
            var deleteRoleDialogViewModel = new DeleteRoleDialogViewModel(obj, _dataService, _exceptionHandlerService);
            deleteRoleDialog.ViewModel = deleteRoleDialogViewModel;
            deleteRoleDialog.Show();
        }

        private void CreateNewRoleStart(object obj)
        {
            var createNewRoleDialog = new CreateNewRoleDialog();
            var createNewRoleDialogViewModel = new CreateNewRoleViewModel(_dataService);
            createNewRoleDialog.ViewModel = createNewRoleDialogViewModel;
            createNewRoleDialog.Show();
        }
    }
}