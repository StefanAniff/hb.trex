using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.Dialog.SelectTask.Interfaces;
using Trex.Dialog.SelectTask.Viewmodels;
using Trex.Dialog.SelectTask.Viewmodels.Itemviewmodel;
using Trex.SmartClient.Core.Model;

namespace Trex.Dialog.SelectTask.Designdata
{
    public class DesignSelectTaskViewModel : ISelectTaskViewModel
    {
        private Project _selectedProject;
        private string _modeSwitchText;
        private bool _showContinueSearchOnServer;
        private bool _treeviewMode;

        public DelegateCommand<object> SearchServerCommand { get; set; }
        public bool IsSearching { get; set; }

        public DelegateCommand<object> CreateNewCommand { get; set; }

        public DelegateCommand<object> SwitchModeCommand { get; set; }

        public DelegateCommand<object> SelectCommand { get; set; }

        public DelegateCommand<object> CancelCommand { get; set; }

        public DelegateCommand<string> SearchTasksCommand { get; set; }

        public bool CanCreateTask
        {
            get { return true; }
        }

        public string SearchString
        {
            get { return "tes"; }
            set { }
        }

        public string ProjectSearchString { get; set; }

        public bool SyncInProgress { get; set; }

        public bool IsSearchEnabled { get; set; }

        public ObservableCollection<TaskListViewModelbase> FoundTasks
        {
            get
            {
                var task1 = Task.Create(Guid.NewGuid(), 1, "5352 - Fixing this window", "description text", FoundProjects.First(), DateTime.Now, true, "", false, TimeRegistrationTypeEnum.Standard);
                var task2 = Task.Create(Guid.NewGuid(), 1, "5352 - Fixing login issues", "description text", FoundProjects.First(), DateTime.Now, true, "", false, TimeRegistrationTypeEnum.Standard);
                var task3 = Task.Create(Guid.NewGuid(), 1, "5352 - blah balh blah", "description text", FoundProjects.First(), DateTime.Now, true, "", false, TimeRegistrationTypeEnum.Standard);

                return new ObservableCollection<TaskListViewModelbase>(new List<TaskListViewModelbase>
                                                                       {
                                                                           new TaskListViewModel(task1),
                                                                           new TaskListViewModel(task2),
                                                                           new TaskListViewModel(task3),
                                                                           new SearchOnServerTaskViewmodel()
                                                                       });
            }
            set { }
        }

        public ObservableCollection<Project> FoundProjects
        {
            get
            {
                var company = Company.Create("D60", 1, false, false);
                var project1 = Project.Create(1, "DCC", company, false);
                var projects = new List<Project>
                    {
                        project1
                    };
                return new ObservableCollection<Project>(projects);
            }
            set { }
        }

        public TaskListViewModelbase SelectedTask
        {
            get { return FoundTasks.First(); }
            set { }
        }

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set { _selectedProject = value; }
        }

        public bool IsInNewTaskMode
        {
            get { return false; }
            set { }
        }

        public string ModeSwitchText
        {
            get { return "Create new..."; }
        }

        public bool IsInExistingTaskMode
        {
            get { return true; }
        }

        public bool IsBillable { get; set; }

        public IEnumerable<TaskListTreeCompany> FoundTasksTree
        {
            get
            {
                Project project1 = FoundProjects.First();
                var task1 = Task.Create(Guid.NewGuid(), 1, "5352 - Fixing this window", "description text", project1, DateTime.Now, true, "", false, TimeRegistrationTypeEnum.Standard);
                var task2 = Task.Create(Guid.NewGuid(), 1, "5352 - Fixing login issues", "description text", project1, DateTime.Now, true, "", false, TimeRegistrationTypeEnum.Standard);
                var task3 = Task.Create(Guid.NewGuid(), 1, "5352 - blah balh blah", "description text", project1, DateTime.Now, true, "", false, TimeRegistrationTypeEnum.Standard);

                var list = new List<TaskListTreeCompany>();
                var project = new TaskListTreeProject
                {
                    ProjectName = project1.Name,
                    Tasks = new List<TaskListViewModel>
                            {
                                new TaskListViewModel(task1),
                                new TaskListViewModel(task2),
                                new TaskListViewModel(task3)
                            }
                };

                Company company = project1.Company;
                list.Add(new TaskListTreeCompany
                {
                    CompanyName = company.Name,
                    Projects = new List<TaskListTreeProject> { project }
                });
                list.Add(new TaskListTreeCompany
                {
                    CompanyName = company.Name + 2,
                    Projects = new List<TaskListTreeProject> { project }
                });
                return list;
            }
        }

        public bool ShowContinueSearchOnServer
        {
            get { return true; }
        }

        public bool ShowTreeviewSelector
        {
            get { return true; }
        }

        public bool ShowFavorites { get;  set; }
    }
}