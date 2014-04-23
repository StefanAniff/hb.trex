using System.Windows;
using System.Windows.Controls;

namespace Trex.Dialog.SelectTask.Selectors
{
    /// <summary>
    /// Interaction logic for ListBoxSelector.xaml
    /// </summary>
    public partial class ListBoxSelector : UserControl
    {
        public ListBoxSelector()
        {
            InitializeComponent();
            taskListBox.GotFocus += taskListBox_GotFocus;

        }

        void taskListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (taskListBox.Items.Count > 0)
            {
                taskListBox.SelectedIndex = 0;
            }
        }
    }
}
