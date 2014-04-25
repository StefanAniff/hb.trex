using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Forecast.ForecastRegistration.Helpers;
using System.Linq;
using Trex.SmartClient.Forecast.Shared;
using Trex.SmartClient.Infrastructure.Commands;
using Task = System.Threading.Tasks.Task;
using TimeRegistrationTypeEnum = Trex.SmartClient.Core.Model.TimeRegistrationTypeEnum;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    public class ForecastRegistrationViewModel : ViewModelDirtyHandlingBase, IForecastRegistrationViewModel
    {
        private readonly ForecastRegistrationDataGenerator _forecastRegistrationDataGenerator;
        private readonly ProjectSearchViewModel _projectSearchViewModel;
        private readonly IForecastService _forecastService;
        private readonly ICommonDialogs _commonDialogs;
        private readonly SaveForecastCommandHandler _saveForecastCommandHandler;
        private readonly ResetForecastCommandHandler _resetForecastCommandHandler;
        private readonly CopyPreviousMonthCommandHandler _copyPreviousMonthCommandHandler;
        private readonly ForecastTypeProvider _forecastTypeProvider;
        private readonly ITimeEntryService _timeEntryService;
        private readonly IAppSettings _appSettings;
        private readonly ForecastRegistrationSelectedUserHandler _selectedUserHandler;
        private ObservableCollection<ForecastType> _forecastTypes;
        private DateTime _selectedDate;
        private bool _forecastMonthIsLocked;
        private int _projectForecastTypeId;

        private ProjectRegistrations _projectRegistrations = new ProjectRegistrations();
        private ForecastDateColumns _dateColumns;
        private bool _isBusy;

        public DelegateCommand<object> NextMonthCommand { get; private set; }
        public DelegateCommand<object> PreviousMonthCommand { get; private set; }
        public DelegateCommand<object> CurrentMonthCommand { get; private set; }
        public DelegateCommand<object> DeleteProjectCommand { get; private set; }
        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> ResetCommand { get; private set; }
        public DelegateCommand<object> CopyPreviousMonth { get; private set; }

        public int ForecastMonthId { get; set; }

        public ForecastRegistrationViewModel(ForecastRegistrationDataGenerator forecastRegistrationDataGenerator
            , ProjectSearchViewModel projectSearchViewModel
            , IForecastService forecastService
            , ICommonDialogs commonDialogs
            , SaveForecastCommandHandler saveForecastCommandHandler
            , ResetForecastCommandHandler resetForecastCommandHandler
            , CopyPreviousMonthCommandHandler copyPreviousMonthCommandHandler
            , ForecastTypeProvider forecastTypeProvider
            , ITimeEntryService timeEntryService
            , IAppSettings appSettings
            , ForecastRegistrationSelectedUserHandler selectedUserHandler)
        {
            _selectedDate = DateTime.Now.FirstDayOfMonth();
            ForecastMonthId = 0;
            _forecastRegistrationDataGenerator = forecastRegistrationDataGenerator;
            _projectSearchViewModel = projectSearchViewModel;
            _forecastService = forecastService;
            _commonDialogs = commonDialogs;
            _saveForecastCommandHandler = saveForecastCommandHandler;
            _resetForecastCommandHandler = resetForecastCommandHandler;
            _copyPreviousMonthCommandHandler = copyPreviousMonthCommandHandler;
            _forecastTypeProvider = forecastTypeProvider;
            _timeEntryService = timeEntryService;
            _appSettings = appSettings;
            _selectedUserHandler = selectedUserHandler;

            _projectRegistrations.InitializeDirtyCheck();
        }

        private void OnNewProjectAdded(object objProject)
        {
            var project = objProject as Project;
            if (project == null)
                return;

            // Check if item already exists in list
            if (_projectRegistrations.Any(x => x.ProjectId == project.Id))
            {
                var msg = new StringBuilder();
                msg.AppendLine(string.Format("Project already {0} exists in your current list.", project.Name));
                msg.AppendLine();
                msg.AppendLine("Action aborted.");

                _commonDialogs.Ok(msg.ToString(), "Project", MessageBoxImage.Stop);
                return;
            }

            AddNewProjectRegistration(project.Id, project.Name, project.Company.Name);
            RaiseCanExecuteActions();
        }

        public void RaiseCanExecuteActions()
        {
            SaveCommand.RaiseCanExecuteChanged();
            ResetCommand.RaiseCanExecuteChanged();
            CopyPreviousMonth.RaiseCanExecuteChanged();
            CurrentMonthCommand.RaiseCanExecuteChanged();
        }

        public ProjectRegistration AddNewProjectRegistration(int projectId, string projectName, string companyName)
        {
            var project = new ProjectRegistration
            {
                ProjectId = projectId,
                ProjectName = projectName,
                ComapyName = companyName
            };

            project.Registrations = _forecastRegistrationDataGenerator.CreateProjectHoursFromHeaders(DateColumns, this, project);
            ProjectRegistrations.Add(project);
            CalculateTotals();

            return project;
        }

        private void InitializeDefaults()
        {
            SelectedDate = DateTime.Now.FirstDayOfMonth();
        }

        private void InitializeCommands()
        {
            
            PreviousMonthCommand = new DelegateCommand<object>(_ =>
            {
                if (CancelDataIsDirty())
                    return;

                SelectedDate = SelectedDate.FirstDayOfPreviousMonth();
            }, x => !IsBusy);

            NextMonthCommand = new DelegateCommand<object>(_ =>
            {
                if (CancelDataIsDirty())
                    return;

                SelectedDate = SelectedDate.FirstDayOfNextMonth();
            }, x => !IsBusy);

            CurrentMonthCommand = new DelegateCommand<object>(_ =>
                {
                    if (CancelDataIsDirty())
                        return;
                    
                    SelectedDate = DateTime.Now.FirstDayOfMonth();
                }, x => !IsBusy);

            DeleteProjectCommand = new DelegateCommand<object>(DeleteProjectExecute);

            SaveCommand = new DelegateCommand<object>(_ => _saveForecastCommandHandler.Execute(this), _ => _saveForecastCommandHandler.CanExecute(this));
            ResetCommand = new DelegateCommand<object>(_ => _resetForecastCommandHandler.Execute(this), _ => _resetForecastCommandHandler.CanExecute(this));
            CopyPreviousMonth = new DelegateCommand<object>(_ =>
            {
                if (!_copyPreviousMonthCommandHandler.ShouldContinue(this))
                    return;

                IsBusy = true;
                _copyPreviousMonthCommandHandler.Execute(this);
                IsBusy = false;
                RaiseCanExecuteActions();
            }, _ => _copyPreviousMonthCommandHandler.CanExecute(this));
        }



        private bool CancelDataIsDirty()
        {
            if (!IsDirty || ForecastMonthIsLocked)
                return false;

            var msg = new StringBuilder()
                .AppendLine("If you continue, unsaved data will be lost!")
                .AppendLine()
                .AppendLine("Continue?");

            return !_commonDialogs.ContinueWarning(msg.ToString(), "Unsaved changes");
        }

        private void DeleteProjectExecute(object obj)
        {
            var toDelete = obj as ProjectRegistration;
            if (toDelete == null)
                return;

            if (!_commonDialogs.ContinueWarning("Delete project row?", "Confirm"))
                return;

            ProjectRegistrations.Remove(toDelete);

            // Cleanup subscriptions
            toDelete.ResetHoursUpdatedSubscriptions();

            RaiseCanExecuteActions();
            CalculateTotals();
        }

        [NoDirtyCheck]
        public ObservableCollection<ForecastType> ForecastTypes
        {
            get { return _forecastTypes; }
            set
            {
                _forecastTypes = value;
                OnPropertyChanged(() => ForecastTypes);
            }
        }

        public void Initialize()
        {
            DoInitialize();
        }

        private async void DoInitialize()
        {
            InitializeCommands();
            InitializeCompositeCommands();
            ForecastTypes = new ObservableCollection<ForecastType>(await _forecastTypeProvider.Initialize()); // Forecasttypes are needed
            SelectedUserHandler.Initialize(this);
            InitializeDefaults();
        }

        private void InitializeCompositeCommands()
        {
            ForecastLocalCompositeCommands.ForecastRegistrationProjectSelected.RegisterCommand(new DelegateCommand<object>(OnNewProjectAdded));
        }

        public ProjectRegistrations ProjectRegistrations
        {
            get { return _projectRegistrations; }
            set
            {
                _projectRegistrations = value;
                OnPropertyChanged(() => ProjectRegistrations);
            }
        }

        [NoDirtyCheck]
        public ForecastDateColumns DateColumns
        {
            get { return _dateColumns; }
            set
            {
                _dateColumns = value;
                OnPropertyChanged(() => DateColumns);
            }
        }

        [NoDirtyCheck]
        public IEnumerable<HourRegistration> DateTotals
        {
            get { return DateColumns != null ? DateColumns.Select(x => x.DateTotal) : null; }
        }

        private IEnumerable<decimal?> _dateRealizedTotals;
        private string _saveDisabledText;

        [NoDirtyCheck]
        public IEnumerable<decimal?> DateRealizedTotals
        {
            get { return _dateRealizedTotals; }
            set
            {
                _dateRealizedTotals = value;
                OnPropertyChanged(() => DateRealizedTotals);
                OnPropertyChanged(() => DateRealizedTotalsSum);
            }
        }

        [NoDirtyCheck]
        public decimal DateRealizedTotalsSum
        {
            get
            {
                return DateRealizedTotals != null ? DateRealizedTotals.Sum(x => x.GetValueOrDefault()) : 0;
            }
        }

        [NoDirtyCheck]
        public decimal DateTotalsSum
        {
            get
            {
                return DateTotals != null ? DateTotals.Where(y => y != null).Sum(x => x.Hours) : 0;
            }
        }

        public IEnumerable<ForecastTypeRegistration> PresenceRegistrations
        {
            get { return DateColumns != null ? DateColumns.Select(x => x.ForecastTypeRegistration) : null; }
        }

        public void RaisePresenceRegistrationsOnPropertyChanged()
        {
            OnPropertyChanged(() => PresenceRegistrations);
        }

        [NoDirtyCheck]
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate == value)
                    return;

                _selectedDate = value;
                OnPropertyChanged(() => SelectedDate);
                OnSelectedDateChanged();
            }
        }

        private void OnSelectedDateChanged()
        {
            OnPropertyChanged(() => SelectedMonthString);
            RefreshViewData();
        }

        public void RefreshViewData()
        {
            if (IsBusy)
                return;

            DisableDirtyHandling = true;
            IsBusy = true;
            _forecastRegistrationDataGenerator.GenerateBaseDataByDate(SelectedDate, this);

            FetchForecasts();
        }

        private async void FetchForecasts()
        {                       
            var response = await _forecastService.GetByUserIdAndMonth(SelectedUserHandler.UserId, SelectedDate.Month, SelectedDate.Year);
            if (response == null)
            {
                IsBusy = false;
                return;
            }

            _forecastRegistrationDataGenerator.MergeHolidays(this, response.Holidays);
            _forecastRegistrationDataGenerator.MergeForecastMonth(this, response.ForecastMonth);
            _projectForecastTypeId = response.ProjectForecastTypeId;

            //await FetchTimeEntries();

            IsBusy = false;
            RaisePresenceRegistrationsOnPropertyChanged();
            InitializeDirtyCheck();
            RaiseCanExecuteActions();

            // Fetch statistics
            FetchStatistics();
        }        

        private void FetchStatistics()
        {
            if (SelectedDate != default(DateTime))
            {
                ApplicationCommands.GetForecastStatistics.Execute(SelectedDate);
            }
        }        

        public bool Initializing { get; set; }

        public void CalculateTotals()
        {
            // Dont calculate when initializing due to performance
            if (Initializing)
                return;

            DateColumns.CalculateTotals();
            OnPropertyChanged(() => DateTotals);
            OnPropertyChanged(() => DateTotalsSum);
        }

        public void CalculateTotals(ForecastRegistrationDateColumn dateItem)
        {
            if (Initializing)
            {
                return;
            }

            dateItem.CalculateTotal();
        }

        public string SelectedMonthString
        {
            get { return string.Format("{0} {1}", SelectedDate.ToString("MMMM", CultureInfo.InvariantCulture), SelectedDate.Year); }
        }

        [NoDirtyCheck]
        public ProjectSearchViewModel ProjectSearchViewModel
        {
            get { return _projectSearchViewModel; }
        }

        [NoDirtyCheck]
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
                NextMonthCommand.RaiseCanExecuteChanged();
                PreviousMonthCommand.RaiseCanExecuteChanged();
            }
        }


        public override bool IsDirty
        {
            get
            {
                return DirtyPresenceRegitrations.Any()
                       || DirtyProjectHours.Any()
                       || RemovedProjects.Any();
            }
        }

        public string SaveDisabledText
        {
            get { return _saveDisabledText; }
            set
            {
                _saveDisabledText = value;
                OnPropertyChanged(() => SaveDisabledText);
            }
        }

        public IEnumerable<ForecastTypeRegistration> DirtyPresenceRegitrations
        {
            get { return ChangedObjects.OfType<ForecastTypeRegistration>(); }
        }

        public IEnumerable<ProjectHourRegistration> DirtyProjectHours
        {
            get { return ChangedObjects.OfType<ProjectHourRegistration>(); }
        }

        public IEnumerable<ProjectRegistration> RemovedProjects
        {
            get { return ProjectRegistrations.RemovedItems; }
        }

        public bool ForecastMonthIsLocked
        {
            get { return _forecastMonthIsLocked; }
            set
            {
                _forecastMonthIsLocked = value;
                OnPropertyChanged(() => ForecastMonthIsLocked);
            }
        }

        public int ProjectForecastTypeId
        {
            get { return _projectForecastTypeId; }
        }

        [NoDirtyCheck]
        public ForecastRegistrationSelectedUserHandler SelectedUserHandler
        {
            get { return _selectedUserHandler; }
        }

        public override void InitializeDirtyCheck()
        {
            base.InitializeDirtyCheck();
            ProjectRegistrations.InitializeDirtyCheck();
        }

        public override bool IsValid()
        {
            var valid = base.IsValid();
            if (!valid)
                return false;

            if (DateTotals.Any(totalHour => totalHour != null && !totalHour.IsValid()))
                return false;

            if (PresenceRegistrations.Any(x => !x.IsValid()))
                return false;

            return true;
        }

        #region DISABLED FOR H&B

        //private async Task FetchTimeEntries()
        //{
        //    var timeEntriesForMonth =
        //        await _timeEntryService.GetTimeEntriesByDateIgnoreEmptyTimeEntries(SelectedDate, SelectedDate.AddMonths(1));
        //    DateRealizedTotals = GenerateRealizedTotals(timeEntriesForMonth);
        //}

        //private IEnumerable<decimal?> GenerateRealizedTotals(IEnumerable<TimeEntry> timeEntriesForMonth)
        //{
        //    var tmptimeEntriesForMonth = timeEntriesForMonth
        //        .Where(x => x.Task.TimeRegistrationType == TimeRegistrationTypeEnum.Standard)
        //        .ToList();
        //    if (_appSettings.WorkPlanRealizedHourBillableOnly)
        //    {
        //        tmptimeEntriesForMonth = tmptimeEntriesForMonth.Where(x => x.Billable).ToList();
        //    }
        //    var tmpDateRealizedTotals = new List<decimal?>();
        //    foreach (var hourRegistration in DateTotals)
        //    {
        //        var forecastDateColumn = hourRegistration.DateColumn;
        //        var timeSpentOnDate = (decimal)tmptimeEntriesForMonth.Where(x => x.StartTime.Date == forecastDateColumn.Date)
        //            .Sum(x => x.TimeSpent.TotalHours);
        //        var normalDay = forecastDateColumn.IsWorkDay && forecastDateColumn.ForecastType.SupportsProjectHours;
        //        if (timeSpentOnDate == decimal.Zero && !normalDay)
        //        {
        //            tmpDateRealizedTotals.Add(null);
        //        }
        //        else
        //        {
        //            tmpDateRealizedTotals.Add(timeSpentOnDate);
        //        }
        //    }

        //    return tmpDateRealizedTotals;
        //}

        #endregion

    }
}