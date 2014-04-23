using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.Dialogs.EditTimeEntryView
{
    public partial class EditTimeEntryViewWindow : ChildWindow, IView
    {

        private DelegateCommand<object> _editCompleted;

        public EditTimeEntryViewWindow()
        {
            InitializeComponent();
            GotFocus += EditTimeEntryViewWindow_GotFocus;
            _editCompleted = new DelegateCommand<object>(TimeEntryEditCompleted);
            InternalCommands.TimeEntryEditCompleted.RegisterCommand(_editCompleted);
            InternalCommands.TimeEntryAddCompleted.RegisterCommand(_editCompleted);
        }

        void EditTimeEntryViewWindow_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            this.GotFocus -= EditTimeEntryViewWindow_GotFocus;
            txtDate.Focus();
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }


        #endregion

        private void TimeEntryEditCompleted(object obj)
        {
           
            InternalCommands.TimeEntryEditCompleted.UnregisterCommand(_editCompleted);
            InternalCommands.TimeEntryAddCompleted.UnregisterCommand(_editCompleted);

            Close();
        }

        private void ChildWindowKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                if(OKButton.Command.CanExecute(null))
                OKButton.Command.Execute(null);
        }

    
    }
}