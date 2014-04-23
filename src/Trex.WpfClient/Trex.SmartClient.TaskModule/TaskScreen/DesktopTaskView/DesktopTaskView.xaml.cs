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
using System.Windows.Shapes;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Commands;
using ApplicationCommands = Trex.SmartClient.Infrastructure.Commands.ApplicationCommands;

namespace Trex.SmartClient.TaskModule.TaskScreen.DesktopTaskView
{
    /// <summary>
    /// Interaction logic for DesktopTaskView.xaml
    /// </summary>
    public partial class DesktopTaskView : Window, IView
    {
        public DesktopTaskView()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += DesktopTaskView_MouseLeftButtonDown;
            this.MouseDoubleClick += DesktopTaskView_MouseDoubleClick;
            this.Closed += DesktopTaskView_Closed;
            DataContextChanged += DesktopTaskView_DataContextChanged;
        }

        /// <summary>
        /// Handles the DataContextChanged event of the ActiveTaskView control.
        /// This is a necessary hack, since the tooltip will only bind once. When the datacontext changes, it doesn't update.
        /// This is why it is reinstantiated programmatically
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        void DesktopTaskView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext == null)
                return;
            var viewModel = (DesktopTaskViewModel)DataContext;

            var toolTipView = new TaskToolTipView.TaskToolTipView();
            toolTipView.DataContext = viewModel.ToolTipViewModel;
            var toolTip = new ToolTip();
            toolTip.Content = toolTipView;
            this.ToolTip = toolTip;
            toolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            toolTip.Padding = new Thickness(0);
        }

        void DesktopTaskView_Closed(object sender, EventArgs e)
        {
            ApplicationCommands.DeskTopWindowClosed.Execute(null);
        }

        void DesktopTaskView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TaskCommands.ToggleActiveTask.Execute(null);
        }

        void DesktopTaskView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private void ContextMenuClick(object sender, RadRoutedEventArgs e)
        {
            string menuItem = (e.OriginalSource as RadMenuItem).Name as string;
            switch (menuItem)
            {
                case "close":
                    this.Close();
                    break;
                case "toggletask":
                    TaskCommands.ToggleActiveTask.Execute(null);
                    break;
                case "stop":
                    TaskCommands.StopActiveTask.Execute(null);
                    ApplicationCommands.OpenMainWindow.Execute(null);
                    break;
                case "togglewindow":
                    ApplicationCommands.ToggleWindow.Execute(null);
                    break;
                case "assign":
                    TaskCommands.AssignTask.Execute(null);
                    ApplicationCommands.OpenMainWindow.Execute(null);
                    break;

            }
        }
    }
}