using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.TaskModule.TaskScreen.HistoryFeedView;

namespace Trex.SmartClient.TaskModule.TaskScreen
{
    public interface IHistoryFeedViewModel : IViewModel
    {
        DelegateCommand<object> CloseErrorMessageCommand { get; set; }
         DelegateCommand<object> CancelChanges { get; set; }
        string ErrorMessage { get; set; }
        bool HasErrors { get; set; }
        ObservableCollection<HistoryFeedRowViewModel> TimeEntries { get; set; }
        double RegisteredHoursToday { get; }
        double BillableHoursToday { get; }
        double RegisteredHoursThisWeek { get; }
        double BillableHoursThisWeek { get; }
        double RegisteredHoursThisMonth { get; }
        double BillableHoursThisMonth { get; }
        bool SyncInProgress { get; set; }
        int SyncProgress { get; set; }
        string SyncMessage { get; set; }
        List<TimeEntryType> TimeEntryTypes { get; }
        TimeEntryType UserDefaultTimeEntryType { get; set; }
        bool UserDefaultTimeEntryTypeSelectionIsEnabled { get; }
    }
    public class DesignHistoryFeedViewModel : ViewModelBase, IHistoryFeedViewModel
    {
        public DelegateCommand<object> CloseErrorMessageCommand { get; set; }
        public DelegateCommand<object> CancelChanges { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasErrors { get; set; }

        public DesignHistoryFeedViewModel()
        {
            UserDefaultTimeEntryType = TimeEntryTypes.First();
        }

        public bool UserDefaultTimeEntryTypeSelectionIsEnabled
        {
            get { return true; }
        }
        public bool UseDefaultType
        {
            get { return true; }
        }

        public ObservableCollection<HistoryFeedRowViewModel> TimeEntries
        {
            get
            {
                var _timeEntries = new ObservableCollection<HistoryFeedRowViewModel>();
                var company2 = Company.Create("Danske Commodities", 0, true, false);
                var company1 = Company.Create("Global Risk Management", 0, true, false);
                var project2 = Project.Create(0, "Support og vedligeholdelse", company1, false);
                var project1 = Project.Create(0, "Rapporting DCBI", company2, false);
                var task1 = Task.Create(Guid.Empty, 0, "DCBI-4965 Renewables - portfolio report - testing", "des", project1, DateTime.Now, true, string.Empty, false, TimeRegistrationTypeEnum.Standard);
                var task2 = Task.Create(Guid.Empty, 0, "BECH-6104 Oprettede sager de sidste 14 dage.", "des", project2, DateTime.Now, true, string.Empty, false, TimeRegistrationTypeEnum.Standard);

                var timeEntryType = TimeEntryType.Create(0, true, true, "On-premise (Cient)", 0);
                var billableTime = new TimeSpan(1, 0, 0);
                _timeEntries.Add(new HistoryFeedRowViewModel(TimeEntry.Create(Guid.Empty, task1, timeEntryType, billableTime,
                                                                              billableTime,
                                                                              "entry dec", DateTime.Now, DateTime.Now, 0, true,
                                                                              "sync", true, new TimeEntryHistory(), false,
                                                                              DateTime.Now, 0, false)));
                _timeEntries.Add(new HistoryFeedRowViewModel(TimeEntry.Create(Guid.Empty, task1, timeEntryType, billableTime,
                                                                           billableTime,
                                                                           "entry dec", DateTime.Now, DateTime.Now, 0, true,
                                                                           "sync", true, new TimeEntryHistory(), false,
                                                                           DateTime.Now, 0, false)));

                _timeEntries.Add(new HistoryFeedRowViewModel(TimeEntry.Create(Guid.Empty, task2, timeEntryType, billableTime,
                                                                        billableTime,
                                                                        "lorem ipsum, lorem ipsum ,lorem ipsum,lorem ipsum,lorem ipsum,", DateTime.Now, DateTime.Now, 0, true,
                                                                        "sync", true, new TimeEntryHistory(), false,
                                                                        DateTime.Now, 0, false)));
                return _timeEntries;
            }
            set { }
        }

        public double RegisteredHoursToday { get; private set; }
        public double BillableHoursToday { get; private set; }
        public double RegisteredHoursThisWeek { get; private set; }
        public double BillableHoursThisWeek { get; private set; }
        public double RegisteredHoursThisMonth { get; private set; }
        public double BillableHoursThisMonth { get; private set; }
        public bool SyncInProgress { get; set; }
        public int SyncProgress { get; set; }
        public string SyncMessage { get; set; }

        public List<TimeEntryType> TimeEntryTypes
        {
            get
            {
                var timeEntryTypes = new List<TimeEntryType>();
                timeEntryTypes.Add(new TimeEntryType
                    {
                        Name = "On-premise (Client)",
                        Id = 0
                    });
                timeEntryTypes.Add(new TimeEntryType
                    {
                        Name = "Off-premise",
                        Id = 1
                    });
                return timeEntryTypes;
            }
        }

        private TimeEntryType _userDefaultTimeEntryType;

        public TimeEntryType UserDefaultTimeEntryType
        {
            get { return _userDefaultTimeEntryType; }
            set
            {
                _userDefaultTimeEntryType = value;
                OnPropertyChanged(() => UserDefaultTimeEntryType);
            }
        }

        public bool PlaceHolderActivated { get; set; }
    }
}
