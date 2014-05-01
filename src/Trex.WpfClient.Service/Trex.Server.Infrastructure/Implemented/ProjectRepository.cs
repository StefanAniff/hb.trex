using System;
using System.Collections.Generic;
using Trex.Server.Core.Model;
using NHibernate;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ISession openSession)
            : base(openSession)
        {
        }

        public ProjectRepository(IActiveSessionManager activeSessionManager)
            : base(activeSessionManager)
        {
        }


        /// <summary>
        /// Gets projects changed after a given date
        /// </summary>
        public IEnumerable<Project> GetByChangeDate(DateTime startDate)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();

            User pUser = null;
            Company company = null;
            var query = session.QueryOver<Project>()
                               .Left.JoinAlias(p => p.CreatedBy, () => pUser)
                               .Left.JoinAlias(p => p.Company, () => company)
                               .Where(proj => proj.CreateDate >= startDate
                                              || (proj.ChangeDate != null
                                                  && proj.ChangeDate >= startDate));
            var project = query.And(p => !p.Inactive
                                         && !company.Inactive);

            return project.List();
        }

        public IEnumerable<Project> GetByCustomerId(int customerId)
        {
            Company companyAlias = null;

            var result = Session
                .QueryOver<Project>()
                .JoinAlias(x => x.Company, () => companyAlias)
                .Where(x => companyAlias.CustomerID == customerId);

            return result.List();
        }
    }
}