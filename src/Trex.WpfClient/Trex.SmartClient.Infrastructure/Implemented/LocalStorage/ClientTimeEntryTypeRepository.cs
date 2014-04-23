using System.Collections.Generic;
using System.Linq;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Data;

namespace Trex.SmartClient.Infrastructure.Implemented.LocalStorage
{
    public class ClientTimeEntryTypeRepository : ITimeEntryTypeRepository
    {
        private readonly DataSetWrapper _dataWrapper;        

        public ClientTimeEntryTypeRepository(DataSetWrapper dataSetWrapper)
        {
            _dataWrapper = dataSetWrapper;
        }

        public List<TimeEntryType> GetGlobal()
        {
            var global = _dataWrapper.TimeEntryTypes.Where(row => row.CustomerId == 0);
            return global.Select(timeEntryTypesRow => CreateTimeEntryType(timeEntryTypesRow)).ToList();
        }


        private void Add(TimeEntryType timeEntryType)
        {
            _dataWrapper.AddTimeEntryType(timeEntryType);
        }

        public void Add(IEnumerable<TimeEntryType> timeEntryTypes)
        {
            foreach (var timeEntryType in timeEntryTypes)
            {
                Add(timeEntryType);
            }
        }

        public TimeEntryType GetById(int id)
        {
            var timeEntryTypeRow = _dataWrapper.TimeEntryTypes.SingleOrDefault(t => t.Id == id);

            if (timeEntryTypeRow == null)
            {
                throw new MissingHieracleDataException("TimeEntryType not found by Id: " + id);
            }

            var companyId = timeEntryTypeRow.CustomerId == 0 ? null : (int?) timeEntryTypeRow.CustomerId;
            return CreateTimeEntryType(timeEntryTypeRow, companyId);
        }

        private List<TimeEntryType> GetByCompanyId(int companyId)
        {
            var types = _dataWrapper.TimeEntryTypes.Where(t => t.CustomerId == companyId);

            return types.Select(timeEntryTypeRow => CreateTimeEntryType(timeEntryTypeRow, companyId)).ToList();
        }

        public List<TimeEntryType> GetByCompany(Company company)
        {
            return company.InheritsTimeEntryTypes ? GetGlobal() : GetByCompanyId(company.Id);
        }

        private static TimeEntryType CreateTimeEntryType(TimeTrackerDataSet.TimeEntryTypesRow row, int? companyId = null)
        {
            return TimeEntryType.Create(row.Id, row.IsDefault, row.IsBillableByDefault, row.Name, companyId);
        }
    }
}