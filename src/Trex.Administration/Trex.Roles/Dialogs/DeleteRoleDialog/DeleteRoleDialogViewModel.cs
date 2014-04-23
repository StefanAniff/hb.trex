using System;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Roles.Commands;
using Trex.ServiceContracts;


namespace Trex.Roles.Dialogs.DeleteRoleDialog
{
    public class DeleteRoleDialogViewModel : ViewModelBase
    {
        
        private readonly Role _role;
        private readonly IDataService _dataService;
        private readonly IExceptionHandlerService _exceptionHandlerService;
        private string _errorMessage;

        public DeleteRoleDialogViewModel( Role role,IDataService dataService,IExceptionHandlerService exceptionHandlerService)
        {
            _role = role;
            _dataService = dataService;
            _exceptionHandlerService = exceptionHandlerService;
            DeleteCommand = new DelegateCommand<object>(ExecuteDelete);
            CancelCommand = new DelegateCommand<object>(ExecuteCancel);
        }

        public DelegateCommand<object> DeleteCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }

        public string SelectedRoleName
        {
            get { return _role.Title; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private void ExecuteCancel(object obj)
        {
            InternalCommands.DeleteRoleCompleted.Execute(null);
        }

        private void ExecuteDelete(object obj)
        {
            _dataService.DeleteRole(_role.Title).Subscribe(
               r => InternalCommands.DeleteRoleCompleted.Execute(null),
                   exception => ErrorMessage = exception.Message);
                
        }

    }
}