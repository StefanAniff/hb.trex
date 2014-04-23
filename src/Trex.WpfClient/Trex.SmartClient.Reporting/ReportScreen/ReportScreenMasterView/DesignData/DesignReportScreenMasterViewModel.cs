using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Infrastructure.Extensions;
using Trex.SmartClient.Reporting.ReportScreen.ReportScreenMasterView;

namespace Trex.SmartClient.Reporting.ReportScreen
{
    public interface IReportScreenMasterViewModel
    {
        DelegateCommand<object> Search { get; set; }
        DateTime FromDate { get; set; }
        DateTime ToDate { get; set; }
        ObservableCollection<GridRowItemViewModel> GridRows { get; }
        GridRowItemViewModel SelectedItem { get; set; }
        ObservableCollection<PredefinedSearchViewModel> PredefinedSearchItems { get; set; }
        PredefinedSearchViewModel SelectedPredefinedSearch { get; set; }
        DelegateCommand<object> EditTimeEntryCommand { get; set; }
        bool IsOnline { get; }
        bool IsReadOnly { get; }
        bool IsInEditMode { get; }
        List<TimeEntryType> TimeEntryTypes { get; set; }
        DelegateCommand<object> SaveCommand { get; set; }
        bool AdvancedSettingsEnabled { get; }
    }

    public class DesignReportScreenMasterViewModel : IReportScreenMasterViewModel
    {
        public DelegateCommand<object> Search { get; set; }
        public DelegateCommand<object> EditTimeEntryCommand { get; set; }
        public DelegateCommand<object> SaveCommand { get; set; }

        public ObservableCollection<GridRowItemViewModel> GridRows
        {
            get
            {
                var items = new ObservableCollection<GridRowItemViewModel>();
                var company2 = Company.Create("Danske Commodities", 0, true, false);
                var company1 = Company.Create("Global Risk Management", 0, true, false);
                var project2 = Project.Create(0, "Support og vedligeholdelse", company1, false);
                var project1 = Project.Create(0, "Rapporting DCBI", company2, false);
                var task1 = Task.Create(Guid.Empty, 0, "DCBI-4965 Renewables - portfolio report - testing", "des", project1, DateTime.Now, true, string.Empty, false, TimeRegistrationTypeEnum.Standard);
                var task2 = Task.Create(Guid.Empty, 0, "BECH-6104 Oprettede sager de sidste 14 dage.", "des", project2, DateTime.Now, true, string.Empty, false, TimeRegistrationTypeEnum.Standard);

                var timeEntryType = TimeEntryType.Create(0, true, true, "On-premise (Cient)", 0);
                var billableTime = new TimeSpan(1, 0, 0);
                items.Add(new GridRowItemViewModel(TimeEntry.Create(Guid.Empty, task1, timeEntryType, billableTime,
                                                                              billableTime,
                                                                              "entry dec", DateTime.Now, DateTime.Now, 0, true,
                                                                              "sync", true, new TimeEntryHistory(), false,
                                                                              DateTime.Now, 0, false), timeEntryType));
                items.Add(new GridRowItemViewModel(TimeEntry.Create(Guid.Empty, task1, timeEntryType, billableTime,
                                                                           billableTime,
                                                                           "entry dec", DateTime.Now, DateTime.Now, 0, true,
                                                                           "sync", true, new TimeEntryHistory(), false,
                                                                           DateTime.Now, 0, false), timeEntryType));

                items.Add(new GridRowItemViewModel(TimeEntry.Create(Guid.Empty, task2, timeEntryType, billableTime,
                                                                        billableTime,
                                                                        "lorem ipsum, lorem ipsum ,lorem ipsum,lorem ipsum,lorem ipsum,", DateTime.Now, DateTime.Now, 0, true,
                                                                        "sync", true, new TimeEntryHistory(), false,
                                                                        DateTime.Now, 0, false), timeEntryType));
                return items;
            }
            set { }
        }

        public GridRowItemViewModel SelectedItem
        {
            get { return GridRows.First(); }
            set { }
        }

        public ObservableCollection<PredefinedSearchViewModel> PredefinedSearchItems
        {
            get
            {
                return new ObservableCollection<PredefinedSearchViewModel>
                    {
                        new PredefinedSearchViewModel("This week", DateTime.Now.FirstDayOfWeek(),
                                                      DateTime.Now),
                        new PredefinedSearchViewModel("This month", DateTime.Now.FirstDayOfMonth(),
                                                      DateTime.Now),
                        new PredefinedSearchViewModel("Last month",
                                                      DateTime.Now.AddMonths(-1).FirstDayOfMonth(),
                                                      DateTime.Now.FirstDayOfMonth().AddDays(-1)),
                        new PredefinedSearchViewModel("This year", new DateTime(DateTime.Now.Year, 1, 1),
                                                      DateTime.Now)
                    };
            }
            set { }
        }

        public DateTime FromDate
        {
            get { return PredefinedSearchItems[1].FromDate; }
            set { }
        }

        public DateTime ToDate
        {
            get { return PredefinedSearchItems[1].ToDate; }
            set { }
        }

        public PredefinedSearchViewModel SelectedPredefinedSearch
        {
            get { return PredefinedSearchItems[1]; }
            set { }
        }

        public bool IsOnline
        {
            get { return true; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsInEditMode
        {
            get { return true; }
            set{}
        }

        public List<TimeEntryType> TimeEntryTypes { get; set; }
        public bool AdvancedSettingsEnabled
        {
            get { return true; }
        }
    }

}
