using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.Infrastructure.Commands;

namespace TrexSL.Shell
{
    public partial class Shell : UserControl, IView
    {
        private DependencyObject _savedParentElement;

        public Shell()
        {
            InitializeComponent();

            ApplicationCommands.GotoFullScreenMode.RegisterCommand(new DelegateCommand<UserControl>(ExecuteGotoFullScreen));
            Application.Current.Host.Content.FullScreenChanged += Content_FullScreenChanged;
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void Content_FullScreenChanged(object sender, EventArgs e)
        {
            FullScreenGrid.Visibility = Application.Current.Host.Content.IsFullScreen
                                            ? Visibility.Visible
                                            : Visibility.Collapsed;

            if (!Application.Current.Host.Content.IsFullScreen)
            {
                if (_savedParentElement is ContentControl)
                {
                    var fullscreenObj = FullScreenGrid.Children[0];
                    FullScreenGrid.Children.Remove(fullscreenObj);
                    ((ContentControl) _savedParentElement).Content = fullscreenObj;
                }
            }
        }

        private void ExecuteGotoFullScreen(UserControl obj)
        {
            _savedParentElement = obj.Parent;
            if (_savedParentElement is ContentControl)
            {
                ((ContentControl) _savedParentElement).Content = null;
            }

            FullScreenGrid.Children.Add(obj);
        }
    }
}