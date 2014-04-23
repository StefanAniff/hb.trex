using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Trex.Dialog.SelectTask.Interfaces;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.TaskModule.Dialogs.DesignData
{
    public interface ISaveTaskDialogViewModel
    {
        ISelectTaskViewModel SelectTaskViewModel { get; set; }
        DelegateCommand<object> SaveTask { get; set; }
        DelegateCommand<object> CloseCommand { get; set; }
        bool IsOpen { get; set; }
        bool IsRunning { get; }
        DateTime SelectedDate { get; set; }
        TimeSpan TimeSpent { get; set; }
        DateTime EndTime { get; set; }
        bool IsInSelectionMode { get; set; }
        string AssignedTask { get; }
        string Description { get; set; }
        bool IsBillable { get; set; }
        string PricePrHour { get; set; }
        TimeEntryType SelectedTimeEntryType { get; set; }
        ObservableCollection<TimeEntryType> TimeEntryTypes { get; set; }
        bool TimeEntryViewTimeSpentSelected { get; set; }
        bool TimeEntryViewPeriodSelected { get; set; }
        bool Invoiced { get; }
    }
}