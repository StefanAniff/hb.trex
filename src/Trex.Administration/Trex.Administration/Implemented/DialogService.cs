using Microsoft.Practices.Prism.Commands;
using Trex.Administration.Commands;
using Trex.Administration.Dialogs.EditUserDialog;
using Trex.Administration.Dialogs.EditUserPricesDialog;
using Trex.Administration.Dialogs.InviteUsersDialog;
using Trex.Administration.Interfaces;
using Trex.Core.Services;
using Trex.ServiceContracts;

namespace Trex.Administration.Implemented
{
    public class DialogService : IDialogService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDataService _dataService;
        private readonly IUserSession _userSession;
        private readonly IExceptionHandlerService _exceptionHandlerService;

        public DialogService(IDataService dataService, ICustomerRepository customerRepository, IUserRepository userRepository, IUserSession userSession,IExceptionHandlerService exceptionHandlerService)
        {
            _dataService = dataService;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _userSession = userSession;
            _exceptionHandlerService = exceptionHandlerService;
            InternalCommands.CreateNewUserStart.RegisterCommand(new DelegateCommand<object>(CreateNewUserStart));
            InternalCommands.InviteUsersStart.RegisterCommand(new DelegateCommand<object>(InviteUsersStart));
            InternalCommands.EditUserPricesStart.RegisterCommand(new DelegateCommand<User>(EditUserPricesStart));
            InternalCommands.EditUserStart.RegisterCommand(new DelegateCommand<User>(EditUserStart));
        }

        private void EditUserStart(User obj)
        {
            var editUserDialog = new EditUserDialog();
            var editUserViewModel = new EditUserDialogViewModel(obj, _dataService,_userRepository);

            editUserDialog.ViewModel = editUserViewModel;

            editUserDialog.Show();
        }

        private void EditUserPricesStart(User obj)
        {
            var editUserPricesDialog = new EditUserPricesDialog();
            var editUserPricesDialogViewModel = new EditUserPricesDialogViewModel(obj, _customerRepository, _dataService, _exceptionHandlerService);
            editUserPricesDialog.ViewModel = editUserPricesDialogViewModel;
            editUserPricesDialog.Show();
        }

        private void CreateNewUserStart(object obj)
        {
            var editUserDialog = new EditUserDialog();
            var editUserViewModel = new EditUserDialogViewModel(new User(), _dataService,_userRepository);

            editUserDialog.ViewModel = editUserViewModel;

            editUserDialog.Show();
        }

        private void InviteUsersStart(object o)
        {
            var inviteUsersDialog = new InviteUsersDialog();
            var inviteUsersDialogViewModel = new InviteUsersDialogViewModel(_dataService, _userSession);
            inviteUsersDialog.ApplyViewModel(inviteUsersDialogViewModel);
            inviteUsersDialog.Show();
        }
    }
}