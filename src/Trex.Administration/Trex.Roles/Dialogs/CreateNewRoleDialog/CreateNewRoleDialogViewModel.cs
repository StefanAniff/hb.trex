using System;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Roles.Commands;

namespace Trex.Roles.Dialogs.CreateNewRoleDialog
{
    public class CreateNewRoleViewModel : ViewModelBase
    {
        
        private readonly IDataService _dataService;
        private string _errorMessage;
        private string _roleName;

        public CreateNewRoleViewModel( IDataService dataService)
        {
 
            _dataService = dataService;

            CreateCommand = new DelegateCommand<object>(ExecuteCreate);
            CancelCommand = new DelegateCommand<object>(ExecuteCancel);
        }

        public DelegateCommand<object> CreateCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }

        public string RoleName
        {
            get { return _roleName; }
            set
            {
                _roleName = value;
                OnPropertyChanged("RoleName");
            }
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
            InternalCommands.CreateNewRoleCompleted.Execute(null);
        }

        private void ExecuteCreate(object obj)
        {
            if (_roleName != null)
            {
                _dataService.CreateRole(_roleName).Subscribe(
                    result => InternalCommands.CreateNewRoleCompleted.Execute(null),
                    exception =>
                    ErrorMessage = exception.Message
                    );
            }
        }

     
    }
}