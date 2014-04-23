using System.Windows.Input;
using Trex.Core.Interfaces;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.TaskManagementScreen.FilterView
{
    public partial class FilterView : IView
    {
        public FilterView()
        {
            InitializeComponent();
            ConsultantSearchBox.KeyUp += ConsultantSearchBox_KeyUp;
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
        #endregion

        private void ConsultantSearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                InternalCommands.UserSelected.Execute(null);
                ConsultantSearchBox.Text = string.Empty;
            }
        }
    }
}