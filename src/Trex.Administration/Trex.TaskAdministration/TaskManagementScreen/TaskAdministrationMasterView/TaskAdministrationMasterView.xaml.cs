using System.Windows.Controls;
using Trex.Core.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.TaskAdministrationMasterView
{
    public partial class TaskAdministrationMasterView : UserControl, IView
    {
        public TaskAdministrationMasterView()
        {
            InitializeComponent();
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion
    }
}