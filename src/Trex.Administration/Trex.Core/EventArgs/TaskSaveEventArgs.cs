using Trex.Core.Model;
using Trex.ServiceModel.Model;

namespace Trex.Core.EventArgs
{
    public class TaskSaveEventArgs : System.EventArgs
    {
        public TaskSaveEventArgs(Task task)
        {
            Task = task;
        }

        public Task Task { get; set; }
    }
}