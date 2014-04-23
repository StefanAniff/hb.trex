using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServiceStack.ServiceHost;
using ServiceStack.Common.Utils;
using ServiceStack.ServiceHost;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;

namespace TrexSL.Web.ServiceStackServices.TimeEntry
{
    public class TimeEntryService : NhServiceBase, 
        IPost<GetTimeEntryByPeriodAndUserRequest>,
        IPost<SaveOrUpdateTimeEntriesRequest>,
        IGet<GetTimeEntryByPeriodAndUserRequest>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITimeEntryTypeRepository _timeEntryTypeRepository;
        private readonly IPriceService _priceService;
        private readonly ITaskRepository _taskRepository;
        private readonly ITimeEntryFactory _timeEntryFactory;

        public TimeEntryService(ITimeEntryRepository timeEntryRepository,
            IUserRepository userRepository,
            ITimeEntryTypeRepository timeEntryTypeRepository,
            IPriceService priceService,
            ITaskRepository taskRepository,
            ITimeEntryFactory timeEntryFactory)
        {
            _timeEntryRepository = timeEntryRepository;
            _userRepository = userRepository;
            _timeEntryTypeRepository = timeEntryTypeRepository;
            _priceService = priceService;
            _taskRepository = taskRepository;
            _timeEntryFactory = timeEntryFactory;
        }



        public object Post(GetTimeEntryByPeriodAndUserRequest request)
        {
            return GetValue(request);
        }

        public object Get(GetTimeEntryByPeriodAndUserRequest request)
        {
            return GetValue(request);
        }

        private GetTimeEntryByPeriodAndUserResponse GetValue(GetTimeEntryByPeriodAndUserRequest request)
        {
            var timeEntriesByPeriodAndUser = _timeEntryRepository.GetTimeEntriesByPeriodAndUser(request.UserId, request.StartDate, request.EndDate).ToList();
            var timeEntryDtos = Mapper.Map<List<Trex.Server.Core.Model.TimeEntry>, List<TimeEntryDto>>(timeEntriesByPeriodAndUser);

            MarkInactive(timeEntryDtos, request.UserId, request.StartDate, request.EndDate);

            return new GetTimeEntryByPeriodAndUserResponse()
            {
                TimeEntries = timeEntryDtos
            };
        }

        private void MarkInactive(List<TimeEntryDto> timeEntryDtos, int userId, DateTime startDate, DateTime dateTime)
        {
            var inactivetimeEntriesByPeriodAndUser = _timeEntryRepository.GetInactiveTimeEntriesByPeriodAndUser(userId, startDate, dateTime).ToList();

            foreach (var inactivetimeEntry in inactivetimeEntriesByPeriodAndUser)
            {
                var timeEntryDto = timeEntryDtos.Single(x => x.Id == inactivetimeEntry.Id);
                timeEntryDto.TaskIsInactive = true;
            }
        }

        private TimeEntryUpdatedStatusDto SaveOrUpdateTimeEntry(TimeEntryDto timeEntryDto, User user)
        {
            var response = new TimeEntryUpdatedStatusDto();

            response.Guid = timeEntryDto.Guid;
            response.IsOK = true;

            var task = _taskRepository.GetByGuid(timeEntryDto.TaskGuid);
            var timeEntryType = _timeEntryTypeRepository.GetById(timeEntryDto.TimeEntryTypeId);
            var pricePrHour = _priceService.GetPrice(timeEntryDto.PricePrHour, user, task);

            if (_timeEntryRepository.Exists(timeEntryDto.Guid))
            {
                var changedTimeEntry = _timeEntryRepository.GetByGuid(timeEntryDto.Guid);

                if (changedTimeEntry.Invoice != null)
                {
                    //throw
                    response.IsOK = false;
                    response.ReasonText = "Can not edit timeentry that has marked as invoiced!";
                    return response;
                }

                changedTimeEntry.User = user;
                changedTimeEntry.Task = task;
                changedTimeEntry.TimeEntryType = timeEntryType;
                changedTimeEntry.StartTime = timeEntryDto.StartTime;
                changedTimeEntry.EndTime = timeEntryDto.EndTime;
                changedTimeEntry.Description = timeEntryDto.Description;
                changedTimeEntry.TimeSpent = timeEntryDto.TimeSpent;
                changedTimeEntry.BillableTime = timeEntryDto.BillableTime;
                changedTimeEntry.Billable = timeEntryDto.Billable;
                changedTimeEntry.Price = pricePrHour;

                _timeEntryRepository.SaveOrUpdate(changedTimeEntry);
                return response;
            }

            var newTimeEntry = _timeEntryFactory.Create(
                timeEntryDto.Guid,
                user,
                task,
                timeEntryType,
                timeEntryDto.StartTime,
                timeEntryDto.EndTime,
                timeEntryDto.Description,
                timeEntryDto.TimeSpent,
                0,
                timeEntryDto.BillableTime,
                timeEntryDto.Billable,
                pricePrHour,
                timeEntryDto.ClientSourceId
                );

            _timeEntryRepository.SaveOrUpdate(newTimeEntry);

            return response;
        }



        public object Post(SaveOrUpdateTimeEntriesRequest request)
        {
            var user = _userRepository.GetByUserID(request.UserId);

            var list = request.TimeEntries.Select(timeEntry => SaveOrUpdateTimeEntry(timeEntry, user)).ToList();
            return new SaveOrUpdateTimeEntriesResponse
            {
                TimeEntryStatus = list
            };
        }
    }
}