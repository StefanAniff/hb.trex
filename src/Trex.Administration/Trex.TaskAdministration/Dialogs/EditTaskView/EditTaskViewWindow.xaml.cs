using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.Dialogs.EditTaskView
{
    public partial class EditTaskViewWindow : ChildWindow, IView
    {
        

        private DelegateCommand<object> _taskEditCompleted;

        public EditTaskViewWindow()
        {
            InitializeComponent();
            this.GotFocus += new RoutedEventHandler(EditTaskViewWindow_GotFocus);
            _taskEditCompleted = new DelegateCommand<object>(TaskEditCompleted);

            InternalCommands.TaskEditCompleted.RegisterCommand(_taskEditCompleted);
            InternalCommands.TaskAddCompleted.RegisterCommand(_taskEditCompleted);
        }

        void EditTaskViewWindow_GotFocus(object sender, RoutedEventArgs e)
        {
            this.GotFocus -= EditTaskViewWindow_GotFocus;
            taskName.Focus();
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void TaskEditCompleted(object obj)
        {
            InternalCommands.TaskEditCompleted.UnregisterCommand(_taskEditCompleted);
            InternalCommands.TaskAddCompleted.UnregisterCommand(_taskEditCompleted);
            Close();
        }

        private void textBoxGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)e.OriginalSource).SelectAll();
        }
    }
}