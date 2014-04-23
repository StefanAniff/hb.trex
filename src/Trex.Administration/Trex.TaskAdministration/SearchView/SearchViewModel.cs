using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using System;

namespace Trex.TaskAdministration.SearchView
{
    public class SearchViewModel : ViewModelBase
    {
        private const int TASK_SEARCH = 0;
        private const int PROJECT_SEARCH = 1;
        private readonly IDataService _dataService;


        private ObservableCollection<ProjectSearchResultViewModel> _projectResults;

        private string _searchText;
        private ProjectSearchResultViewModel _selectedProject;
        private int _selectedSearchMode;
        private TaskSearchResultViewModel _selectedTask;

        private ObservableCollection<TaskSearchResultViewModel> _taskResults;

        public SearchViewModel(IDataService dataService)
        {
            _dataService = dataService;

            SearchComplete = new DelegateCommand<object>(ExecuteSearchComplete, CanExecuteSearchComplete);
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                if (!string.IsNullOrEmpty(_searchText))
                {
                    if (SelectedSearchMode == TASK_SEARCH)
                        _dataService.SearchTasks(_searchText).Subscribe(TaskSearchCompleted);

                    else if (SelectedSearchMode == PROJECT_SEARCH)
                        _dataService.SearchProjects(_searchText).Subscribe(ProjectSearchCompleted);

                }

                else
                {
                    TaskResults = null;
                    ProjectResults = null;
                }
            }
        }

        public ObservableCollection<TaskSearchResultViewModel> TaskResults
        {
            get { return _taskResults; }
            set
            {
                _taskResults = value;
                OnPropertyChanged("TaskResults");
                OnPropertyChanged("HasTaskResults");
            }
        }

        public ObservableCollection<ProjectSearchResultViewModel> ProjectResults
        {
            get { return _projectResults; }
            set
            {
                _projectResults = value;
                OnPropertyChanged("ProjectResults");
                OnPropertyChanged("HasProjectResults");
            }
        }

        public TaskSearchResultViewModel SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                if (value != null)
                {
                    //InternalCommands.TaskSearchCompleted.Execute(_selectedTask.Task);
                    //TaskResults = null;
                    //ProjectResults = null;
                }

                OnPropertyChanged("SelectedTask");
            }
        }

        public ProjectSearchResultViewModel SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged("SelectedProject");
            }
        }

        public bool HasProjectResults
        {
            get { return (ProjectResults != null && ProjectResults.Count > 0); }
        }

        public bool HasTaskResults
        {
            get { return (TaskResults != null && TaskResults.Count > 0); }
        }

        public int SelectedSearchMode
        {
            get { return _selectedSearchMode; }
            set
            {
                _selectedSearchMode = value;
                OnPropertyChanged("SelectedSearchMode");
            }
        }

        public DelegateCommand<object> SearchComplete { get; set; }

        private void ProjectSearchCompleted(IEnumerable<Project> projects)
        {
            ProjectResults = null;
            var results = new ObservableCollection<ProjectSearchResultViewModel>();
            if (projects != null)
            {
                foreach (var project in projects)
                {
                    results.Add(new ProjectSearchResultViewModel(project));
                }
            }

            ProjectResults = results;
        }

        private bool CanExecuteSearchComplete(object arg)
        {
            if (SelectedSearchMode == TASK_SEARCH)
            {
                return _selectedTask != null;
            }

            return _selectedProject != null;
        }

        private void ExecuteSearchComplete(object obj)
        {
            IEntity selectedEntity = null;
            if (SelectedSearchMode == TASK_SEARCH && _selectedTask != null)
                selectedEntity = _selectedTask.Task;
            else if (_selectedProject != null)
                selectedEntity = _selectedProject.Project;

            TaskResults = null;
            ProjectResults = null;
            InternalCommands.TaskSearchCompleted.Execute(selectedEntity);
        }

        private void TaskSearchCompleted(IEnumerable<Task> result)
        {
            TaskResults = null;
            var results = new ObservableCollection<TaskSearchResultViewModel>();
            if (result != null)
            {
                foreach (var task in result)
                {
                    results.Add(new TaskSearchResultViewModel(task));
                }
            }

            TaskResults = results;
        }
    }
}