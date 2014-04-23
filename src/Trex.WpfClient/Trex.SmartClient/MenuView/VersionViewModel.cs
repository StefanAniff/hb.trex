using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.MenuView
{
    public class VersionViewModel : ViewModelBase, IVersionViewModel
    {
        public DelegateCommand<object> GoToNext { get; set; }
        public DelegateCommand<object> GoToPrevious { get; set; }

        public List<IRelease> Releases { get; set; }

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged("SelectedRelease");
                OnPropertyChanged("GoToNextCanExecute");
                OnPropertyChanged("GoToPreviousCanExecute");
                GoToNext.RaiseCanExecuteChanged();
                GoToPrevious.RaiseCanExecuteChanged();
            }
        }

        public VersionViewModel(IRelease[] releases)
        {
            Releases = releases.OrderBy(x => x.VersionSize).ToList();

            GoToNext = new DelegateCommand<object>(GoToNextExecute, o => GoToNextCanExecute);
            GoToPrevious = new DelegateCommand<object>(GoToPreviousExecute, o => GoToPreviousCanExecute);

            Index = Releases.Count() - 1;
        }

        public bool GoToNextCanExecute
        {
            get { return Index < Releases.Count() - 1; }
        }

        public bool GoToPreviousCanExecute
        {
            get { return Index > 0; }
        }


        public IRelease SelectedRelease
        {
            get { return Releases[Index]; }
        }


        private void GoToPreviousExecute(object obj)
        {
            Index -= 1;
        }

        private void GoToNextExecute(object obj)
        {
            Index += 1;
        }
    }
}
