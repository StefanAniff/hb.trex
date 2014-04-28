using System.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Project.ProjectAdministration
{
    /// <summary>
    /// Interaction logic for ProjectAdministrationView.xaml
    /// </summary>
    public partial class ProjectAdministrationView : UserControl, IProjectAdministrationView
    {
        public ProjectAdministrationView()
        {
            InitializeComponent();
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}
