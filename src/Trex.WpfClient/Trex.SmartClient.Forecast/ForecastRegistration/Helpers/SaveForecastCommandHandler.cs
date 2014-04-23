using System.Linq;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.Forecast.ForecastRegistration.Helpers
{
    public class SaveForecastCommandHandler
    {
        private readonly IForecastService _forecastService;
        private readonly IUserSession _userSession;
        private readonly ForecastRegistrationSelectedUserHandler _selectedUserHandler;

        public SaveForecastCommandHandler(IForecastService forecastService
            , IUserSession userSession
            , ForecastRegistrationSelectedUserHandler selectedUserHandler)
        {
            _forecastService = forecastService;
            _userSession = userSession;
            _selectedUserHandler = selectedUserHandler;
        }

        public async void Execute(IForecastRegistrationViewModel vm)
        {
            if (!vm.IsValid())
                return;

            vm.IsBusy = true;
            var forecastMonthDto = new ForecastMonthDto
                {
                    Id = vm.ForecastMonthId,
                    Month = vm.SelectedDate.Month,
                    Year = vm.SelectedDate.Year,
                    UserId = _selectedUserHandler.UserId,
                    CreatedById = _userSession.CurrentUser.Id,
                    ForecastDtos = vm.DateColumns
                                        .GetItemsToSave(vm.ProjectForecastTypeId)
                                        .Select(col => new ForecastDto
                                                            {
                                                                Date = col.Date,
                                                                DedicatedForecastTypeHours = col.SelectedForecastTypeDedicatedHours,
                                                                ForecastType = col.ForecastType.ToServerDto(),
                                                                ForecastProjectHoursDtos = col.ProjectHoursWithValue.Select(BuildForecastProjectHoursDto).ToList()
                                                            }).ToList()
                };            

            var saveResponse = await _forecastService.SaveForecasts(forecastMonthDto);

            // Null if an error occured
            if (saveResponse != null)
                vm.ForecastMonthId = saveResponse.ForecastMonthId;

            vm.IsBusy = false;
            vm.InitializeDirtyCheck();
            vm.RaiseCanExecuteActions();
            ApplicationCommands.GetForecastStatistics.Execute(vm.SelectedDate);
        }

        private static ForecastProjectHoursDto BuildForecastProjectHoursDto(ProjectHourRegistration projHours)
        {
            return new ForecastProjectHoursDto
                {
                    Hours = projHours.Hours, 
                    Project = new ProjectDto
                        {
                            Id = projHours.Parent.ProjectId, 
                            Name = projHours.Parent.ProjectName
                        }
                };
        }

        public bool CanExecute(IForecastRegistrationViewModel vm)
        {
            vm.SaveDisabledText = string.Empty;

            if (vm.SelectedUserHandler.SelectedUser == null)
            {
                vm.SaveDisabledText = "Please select a workplan owner";
                return false;                                
            }

            if (vm.ForecastMonthIsLocked)
            {
                vm.SaveDisabledText = "This workplan is locked. Please contact an admin, if you wish to update it";
                return false;
            }

            if (!vm.IsDirty)
                return false;

            if (!vm.IsValid())
            {
                vm.SaveDisabledText = "Input is required";
                return false;
            }

            return true;
        }
    }
}