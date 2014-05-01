using Trex.SmartClient.Core.Implemented;

namespace Trex.SmartClient.Project.CommonViewModels
{
    public class CompanyViewModel : ViewModelBase
    {
        private ObservableCollectionExtended<ProjectViewModel> _projects = new ObservableCollectionExtended<ProjectViewModel>();

        public string Name { get; set; }
        public int Id { get; set; }
        public bool Inactive { get; set; }

        public ObservableCollectionExtended<ProjectViewModel> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                OnPropertyChanged(() => Projects);
            }
        }

        public static CompanyViewModel Create(string name, int id, bool inactive)
        {
            return new CompanyViewModel {Id = id, Name = name, Inactive = inactive}; 
        }
    }
}