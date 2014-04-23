using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Core.Utils;
using Trex.SmartClient.Infrastructure.Commands;
using log4net;
using Task = System.Threading.Tasks.Task;

namespace Trex.SmartClient.Overview.DayOverviewScreen.Viewmodels
{
    public class DayOverviewViewModel : ViewModelBase, IDayOverviewViewModel
    {
        private readonly ITimeEntryService _timeEntryService;
        private readonly IBusyService _busyService;
        private readonly DelegateCommand<object> _saveCommandCompletedCommand;
        private bool _isShowingTextbox;

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DelegateCommand<object> PreviousDateCommand { get; set; }
        public DelegateCommand<object> NextDayCommand { get; set; }
        public DelegateCommand<object> EditTaskCommand { get; set; }
        public DelegateCommand<object> TodayCommand { get; set; }

        public DayOverviewViewModel(ITimeEntryService timeEntryService, IBusyService busyService)
        {
            _timeEntryService = timeEntryService;
            _busyService = busyService;
            _busyService.ShowBusy(_dayoverviewKey);
            TimeEntries = new List<DataOverviewItemViewModel>();

            PreviousDateCommand = new DelegateCommand<object>(a => StartDate = StartDate.AddDays(-1));
            NextDayCommand = new DelegateCommand<object>(a => StartDate = StartDate.AddDays(1));
            TodayCommand = new DelegateCommand<object>(a => StartDate = GetNuetralKindDateTimeToday());
            EditTaskCommand = new DelegateCommand<object>(EditTaskExecute);
            _saveCommandCompletedCommand = new DelegateCommand<object>(SaveTaskCompleted);
            TaskCommands.SaveTaskCompleted.RegisterCommand(_saveCommandCompletedCommand);
            OverviewCommands.GoToDaySubMenu.RegisterCommand(new DelegateCommand<object>(ChangeDate));

            VisiblePeriodStart = StartDate.AddHours(6);
            VisiblePeriodEnd = EndDate.AddHours(-6);
            StartDate = GetNuetralKindDateTimeToday();
        }

        /// <summary>
        /// RadTimeline is having issues working with DateTime with kind "Local".
        /// Override kind to "Unspecified".
        /// RadDatePicker sets date with Unspecified.
        /// When manually settings dates, use therefore this method
        /// NOTE! If kind not set to Unspecified, will constantly fail in production, and periodically in debug
        /// </summary>
        /// <returns></returns>
        private static DateTime GetNuetralKindDateTimeToday()
        {
            return DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Unspecified);
        }

        private static void EditTaskExecute(object obj)
        {
            var itemViewModel = (DataOverviewItemViewModel)obj;
            TaskCommands.EditTaskStart.Execute(itemViewModel.TimeEntry);
        }

        private void ChangeDate(object obj)
        {
            StartDate = ((DateTime)obj).Date;
        }

        private void SaveTaskCompleted(object obj)
        {
            var timeEntry = obj as TimeEntry;
            if (timeEntry == null || timeEntry.StartTime.Date != StartDate)
            {
                return;
            }

            var localTimeEntries = TimeEntries;

            var oldTimeEntry = localTimeEntries.FirstOrDefault(x => x.TimeEntry.Guid == timeEntry.Guid);

            var updatedItem = new DataOverviewItemViewModel(timeEntry);
            localTimeEntries.Remove(oldTimeEntry);
            localTimeEntries.Add(updatedItem);

            TimeEntries = localTimeEntries.OrderBy(x => x.TimeEntry.Guid).ToList();

        }

        public DateTime EndDate
        {
            get { return _startDate.AddDays(1); }
            set { }
        }

        private DateTime _visiblePeriodEnd;

        public DateTime VisiblePeriodEnd
        {
            get { return _visiblePeriodEnd; }
            set
            {
                _visiblePeriodEnd = value;
                OnPropertyChanged(() => VisiblePeriodEnd);
            }
        }

        private DateTime _visiblePeriodStart;

        public DateTime VisiblePeriodStart
        {
            get { return _visiblePeriodStart; }
            set
            {
                _visiblePeriodStart = value;
                OnPropertyChanged(() => VisiblePeriodStart);
            }
        }

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(() => StartDate);
                OnPropertyChanged(() => EndDate);

                LoadTimeEntries();
                VisiblePeriodStart = StartDate;
                VisiblePeriodEnd = EndDate;
            }
        }

        private async void LoadTimeEntries()
        {
            // Is being invoked twice when date is selected in bound datepicker
            if (IsBusy)
                return;

            IsBusy = true;
            _busyService.ShowBusy(_dayoverviewKey);

            IEnumerable<DataOverviewItemViewModel> timeEntries = new List<DataOverviewItemViewModel>();
            try
            {
                //enddate tasks are also included
                var timeEntriesDtos = await _timeEntryService.GetTimeEntriesByDateIgnoreEmptyTimeEntries(StartDate, StartDate);
                timeEntries = timeEntriesDtos.Select(timeEntry => new DataOverviewItemViewModel(timeEntry));
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
            TimeEntries = timeEntries.OrderBy(x => x.TimeEntry.Guid).ToList();
            await Task.Run(() => ThreadUtils.WaitForUIRendering());

            IsBusy = false;            
            _busyService.HideBusy(_dayoverviewKey);            
        }

        private List<DataOverviewItemViewModel> _timeEntries;
        private string _dayoverviewKey = "DayOverview";

        public List<DataOverviewItemViewModel> TimeEntries
        {
            get { return _timeEntries; }
            set
            {
                _timeEntries = value;
                OnPropertyChanged(() => TimeEntries);
                OnPropertyChanged(() => TotalSumTimeEntry);
                OnPropertyChanged(() => TotalSpent);
            }
        }

        public List<TotalDataOverviewItemViewModel> TotalSumTimeEntry
        {
            get { return TimeEntries.Select(timeEntry => new TotalDataOverviewItemViewModel(timeEntry.TimeEntry)).ToList(); }
        }

        public TimeSpan TotalSpent
        {
            get { return new TimeSpan(TimeEntries.Where(x => x.TimeEntry.Task.TimeRegistrationType != TimeRegistrationTypeEnum.Projection).Sum(x => x.Duration.Ticks)); }
        }
    }
}