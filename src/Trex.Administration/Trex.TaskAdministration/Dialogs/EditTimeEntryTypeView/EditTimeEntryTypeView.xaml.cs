using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.Dialogs.EditTimeEntryTypeView
{
    public partial class EditTimeEntryTypeView : ChildWindow, IView
    {
        
        private readonly DelegateCommand<object> _editTimeEntryTypeCompleted;

        public EditTimeEntryTypeView()
        {
            _editTimeEntryTypeCompleted = new DelegateCommand<object>(EditTimeEntryTypeCompleted);

            InitializeComponent();
            InternalCommands.CreateTimeEntryTypeCompleted.RegisterCommand(_editTimeEntryTypeCompleted);
            InternalCommands.EditTimeEntryTypeCompleted.RegisterCommand(_editTimeEntryTypeCompleted);
        }

        #region IView Members


        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void EditTimeEntryTypeCompleted(object obj)
        {
          
            InternalCommands.CreateTimeEntryTypeCompleted.UnregisterCommand(_editTimeEntryTypeCompleted);
            InternalCommands.EditTimeEntryTypeCompleted.UnregisterCommand(_editTimeEntryTypeCompleted);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}