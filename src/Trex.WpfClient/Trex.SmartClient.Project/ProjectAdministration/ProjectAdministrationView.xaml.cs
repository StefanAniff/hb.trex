using System.Windows.Controls;
using Telerik.Windows.Controls.Scheduling;
using Trex.SmartClient.Core.Interfaces;
using ViewBase = Trex.SmartClient.Core.Implemented.ViewBase;

namespace Trex.SmartClient.Project.ProjectAdministration
{
    /// <summary>
    /// Interaction logic for ProjectAdministrationView.xaml
    /// </summary>
    public partial class ProjectAdministrationView : ViewBase, IProjectAdministrationView
    {
        public ProjectAdministrationView()
        {
            InitializeComponent();
        }        
    }
}
