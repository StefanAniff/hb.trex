using System.Windows.Controls;
using Trex.Core.Interfaces;

namespace Trex.Roles.RoleScreen.MainView
{
    public partial class MainView : UserControl, IView
    {
        public MainView()
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