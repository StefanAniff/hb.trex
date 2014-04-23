using System.Windows.Controls;
using Trex.Core.Interfaces;

namespace Trex.Core.Implemented
{
    public class ViewBase : UserControl, IView
    {
        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion
    }
}