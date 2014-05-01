using System.Collections.ObjectModel;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Project.CommonViewModels;

namespace Trex.SmartClient.Project.ProjectAdministration
{
    public interface IProjectAdministrationViewModel : IViewModelInitializable
    {
        CompanyViewModel SelectedCompany { get; set; }
        ObservableCollection<CompanyViewModel> AvailableCompanies { get; set; }
        bool IsBusy { get; set; }
        ProjectViewModel SelectedProject { get; set; }
    }
}