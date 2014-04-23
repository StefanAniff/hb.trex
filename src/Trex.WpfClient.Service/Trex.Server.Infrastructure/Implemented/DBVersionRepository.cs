using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using Trex.Server.Core.Model;
using Trex.Server.Infrastructure.Implemented;
using Trex.Server.Core.Exceptions;
using System.Collections;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class DBVersionRepository : GenericRepository<DBVersion>, IVersionRepository
    {
        public DBVersionRepository(ISession openSession)
            : base(openSession)
        {
        }

        public DBVersionRepository(IActiveSessionManager activeSessionManager)
            : base(activeSessionManager)
        {
        }

        /// <summary>
        /// Gets the current DB version.
        /// </summary>
        /// <returns></returns>
        public DBVersion GetCurrentVersion()
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();
            var version = session.QueryOver<DBVersion>().Select(versions => versions)
                                 .OrderBy(sort => sort.CreateDate).Asc.List();

            if (!version.Any())
                throw new VersionException("No version entries found in database. Database must have a valid version");

            return version.Last<DBVersion>();
        }
    }
}
