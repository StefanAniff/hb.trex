using System.Windows.Controls;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Trex.Core.Interfaces;
using TrexSL.Shell.Menu.MenuModel;

namespace TrexSL.Shell.Menu.MenuView
{
    public partial class MenuView : UserControl, IView
    {
        public MenuView()
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

        private void ItemClicked(object sender, RadRoutedEventArgs e)
        {
            var item = e.OriginalSource as RadMenuItem;
            if (item != null)
            {
                var menuItem = item.DataContext as MenuItem;
                menuItem.ItemClicked.Execute(null);
            }
        }
    }
}