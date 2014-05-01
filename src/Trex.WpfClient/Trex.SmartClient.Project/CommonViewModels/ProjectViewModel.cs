using System.Collections.Generic;
using Trex.SmartClient.Core.Implemented;

namespace Trex.SmartClient.Project.CommonViewModels
{
    public class ProjectViewModel : ViewModelDirtyHandlingBase
    {
        private string _name;
        private int _id;
        private bool _inactive;
        private bool _isFixedPrice;
        private bool _isInternal;
        private UserViewModel _manager;
        private List<UserViewModel> _availableProjectManagers;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(() => Id);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }

        public bool Inactive
        {
            get { return _inactive; }
            set
            {
                _inactive = value;
                OnPropertyChanged(() => Inactive);
            }
        }

        public bool IsFixedPrice
        {
            get { return _isFixedPrice; }
            set
            {
                _isFixedPrice = value;
                OnPropertyChanged(() => IsFixedPrice);
            }
        }

        public bool IsInternal
        {
            get { return _isInternal; }
            set
            {
                _isInternal = value;
                OnPropertyChanged(() => IsInternal);
            }
        }

        public UserViewModel Manager
        {
            get { return _manager; }
            set
            {
                _manager = value;
                OnPropertyChanged(() => Manager);
            }
        }

        public List<UserViewModel> AvailableProjectManagers
        {
            get { return _availableProjectManagers; }
            set
            {
                _availableProjectManagers = value;
                OnPropertyChanged(() => AvailableProjectManagers);
            }
        }
    }    
}