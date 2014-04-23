using System;
using System.Collections.Generic;
using System.Linq;
using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.ServiceContracts;

namespace Trex.Infrastructure.Implemented
{
    public class ProjectSearchService : IProjectSearchService
    {
        private readonly IDataService _dataService;

        public ProjectSearchService(IDataService dataService)
        {
            _dataService = dataService;
        }

        #region IProjectSearchService Members

        public void Search(string searchString)
        {
            _dataService.SearchProjects(searchString).Subscribe(
                projects =>
                    {
                        Result = projects.Select(p => p as IEntity);
                        SearchCompleted(this, null);
                    }
                );
        }

        public event EventHandler SearchCompleted;

        public IEnumerable<IEntity> Result { get; private set; }

        #endregion

     
    }
}