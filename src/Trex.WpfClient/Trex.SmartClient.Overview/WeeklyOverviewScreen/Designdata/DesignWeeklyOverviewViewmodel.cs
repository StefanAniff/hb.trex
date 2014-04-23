using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Data;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Overview.WeeklyOverviewScreen.Viewmodels;
using Trex.SmartClient.Overview.WeeklyOverviewScreen.Viewmodels.Itemviewmodel;

namespace Trex.SmartClient.Overview.WeeklyOverviewScreen
{
    public interface IWeeklyOverviewViewmodel : IViewModel
    {
        DelegateCommand<object> PreviousDateCommand { get; set; }
        DelegateCommand<object> NextDayCommand { get; set; }
        DelegateCommand<object> SaveCommand { get; set; }
        DelegateCommand<object> SwitchToDayViewCommand { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        string Text { get; }
        List<TimeEntryType> TimeEntryTypes { get; }
        ObservableItemCollection<TaskItemViewmodel> Rows { get; set; }
        DayItemHeaderViewmodel Day1 { get; }
        DayItemHeaderViewmodel Day2 { get; }
        DayItemHeaderViewmodel Day3 { get; }
        DayItemHeaderViewmodel Day4 { get; }
        DayItemHeaderViewmodel Day5 { get; }
        DayItemHeaderViewmodel Day6 { get; }
        DayItemHeaderViewmodel Day7 { get; }
        double Total { get; }
        bool HasChanges { get; }
        bool CanCopyPreviousTimesheet { get; }
        DelegateCommand<object> CopyPreviousTasksToSelectedDate { get; set; }
        DelegateCommand<object> DeleteTimeEntry { get; set; }
        bool IsSyncing { get; }
        DelegateCommand<object> AddTaskCommand { get; set; }
        DelegateCommand<object> TodayCommand { get; set; }
    }

    public class DesignWeeklyOverviewViewmodel : IWeeklyOverviewViewmodel
    {
        public DelegateCommand<object> PreviousDateCommand { get; set; }
        public DelegateCommand<object> NextDayCommand { get; set; }
        public DelegateCommand<object> SaveCommand { get; set; }
        public DelegateCommand<object> SwitchToDayViewCommand { get; set; }
        public DelegateCommand<object> CopyPreviousTasksToSelectedDate { get; set; }
        public DelegateCommand<object> DeleteTimeEntry { get; set; }

        public DateTime StartDate
        {
            get { return new DateTime(2013, 03, 18); }
            set { }
        }

        public DateTime EndDate
        {
            get { return new DateTime(2013, 03, 24); }
            set { }
        }

        public string Text
        {
            get { return string.Format("{0:dd} – {1:dd} {0:MMM} {0:yyyy}", StartDate, EndDate); }
        }
        public List<TimeEntryType> TimeEntryTypes
        {
            get
            {
                var list = new List<TimeEntryType>();
                list.Add(TimeEntryType.Create(0, false, false, "On premise", null));
                return list;
            }
        }

        public ObservableItemCollection<TaskItemViewmodel> Rows
        {
            get
            {
                var timeEntrieses = new List<TimeEntry>();
                var timeEntry = TimeEntry.Create();
                timeEntry.TimeSpent = new TimeSpan(5, 30, 0);
                timeEntrieses.Add(timeEntry);
                var list = new ObservableItemCollection<TaskItemViewmodel>();
                var company2 = Company.Create("Danske Commodities", 0, true, false);
                var project1 = Project.Create(0, "Rapporting DCBI", company2, false);
                var task = Task.Create(Guid.Empty, 0, "DCBI-4965 Renewables - portfolio report - testing", "des", project1, DateTime.Now, true, string.Empty, false, TimeRegistrationTypeEnum.Standard);

                list.Add(new DesignTaskItemViewmodel(task, timeEntrieses, StartDate));

                var timeEntryByTaskWithComments = timeEntrieses;
                var timeEntryWithComment = TimeEntry.Create();
                timeEntryWithComment.StartTime = StartDate.AddDays(-2);
                timeEntryWithComment.TimeSpent = new TimeSpan(5, 30, 0);
                timeEntryWithComment.Description = "hej";
                timeEntryByTaskWithComments.Add(timeEntryWithComment);

                list.Add(new DesignTaskItemViewmodel(task, timeEntryByTaskWithComments, StartDate));

                list.Add(new DesignTaskItemViewmodel(task, timeEntrieses, StartDate));

                list.Add(new DesignTaskItemViewmodel(task, timeEntrieses, StartDate));

                list.Add(new DesignTaskItemViewmodel(task, timeEntrieses, StartDate));

                list.Add(new DesignTaskItemViewmodel(task, timeEntrieses, StartDate));

                list.Add(new DesignTaskItemViewmodel(task, timeEntrieses, StartDate));

                list.Add(new DesignTaskItemViewmodel(task, timeEntrieses, StartDate));

                list.Add(new DesignTaskItemViewmodel(task, timeEntrieses, StartDate));

                list.Add(new DesignTaskItemViewmodel(task, timeEntrieses, StartDate));

                list.Add(new DesignTaskItemViewmodel(task, timeEntrieses, StartDate));
                return list;
            }
            set { }
        }

        public DayItemHeaderViewmodel Day1
        {
            get
            {
                return new DayItemHeaderViewmodel
                    {
                        Date = new DateTime(2013, 03, 18)
                    };
            }
        }

        public DayItemHeaderViewmodel Day2
        {
            get
            {
                return new DayItemHeaderViewmodel
                    {
                        Date = new DateTime(2013, 03, 19),
                    };
            }
        }

        public DayItemHeaderViewmodel Day3
        {
            get
            {
                return new DayItemHeaderViewmodel
                    {
                        Date = new DateTime(2013, 03, 20)
                    };
            }
        }

        public DayItemHeaderViewmodel Day4
        {
            get
            {
                return new DayItemHeaderViewmodel
                    {
                        Date = new DateTime(2013, 03, 21)
                    };
            }
        }

        public DayItemHeaderViewmodel Day5
        {
            get
            {
                return new DayItemHeaderViewmodel
                    {
                        Date = new DateTime(2013, 03, 22)
                    };
            }
        }

        public DayItemHeaderViewmodel Day6
        {
            get
            {
                return new DayItemHeaderViewmodel
                    {
                        Date = new DateTime(2013, 03, 23)
                    };
            }
        }

        public DayItemHeaderViewmodel Day7
        {
            get
            {
                return new DayItemHeaderViewmodel
                    {
                        Date = new DateTime(2013, 03, 24)
                    };
            }
        }

        public double Total
        {
            get { return 5.5; }
        }

        public bool HasChanges
        {
            get { return true; }
        }

        public bool CanCopyPreviousTimesheet
        {
            get { return false; }
        }

        public bool IsSyncing
        {
            get { return false; }
        }

        public DelegateCommand<object> AddTaskCommand { get; set; }

        public DelegateCommand<object> TodayCommand { get; set; }

        public void Dispose()
        {

        }
    }
    public class DesignTaskItemViewmodel : TaskItemViewmodel
    {
        public DesignTaskItemViewmodel(Task task, List<TimeEntry> timeEntryByTask, DateTime date)
            : base(task, timeEntryByTask, date)
        {
            IsBillable = false;
        }
    }
}
