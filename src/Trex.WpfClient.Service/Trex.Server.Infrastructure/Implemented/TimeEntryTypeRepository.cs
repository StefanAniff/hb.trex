using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TimeEntryTypeRepository : GenericRepository<TimeEntryType>, ITimeEntryTypeRepository
    {
          public TimeEntryTypeRepository(ISession openSession)
            : base(openSession)
        {
        }

          public TimeEntryTypeRepository(IActiveSessionManager activeSessionManager)
            : base(activeSessionManager)
        {
        }
    }
}
