using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.Dialog.SelectTask.Interfaces;
using Trex.Dialog.SelectTask.Viewmodels.Itemviewmodel;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Implemented;
using log4net;

namespace Trex.Dialog.SelectTask.Viewmodels
{
    public class SelectTaskViewModel : ViewModelBase, ISelectTaskViewModel
    {
        private readonly ITaskSearchService _taskSearchService;
        private readonly IUserSession _userSession;
        private readonly IAppSettings _appSettings;
        private readonly ITaskRepository _taskRepository;
        private bool _isShowingTextbox;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DelegateCommand _getLastTaskFinishedCommand;
        public DelegateCommand<object> CreateNewCommand { get; set; }
        public DelegateCommand<object> SwitchModeCommand { get; set; }
        public DelegateCommand<object> SelectCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }
        public DelegateCommand<string> SearchTasksCommand { get; set; }
        public DelegateCommand<object> SearchServerCommand { get; set; }


        public SelectTaskViewModel(ITaskSearchService taskSearchService, IUserSession userSession, IAppSettings appSettings,
            ITaskRepository taskRepository)
        {
            _taskSearchService = taskSearchService;
            SwitchModeCommand = new DelegateCommand<object>(ExecuteSwitchMode);
            CreateNewCommand = new DelegateCommand<object>(ExecuteCreateNew, CanCreateNew);
            SelectCommand = new DelegateCommand<object>(ExecuteSelectTask, CanSelectTask);
            CancelCommand = new DelegateCommand<object>(ExecuteCancel);
            SearchTasksCommand = new DelegateCommand<string>(SearchTasksCommandExecute);
            SearchServerCommand = new DelegateCommand<object>(SearchServerCommandExecute);
            _getLastTaskFinishedCommand = new DelegateCommand(() => SearchTasksCommandExecute(null));
            ApplicationCommands.GetLatestTasksFinished.RegisterCommand(_getLastTaskFinishedCommand);

            IsInNewTaskMode = false;
            _userSession = userSession;
            _appSettings = appSettings;
            _taskRepository = taskRepository;

            LoadLastTasks();
        }

        private void SearchServerCommandExecute(object obj)
        {
            IsSearching = true;
            ApplicationCommands.GetLatestTasks.Execute(null);
        }

        private async void SearchTasksCommandExecute(object obj)
        {
            try
            {
                if (_searchString == null)
                {
                    return;
                }
                IsSearching = true;

                List<Task> searchResults;
                var getEverything = _searchString == "*" && ShowTreeviewSelector;
                if (getEverything)
                {
                    searchResults = _taskSearchService.GetAll();
                }
                else
                {
                    searchResults = await _taskSearchService.SearchTasks(_searchString);
                }

                ObservableCollection<TaskListViewModelbase> result;
                if (searchResults != null)
                {
                    var tasks = searchResults.Select(searchResult => new TaskListViewModel(searchResult));
                    result = new ObservableCollection<TaskListViewModelbase>(tasks);
                }
                else
                {
                    result = new ObservableCollection<TaskListViewModelbase>();
                }
                var searchOnServerOption = new SearchOnServerTaskViewmodel();
                result.Add(searchOnServerOption);
                FoundTasks = result;
            }
            catch (Exception ex)
            {
                if (!_isShowingTextbox)
                {
                    _isShowingTextbox = true;
                    var result = MessageBox.Show("Search failed", "Error", MessageBoxButton.OK);
                    _isShowingTextbox = result == MessageBoxResult.None;
                }
                Logger.Error(ex);
            }
            finally
            {
                IsSearching = false;
            }
        }

        private bool _isSearching;

        public bool IsSearching
        {
            get { return _isSearching; }
            set
            {
                _isSearching = value;
                OnPropertyChanged(() => IsSearching);
            }
        }

        private bool _showFavorites;

        public bool ShowFavorites
        {
            get { return _showFavorites; }
            set
            {
                _showFavorites = value;
                OnPropertyChanged(() => ShowFavorites);

                if (ShowFavorites)
                {
                    var taskListViewModels = _appSettings.FavoriteTaskGuids.Where(guid => _taskRepository.Exists(guid))
                                                         .Select(taskGuid => new TaskListViewModel(_taskRepository.GetByGuid(taskGuid)));
                    FoundTasks = new ObservableCollection<TaskListViewModelbase>(taskListViewModels);
                }
                else
                {
                    LoadLastTasks();
                }
            }
        }


        public bool IsBillable { get; set; }

        private void ExecuteSwitchMode(object obj)
        {
            IsInNewTaskMode = !IsInNewTaskMode;
        }


        private static void ExecuteCancel(object obj)
        {
            TaskCommands.SaveTaskCancelled.Execute(null);
        }

