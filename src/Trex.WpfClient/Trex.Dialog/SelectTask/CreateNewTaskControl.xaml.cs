using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trex.Dialog.SelectTask.Viewmodels;

namespace Trex.Dialog.SelectTask
{
    public partial class CreateNewTaskControl : UserControl
    {
        public CreateNewTaskControl()
        {
            InitializeComponent();

            TxtSearchProjects.KeyUp += txtSearchProjects_KeyUp;
            ProjectListBox.GotFocus += ProjectListBox_GotFocus;
            ProjectListBox.KeyUp += ProjectListBox_KeyUp;
            IsVisibleChanged += OnIsVisibleChanged;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (Visibility == Visibility.Visible)
            {
                TxtSearchProjects.Focus();
            }
        }


        private void ProjectListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ProjectListBox.Items.Count > 0)
                ProjectListBox.SelectedIndex = 0;
        }

        private void txtSearchProjects_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                ProjectListBox.Focus();
                return;
            }
            var viewModel = (SelectTaskViewModel) DataContext;
            viewModel.ProjectSearchString = TxtSearchProjects.Text;
        }

        private void ProjectListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var viewModel = (SelectTaskViewModel) DataContext;
                if (viewModel.CreateNewCommand.CanExecute(null))
                    viewModel.CreateNewCommand.Execute(null);
            }
        }
    }
}