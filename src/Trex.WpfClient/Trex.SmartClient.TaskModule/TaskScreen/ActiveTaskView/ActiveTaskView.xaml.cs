using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Expression.Interactivity.Layout;

namespace Trex.SmartClient.TaskModule.TaskScreen.ActiveTaskView
{
    /// <summary>
    /// Interaction logic for ActiveTaskView.xaml
    /// </summary>
    public partial class ActiveTaskView : UserControl
    {

        private bool _isDetailsOpen;
        public ActiveTaskView()
        {
            
            InitializeComponent();
            //var mouseDragElementBehavior = new MouseDragElementBehavior();
            //mouseDragElementBehavior.Attach(this);
            //this.DataContextChanged += new System.Windows.DependencyPropertyChangedEventHandler(ActiveTaskView_DataContextChanged);
            BtnToggleDetails.Click += btnToggleDetailsClick;
            VisualStateManager.GoToState(this, "Closed", false);
        }

        private void btnToggleDetailsClick(object sender, RoutedEventArgs e)
        {
            if (_isDetailsOpen)
            {
                VisualStateManager.GoToState(this, "Closed", true);
                _isDetailsOpen = false;
            }
            else
            {
                VisualStateManager.GoToState(this, "Open", true);
                _isDetailsOpen = true;
            }
        }



        /// <summary>
        /// Handles the DataContextChanged event of the ActiveTaskView control.
        /// This is a necessary hack, since the tooltip will only bind once. When the datacontext changes, it doesn't update.
        /// This is why it is reinstantiated programmatically
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        void ActiveTaskView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if(DataContext == null)
                return;
            var viewModel = (ActiveTaskViewModel)DataContext;

            var toolTipView = new TaskToolTipView.TaskToolTipView();
            toolTipView.DataContext = viewModel.ToolTipViewModel;
            var toolTip = new ToolTip();
            toolTip.Content = toolTipView;
            this.ToolTip = toolTip;
            toolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            toolTip.Padding = new Thickness(0);
        }

        void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ToolTipViewModel")
            {
                var toolTipView =  new TaskToolTipView.TaskToolTipView();
                toolTipView.DataContext = ((ActiveTaskViewModel)DataContext).ToolTipViewModel;
                var toolTip = new ToolTip();
                toolTip.Content = toolTipView;
                this.ToolTip = toolTip;
                toolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                
                
            } 
        }


        private void TxtTask_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (sender as TextBox);
            if (textBox != null && !textBox.IsReadOnly)
            {
                textBox.Background = Brushes.White;
                textBox.BorderThickness = new Thickness(1);
                textBox.Foreground = Brushes.Black;
                textBox.SelectAll();
            }
        }

     

        private void TxtTask_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (sender as TextBox);
            if (textBox != null)
            {
                textBox.Background = Brushes.Transparent;
                textBox.BorderThickness = new Thickness(0);
                textBox.Foreground = Brushes.White;
            }
        }

        private void TxtTask_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                outerGrid.Focus();
            }
        }
    }
}