using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Trex.Core.Interfaces;
using Telerik.Windows.Controls.GridView;

namespace Trex.Administration.UserAdministrationScreen.MainView
{
    public partial class MainView : UserControl, IView
    {
        public MainView()
        {
            InitializeComponent();
            // grdUsers.CellEditEnded += grdUsers_CellEditEnded;
            grdUsers.CellEditEnded += new System.EventHandler<Telerik.Windows.Controls.GridViewCellEditEndedEventArgs>(grdUsers_CellEditEnded);
            grdUsers.RowLoaded += new System.EventHandler<Telerik.Windows.Controls.GridView.RowLoadedEventArgs>(grdUsers_RowLoaded);
        }

        void grdUsers_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            var user = e.DataElement as UserRowViewModel;
            if (user == null)
                return;

            if (user.Inactive)
                e.Row.Foreground = new SolidColorBrush(Colors.DarkGray);
        }

        void grdUsers_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            var user = e.Cell.DataContext as UserRowViewModel;

            user.SubmitChanges();
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void grdUsers_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            ((MainViewModel)DataContext).SaveCommand.RaiseCanExecuteChanged();
            ((MainViewModel)DataContext).CancelCommand.RaiseCanExecuteChanged();
        }
    }
}