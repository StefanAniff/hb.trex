using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IHolidayRepository : IRepository<Holiday> 
    {
        IEnumerable<Holiday> GetByMonth(int month, int year);
    }
}