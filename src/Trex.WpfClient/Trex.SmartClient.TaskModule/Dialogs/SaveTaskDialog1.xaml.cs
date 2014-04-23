using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.TaskModule.Dialogs.Viewmodels;

namespace Trex.SmartClient.TaskModule.Dialogs
{
    public partial class SaveTaskDialog1 : UserControl, IView
    {
        public SaveTaskDialog1()
        {
            InitializeComponent();
            TaskCommands.TaskAssigned.RegisterCommand(new DelegateCommand<object>(TaskAssigned));
            KeyUp += SaveTaskDialog1_KeyUp;
            KeyDown += SaveTaskDialog1_KeyDown;
            Windows8Palette.Palette.BasicColor = Colors.DarkGray;
            Windows8Palette.Palette.AccentColor = Colors.DarkGray;
            Loaded += (sender, args) =>
                {
                    if (!taskSelector.IsExpanded)
                    {
                        FocusOnDescriptionBox();
                    }
                };

        }

        private void SaveTaskDialog1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                TaskCommands.SaveTaskCancelled.Execute(null);
            }
        }

        private void SaveTaskDialog1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SaveButton.Focus();
                var viewModel = (SaveTaskDialogViewModel) DataContext;
                if (viewModel.SaveTask.CanExecute(null))
                    viewModel.SaveTask.Execute(null);
            }
        }

        private void TaskAssigned(object obj)
        {
            FocusOnDescriptionBox();
        }

        private void FocusOnDescriptionBox()
        {
            TxtDescription.Focus();
            TxtDescription.Select(0, 0);
        }


        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
            if (!((SaveTaskDialogViewModel) DataContext).IsInSelectionMode)
                TxtDescription.Focus();

        }

        private void RadSplitButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            var button = sender as RadSplitButton;
            button.IsOpen = true;
        }
    }
}