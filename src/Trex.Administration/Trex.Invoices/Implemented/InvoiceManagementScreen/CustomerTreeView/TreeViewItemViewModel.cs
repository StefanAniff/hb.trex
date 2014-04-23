using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.ServiceContracts;

namespace Trex.Invoices.InvoiceManagementScreen.CustomerTreeView
{
    public class TreeViewItemViewModel : ViewModelBase
    {

        private bool _isSelected;

        protected TreeViewItemViewModel(IEntity entity)
        {
            Entity = entity;
        }

        public virtual string DisplayName
        {
            get;
            set;
        }

        public virtual bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public IEntity Entity
        {
            get;
            set;
        }


    }
}
