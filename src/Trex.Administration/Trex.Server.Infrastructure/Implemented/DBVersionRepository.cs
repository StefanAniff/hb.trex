using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class DBVersionRepository : RepositoryBase, IVersionRepository
    {
        #region IVersionRepository Members

        /// <summary>
        /// Gets the current DB version.
        /// </summary>
        /// <returns></returns>
        public DBVersion GetCurrentVersion()
        {
            var session = GetSession();
            var version = from versions in session.Linq<DBVersion>()
                          select versions;

            //Hack, because the linq fails
            IList<DBVersion> allVersions = version.ToList();
            allVersions.OrderBy(sort => sort.CreateDate);

            if (version.Count() == 0)
            {
                throw new VersionException("No version entries found in database. Database must have a valid version");
            }

            return allVersions.Last();
        }

        #endregion
    }
}