using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Implemented;

namespace Trex.SmartClient.TaskModule.TaskScreen.HistoryFeedView
{
    public class HistoryFeedViewModel : ViewModelBase, IHistoryFeedViewModel
    {
        private readonly IUserSession _userSession;
        private readonly ISyncService _syncService;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ITimeEntryTypeRepository _timeEntryTypeRepository;
        private readonly IUserWlanSettingsService _userWlanSettingsService;
        private readonly IAppSettings _appSettings;

        public DelegateCommand<object> CloseErrorMessageCommand { get; set; }
        public DelegateCommand<object> CancelChanges { get; set; }

        public HistoryFeedViewModel(IUserSession userSession, ISyncService syncService, ITimeEntryRepository timeEntryRepository,
            ITimeEntryTypeRepository timeEntryTypeRepository,
             IUserWlanSettingsService userWlanSettingsService,
            IAppSettings appSettings)
        {
            _userSession = userSession;
            _syncService = syncService;
            _timeEntryRepository = timeEntryRepository;
            _timeEntryTypeRepository = timeEntryTypeRepository;
            _userWlanSettingsService = userWlanSettingsService;
            _appSettings = appSettings;


            TimeEntries = new ObservableCollection<HistoryFeedRowViewModel>();
            ApplicationCommands.SyncCompleted.RegisterCommand(new DelegateCommand<object>(SyncComplete));
            ApplicationCommands.SyncProgressChanged.RegisterCommand(new DelegateCommand<Tuple<int, string>>(SyncProgressChanged));
            ApplicationCommands.SyncStarted.RegisterCommand(new DelegateCommand<object>(SyncStarted));
            TaskCommands.SaveTaskCompleted.RegisterCommand(new DelegateCommand<object>(TaskSaved));
            SyncInProgress = _syncService.SyncInProgress;
            ApplicationCommands.SyncFailed.RegisterCommand(new DelegateCommand<string>(SyncFailedCommand));
            CloseErrorMessageCommand = new DelegateCommand<object>(ExecuteCloseErrorMessage);
            CancelChanges = new DelegateCommand<object>(CancelChangesExecute);

            TimeEntryTypes = _timeEntryTypeRepository.GetGlobal();

            ApplicationCommands.PremiseSettingChanged.RegisterCommand(new DelegateCommand<object>(SetDefaultType));
        }

        private void CancelChangesExecute(object obj)
        {
           // TODO: Verify
            _timeEntryRepository.RemoveTimeEntiresWithErrors();
            ApplicationCommands.Resync.Execute(null);
            PlaceHolderActivated = false;
            //TaskSaved(null);
        }


        private void SyncFailedCommand(string message)
        {
            ErrorMessage = message;
            HasErrors = true;
        }

        private void TaskSaved(object obj)
        {
            var unsyncedTimeEntries = _timeEntryRepository.GetUnsyncedTimeEntries();
            if (unsyncedTimeEntries.Any(x => x.HasSyncError))
            {
                ErrorMessage = "There were timeentries that could not be saved!";
                HasErrors = true;
                PlaceHolderActivated = true;

                var historyFeedRowViewModels = unsyncedTimeEntries.Where(x => x.HasSyncError).Select(te => new HistoryFeedRowViewModel(te)).ToList();

                var notificationViewModelItem = new HistoryFeedRowViewModel(
                    DateTime.MaxValue,
                    "[PlaceHolder]");

                historyFeedRowViewModels.Add(notificationViewModelItem);

                TimeEntries = new ObservableCollection<HistoryFeedRowViewModel>(historyFeedRowViewModels.OrderByDescending(te => te.Date));
            }
            else
            {
                var feedRows = _userSession.UserStatistics.LastXDaysTimeEntries.Select(te => new HistoryFeedRowViewModel(te)).ToList();
                feedRows.AddRange(unsyncedTimeEntries.Where(x => !x.HasSyncError).Select(te => new HistoryFeedRowViewModel(te)));

                TimeEntries = new ObservableCollection<HistoryFeedRowViewModel>(feedRows.OrderByDescending(te => te.Date));
            }
        }

        private void ExecuteCloseErrorMessage(object obj)
        {
            HasErrors = false;
            ErrorMessage = string.Empty;
            SyncInProgress = _syncService.SyncInProgress;
        }

        private void SyncStarted(object displayForUser)
        {
            SyncProgress = 0;
            SyncInProgress = (bool)displayForUser;
        }

        private void SyncProgressChanged(Tuple<int, string> tuple)
        {
            SyncProgress += tuple.Item1;
            SyncMessage = tuple.Item2;
        }

        private void SyncComplete(object obj)
        {
            SyncInProgress = false;

            if (_userSession.UserStatistics != null && _userSession.UserStatistics.LastXDaysTimeEntries != null)
            {
                TaskSaved(obj);
                Update();
            }
            TimeEntryTypes = _timeEntryTypeRepository.GetGlobal();

        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(() => ErrorMessage);
            }
        }

        private bool _hasErrors;

