using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trex.Dialog;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.TaskModule.Dialogs
{
    /// <summary>
    /// Interaction logic for SaveTaskDialog.xaml
    /// </summary>
    public partial class SaveTaskDialog : UserControl, IView
    {
        public SaveTaskDialog()
        {
            InitializeComponent();
          
            newTaskTextBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(newTaskTextBox_GotKeyboardFocus);
            taskNameTextBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(taskNameTextBox_GotKeyboardFocus);
        }

        void taskNameTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //((SaveTaskDialogViewModel)DataContext).IsInExistingTaskMode = true;
        }

        void newTaskTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //((SaveTaskDialogViewModel) DataContext).IsInNewTaskMode = true;
        }

      

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}
