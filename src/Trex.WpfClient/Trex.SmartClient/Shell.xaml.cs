using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient
{
    /// <summary>
    /// Interaction logic for Shell1.xaml
    /// </summary>
    public partial class Shell : Window
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IAppSettings _appSettings;
        private readonly IUserSession _userSession;

        public Shell(IUnityContainer unityContainer)
        {
            InitializeComponent();
            _unityContainer = unityContainer;
            _appSettings = _unityContainer.Resolve<IAppSettings>();
            _userSession = _unityContainer.Resolve<IUserSession>();
            IApplicationStateService applicationStateService = _unityContainer.Resolve<IApplicationStateService>();
            this.Loaded += new RoutedEventHandler(Shell_Loaded);
            //this.StateChanged += new EventHandler(Shell_StateChanged);
            this.Closing += new System.ComponentModel.CancelEventHandler(Shell_Closing);
            //this.Activated += new EventHandler(Shell_Activated);


            ApplicationCommands.ToggleWindow.RegisterCommand(new DelegateCommand<object>(ToggleWindow));
            ApplicationCommands.OpenMainWindow.RegisterCommand(new DelegateCommand<object>(OpenWindow));


            var toggleState = new DelegateCommand<object>(UpdateButtons);
            UpdateButtons(null);

            applicationStateService.CurrentState.PropertyChanged += (o, e) => UpdateButtons(null);
        }

        private void UpdateButtons(object obj)
        {
            var shellViewModel = DataContext as IShellViewModel;
            if (shellViewModel != null && shellViewModel.IsRunning)
            {
                taskBarItemInfo1.Overlay = (DrawingImage)FindResource("PlayImage");
            }
            else
            {
                taskBarItemInfo1.Overlay = (DrawingImage)FindResource("StopImage");
            }
        }


        void Shell_Activated(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
                OpenWindow(null);
        }

        private void OpenWindow(object obj)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized || !this.IsActive)
            {
                this.Show();
                this.Activate();
                WindowState = WindowState.Normal;
                ShowInTaskbar = true;
            }

        }

        void Shell_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            Application.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
            //this.WindowState = WindowState.Minimized;
            if (_appSettings.HideWhenMinimized)
                ShowInTaskbar = false;


            e.Cancel = true;
        }

        void Shell_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                if (_appSettings.HideWhenMinimized)
                    ShowInTaskbar = false;
            }
            else
                ShowInTaskbar = true;
        }



        private void ToggleWindow(object obj)
        {

            if (!this.IsActive)
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.Activate();
                WindowState = WindowState.Normal;
                ShowInTaskbar = true;
            }

            else
            {

                Application.Current.MainWindow.Hide();
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
                if (_appSettings.HideWhenMinimized)
                    ShowInTaskbar = false;
            }



        }

        void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            var notificationListener = _unityContainer.Resolve<INotificationListener>();
            //Start up a new tray icon
            windowsFormsHost1.Child = new NotifyControl.NotifyControl(notificationListener, _userSession);

            var menuViewModel = _unityContainer.Resolve<IMenuViewModel>();


            MenuView.ApplyViewModel(menuViewModel);



        }



    }
}
