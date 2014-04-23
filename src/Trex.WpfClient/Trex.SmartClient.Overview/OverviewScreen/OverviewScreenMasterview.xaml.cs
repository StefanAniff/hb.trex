using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Utils;
using Trex.SmartClient.Infrastructure.Commands;
using ApplicationCommands = Trex.SmartClient.Infrastructure.Commands.ApplicationCommands;

namespace Trex.SmartClient.Overview.OverviewScreen
{
    public interface IOverviewScreenMasterview : IView
    {

    }

    public partial class OverviewScreenMasterview : IOverviewScreenMasterview
    {
        public OverviewScreenMasterview()
        {
            InitializeComponent();
            ApplicationCommands.ExitApplication.RegisterCommand(new DelegateCommand<object>(ApplicationShutdown));
            ApplicationCommands.ConnectivityChanged.RegisterCommand(new DelegateCommand<object>(ConnectivityChangedExecute));
            KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && Keyboard.Modifiers == ModifierKeys.Control)
            {
                OverviewCommands.GoToPreviousView.Execute(null);
            }
        }

        private void ConnectivityChangedExecute(object isOnline)
        {
            //Execute.InUIThread(() =>
            //    {
            //        IsEnabled = (bool)isOnline;
            //    });
        }


        private void ApplicationShutdown(object obj)
        {
            if (DataContext != null)
            {
                var vm = DataContext as IViewModel;
                if (vm != null)
                {
                    vm.Dispose();
                }
            }
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}
