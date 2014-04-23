using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Trex.Dialog.SelectTask.Viewmodels.Itemviewmodel;
using Trex.SmartClient.Core.Model;

namespace Trex.Dialog.SelectTask.Interfaces
{
    public interface ISelectTaskViewModel
    {
        DelegateCommand<object> CreateNewCommand { get; set; }
        DelegateCommand<object> SwitchModeCommand { get; set; }
        DelegateCommand<object> SelectCommand { get; set; }
        DelegateCommand<object> CancelCommand { get; set; }
        DelegateCommand<string> SearchTasksCommand { get; set; }
        bool CanCreateTask { get; }
        string SearchString { get; set; }
        string ProjectSearchString { get; set; }
        ObservableCollection<TaskListViewModelbase> FoundTasks { get; set; }
        ObservableCollection<Project> FoundProjects { get; set; }
        TaskListViewModelbase SelectedTask { get; set; }
        Project SelectedProject { get; set; }
        bool IsInNewTaskMode { get; set; }
        string ModeSwitchText { get; }
        bool IsInExistingTaskMode { get; }
        DelegateCommand<object> SearchServerCommand { get; set; }
        bool IsSearching { get; set; }
        bool IsBillable { get; set; }
        IEnumerable<TaskListTreeCompany> FoundTasksTree { get; }
        bool ShowContinueSearchOnServer { get; }
        bool ShowTreeviewSelector { get; }
        bool ShowFavorites { get; set; }
    }
}