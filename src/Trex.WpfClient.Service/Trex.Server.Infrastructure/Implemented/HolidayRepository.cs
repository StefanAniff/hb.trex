using System;
using System.Collections.Generic;
using NHibernate;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class HolidayRepository : GenericRepository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(ISession openSession) : base(openSession)
        {
        }

        public HolidayRepository(IActiveSessionManager activeSessionManager) : base(activeSessionManager)
        {
        }


        public IEnumerable<Holiday> GetByMonth(int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = DateSpan.EndOfMonth(startDate);

            return Session
                .QueryOver<Holiday>()
                .Where(x => x.Date >= startDate)
                .And(x => x.Date <= endDate)
                .List();
        }
    }
}