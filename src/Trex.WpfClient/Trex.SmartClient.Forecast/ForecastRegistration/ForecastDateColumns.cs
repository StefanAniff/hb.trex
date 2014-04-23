using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    public class ForecastDateColumns : ObservableCollection<ForecastRegistrationDateColumn>
    {
        #region Constructors

        public ForecastDateColumns()
        {
        }

        public ForecastDateColumns(List<ForecastRegistrationDateColumn> list) : base(list)
        {
        }

        public ForecastDateColumns(IEnumerable<ForecastRegistrationDateColumn> collection) : base(collection)
        {
        }

        #endregion

        public virtual void CalculateTotals()
        {
            foreach (var dateColumn in Items)
            {
                dateColumn.CalculateTotal();
            }
        }

        public virtual void EnableClientHours()
        {
            foreach (var dateColumn in Items)
            {
                dateColumn.ForecastTypeRegistration.EnableClientHours();
            }
        }

        /// <summary>
        /// Returns collection of forecasts to save
        /// </summary>
        /// <param name="projectForecastTypeId">Id for pure project type. Typically "Project"</param>
        /// <returns></returns>
        public IEnumerable<ForecastRegistrationDateColumn> GetItemsToSave(int projectForecastTypeId)
        {
            return this.Where(x => x.IsWorkDay && !x.IsEmptyProjectRegistration(projectForecastTypeId));
        } 
    }
}