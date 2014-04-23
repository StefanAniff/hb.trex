using System.Windows.Controls;
using Trex.Core.Interfaces;

namespace Trex.TaskAdministration.TimeEntryTypeScreen.TimeEntryTypeListView
{
    public partial class TimeEntryTypeListView : UserControl, IView
    {
        public TimeEntryTypeListView()
        {
            InitializeComponent();
        }

        #region IView Members


        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion
    }
}