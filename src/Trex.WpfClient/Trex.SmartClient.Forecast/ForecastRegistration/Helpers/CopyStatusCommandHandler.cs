using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using System.Linq;

namespace Trex.SmartClient.Forecast.ForecastRegistration.Helpers
{
    public class CopyStatusCommandHandler
    {
        public void ExecuteCopyForward(ForecastTypeRegistration source, IEnumerable<ForecastTypeRegistration> hostCollection)
        {
            ApplyForecastType(x => x.DateColumn.Date > source.DateColumn.Date && x.DateColumn.IsWorkDay, source, hostCollection);
        }

        public void ExecuteCopyBackwards(ForecastTypeRegistration source, IEnumerable<ForecastTypeRegistration> hostCollection)
        {
            ApplyForecastType(x => x.DateColumn.Date < source.DateColumn.Date && x.DateColumn.IsWorkDay, source, hostCollection);
        }

        public void ExecuteCopyToAll(ForecastTypeRegistration source, IEnumerable<ForecastTypeRegistration> hostCollection)
        {
            ApplyForecastType(presenceReg => !presenceReg.Equals(source) && presenceReg.DateColumn.IsWorkDay, source, hostCollection);
        }

        private static void ApplyForecastType(Func<ForecastTypeRegistration, bool> predicate, ForecastTypeRegistration source, IEnumerable<ForecastTypeRegistration> hostCollection)
        {
            foreach (var registration in hostCollection.Where(predicate))
            {
                registration.SilentStatusSetById(source.SelectedForecastType.Id);
                registration.DedicatedHours = source.DedicatedHours;
            }
        }

        public void InitializePresenceCopyCommands(ForecastTypeRegistration newForecastType, IEnumerable<ForecastTypeRegistration> hostCollection)
        {
            newForecastType.CopyForwardCommand = new DelegateCommand<object>(_ => ExecuteCopyForward(newForecastType, hostCollection));
            newForecastType.CopyBackwardsCommand = new DelegateCommand<object>(_ => ExecuteCopyBackwards(newForecastType, hostCollection));
            newForecastType.CopyToAllCommand = new DelegateCommand<object>(_ => ExecuteCopyToAll(newForecastType, hostCollection));
        } 
    }
}