using System.Collections.ObjectModel;
using Trex.TaskAdministration.TaskManagementScreen.TaskTreeView;

namespace Trex.TaskAdministration.Interfaces
{
    public interface ITreeViewItemViewModel
    {
        bool IsLoadOnDemandEnabled { get; set; }
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }
        string DisplayName { get; }
        ITreeViewItemViewModel Parent { get; set; }
        ObservableCollection<TreeViewItemViewModel> Children { get; }
    }
}