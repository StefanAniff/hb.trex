using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Overview.WeeklyOverviewScreen
{
    /// <summary>
    /// Interaction logic for WeeklyOverviewScreen.xaml
    /// </summary>
    public partial class WeeklyOverviewScreen : UserControl, IView
    {
        public WeeklyOverviewScreen()
        {
            InitializeComponent();
            Loaded += OnLoaded;

            Windows8Palette.Palette.BasicColor = Colors.DarkGray;
            Windows8Palette.Palette.AccentColor = Colors.DarkGray;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            TodayDateButton.Focus();
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }

        private void TimeEntry_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemComma)
            {
                var txtbox = sender as TextBox;
                if (txtbox != null)
                {
                    txtbox.SelectionStart++;
                    e.Handled = true;
                }
            }
        }

        private void RadSplitButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as RadSplitButton;
            button.IsOpen = true;
        }
    }
}
