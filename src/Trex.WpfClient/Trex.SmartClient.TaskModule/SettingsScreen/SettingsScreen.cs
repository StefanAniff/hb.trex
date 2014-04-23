using System;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Attributes;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.TaskModule.Interfaces;

namespace Trex.SmartClient.TaskModule.SettingsScreen
{
    [Screen(IsStartScreen = false, CanBeDeactivated = false)]
    public class SettingsScreen : ScreenBase, IDialogScreen
    {
        private readonly IUnityContainer _unityContainer;
        private SettingsView.SettingsView _settingsView;


        public SettingsScreen(Guid guid, IUnityContainer unityContainer)
            : base(guid)
        {
            _unityContainer = unityContainer;
            ApplicationCommands.SettingsSaved.RegisterCommand(new DelegateCommand<object>(SettingsSaveExecute));
        }

        private void SettingsSaveExecute(object obj)
        {
            if (_settingsView != null && _settingsView.IsVisible)
            {
                (_settingsView.DataContext as IViewModel).Dispose();
                _settingsView.Close();
            }
        }

        public void Open()
        {
            if (_settingsView != null && _settingsView.IsVisible)
            {
                return;
            }

            _settingsView = new SettingsView.SettingsView();

            MasterView = _settingsView;
            var viewModel = _unityContainer.Resolve<ISettingsViewModel>();
            MasterView.ApplyViewModel(viewModel);
            _settingsView.Owner = Application.Current.MainWindow;
            _settingsView.ShowDialog();
        }
    }
}
