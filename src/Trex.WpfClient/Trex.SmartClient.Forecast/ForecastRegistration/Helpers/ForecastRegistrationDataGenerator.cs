using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastRegistration.Helpers
{
    /// <summary>
    /// Generator for wiring up forecast viewmodel data.
    /// Important that all data generation that has some kind of raltion to a date-column (dateColumn)
    /// is constructed though this class.
    /// </summary>
    public class ForecastRegistrationDataGenerator
    {
        private readonly ForecastTypeProvider _forecastTypeProvider;
        private readonly ForecastGuiMapper _guiMapper;
        private readonly CopyStatusCommandHandler _copyStatusCommandHandler;

        public ForecastRegistrationDataGenerator(ForecastTypeProvider forecastTypeProvider
            , ForecastGuiMapper guiMapper
            , CopyStatusCommandHandler copyStatusCommandHandler)
        {
            _forecastTypeProvider = forecastTypeProvider;
            _guiMapper = guiMapper;
            _copyStatusCommandHandler = copyStatusCommandHandler;
        }

        /// <summary>
        /// Generates date columns for view
        /// </summary>
        /// <param name="date"></param>
        /// <param name="viewModel"></param>
        public virtual void GenerateBaseDataByDate(DateTime date, IForecastRegistrationViewModel viewModel)
        {
            viewModel.Initializing = true;
            CleanUp(viewModel);

            // 1. Generate dates for month
            var dateHeaders = CreateDateColumns(date);

            // 2. Initialize Headers
            viewModel.DateColumns = dateHeaders;

            // 3. Initialize Presence registrations
            InitializeForecastTypeRegistrations(viewModel);       
            viewModel.RaisePresenceRegistrationsOnPropertyChanged();

            viewModel.Initializing = false;
        }

        /// <summary>
        /// Manually dispose items making sure that subscriptions 
        /// are closed
        /// </summary>
        /// <param name="vm"></param>
        private void CleanUp(IForecastRegistrationViewModel vm)
        {
            vm.DateColumns = null;

            if (vm.PresenceRegistrations != null)
            {
                foreach (var presenceRegistration in vm.PresenceRegistrations)
                {
                    presenceRegistration.Dispose();
                }
            }

            foreach (var project in vm.ProjectRegistrations)
            {
                project.Dispose();
            }

            vm.ProjectRegistrations.Clear();
            vm.DateRealizedTotals = null;
        }

        private ForecastDateColumns CreateDateColumns(DateTime date)
        {
            return new ForecastDateColumns(date
                                            .CreateDatesForMonth()
                                            .Select(x => new ForecastRegistrationDateColumn(x)));
        }
        
        public virtual void InitializeForecastTypeRegistrations(IForecastRegistrationViewModel vm)
        {
            foreach (var dateColumn in vm.DateColumns)
            {
                var forecastTypeReg = new ForecastTypeRegistration(_forecastTypeProvider.Default, _forecastTypeProvider.ForecastTypes);
                dateColumn.ForecastTypeRegistration = forecastTypeReg;

                // Initialize commands here, since contextmenu is not part of visual-tree (cant use FindAncestor on binding)
                _copyStatusCommandHandler.InitializePresenceCopyCommands(forecastTypeReg, vm.PresenceRegistrations);

                // Selected PresenceType affects validation and commands.
                forecastTypeReg.ForecastTypeRegistrationChanged.Subscribe(_ =>
                    {                        
                        vm.CalculateTotals();
                        vm.RaiseCanExecuteActions();
                    });
            }            
        }        

        public virtual ObservableCollection<ProjectHourRegistration> CreateProjectHoursFromHeaders(IEnumerable<ForecastRegistrationDateColumn> dateHeaders
            , IForecastRegistrationViewModel viewModel
            , ProjectRegistration parent)
        {
            var result = new ObservableCollection<ProjectHourRegistration>();

            foreach (var dateHeader in dateHeaders)
            {
                var projectHours = new ProjectHourRegistration(parent);
                dateHeader.AddProjectHours(projectHours);

                // Hookup for updating totals
                var dateItem = projectHours.DateColumn;
                projectHours
                    .HoursUpdated
                    .Subscribe(x =>
                    {
                        viewModel.CalculateTotals(dateItem);
                        viewModel.RaiseCanExecuteActions();
                    });

                InitializeIsEditEnabled(viewModel, projectHours);
                result.Add(projectHours);
            }            

            return result;
        }

        private static void InitializeIsEditEnabled(IForecastRegistrationViewModel viewModel, HourRegistration hourReg)
        {
            // Get the presenceregistration on the same date, and set IsEditEnabled
            var dateItemSibling = viewModel
                .PresenceRegistrations
                .Single(x => x.DateColumn.Equals(hourReg.DateColumn));

            hourReg.InitializeIsEditEnabled(dateItemSibling.SelectedForecastType);
        }        

        public void MergeForecastMonth(IForecastRegistrationViewModel vm, ForecastMonthDto forecastMonth)
        {
            vm.ForecastMonthId = forecastMonth.Id;
            vm.ForecastMonthIsLocked = forecastMonth.IsLocked;
            vm.Initializing = true;
            foreach (var forecast in forecastMonth.ForecastDtos)
            {
                _guiMapper.Map(forecast, vm);
            }

            vm.ProjectRegistrations.InitializeDirtyCheck();
            vm.Initializing = false;
            vm.CalculateTotals();
        }


        /// <summary>
        /// Sets date-relating DateColumns to Holiday
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="holidays"></param>
        public virtual void MergeHolidays(IForecastRegistrationViewModel vm, List<HolidayDto> holidays)
        {
            foreach (var holiday in holidays)
            {
                // Must exist. If not there is something wrong with date-generation
                var toUpdate = vm.DateColumns.Single(x => x.Date == holiday.Date);
                toUpdate.SetToHoliday(holiday);
            }
        }
    }
}