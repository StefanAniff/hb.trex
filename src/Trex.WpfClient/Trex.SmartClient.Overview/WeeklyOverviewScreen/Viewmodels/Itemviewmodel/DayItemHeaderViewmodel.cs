using System;
using System.Collections.Generic;
using System.Linq;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Overview.WeeklyOverviewScreen.Viewmodels.Itemviewmodel
{
    public class DayItemHeaderViewmodel : ViewModelBase
    {
        public readonly IEnumerable<DayItemViewmodel> _timeSpent;
        public IEnumerable<TimeEntry> TimeEntries
        {
            get { return _timeSpent.SelectMany(x => x.TimeEntries); }
        }
        public DateTime Date { get; set; }

        public bool IsToday
        {
            get { return Date == DateTime.Today; }
        }

        public string Header
        {
            get
            {
                var dayOfWeek = Date.DayOfWeek;
                var length = 3;

                //if (dayOfWeek == DayOfWeek.Thursday || dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                //{
                //    length++;
                //}
                return dayOfWeek.ToString().Substring(0, length);
            }
        }

        public double Total
        {
            get { return _timeSpent.Where(x => x.Task.TimeRegistrationType != TimeRegistrationTypeEnum.Projection).Sum(x => x.RegisteredHours.GetValueOrDefault()); }
        }


        public string ToolTip
        {
            get
            {
                return string.Format("Billable: {0:N2}. Non-billable: {1:N2}", _timeSpent.Where(x => x.Billable).Sum(x => x.RegisteredHours),
                                     _timeSpent.Where(x => !x.Billable).Sum(x => x.RegisteredHours));
            }
        }



        private TimeEntryType _selectTimeEntryType;

        public TimeEntryType SelectTimeEntryType
        {
            get { return _selectTimeEntryType; }
            set
            {
                if (_selectTimeEntryType != null)
                {
                    foreach (var itemViewmodel in _timeSpent)
                    {
                        itemViewmodel.SetTimeEntryType(value);
                    }
                }
                _selectTimeEntryType = value;
                OnPropertyChanged(() => SelectTimeEntryType);
            }
        }

        public DayItemHeaderViewmodel()
        {

        }


        public DayItemHeaderViewmodel(DateTime dateTime, IEnumerable<DayItemViewmodel> timeSpent)
        {
            _timeSpent = timeSpent;
            Date = dateTime;
        }
    }

}
