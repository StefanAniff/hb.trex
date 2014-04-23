using AutoMapper;
using Trex.Common.DataTransferObjects;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;
using TrexSL.Web.DataContracts;
using CompanyDto = TrexSL.Web.DataContracts.CompanyDto;
using HolidayDto = Trex.Common.DataTransferObjects.HolidayDto;
using ProjectDto = Trex.Common.DataTransferObjects.ProjectDto;
using TaskDto = TrexSL.Web.DataContracts.TaskDto;
using ProjectDtoWcf = TrexSL.Web.DataContracts.ProjectDto;

namespace TrexSL.Web
{
    public static class RegisterAutoMapperMappings
    {
        public static void Register()
        {
            RegisterLegacy();
            RegisterForServiceStack();

            Mapper.AssertConfigurationIsValid();
        }


        private static void RegisterForServiceStack()
        {

            Mapper.CreateMap<TimeEntry, Trex.Common.DataTransferObjects.TimeEntryDto>()
                .ForMember(x => x.PricePrHour, s => s.MapFrom(x => x.Price))
                .ForMember(x => x.TaskId, s => s.MapFrom(x => x.Task.TaskID))
                .ForMember(x => x.TaskName, s => s.MapFrom(x => x.Task.TaskName))
                .ForMember(x => x.TaskIsInactive, s => s.UseValue(false))
                .ForMember(x => x.Invoiced, s => s.MapFrom(t => t.Invoice != null));

            Mapper.CreateMap<Task, Trex.Common.DataTransferObjects.TaskDto>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.TaskID))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.TaskName))
                .ForMember(x => x.RegisteredTime, s => s.UseValue(0))
                .ForMember(dest => dest.ParentTaskId, opt => opt.MapFrom(t => t.ParentTask != null
                    ? t.ParentTask.TaskID
                    : (int?) null))
                .ForMember(x => x.ProjectId, s => s.MapFrom(x => x.Project.ProjectID))
                .ForMember(x => x.TimeRegistrationType, s => s.MapFrom(x => (Trex.Common.DataTransferObjects.TimeRegistrationTypeEnum) x.TimeRegistrationTypeId));


            Mapper.CreateMap<Task, FullTaskDto>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.TaskID))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.TaskName))
                .ForMember(x => x.RegisteredTime, s => s.UseValue(0))
                .ForMember(dest => dest.ParentTaskId, opt => opt.MapFrom(t => t.ParentTask != null
                    ? t.ParentTask.TaskID
                    : (int?) null))
                .ForMember(x => x.TimeRegistrationType, s => s.MapFrom(x => (Trex.Common.DataTransferObjects.TimeRegistrationTypeEnum) x.TimeRegistrationTypeId));

            Mapper.CreateMap<Project, ProjectDto>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.ProjectID))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.ProjectName));

            Mapper.CreateMap<Company, Trex.Common.DataTransferObjects.CompanyDto>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.CustomerID))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.CustomerName))
                .ForMember(x => x.PaymentTermIncludeCurrentMonth, s => s.MapFrom(x => x.PaymentTermsIncludeCurrentMonth))
                .ForMember(x => x.PaymentTermNumberOfDays, s => s.MapFrom(x => x.PaymentTermsNumberOfDays));

        }

        private static void RegisterLegacy()
        {
            Mapper.CreateMap<Company, CompanyDto>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.CustomerID))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.CustomerName))
                .ForMember(x => x.PaymentTermIncludeCurrentMonth, s => s.MapFrom(x => x.PaymentTermsIncludeCurrentMonth))
                .ForMember(x => x.PaymentTermNumberOfDays, s => s.MapFrom(x => x.PaymentTermsNumberOfDays));

            Mapper.CreateMap<TimeEntry, GeneralTimeEntryDto>()
                .ForMember(x => x.PricePrHour, s => s.MapFrom(x => x.Price))
                .ForMember(x => x.TaskId, s => s.MapFrom(x => x.Task.TaskID))
                .ForMember(x => x.ProjectId, s => s.MapFrom(x => x.Task.Project.ProjectID))
                .ForMember(x => x.UserId, s => s.MapFrom(x => x.User.UserID));

            Mapper.CreateMap<PermissionItem, PermissionItemDto>();

            Mapper.CreateMap<Project, ProjectDto>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.ProjectID))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.ProjectName))
                .ForMember(x => x.CompanyDto, s => s.MapFrom(x => x.Company));

            Mapper.CreateMap<Project, ProjectDtoWcf>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.ProjectID))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.ProjectName))
                .ForMember(x => x.CompanyId, s => s.MapFrom(x => x.Company.CustomerID));

            Mapper.CreateMap<Task, TaskDto>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.TaskID))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.TaskName))
                .ForMember(x => x.RegisteredTime, s => s.UseValue(0))
                .ForMember(dest => dest.ParentTaskId, opt => opt.MapFrom(t => t.ParentTask != null
                    ? t.ParentTask.TaskID
                    : (int?) null))
                .ForMember(x => x.ProjectId, s => s.MapFrom(x => x.Project.ProjectID));


            Mapper.CreateMap<TimeEntry, TrexSL.Web.DataContracts.TimeEntryDto>()
                .ForMember(x => x.PricePrHour, s => s.MapFrom(x => x.Price))
                .ForMember(x => x.TaskId, s => s.MapFrom(x => x.Task.TaskID))
                .ForMember(x => x.TaskName, s => s.MapFrom(x => x.Task.TaskName))
                .ForMember(x => x.ProjectName, s => s.MapFrom(x => x.Task.Project.ProjectName));


            Mapper.CreateMap<TimeEntryType, TimeEntryTypeDto>()
                .ForMember(x => x.Guid, s => s.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(t => t.Company != null
                    ? t.Company.CustomerID
                    : (int?) null));
            Mapper.CreateMap<User, UserDto>()
                .ForMember(x => x.FullName, s => s.MapFrom(x => x.Name))
                .ForMember(x => x.Permissions, s => s.Ignore())
                .ForMember(x => x.Roles, s => s.Ignore());


            Mapper.CreateMap<Company, Trex.Common.DataTransferObjects.CompanyDto>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.CustomerID))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.CustomerName))
                .ForMember(x => x.PaymentTermIncludeCurrentMonth, s => s.MapFrom(x => x.PaymentTermsIncludeCurrentMonth))
                .ForMember(x => x.PaymentTermNumberOfDays, s => s.MapFrom(x => x.PaymentTermsNumberOfDays));

            Mapper.CreateMap<ForecastType, ForecastTypeDto>();
            Mapper.CreateMap<ForecastProjectHours, ForecastProjectHoursDto>();
            Mapper.CreateMap<Forecast, ForecastDto>()
                   .ForMember(x => x.ForecastProjectHoursDtos, s => s.MapFrom(x => x.ProjectRegistrations));

            Mapper.CreateMap<ForecastMonth, ForecastMonthDto>()
                  .ForMember(x => x.UserId, s => s.MapFrom(x => x.User.UserID))
                  .ForMember(x => x.UserName, s => s.MapFrom(x => x.User.Name))
                  .ForMember(x => x.CreatedById, s => s.MapFrom(x => x.CreatedBy.UserID))
                  .ForMember(x => x.ForecastDtos, s => s.MapFrom(x => x.Forecasts))
                  .ForMember(x => x.IsLocked, s => s.Ignore()); // Ignore since extracted from service logic

            Mapper.CreateMap<Holiday, HolidayDto>();

            Mapper.CreateMap<User, ForecastUserDto>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}
