using System.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Project.TaskDisposition
{
    /// <summary>
    /// Interaction logic for TaskDispositionView.xaml
    /// </summary>
    public partial class TaskDispositionView : UserControl, ITaskDispositionView
    {
        public TaskDispositionView()
        {
            InitializeComponent();
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}
