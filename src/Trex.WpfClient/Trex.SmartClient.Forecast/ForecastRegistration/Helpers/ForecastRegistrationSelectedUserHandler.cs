using System;
using System.Collections.ObjectModel;
using System.Linq;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Forecast.ForecastRegistration.Helpers
{
    /// <summary>
    /// Handler for managing selected User which forecast is being edited for
    /// </summary>
    public class ForecastRegistrationSelectedUserHandler : ViewModelBase
    {
        private readonly IUserSession _userSession;
        private readonly IForecastService _forecastService;
        private ForecastUserDto _selectedUser;
        private ObservableCollection<ForecastUserDto> _users;
        private bool _initialized;
        private IForecastRegistrationViewModel _forecastRegistrationViewModel;

        public ForecastRegistrationSelectedUserHandler(IUserSession userSession, IForecastService forecastService)
        {
            _userSession = userSession;
            _forecastService = forecastService;
        }

        public virtual ForecastUserDto SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(() => SelectedUser);
                OnPropertyChanged(() => IsEditingOthersWorkplan);
                UpdateForecastRegistrationViewModel();
            }
        }

        private void UpdateForecastRegistrationViewModel()
        {
            if (_forecastRegistrationViewModel == null)
                return;

            if (_selectedUser != null)
                _forecastRegistrationViewModel.RefreshViewData();

            _forecastRegistrationViewModel.RaiseCanExecuteActions();
        }

        public ObservableCollection<ForecastUserDto> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(() => Users);
            }
        }

        public virtual int UserId
        {
            get { return SelectedUser != null ?  SelectedUser.UserId : 0; }
        }

        public bool UserIsSelected
        {
            get { return SelectedUser != null; }
        }

        public async void Initialize(IForecastRegistrationViewModel forecastRegistrationViewModel)
        {
            if (_initialized)
                throw new Exception("ForecastRegistrationSelectedUserHandler may only be initialized once");

            _forecastRegistrationViewModel = forecastRegistrationViewModel;

            // Only load user-selectables if has permission to edit others workplan
            if (_userSession.MayEditOthersWorksplan)
            {
                var searchOptionsResponse = await _forecastService.GetOverivewSearchOptions();
                if (searchOptionsResponse == null) // Request failed
                {
                    SetUserFromUserSession();
                    return;
                }

                Users = new ObservableCollection<ForecastUserDto>(searchOptionsResponse.Users);
                SelectedUser = Users.Single(x => x.UserId == _userSession.CurrentUser.Id); // Set SelectedUser as UserSession
            }
            else
            {
                SetUserFromUserSession();
            }

            _initialized = true;
        }

        private void SetUserFromUserSession()
        {
            var sessionUser = new ForecastUserDto
                {
                    UserId = _userSession.CurrentUser.Id,
                    Name = _userSession.CurrentUser.Name,
                    UserName = _userSession.CurrentUser.UserName
                };

            Users = new ObservableCollection<ForecastUserDto> { sessionUser };

            SelectedUser = sessionUser;
        }

        public bool MayEditOthersWorkplan
        {
            get { return _userSession.MayEditOthersWorksplan; }
        }

        public bool IsEditingOthersWorkplan
        {
            get { return SelectedUser != null 
                         && SelectedUser.UserId != _userSession.CurrentUser.Id; }
        }
    }
}