using System.Windows.Controls;
using Trex.Core.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView
{
    public partial class ButtonPanelView : UserControl, IView
    {
        public ButtonPanelView()
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