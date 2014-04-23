using System;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.Dialogs.EditProjectView
{
    public partial class EditProjectViewWindow : ChildWindow, IView
    {
        private bool _isClosing = false;
        private DelegateCommand<object> _editCompleted;
        private DelegateCommand<object> _addCompleted;


        public EditProjectViewWindow()
        {
            _editCompleted = new DelegateCommand<object>(EditCompleted);
            _addCompleted = new DelegateCommand<object>(EditCompleted);
            _editCompleted.IsActive = true;
            InitializeComponent();
            InternalCommands.ProjectEditCompleted.RegisterCommand(_editCompleted);
            InternalCommands.ProjectAddCompleted.RegisterCommand(_addCompleted);
        }

        private void EditCompleted(object obj)
        {
            //if (!_isClosing)
            //{

            InternalCommands.ProjectEditCompleted.UnregisterCommand(_editCompleted);
            InternalCommands.ProjectAddCompleted.UnregisterCommand(_editCompleted);
            Close();
            _isClosing = true;
            //}
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        //private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (!System.Text.RegularExpressions.Regex.IsMatch(e.Key.ToString(), "\\d+"))
        //        e.Handled = true;
        //}
    }
}