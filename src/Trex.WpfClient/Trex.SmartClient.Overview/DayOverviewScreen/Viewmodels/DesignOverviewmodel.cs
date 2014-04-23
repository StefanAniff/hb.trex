using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Overview.DayOverviewScreen.Viewmodels
{
    public interface IDayOverviewViewModel : IViewModel
    {
        DelegateCommand<object> PreviousDateCommand { get; set; }
        DelegateCommand<object> NextDayCommand { get; set; }

        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        DateTime VisiblePeriodEnd { get; set; }
        DateTime VisiblePeriodStart { get; set; }

        List<DataOverviewItemViewModel> TimeEntries { get; }
        List<TotalDataOverviewItemViewModel> TotalSumTimeEntry { get; }

        TimeSpan TotalSpent { get; }
        DelegateCommand<object> EditTaskCommand { get; set; }
        DelegateCommand<object> TodayCommand { get; set; }
        bool IsBusy { get; }
    }

    public class DesignDayOverviewViewModel : IDayOverviewViewModel
    {

        public DelegateCommand<object> PreviousDateCommand { get; set; }
        public DelegateCommand<object> NextDayCommand { get; set; }
        public DelegateCommand<object> EditTaskCommand { get; set; }
        public DelegateCommand<object> TodayCommand { get; set; }

        public bool IsBusy
        {
            get { return false; }
        }
        public DateTime StartDate
        {
            get { return DateTime.Now.Date; }
            set
            { }
        }


        public DateTime EndDate
        {
            get { return StartDate.AddDays(1); }
            set
            { }
        }

        public List<DataOverviewItemViewModel> TimeEntries
        {
            get
            {
                var dates = new List<DataOverviewItemViewModel>();
                dates.Add(new DataOverviewItemViewModel(new TimeEntry
                    {
                        StartTime = DateTime.Now.Date,
                        TimeSpent = new TimeSpan(0, 1, 0, 0, 0),
                        Task = Task.Create(Guid.Empty, 0, "Task1", "", null, DateTime.Now, true, "", false, TimeRegistrationTypeEnum.Standard)
                    }));
                dates.Add(new DataOverviewItemViewModel(new TimeEntry
                   {
                       StartTime = DateTime.Now.Date.AddHours(3),
                       TimeSpent = new TimeSpan(0, 1, 0, 0, 0),
                       Task = Task.Create(Guid.Empty, 0, "Task1", "", null, DateTime.Now, true, "", false, TimeRegistrationTypeEnum.Standard)
                   }));
                dates.Add(new DataOverviewItemViewModel(new TimeEntry
                   {
                       StartTime = DateTime.Now.Date,
                       TimeSpent = new TimeSpan(0, 10, 0, 0, 0),
                       Task = Task.Create(Guid.Empty, 0, "Task2", "", null, DateTime.Now, true, "", false, TimeRegistrationTypeEnum.Standard)
                   }));
                return dates;
            }
        }

        public List<TotalDataOverviewItemViewModel> TotalSumTimeEntry
        {
            get
            {
                var timeEntries = TimeEntries.Select(timeEntry => new TotalDataOverviewItemViewModel(timeEntry.TimeEntry));
                return new List<TotalDataOverviewItemViewModel>(timeEntries);
            }
        }

        public TimeSpan TotalSpent
        {
            get { return new TimeSpan(TimeEntries.Sum(x => x.Duration.Ticks)); }
        }

        public DateTime VisiblePeriodEnd
        {
            get { return EndDate; }
            set { }
        }

        public DateTime VisiblePeriodStart
        {
            get { return StartDate; }
            set { }
        }




        public void Dispose()
        {

        }
    }
}
