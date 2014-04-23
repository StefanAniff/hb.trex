using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Infrastructure.Commands;
using ApplicationCommands = Trex.SmartClient.Infrastructure.Commands.ApplicationCommands;

namespace Trex.SmartClient.TaskModule.TaskScreen.HistoryFeedView
{
    /// <summary>
    /// Interaction logic for HistoryFeedView.xaml
    /// </summary>
    public partial class HistoryFeedView : UserControl, IView
    {
        public HistoryFeedView()
        {
            InitializeComponent();
            tabGrid.MouseLeftButtonUp += tabGridClicked;
            Windows8Palette.Palette.BasicColor = Colors.DarkGray;
            Windows8Palette.Palette.AccentColor = Colors.DarkGray;

        }

        private static void tabGridClicked(object sender, MouseButtonEventArgs e)
        {
            ApplicationCommands.ToggleHistory.Execute(null);
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private void ListItemClicked(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as Border;
            if (source != null)
            {
                var timeEntry = source.Tag as TimeEntry;
                if (timeEntry != null)
                {
                    TaskCommands.EditTaskStart.Execute(timeEntry);
                }
            }
        }

        private void CtrlCCopyCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var lb = (ListBox)sender;
            if (lb != null)
            {
                var selected = lb.SelectedItem;
                if (selected != null)
                {
                    Clipboard.SetText(selected.ToString());
                }
            }
        }

        private void CtrlCCopyCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            menuItem.Command.Execute(menuItem.CommandParameter);
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            menuItem.Command.Execute(menuItem.CommandParameter);
        }

       
    }
}