        public bool HasErrors
        {
            get { return _hasErrors; }
            set
            {
                _hasErrors = value;
                OnPropertyChanged(() => HasErrors);
            }
        }

        private bool _placeHolderActivated;

        public bool PlaceHolderActivated
        {
            get { return _placeHolderActivated; }
            set
            {
                _placeHolderActivated = value;
                OnPropertyChanged(() => PlaceHolderActivated);
            }
        }

        private ObservableCollection<HistoryFeedRowViewModel> _timeEntries;

        public ObservableCollection<HistoryFeedRowViewModel> TimeEntries
        {
            get { return _timeEntries; }
            set
            {
                _timeEntries = value;
                OnPropertyChanged(() => TimeEntries);
            }
        }

        public double RegisteredHoursToday
        {
            get { return _userSession.UserStatistics.RegisteredHoursToday; }
        }

        public double BillableHoursToday
        {
            get { return _userSession.UserStatistics.BillableHoursToday; }
        }

        public double RegisteredHoursThisWeek
        {
            get { return _userSession.UserStatistics.RegisteredHoursThisWeek; }
        }


        public double BillableHoursThisWeek
        {
            get { return _userSession.UserStatistics.BillableHoursThisWeek; }
        }

        public double RegisteredHoursThisMonth
        {
            get { return _userSession.UserStatistics.RegisteredHoursThisMonth; }
        }

        public double BillableHoursThisMonth
        {
            get { return _userSession.UserStatistics.BillableHoursThisMonth; }
        }

        private bool _syncInProgress;

        public bool SyncInProgress
        {
            get { return _syncInProgress; }
            set
            {
                _syncInProgress = value;
                OnPropertyChanged(() => SyncInProgress);
            }
        }

        private int _syncProgress;

        public int SyncProgress
        {
            get { return _syncProgress; }
            set
            {
                _syncProgress = value;
                OnPropertyChanged(() => SyncProgress);
            }
        }

        private string _syncMessage;

        public string SyncMessage
        {
            get { return _syncMessage; }
            set
            {
                _syncMessage = value;
                OnPropertyChanged(() => SyncMessage);
            }
        }


        private List<TimeEntryType> _timeEntryTypes;

        public List<TimeEntryType> TimeEntryTypes
        {
            get { return _timeEntryTypes; }
            set
            {
                _timeEntryTypes = value;

                if (_timeEntryTypes == null)
                {
                    return;
                }
                SetDefaultType(null);
            }
        }

        private async void SetDefaultType(object o)
        {
            var userWLanMatches = await _userWlanSettingsService.FindAnyUserWLanMatches();
            if (userWLanMatches.Any())
            {
                if (_userWlanSettingsService.BoundToWLan)
                {
                    UserDefaultTimeEntryType = _timeEntryTypes.FirstOrDefault(tt => tt.Id == _userWlanSettingsService.UserWLanTimeEntryTypeId);
                }
                else
                {
                    var randomUserWlanMatch = userWLanMatches.FirstOrDefault();
                    UserDefaultTimeEntryType = _timeEntryTypes.FirstOrDefault(tt => tt.Id == randomUserWlanMatch.DefaultTimeEntryTypeId);
                }
            }
            else if (UserDefaultTimeEntryType == null) //should only overwrite if the user has not made an choice
            {
                UserDefaultTimeEntryType = _timeEntryTypes.FirstOrDefault(tt => tt.IsDefault);
            }
            else
            {
                UserDefaultTimeEntryType = _timeEntryTypes.FirstOrDefault(tt => tt.Id == UserDefaultTimeEntryType.Id);
            }
            OnPropertyChanged(() => UserDefaultTimeEntryType);
            OnPropertyChanged(() => UserDefaultTimeEntryTypeSelectionIsEnabled);
            OnPropertyChanged(() => TimeEntryTypes);
        }


        private TimeEntryType _userDefaultTimeEntryType;

        public TimeEntryType UserDefaultTimeEntryType
        {
            get { return _userDefaultTimeEntryType; }
            set
            {
                _userDefaultTimeEntryType = value;
                var isNotSystemDefault = _userDefaultTimeEntryType != null && !_userDefaultTimeEntryType.IsDefault;
                _appSettings.UserDefaultTimeEntryTypeId = isNotSystemDefault ? (int?)_userDefaultTimeEntryType.Id : null;
                OnPropertyChanged(() => UserDefaultTimeEntryType);
            }
        }

        public bool UserDefaultTimeEntryTypeSelectionIsEnabled
        {
            get { return !_userWlanSettingsService.BoundToWLan; }
        }

        private void Update()
        {
            OnPropertyChanged(() => RegisteredHoursToday);
            OnPropertyChanged(() => BillableHoursToday);
            OnPropertyChanged(() => RegisteredHoursThisWeek);
            OnPropertyChanged(() => BillableHoursThisWeek);
            OnPropertyChanged(() => RegisteredHoursThisMonth);
            OnPropertyChanged(() => BillableHoursThisMonth);
        }
    }
}