using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Task = Trex.SmartClient.Core.Model.Task;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class TaskSearchService : ITaskSearchService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        public TaskSearchService(IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
        }

        public List<Task> GetAll()
        {
            return _taskRepository.GetAll();
        }

        public Task<List<Task>> SearchTasks(string searchString)
        {
            var tsc = new TaskCompletionSource<List<Task>>();
            System.Threading.Tasks.Task.Run(() =>
                {
                    var allTasks = _taskRepository.GetAll();

                    var searchTasks = allTasks.Select(task => new
                        {
                            Task = task,
                            SearchString = string.Concat(task.Name, " ", task.Project.Name, " ", task.Project.Company.Name).ToLower()
                        });

                    var searchArray = searchString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToList();
                    searchArray = searchArray.Select(x => x.ToLower()).ToList();
                    var result = new List<Task>();
                    var query = searchTasks.Select(t => t);
                    searchArray.ForEach(s => query = query.Where(st => st.SearchString.Contains(s)));
                    result.AddRange(query.Select(t => t.Task));

                    tsc.SetResult(result);
                });
            return tsc.Task;
        }

        public List<Project> SearchProjects(string searchString)
        {
            var allProjects = _projectRepository.GetAll();

            var searchProjects =
                allProjects.Select(project => new
                    {
                        Project = project,
                        SearchString = string.Concat(project.Name, " ", project.Company.Name)
                    });



            var searchArray = searchString.Split(" ".ToCharArray());
            searchArray = searchArray.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            var result = new List<Project>();
            var query = searchProjects.Select(t => t);
            searchArray.ToList().ForEach(s => query = query.Where(st => st.SearchString.ToLower().Contains(s.ToLower())));
            result.AddRange(query.ToList().Select(t => t.Project));

            return result;
        }
    }
}