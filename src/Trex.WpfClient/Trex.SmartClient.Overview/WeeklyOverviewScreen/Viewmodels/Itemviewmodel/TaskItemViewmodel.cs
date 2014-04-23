using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Overview.WeeklyOverviewScreen.Viewmodels.Itemviewmodel
{
    public class TaskItemViewmodel : ViewModelBase
    {
        private bool _initial;

        public bool Initial
        {
            get { return _initial; }
            set
            {
                _initial = value;
                OnPropertyChanged(() => Initial);
            }
        }

        private readonly Task _task;
        private bool _isBillable;

        public bool IsBillable
        {
            get { return _isBillable; }
            set
            {
                _isBillable = value;
                foreach (var dayItemViewmodel in AllDays)
                {
                    dayItemViewmodel.Billable = _isBillable;
                }
                OnPropertyChanged(() => IsBillable);
            }
        }

        public bool HasChanges
        {
            get { return AllDays.Any(x => x.HasChanges); }
        }

        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string CustomerName { get; set; }

        public string Comment
        {
            get
            {
                var timeEntries = AllDays.Select(x => x.Comment);
                var comment = ExtractComment(timeEntries);

                return "Comment: " + comment;
            }
            set
            {
                
            }
        }

        private static string ExtractComment(IEnumerable<string> descriptions)
        {
            var commentBuilder = new StringBuilder();
            foreach (var description in descriptions)
            {
                commentBuilder.Append(description);
                commentBuilder.Append(description.EndsWith(".") ? " " : ". ");
            }
            var comment = commentBuilder.ToString().TrimEnd(' ');
            return comment;
        }

        public DayItemViewmodel Monday { get; set; }

        public DayItemViewmodel Tuesday { get; set; }

        public DayItemViewmodel Wednesday { get; set; }

        public DayItemViewmodel Thursday { get; set; }

        public DayItemViewmodel Friday { get; set; }

        public DayItemViewmodel Saturday { get; set; }

        public DayItemViewmodel Sunday { get; set; }

        public double Total
        {
            get { return AllDays.Sum(x => x.RegisteredHours.GetValueOrDefault()); }
        }

        public double RealTotal
        {
            get { return AllDays.SelectMany(x => x.TimeEntries).Where(x => x.Task.TimeRegistrationType != TimeRegistrationTypeEnum.Projection).Sum(x => x.TimeSpent.TotalHours); }
        }

        public IEnumerable<DayItemViewmodel> AllDays
        {
            get
            {
                yield return Monday;
                yield return Tuesday;
                yield return Wednesday;
                yield return Thursday;
                yield return Friday;
                yield return Saturday;
                yield return Sunday;
            }
        }

        private bool _isDeleted;

        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                _isDeleted = value;
                OnPropertyChanged(() => IsDeleted);
            }
        }


     

        public TaskItemViewmodel(Task task, List<TimeEntry> timeEntryByTask, DateTime date, bool isBillable = false, bool initial = true)
        {
            TaskName = task.Name;
            CustomerName = task.Project.Company.Name;
            ProjectName = task.Project.Name;

            _isBillable = isBillable || timeEntryByTask.Any(x => x.Billable);

            Monday = new DayItemViewmodel(timeEntryByTask.Where(x => x.StartTime.DayOfWeek == DayOfWeek.Monday), task, date, _isBillable);
            Tuesday = new DayItemViewmodel(timeEntryByTask.Where(x => x.StartTime.DayOfWeek == DayOfWeek.Tuesday), task, date.AddDays(1), _isBillable);
            Wednesday = new DayItemViewmodel(timeEntryByTask.Where(x => x.StartTime.DayOfWeek == DayOfWeek.Wednesday), task, date.AddDays(2), _isBillable);
            Thursday = new DayItemViewmodel(timeEntryByTask.Where(x => x.StartTime.DayOfWeek == DayOfWeek.Thursday), task, date.AddDays(3), _isBillable);
            Friday = new DayItemViewmodel(timeEntryByTask.Where(x => x.StartTime.DayOfWeek == DayOfWeek.Friday), task, date.AddDays(4), _isBillable);
            Saturday = new DayItemViewmodel(timeEntryByTask.Where(x => x.StartTime.DayOfWeek == DayOfWeek.Saturday), task, date.AddDays(5), _isBillable);
            Sunday = new DayItemViewmodel(timeEntryByTask.Where(x => x.StartTime.DayOfWeek == DayOfWeek.Sunday), task, date.AddDays(6), _isBillable);

            foreach (var dayItemViewmodel in AllDays)
            {
                dayItemViewmodel.PropertyChanged += DayChanged;
            }


            OnPropertyChanged(() => Total);
            OnPropertyChanged(() => IsBillable);

            Initial = initial;
        }

        private void DayChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OnPropertyChanged(() => Total);
            OnPropertyChanged(() => HasChanges);
        }
    }
}
