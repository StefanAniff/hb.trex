using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.TaskModule.SettingsScreen.SettingsView
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window, IView
    {
        public SettingsView()
        {
            InitializeComponent();
            KeyDown += SettingsView_OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            ShowDataFolder.Visibility = DeleteDataFileButton.Visibility = AdvancedSettingsEnabled.Visibility = Visibility.Collapsed;
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private void SettingsView_OnKeyDown(object sender, KeyEventArgs e)
        {
            ShowDataFolder.Visibility = DeleteDataFileButton.Visibility = AdvancedSettingsEnabled.Visibility = e.Key == Key.LeftCtrl ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}