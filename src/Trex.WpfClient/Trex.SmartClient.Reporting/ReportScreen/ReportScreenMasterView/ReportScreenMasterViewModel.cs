using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Extensions;
using log4net;
using Task = System.Threading.Tasks.Task;

namespace Trex.SmartClient.Reporting.ReportScreen.ReportScreenMasterView
{
    public class ReportScreenMasterViewModel : ViewModelBase, IReportScreenMasterViewModel
    {
        private readonly ITimeEntryService _timeEntryService;
        private readonly IBusyService _busyService;
        private readonly IConnectivityService _connectivityService;
        private readonly ITimeEntryTypeRepository _timeEntryTypeRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IAppSettings _appSettings;
        private bool _isShowingTextbox;

        public DelegateCommand<object> Search { get; set; }
        public DelegateCommand<object> EditTimeEntryCommand { get; set; }
        public DelegateCommand<object> SaveCommand { get; set; }

        private bool _isEditing;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public ReportScreenMasterViewModel(ITimeEntryService timeEntryService, IBusyService busyService, IConnectivityService connectivityService,
            ITimeEntryTypeRepository timeEntryTypeRepository, ITimeEntryRepository timeEntryRepository, IAppSettings appSettings)
        {
            _timeEntryService = timeEntryService;
            _busyService = busyService;
            _connectivityService = connectivityService;
            _timeEntryTypeRepository = timeEntryTypeRepository;
            _timeEntryRepository = timeEntryRepository;
            _appSettings = appSettings;
            Search = new DelegateCommand<object>(ExecuteSearch);
            _busyService.ShowBusy(_reportscreenKey);
            LoadPredefinedSearchItems();
            SelectedPredefinedSearch = PredefinedSearchItems[1];
            EditTimeEntryCommand = new DelegateCommand<object>(EditTimeEntry);
            SaveCommand = new DelegateCommand<object>(SaveCommandExecute);

            try
            {
                TimeEntryTypes = _timeEntryTypeRepository.GetGlobal();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            TaskCommands.SaveTaskCompleted.RegisterCommand(new DelegateCommand(Reload));
            TaskCommands.SaveTaskCancelled.RegisterCommand(new DelegateCommand(Reload));

            ApplicationCommands.ConnectivityChanged.RegisterCommand(new DelegateCommand<object>(ConnectivityChangedExecute));
        }

        private void SaveCommandExecute(object item)
        {
            _busyService.ShowBusy(_reportscreenKey);

            var list = new List<TimeEntry>();
            foreach (var gridRowItemViewModel in GridRows.Where(x => x.HasChanges))
            {
                var timeEntry = gridRowItemViewModel.TimeEntry;

                timeEntry.IsSynced = false;

                timeEntry.Billable = gridRowItemViewModel.EditableBillable;
                timeEntry.TimeEntryType = gridRowItemViewModel.EditableType;

                list.Add(timeEntry);
                TaskCommands.SaveTaskCompleted.Execute(timeEntry);
            }

            _timeEntryRepository.AddOrUpdateRange(list);
            ApplicationCommands.StartSync.Execute(null);

            _busyService.HideBusy(_reportscreenKey);
        }

        private void ConnectivityChangedExecute(object obj)
        {
            OnPropertyChanged(() => IsOnline);
        }

        public bool AdvancedSettingsEnabled
        {
            get { return _appSettings.AdvancedSettingsEnabled; }
        }

        public bool IsOnline
        {
            get { return _connectivityService.IsOnline; }
        }


        public bool IsReadOnly
        {
            get { return !IsInEditMode; }
        }

        private bool _isInEditMode;

        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set
            {
                _isInEditMode = value;
                OnPropertyChanged(() => IsInEditMode);
                OnPropertyChanged(() => IsReadOnly);
            }
        }

        private List<TimeEntryType> _timeEntryTypes;

        public List<TimeEntryType> TimeEntryTypes
        {
            get { return _timeEntryTypes; }
            set
            {
                _timeEntryTypes = value;
                OnPropertyChanged(() => TimeEntryTypes);
            }
        }

        private void Reload()
        {
            if (_isEditing)
            {
                LoadTimeEntries();
            }
            _isEditing = false;
        }

        private void EditTimeEntry(object item)
        {
            _isEditing = true;
            var gridRowItemViewModel = item is GridRowItemViewModel ? item as GridRowItemViewModel : SelectedItem;
            TaskCommands.EditTaskStart.Execute(gridRowItemViewModel.TimeEntry);
        }

