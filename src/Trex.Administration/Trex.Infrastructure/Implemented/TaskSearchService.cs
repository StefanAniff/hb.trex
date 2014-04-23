using System;
using System.Collections.Generic;
using System.Linq;
using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.ServiceContracts;

namespace Trex.Infrastructure.Implemented
{
    public class TaskSearchService : ITaskSearchService
    {
        private readonly IDataService _dataService;
        private IEnumerable<IEntity> _result;

        public TaskSearchService(IDataService dataService)
        {
            _dataService = dataService;
        }

        #region ITaskSearchService Members

        public event EventHandler SearchCompleted;

        public IEnumerable<IEntity> Result
        {
            get { return _result; }
        }

        public void Search(string searchString)
        {
            _dataService.SearchTasks(searchString, TaskSearchCompleted);
        }

        #endregion

        private void TaskSearchCompleted(IEnumerable<Task> obj)
        {
            _result = obj.Select(t => t as IEntity);
            SearchCompleted(this, null);
        }
    }
}