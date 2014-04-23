using System.Windows.Controls;
using Trex.Core.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.RightPanelView
{
    public partial class MainListView : UserControl, IView
    {
        public MainListView()
        {
            // Required to initialize variables
            InitializeComponent();
            //using Telerik.Windows;
        }

        #region IView Members

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        #endregion
    }
}