        private void ExecuteSearch(object obj)
        {
            var selectedIndex = PredefinedSearchItems.IndexOf(SelectedPredefinedSearch);
            LoadPredefinedSearchItems();
            if (selectedIndex != -1)
            {
                _selectedPredefinedSearch = PredefinedSearchItems[selectedIndex];
                OnPropertyChanged(() => SelectedPredefinedSearch);
            }
            LoadTimeEntries();
        }

        private DateTime _fromDate;

        public DateTime FromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
                OnPropertyChanged(() => FromDate);

            }
        }

        private DateTime _toDate;

        public DateTime ToDate
        {
            get { return _toDate; }
            set
            {
                _toDate = value;
                OnPropertyChanged(() => ToDate);
            }
        }

        private ObservableCollection<GridRowItemViewModel> _gridRows;

        public ObservableCollection<GridRowItemViewModel> GridRows
        {
            get { return _gridRows; }
            set
            {
                _gridRows = value;
                OnPropertyChanged(() => GridRows);
            }
        }

        public GridRowItemViewModel SelectedItem { get; set; }

        private async void LoadTimeEntries()
        {
            _busyService.ShowBusy(_reportscreenKey);
            List<TimeEntry> timeEntries = null;

            try
            {
                timeEntries = await _timeEntryService.GetTimeEntriesByDate(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                if (!_isShowingTextbox)
                {
                    _isShowingTextbox = true;
                    var result = MessageBox.Show("Error loading timeentries", "Error", MessageBoxButton.OK);
                    _isShowingTextbox = result == MessageBoxResult.None;
                }
                Logger.Error(ex);
            }
            try
            {
                ReportCommands.DataLoaded.Execute(null);
                if (timeEntries != null)
                {
                    var gridRowViewModels = timeEntries.Select(timeEntry =>
                    {
                        TimeEntryType timeEntryType = null;
                        if (TimeEntryTypes != null)
                        {
                            var @default = TimeEntryTypes.FirstOrDefault(x => x.IsDefault);
                            timeEntryType = timeEntry.TimeEntryType == null
                                                   ? @default
                                                   : TimeEntryTypes.FirstOrDefault(x => x.Id == timeEntry.TimeEntryType.Id);
                        }
                        return new GridRowItemViewModel(timeEntry, timeEntryType);
                    });
                    GridRows = new ObservableCollection<GridRowItemViewModel>(gridRowViewModels);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            _busyService.HideBusy(_reportscreenKey);
        }

        private ObservableCollection<PredefinedSearchViewModel> _predefinedSearchItems;

        public ObservableCollection<PredefinedSearchViewModel> PredefinedSearchItems
        {
            get { return _predefinedSearchItems; }
            set
            {
                _predefinedSearchItems = value;
                OnPropertyChanged(() => PredefinedSearchItems);
            }
        }


        private PredefinedSearchViewModel _selectedPredefinedSearch;
        private string _reportscreenKey = "ReportScreen";

        public PredefinedSearchViewModel SelectedPredefinedSearch
        {
            get { return _selectedPredefinedSearch; }
            set
            {
                _selectedPredefinedSearch = value;
                OnPropertyChanged(() => SelectedPredefinedSearch);
                if (_selectedPredefinedSearch != null)
                {
                    FromDate = _selectedPredefinedSearch.FromDate;
                    ToDate = _selectedPredefinedSearch.ToDate;
                    LoadTimeEntries();
                }
            }
        }

        private void LoadPredefinedSearchItems()
        {
            PredefinedSearchItems = new ObservableCollection<PredefinedSearchViewModel>
                {
                    new PredefinedSearchViewModel("This week", DateTime.Today.FirstDayOfWeek(),
                                                  DateTime.Today),
                    new PredefinedSearchViewModel("This month", DateTime.Now.FirstDayOfMonth(),
                                                  DateTime.Today),
                    new PredefinedSearchViewModel("Last month",
                                                  DateTime.Today.AddMonths(-1).FirstDayOfMonth(),
                                                  DateTime.Today.FirstDayOfMonth().AddDays(-1)),
                    new PredefinedSearchViewModel("This year", new DateTime(DateTime.Now.Year, 1, 1),
                                                  DateTime.Today)
                };
        }
    }
}