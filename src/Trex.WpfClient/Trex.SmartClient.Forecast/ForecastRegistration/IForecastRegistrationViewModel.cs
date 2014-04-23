using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Forecast.ForecastRegistration.Helpers;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    public interface IForecastRegistrationViewModel : IViewModel, INotifyDirtyState
    {
        DateTime SelectedDate { get; set; }
        string SelectedMonthString { get; }
        ForecastDateColumns DateColumns { get; set; }
        ProjectRegistrations ProjectRegistrations { get; set; }
        IEnumerable<HourRegistration> DateTotals { get; }
        IEnumerable<decimal?> DateRealizedTotals { get; set; }
        IEnumerable<ForecastTypeRegistration> PresenceRegistrations { get; }
        ProjectSearchViewModel ProjectSearchViewModel { get; }
        bool Initializing { get; set; }
        DelegateCommand<object> SaveCommand { get; }
        DelegateCommand<object> ResetCommand { get; }
        DelegateCommand<object> CopyPreviousMonth { get; }
        IEnumerable<ForecastTypeRegistration> DirtyPresenceRegitrations { get; }
        IEnumerable<ProjectHourRegistration> DirtyProjectHours { get; }
        IEnumerable<ProjectRegistration> RemovedProjects { get; }
        bool IsBusy { get; set; }
        ObservableCollection<ForecastType> ForecastTypes { get; }

        [NoDirtyCheck]
        decimal DateRealizedTotalsSum { get; }

        [NoDirtyCheck]
        decimal DateTotalsSum { get; }

        bool ForecastMonthIsLocked { get; set; }
        int ForecastMonthId { get; set; }
        string SaveDisabledText { get; set; }
        DelegateCommand<object> CurrentMonthCommand { get; }
        int ProjectForecastTypeId { get; }
        ForecastRegistrationSelectedUserHandler SelectedUserHandler { get; }

        void Initialize();
        void CalculateTotals();
        ProjectRegistration AddNewProjectRegistration(int projectId, string projectName, string companyName);
        void CalculateTotals(ForecastRegistrationDateColumn dateItem);
        void RefreshViewData();
        void RaiseCanExecuteActions();
        bool IsValid();
        void RaisePresenceRegistrationsOnPropertyChanged();
    }
}