using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Trex.Dialog.SelectTask.Interfaces;
using Trex.Dialog.SelectTask.Viewmodels;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.TaskModule.Dialogs.DesignData
{
    public class DesignSaveTaskDialogViewModel : ISaveTaskDialogViewModel
    {
        private DateTime _selectedDate;
        private string _description;
        private bool _timeEntryViewPeriodSelected;
        public ISelectTaskViewModel SelectTaskViewModel { get; set; }
        public DelegateCommand<object> SaveTask { get; set; }
        public DelegateCommand<object> CloseCommand { get; set; }
        public bool IsOpen { get; set; }
        public bool IsRunning { get; private set; }
        public DateTime SelectedDate
        {
            get { return DateTime.Now; }
            set { _selectedDate = value; }
        }

        public TimeSpan TimeSpent { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsInSelectionMode { get; set; }
        public string AssignedTask { get; private set; }
        public string Description
        {
            get { return "design data"; }
            set { _description = value; }
        }

        public bool IsBillable { get; set; }
        public string PricePrHour { get; set; }
        public TimeEntryType SelectedTimeEntryType { get; set; }
        public ObservableCollection<TimeEntryType> TimeEntryTypes { get; set; }

        public bool TimeEntryViewTimeSpentSelected
        {
            get { return false; }
            set { }
        }

        public bool TimeEntryViewPeriodSelected
        {
            get { return true; }
            set { _timeEntryViewPeriodSelected = value; }
        }

        public bool Invoiced { get; private set; }
    }
}
