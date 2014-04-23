using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class DomainSettings : IDomainSettings
    {
        private readonly IDomainSettingRepository _domainSettingRepository;

        #region Name ids

        private const string TimeEntryMinStartDateId = "TimeEntryMinStartDateIdDateTime";
        private const string VacationForecastTypeNameId = "VacationForecastTypeIdInt";
        private const string PastMonthsDayLockNameId = "PastMonthsDayLockInt";
        private const string ProjectForecastTypeNameId = "ProjectForecastTypeNameInt";

        #endregion


        public DomainSettings(IDomainSettingRepository domainSettingRepository)
        {
            _domainSettingRepository = domainSettingRepository;
        }

        /// <summary>
        /// Project ForecastType id
        /// </summary>
        public int ProjectForecastTypeId
        {
            get { return GetIntSettingValueByNameId(ProjectForecastTypeNameId); }
        }

        /// <summary>
        /// Vacation ForecastType id
        /// </summary>
        public int VacationForecastTypeId
        {
            get
            {
                return GetIntSettingValueByNameId(VacationForecastTypeNameId);
            }
        }

        /// <summary>
        /// Minimum startdate for timeentriess
        /// </summary>
        public DateTime TimeEntryMinStartDate
        {
            get
            {
                const string nameId = TimeEntryMinStartDateId;
                var setting = GetSettingByName(nameId);
                DateTime datetimeResult;
                var tryParse = DateTime.TryParse(setting.Value, new CultureInfo("en-US"),
                    DateTimeStyles.None, out datetimeResult);
                return tryParse ? datetimeResult : DateTime.MinValue;
            }
        }

        /// <summary>
        /// d60's database customer id.
        /// Day in current month indicated when previous months are locked.
        /// Fx. if value is 3 and the date today is 4-11-2013, 
        /// all prevoius months will be locked for update
        /// </summary>
        public int PastMonthsDayLock
        {
            get
            {
                return GetIntSettingValueByNameId(PastMonthsDayLockNameId);
            }
        }

        #region Helpers

        public int GetIntSettingValueByNameId(string nameId)
        {
            var setting = GetSettingByName(nameId);
            var intResult = TryExtractInt(setting.Value, nameId);
            return intResult;
        }

        private static IEnumerable<int> TryExtractEnumerableInt(IEnumerable<string> strings, string nameId)
        {
            return strings.Select(s => TryExtractInt(s, nameId)).ToList();
        }

        public static int TryExtractInt(string strIntValue, string nameId)
        {
            var strResult = strIntValue;
            int intResult;
            if (!int.TryParse(strResult, out intResult))
                throw new Exception(
                    string.Format(
                        "{0} in DomainSettings is missing or has an invalid value! String value: {1}", nameId, strResult));
            return intResult;
        }

        public DomainSetting GetSettingByName(string name)
        {
            var setting = _domainSettingRepository.GetByName(name);
            if (setting == null)
                throw new Exception(string.Format("The domainsetting with name {0} is missing", name));

            return setting;
        }

        #endregion

    }

}
