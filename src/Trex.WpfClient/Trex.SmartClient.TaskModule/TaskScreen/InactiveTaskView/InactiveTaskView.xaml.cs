using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.TaskModule.TaskScreen.InactiveTaskView
{
    public partial class InactiveTaskView : UserControl, IView
    {
        public InactiveTaskView()
        {
            InitializeComponent();
            MouseLeftButtonUp += InactiveTaskView_MouseLeftButtonUp;
        }

        private void InactiveTaskView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ActivateButton.Command.Execute(null);
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

 
    }
}