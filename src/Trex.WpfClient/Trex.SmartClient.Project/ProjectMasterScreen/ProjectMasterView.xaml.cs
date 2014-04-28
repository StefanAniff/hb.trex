using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Project.ProjectMasterScreen
{
    /// <summary>
    /// Interaction logic for ProjectMasterView.xaml
    /// </summary>
    public partial class ProjectMasterView : UserControl, IProjectMasterView
    {
        public ProjectMasterView()
        {
            InitializeComponent();
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}
