using System;
using System.Collections.Generic;
using System.Text;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using System.Linq;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastRegistration.Helpers
{
    public class CopyPreviousMonthCommandHandler
    {
        private readonly IForecastService _forecastService;
        private readonly ForecastRegistrationDataGenerator _forecastRegistrationDataGenerator;
        private readonly ICommonDialogs _commonDialogs;
        private readonly ForecastTypeProvider _forecastTypeProvider;
        private readonly MostFrequentDayLayoutSelector _mostFrequentDayLayoutSelector;
        private readonly ForecastRegistrationSelectedUserHandler _selectedUserHandler;

        public CopyPreviousMonthCommandHandler(IForecastService forecastService
            , ForecastRegistrationDataGenerator forecastRegistrationDataGenerator
            , ICommonDialogs commonDialogs
            , ForecastTypeProvider forecastTypeProvider
            , MostFrequentDayLayoutSelector mostFrequentDayLayoutSelector
            , ForecastRegistrationSelectedUserHandler selectedUserHandler)
        {
            _forecastService = forecastService;
            _forecastRegistrationDataGenerator = forecastRegistrationDataGenerator;
            _commonDialogs = commonDialogs;
            _forecastTypeProvider = forecastTypeProvider;
            _mostFrequentDayLayoutSelector = mostFrequentDayLayoutSelector;
            _selectedUserHandler = selectedUserHandler;
        }
         
        public async void Execute(IForecastRegistrationViewModel vm)
        {
            // Initialize datecolumns
            var selectedDate = vm.SelectedDate;
            var prevMonth = selectedDate.FirstDayOfPreviousMonth();
            _forecastRegistrationDataGenerator.GenerateBaseDataByDate(vm.SelectedDate, vm); // Initializes a "clean" month

            // Get data from last month
            var response = await _forecastService.GetByUserIdAndMonth(_selectedUserHandler.UserId, prevMonth.Month, prevMonth.Year, selectedDate.Month, selectedDate.Year);

            // Merge fetched holidays
            _forecastRegistrationDataGenerator.MergeHolidays(vm, response.Holidays);
            var projectDtos = response
                .ForecastMonth
                .ForecastDtos
                .Where(forecast => forecast.ForecastType.SupportsProjectHours)
                .SelectMany(forecast => forecast.ForecastProjectHoursDtos)
                .Select(forecastHoursDto => forecastHoursDto.Project)
                .Distinct();                

            ApplyProjects(vm, projectDtos);
            ApplyStatusAndProjectHours(vm, response);

            vm.RaisePresenceRegistrationsOnPropertyChanged();
            vm.CalculateTotals();
            vm.DateColumns.EnableClientHours();
        }

        private static void ApplyProjects(IForecastRegistrationViewModel vm, IEnumerable<ProjectDto> projectDtos)
        {
            foreach (var project in projectDtos)
            {
                vm.AddNewProjectRegistration(project.Id, project.Name, project.CompanyDto.Name);
            }
        }

        private void ApplyStatusAndProjectHours(IForecastRegistrationViewModel vm, ForecastsByUserAndMonthResponse response)
        {
            /* We dont want app to crash due to this.
             * Just try and dont vomit if an error occurs
             * **/
            try
            {
                TryApplyDayLayout(DayOfWeek.Monday, vm, response.ForecastMonth.ForecastDtos);
                TryApplyDayLayout(DayOfWeek.Tuesday, vm, response.ForecastMonth.ForecastDtos);
                TryApplyDayLayout(DayOfWeek.Wednesday, vm, response.ForecastMonth.ForecastDtos);
                TryApplyDayLayout(DayOfWeek.Thursday, vm, response.ForecastMonth.ForecastDtos);
                TryApplyDayLayout(DayOfWeek.Friday, vm, response.ForecastMonth.ForecastDtos);
            }
            catch (Exception exp)
            {
                var msg = new StringBuilder()
                    .AppendLine("I had problems trying to copy hours.")
                    .AppendLine("Hours will not be copied.")
                    .AppendLine("Please contact someone.")
                    .AppendLine("Sry...")
                    .AppendLine()
                    .AppendLine(exp.ToString());
                _commonDialogs.Error(msg.ToString());
            }            
        }

        
        private void TryApplyDayLayout(DayOfWeek dayOfWeek, IForecastRegistrationViewModel vm, IEnumerable<ForecastDto> srcForecastDtos)
        {            
            // Get the most frequent layout for the given dayOfWeek.
            var dayLayout = _mostFrequentDayLayoutSelector.MostFrequentDayLayout(dayOfWeek, srcForecastDtos);
            if (dayLayout == null)
                return;

            // Get all days by dayOfWeek
            var daysToUpdate = vm.DateColumns.Where(x => x.IsWorkDay && x.Date.DayOfWeek == dayOfWeek);

            foreach (var day in daysToUpdate)
            {
                // Update status type
                day.ForecastTypeRegistration.SilentStatusSetById(dayLayout.ForecastType.Id);

                // Update dedicated hours for status type
                day.ForecastTypeRegistration.DedicatedHours = dayLayout.DedicatedForecastTypeHours;

                // Update project hours
                foreach (var projHoursSrc in dayLayout.ForecastProjectHoursDtos)
                {
                    var toUpdate = day.ProjectHours.FirstOrDefault(x => x.Parent.ProjectId == projHoursSrc.Project.Id);
                    if (toUpdate == null)
                        continue;

                    toUpdate.Hours = projHoursSrc.Hours;
                }
            }
        }

        /// <summary>
        /// If data exists ask user to continue
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public bool ShouldContinue(IForecastRegistrationViewModel vm)
        { 
            if (vm.ProjectRegistrations.Count == 0 && vm.PresenceRegistrations.All(x => (x.SelectedForecastType.Equals(_forecastTypeProvider.Default))))
                return true;

            return _commonDialogs.ContinueWarning("Data already exists for this month!\n\nContinue with overwrite?", "Overwrite");
        }

        public bool CanExecute(IForecastRegistrationViewModel vm)
        {
            return !vm.ForecastMonthIsLocked && !vm.IsBusy;
        }
    }
}