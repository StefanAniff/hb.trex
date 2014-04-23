using System;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.TaskModule.TaskScreen.TaskToolTipView;

namespace Trex.SmartClient.TaskModule.TaskScreen.InactiveTaskView
{
    public interface IInActiveTaskViewModel
    {
        DelegateCommand<object> Activate { get; set; }
        double Height { get; }
        double Width { get; }
        TimeEntry TimeEntry { get; }
         bool IsAssigned { get; }
        string StartDate { get; }
        string PauseDate { get; }
        string TaskName { get; }
        bool IsSaved { get; }
        string ProjectName { get; }
        string CustomerName { get; }
        TimeSpan TimeSpent { get; }
        TaskToolTipViewModel ToolTipViewModel { get; }
        DelegateCommand<object> CloseInactiveTask { get; set; }
        DelegateCommand<object> CloseAllInactiveTask { get; set; }
        double TimeSpentFontSize { get; }
        double DetailsFontSize { get; }
        double TaskNameFontSize { get; }
        void UpdateLayout(double height, double width, double timeSpentFontSize, double taskNameFontSize, double detailsFontSize);
    }

    public class DesignIInActiveTaskViewModel : IInActiveTaskViewModel
    {
        public DelegateCommand<object> Activate { get; set; }
        public DelegateCommand<object> CloseInactiveTask { get; set; }
        public DelegateCommand<object> CloseAllInactiveTask { get; set; }

        public double Height
        {
            get { return 84; }
        }

        public double Width
        {
            get { return 126; }
        }

        public TimeEntry TimeEntry { get; private set; }

        public string StartDate
        {
            get { return DateTime.Now.ToShortDateAndTimeString(); }
        }

        public string PauseDate { get; private set; }

        public string TaskName
        {
            get { return "DC-Connect-9593 Error, Bottomview Volume (Buy, Sell)"; }
        }

        public bool IsSaved { get; private set; }

        public bool IsAssigned
        {
            get { return true; }
        }

        public string ProjectName
        {
            get { return "HandelsModul"; }
        }

        public string CustomerName
        {
            get { return "Microsoft Ireland Operations Ltd."; }
        }

        public TimeSpan TimeSpent
        {
            get { return new TimeSpan(0, 15, 45); }
        }

        public TaskToolTipViewModel ToolTipViewModel { get; private set; }

        public double TimeSpentFontSize
        {
            get { return 18; }
        }

        public double DetailsFontSize
        {
            get { return 11; }
        }

        public double TaskNameFontSize
        {
            get { return 12; }
        }

        public void UpdateLayout(double height, double width, double timeSpentFontSize, double taskNameFontSize, double detailsFontSize)
        {
            throw new NotImplementedException();
        }
    }
}