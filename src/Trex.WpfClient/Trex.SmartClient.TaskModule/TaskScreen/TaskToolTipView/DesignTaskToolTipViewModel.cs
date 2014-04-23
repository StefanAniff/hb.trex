using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.TaskModule.TaskScreen.TaskToolTipView
{
    public interface ITaskToolTipViewModel
    {
        ObservableCollection<TimeInterval> TaskHistory { get; }
        string CustomerName { get; }
        string TaskName { get; }
        string ProjectName { get; }
        bool IsAssigned { get; }
    }

    public class DesignTaskToolTipViewModel : ITaskToolTipViewModel
    {
        public ObservableCollection<TimeInterval> TaskHistory
        {
            get
            {
                var taskHistory = new ObservableCollection<TimeInterval>();

                taskHistory.Add(new TimeInterval
                    {
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now.AddMinutes(5)
                    });

                taskHistory.Add(new TimeInterval
                {
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddMinutes(5)
                });

                taskHistory.Add(new TimeInterval
                {
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddMinutes(5)
                });

                taskHistory.Add(new TimeInterval
                {
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddMinutes(5)
                });

                taskHistory.Add(new TimeInterval
                {
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddMinutes(5)
                });
                return taskHistory;
            }
        }

        public string CustomerName
        {
            get { return "Danske Commodities A/S"; }
        }

        public string TaskName
        {
            get { return "DC-CONNECT-9857 Ability to change trade type"; }
        }

        public string ProjectName
        {
            get { return "Handelsmodul"; }
        }

        public bool IsAssigned
        {
            get { return true; }
        }
    }
}