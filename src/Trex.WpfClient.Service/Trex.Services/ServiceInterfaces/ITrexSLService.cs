using System;
using System.Collections.Generic;
using System.ServiceModel;
using TrexSL.Web.DataContracts;
using TrexSL.Web.Intercepts;

namespace TrexSL.Web.ServiceInterfaces
{
    [ServiceContract(Namespace = "")]
    public interface ITrexSlService
    {
        [OperationContract]
        List<TimeEntryDto> GetTimeEntriesByPeriodAndUser(int userId, DateTime startDate, DateTime endDate);

        [OperationContract]
        TimeEntryDto SaveTimeEntry(TimeEntryDto timeEntryDto, int userId);

        [OperationContract]
        List<GeneralTimeEntryDto> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate);

        [OperationContract]
        void ChangeTaskActiveState(int taskId, bool isInactive);

        [OperationContract]
        List<TimeEntryTypeDto> GetAllTimeEntryTypes();

        [OperationContract]
        bool ValidateUser(string userName, string password);

        [OperationContract]
        UserDto GetUser(string userName, string password);

        [OperationContract]
        bool PingService();

        [OperationContract]
        UserStatistics GetUserStatistics(int userId, int numOfDaysBack);

        [OperationContract]
        List<CompanyDto> GetUnsyncedCompanies(DateTime lastSyncDate);

        [OperationContract]
        CompressedObject GetUnsyncedCompaniesCompressed(DateTime lastSyncDate);

        [OperationContract]
        List<ProjectDto> GetUnsyncedProjects(DateTime lastSyncDate);

        [OperationContract]
        List<TaskDto> GetUnsyncedTasks(DateTime? lastSyncDate);

        [OperationContract]
        CompressedObject GetUnsyncedTasksCompressed(DateTime? lastSyncDate);

        [OperationContract]
        List<TaskDto> UploadNewTasks(List<TaskDto> tasks, int userId);

        [OperationContract]
        void UploadNewTimeEntries(List<TimeEntryDto> timeEntries, int userId);

        [OperationContract]
        bool ChangePassword(string userName, string oldPassword, string newPassword);

        [OperationContract]
        List<string> GetRoles();

        [OperationContract]
        void UpdateUserRoles(string userName, List<string> roles);

        [OperationContract]
        [FaultContract(typeof (ExceptionInfo))]
        void CreateRole(string roleName);

        [OperationContract]
        [FaultContract(typeof (ExceptionInfo))]
        void DeleteRole(string roleName);

        [OperationContract]
        bool ExistsRole(string name);

        [OperationContract]
        List<PermissionItemDto> GetPermissionsForSingleRole(string roleName, int applicationId);

        [OperationContract]
        void UpdatePermissions(List<PermissionItemDto> updatedPermissions, string roleName, int applicationId);

        [OperationContract]
        List<CompanyDto> GetCompaniesByNameSearchString(string searchString);

        [OperationContract]
        List<GeneralTimeEntryDto> GetAllTimeEntriesByPeriod(DateTime startDate, DateTime endDate);
    }
}