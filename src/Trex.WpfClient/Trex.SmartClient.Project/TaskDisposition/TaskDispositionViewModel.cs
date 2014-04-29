using System.Windows;
using Trex.SmartClient.Core.Implemented;

namespace Trex.SmartClient.Project.TaskDisposition
{
    public class TaskDispositionViewModel : ViewModelBase, ITaskDispositionViewModel
    {
        public void Initialize()
        {
            // Initialize stuff
            MessageBox.Show("Get stuff!");
        }
    }
}