        private void LoadLastTasks()
        {
            if (_userSession.UserStatistics == null)
            {
                return;
            }
            if (_userSession.UserStatistics.LastXDaysTimeEntries != null)
            {
                var found = _userSession.UserStatistics.LastXDaysTimeEntries
                    .Where(x => !x.Task.Inactive)
                    .OrderByDescending(te => te.StartTime).Select(te => te.Task)
                    .Distinct().Take(10);
                FoundTasks = new ObservableCollection<TaskListViewModelbase>(found.Select(task => new TaskListViewModel(task)));
            }
            else
            {
                FoundTasks = new ObservableCollection<TaskListViewModelbase>();
            }
        }

        private bool CanSelectTask(object arg)
        {
            return (_selectedTask != null);
        }

        private void ExecuteSelectTask(object obj)
        {
            var tasklistViewModel = SelectedTask as TaskListViewModel;
            if (_selectedTask is SearchOnServerTaskViewmodel)
            {
                SelectedTask = null;
                ApplicationCommands.GetLatestTasks.Execute(null);
                SearchTasksCommand.Execute(_searchString);
            }
            else if (tasklistViewModel != null)
            {
                TaskCommands.TaskSelectCompleted.Execute(tasklistViewModel.Task);
            }
        }

        private void ExecuteCreateNew(object obj)
        {
            var task = Task.Create(Guid.NewGuid(), 0, SearchString, string.Empty,
                                   SelectedProject, DateTime.Now, false, string.Empty, false, TimeRegistrationTypeEnum.Standard);
            TaskCommands.TaskSelectCompleted.Execute(task);
        }

        private bool CanCreateNew(object arg)
        {
            return SelectedProject != null && !string.IsNullOrEmpty(SearchString);
        }

        public bool CanCreateTask
        {
            get
            {
                return false;
                return UserContext.Instance.User.HasPermission(Permissions.CreateTaskPermission);
            }
        }

        private string _searchString;

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                SearchTasksCommand.Execute(_searchString);
                CreateNewCommand.RaiseCanExecuteChanged();
            }
        }

        private string _projectSearchString;

        public string ProjectSearchString
        {
            get { return _projectSearchString; }
            set
            {
                _projectSearchString = value;
                FoundProjects = new ObservableCollection<Project>(_taskSearchService.SearchProjects(_projectSearchString));
            }
        }


        private ObservableCollection<Project> _foundProjects;

        public ObservableCollection<Project> FoundProjects
        {
            get { return _foundProjects; }
            set
            {
                _foundProjects = value;
                OnPropertyChanged(() => FoundProjects);
            }
        }

        private TaskListViewModelbase _selectedTask;

        public TaskListViewModelbase SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                OnPropertyChanged(() => SelectedTask);
                SelectCommand.RaiseCanExecuteChanged();
            }
        }

        private Project _selectedProject;

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged(() => SelectedProject);
                CreateNewCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isInNewTaskMode;

        public bool IsInNewTaskMode
        {
            get { return _isInNewTaskMode; }
            set
            {
                _isInNewTaskMode = value;

                OnPropertyChanged(() => IsInNewTaskMode);
                OnPropertyChanged(() => IsInExistingTaskMode);
                OnPropertyChanged(() => ModeSwitchText);
            }
        }

        public string ModeSwitchText
        {
            get { return IsInNewTaskMode ? "Back to search" : "Create new..."; }
        }

        public bool IsInExistingTaskMode
        {
            get { return !IsInNewTaskMode; }
        }

        public bool ShowContinueSearchOnServer
        {
            get { return FoundTasks.OfType<SearchOnServerTaskViewmodel>().Any(); }
        }

        private ObservableCollection<TaskListViewModelbase> _foundTasks;

        public ObservableCollection<TaskListViewModelbase> FoundTasks
        {
            get { return _foundTasks; }
            set
            {
                _foundTasks = value;
                OnPropertyChanged(() => FoundTasks);
                OnPropertyChanged(() => FoundTasksTree);
                OnPropertyChanged(() => ShowContinueSearchOnServer);
            }
        }

        public bool ShowTreeviewSelector
        {
            get { return _appSettings.ShowTreeviewSelector; }
        }


        public IEnumerable<TaskListTreeCompany> FoundTasksTree
        {
            get
            {
                var list = new List<TaskListTreeCompany>();
                var taskListViewModels = FoundTasks.OfType<TaskListViewModel>();
                foreach (var company in taskListViewModels.GroupBy(x => x.CustomerName))
                {
                    var projectTree = company.GroupBy(x => x.ProjectName)
                                          .Select(projects => new TaskListTreeProject
                                              {
                                                  ProjectName = projects.Key,
                                                  Tasks = projects.ToList()
                                              });
                    list.Add(new TaskListTreeCompany
                        {
                            CompanyName = company.Key,
                            Projects = projectTree.ToList()
                        });
                }
                return list;
            }
        }

        protected override void Dispose(bool disposing)
        {
            ApplicationCommands.GetLatestTasksFinished.UnregisterCommand(_getLastTaskFinishedCommand);
            base.Dispose(disposing);
        }
    }
}