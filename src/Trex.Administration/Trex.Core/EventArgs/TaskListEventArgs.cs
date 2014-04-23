using System.Collections.Generic;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class TaskListEventArgs : System.EventArgs
    {
        public TaskListEventArgs(List<Task> tasks)
        {
            if (Tasks != null)
            {
                Tasks = tasks;
            }
            else
            {
                Tasks = new List<Task>();
            }
        }

        public List<Task> Tasks { get; set; }
    }
}