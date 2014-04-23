using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Commands;
using ApplicationCommands = Trex.SmartClient.Infrastructure.Commands.ApplicationCommands;

namespace Trex.SmartClient.TaskModule.TaskScreen.TaskScreenMasterView
{
    /// <summary>
    /// Interaction logic for TaskScreenMasterView.xaml
    /// </summary>
    public partial class TaskScreenMasterView : UserControl, IView
    {
        private Point mousePosition;
        private const double TabHistoryMinimumHeight = 33;

        public TaskScreenMasterView()
        {
            InitializeComponent();
            ApplicationCommands.ToggleHistory.RegisterCommand(new DelegateCommand<object>(ToggleHistory));
            KeyUp += TaskScreenMasterView_KeyUp;

            ApplicationCommands.InActiveTaskLayoutChanged.RegisterCommand(new DelegateCommand(UpdateActiveTaskUI));

            Loaded += (sender, args) => UpdateActiveTaskUI();
        }

        private void UpdateActiveTaskUI()
        {
            //set active task window to middle of screen
            var isDefault = Canvas.GetLeft(ActiveTask) == -1 && Canvas.GetTop(ActiveTask) == -1;
            if (isDefault)
            {
                Canvas.SetTop(ActiveTask, (ActualHeight / 2) - (ActiveTask.ActualHeight / 2));
                Canvas.SetLeft(ActiveTask, (ActualWidth / 2) - (ActiveTask.ActualWidth / 2));
            }
        }

        private double _lastHeight = 233;
        private void ToggleHistory(object obj)
        {
            if (rowDef.ActualHeight > TabHistoryMinimumHeight) // collapse the history feed
            {
                _lastHeight = rowDef.ActualHeight;
                rowDef.Height = new GridLength(TabHistoryMinimumHeight, GridUnitType.Pixel);
            }
            else // return it to former height
            {
                rowDef.Height = new GridLength(_lastHeight, GridUnitType.Pixel);
            }
        }

        void TaskScreenMasterView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (TaskCommands.CloseActiveTask.CanExecute(null))
                {
                    TaskCommands.CloseActiveTask.Execute(null);
                }
            }

        }


        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }


        private void Img_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ActiveTask.Height += e.Delta;
            ActiveTask.Width += e.Delta;
        }

        private void Img_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPos = e.GetPosition(MasterScreen);

            bool canSetLeft = true;
            bool canSetTop = true;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var modifiedX = (currentPos.X - mousePosition.X);
                var modifiedY = (currentPos.Y - mousePosition.Y);

                var currentLeft = Canvas.GetLeft(ActiveTask);
                var currentTop = Canvas.GetTop(ActiveTask);
                if (currentPos.Y < 0)
                {
                    Canvas.SetTop(ActiveTask, 0);
                    canSetTop = false;
                }
                else if (mousePosition.Y > ActualHeight - ActiveTask.ActualHeight)
                {
                    Canvas.SetTop(ActiveTask, ActualHeight - ActiveTask.ActualHeight);
                    canSetTop = false;
                }
                else if (currentPos.X < 0 || currentLeft < 0 || mousePosition.X < 0)
                {
                    Canvas.SetLeft(ActiveTask, 0);
                    canSetLeft = false;
                }
                else if (currentLeft > ActualWidth - ActiveTask.ActualWidth)
                {
                    canSetLeft = currentPos.X < currentLeft;
                }
                if (canSetTop)
                {
                    Canvas.SetTop(ActiveTask, currentTop + modifiedY);
                }
                if (canSetLeft)
                {
                    Canvas.SetLeft(ActiveTask, currentLeft + modifiedX);
                }
            }
            mousePosition = currentPos;
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ActiveTask.CaptureMouse();
            mousePosition = e.GetPosition(MasterScreen);
        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ActiveTask.ReleaseMouseCapture();
        }
    }
}
