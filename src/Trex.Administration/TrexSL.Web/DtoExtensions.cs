using System.Collections.Generic;
using System.Linq;
using Trex.Server.Core.Model;
using TrexSL.Web.DataContracts;
using Project = TrexSL.Web.DataContracts.Project;
using Task = TrexSL.Web.DataContracts.Task;
using TimeEntry = TrexSL.Web.DataContracts.TimeEntry;
using TimeEntryType = TrexSL.Web.DataContracts.TimeEntryType;
using User = TrexSL.Web.DataContracts.User;
using UserCreationResponse = TrexSL.Web.DataContracts.UserCreationResponse;
using UserCustomerInfo = TrexSL.Web.DataContracts.UserCustomerInfo;

namespace TrexSL.Web
{
    public static class DtoExtensions
    {
        public static List<Company> ToDtoObjects(this IEnumerable<Customer> customers, bool includeParent, bool includeProjects, bool includeTasks, bool includeTimeEntries)
        {
            if (customers != null)
            {
                return customers.Select(c => c.ToDtoObject(includeParent, includeProjects, includeTasks, includeTimeEntries)).OrderBy(c => c.Name).ToList();
            }

            return new List<Company>();
        }

        public static List<Project> ToDtoObjects(this IEnumerable<Trex.Server.Core.Model.Project> projects, bool includeParent, bool includeTasks, bool includeTimeEntries)
        {
            if (projects != null)
            {
                return projects.Select(p => p.ToDtoObject(includeParent, includeTasks, includeTimeEntries)).OrderBy(p => p.Name).ToList();
            }

            return new List<Project>();
        }

        public static List<Task> ToDtoObjects(this IEnumerable<Trex.Server.Core.Model.Task> tasks, bool includeParent, bool includeSubTasks, bool includeTimeEntries)
        {
            if (tasks != null)
            {
                return tasks.Select(t => t.ToDtoObject(includeParent, includeSubTasks, includeTimeEntries)).OrderBy(t => t.Name).ToList();
            }
            return new List<Task>();
        }

        public static List<TimeEntry> ToDtoObjects(this IEnumerable<Trex.Server.Core.Model.TimeEntry> timeEntries, bool includeParent)
        {
            if (timeEntries != null)
            {
                return timeEntries.Select(t => t.ToDtoObject(includeParent)).OrderBy(t => t.StartTime).ToList();
            }

            return new List<TimeEntry>();
        }

        public static List<User> ToDtoObjects(this IEnumerable<Trex.Server.Core.Model.User> users)
        {
            if (users != null)
            {
                return users.Select(u => u.ToDtoObject(false)).ToList();
            }

            return new List<User>();
        }

        public static List<TimeEntryType> ToDtoObjects(
            this IEnumerable<Trex.Server.Core.Model.TimeEntryType> timeEntryTypes)
        {
            if (timeEntryTypes != null)
            {
                return timeEntryTypes.Select(t => t.ToDtoObject()).ToList();
            }

            return new List<TimeEntryType>();
        }

        public static List<UserCustomerInfo> ToDtoObjects(this IEnumerable<Trex.Server.Core.Model.UserCustomerInfo> userCustomerInfos)
        {
            if (userCustomerInfos != null)
            {
                return userCustomerInfos.Select(u => u.ToDtoObject()).ToList();
            }
            return new List<UserCustomerInfo>();
        }

        public static Company ToDtoObject(this Customer customer, bool includeParent, bool includeProjects, bool includeTasks, bool includeTimeEntries)
        {
            var dtoCompany = new Company();
            dtoCompany.Id = customer.Id;
            dtoCompany.Name = customer.Name;
            dtoCompany.Guid = customer.Guid;
            dtoCompany.Inactive = customer.Inactive;
            dtoCompany.PhoneNumber = customer.PhoneNumber;
            dtoCompany.StreetAddress = customer.StreetAddress;
            dtoCompany.ZipCode = customer.ZipCode;
            dtoCompany.City = customer.City;
            dtoCompany.CellPhoneNumber = customer.CellPhoneNumber;
            dtoCompany.ContactName = customer.ContactName;
            dtoCompany.ContactPhone = customer.ContactPhone;
            dtoCompany.Country = customer.Country;
            dtoCompany.CreateDate = customer.CreateDate;
            dtoCompany.CreatedBy = customer.CreatedBy.ToDtoObject(false);
            dtoCompany.Email = customer.Email;
            dtoCompany.PaymentTermIncludeCurrentMonth = customer.PaymentTermIncludeCurrentMonth;
            dtoCompany.PaymentTermNumberOfDays = customer.PaymentTermNumberOfDays;
            dtoCompany.Address2 = customer.Address2;

            if (includeProjects)
            {
                dtoCompany.Projects = customer.Projects.ToDtoObjects(includeParent, includeTasks, includeTimeEntries);
            }
            dtoCompany.TimeEntryTypes = customer.TimeEntryTypes.ToDtoObjects();
            dtoCompany.InheritsTimeEntryTypes = customer.InheritsTimeEntryTypes;
            return dtoCompany;
        }

        public static Project ToDtoObject(this Trex.Server.Core.Model.Project project, bool includeParent, bool includeTasks, bool includeTimeEntries)
        {
            var dtoProject = new Project();
            if (includeParent)
            {
                dtoProject.Company = project.Customer.ToDtoObject(false, false, false, false);
            }

            dtoProject.CompanyId = project.Customer.Id;
            dtoProject.CreateDate = project.CreateDate;
            dtoProject.CreatedBy = project.CreatedBy.ToDtoObject(false);
            dtoProject.Guid = project.Guid;
            dtoProject.Id = project.Id;
            dtoProject.Inactive = project.Inactive;
            dtoProject.Name = project.Name;
            dtoProject.IsEstimatesEnabled = project.IsEstimatesEnabled;

            if (includeTasks)
            {
                dtoProject.Tasks = project.Tasks.ToDtoObjects(includeParent, true, includeTimeEntries);
            }

            return dtoProject;
        }

        public static Task ToDtoObject(this Trex.Server.Core.Model.Task task, bool includeParent, bool includeSubTasks, bool includeTimeEntries)
        {
            var dtoTask = new Task();
            dtoTask.Guid = task.Guid;
            dtoTask.Id = task.Id;
            dtoTask.Name = task.Name;
            dtoTask.Description = task.Description;
            //if (task.ParentTask != null && includeParent)
            //    dtoTask.ParentTask = task.ParentTask.ToDtoObject(false, false, false);
            if (task.ParentTask != null)
            {
                dtoTask.ParentTaskId = task.ParentTask.Id;
            }
            if (includeParent)
            {
                dtoTask.Project = task.Project.ToDtoObject(true, false, false);
            }

            dtoTask.ProjectId = task.Project.Id;
            if (includeSubTasks)
            {
                dtoTask.SubTasks = task.SubTasks.ToDtoObjects(includeParent, true, includeTimeEntries);
            }
            if (includeTimeEntries)
            {
                dtoTask.TimeEntries = task.TimeEntries.ToDtoObjects(includeParent);
            }
            dtoTask.TimeEstimated = task.TimeEstimated;
            dtoTask.TimeLeft = task.TimeLeft;
            dtoTask.BestCaseEstimate = task.BestCaseEstimate;
            dtoTask.WorstCaseEstimate = task.WorstCaseEstimate;
            dtoTask.RealisticEstimate = task.RealisticEstimate;
            dtoTask.Closed = task.Closed;
            dtoTask.CreateDate = task.CreateDate;
            dtoTask.CreatedBy = task.CreatedBy.ToDtoObject(false);

            return dtoTask;
        }

        public static TimeEntry ToDtoObject(this Trex.Server.Core.Model.TimeEntry timeEntry, bool includeParent)
        {
            var dtoTimeEntry = new TimeEntry();
            if (includeParent)
            {
                dtoTimeEntry.Task = timeEntry.Task.ToDtoObject(true, false, false);
            }
            dtoTimeEntry.TaskGuid = timeEntry.Task.Guid;
            dtoTimeEntry.TaskId = timeEntry.Task.Id;

            dtoTimeEntry.Guid = timeEntry.Guid;
            dtoTimeEntry.Id = timeEntry.Id;
            dtoTimeEntry.PricePrHour = timeEntry.Price;
            dtoTimeEntry.StartTime = timeEntry.StartTime;
            dtoTimeEntry.EndTime = timeEntry.EndTime;
            dtoTimeEntry.TimeSpent = timeEntry.TimeSpent;
            dtoTimeEntry.Billable = timeEntry.Billable;
            dtoTimeEntry.BillableTime = timeEntry.BillableTime;
            dtoTimeEntry.Description = timeEntry.Description;
            dtoTimeEntry.User = timeEntry.User.ToDtoObject(false);
            dtoTimeEntry.TimeEntryType = timeEntry.TimeEntryType.ToDtoObject();
            if (timeEntry.Invoice != null)
            {
                dtoTimeEntry.InvoiceId = timeEntry.Invoice.ID;
            }

            return dtoTimeEntry;
        }

        public static User ToDtoObject(this Trex.Server.Core.Model.User user, bool includeProjects)
        {
            var dtoUser = new User();
            dtoUser.UserName = user.UserName;
            dtoUser.UserId = user.Id;
            dtoUser.FullName = user.Name;
            dtoUser.Inactive = user.Inactive;
            dtoUser.Price = user.Price;
            dtoUser.Email = user.Email;
            dtoUser.CustomerInfo = user.CustomerInfo.ToDtoObjects();
            dtoUser.NumOfTimeEntries = user.NumOfTimeEntries;
            dtoUser.TotalTime = user.TotalTime;
            dtoUser.TotalBillableTime = user.TotalBillableTime;

            if (includeProjects)
            {
                dtoUser.Projects = user.Projects.ToDtoObjects(false, false, false);
            }

            return dtoUser;
        }

        public static TimeEntryType ToDtoObject(this Trex.Server.Core.Model.TimeEntryType timeEntryType)
        {
            var dtoTimeEntryType = new TimeEntryType();

            if (timeEntryType.Customer != null)
            {
                dtoTimeEntryType.CustomerId = timeEntryType.Customer.Id;
            }

            dtoTimeEntryType.Id = timeEntryType.Id;
            dtoTimeEntryType.IsBillableByDefault = timeEntryType.IsBillableByDefault;
            dtoTimeEntryType.IsDefault = timeEntryType.IsDefault;
            dtoTimeEntryType.Name = timeEntryType.Name;

            return dtoTimeEntryType;
        }

        public static UserCustomerInfo ToDtoObject(this Trex.Server.Core.Model.UserCustomerInfo customerInfo)
        {
            var dtoUserCustomerInfo = new UserCustomerInfo();

            dtoUserCustomerInfo.CustomerId = customerInfo.CustomerId;
            dtoUserCustomerInfo.UserId = customerInfo.UserId;
            dtoUserCustomerInfo.PricePrHour = customerInfo.PricePrHour;

            return dtoUserCustomerInfo;
        }

        public static UserCreationResponse ToDtoObject(this Trex.Server.Core.Model.UserCreationResponse userCreationResponse)
        {
            var dtoUserCreationResponse = new UserCreationResponse(userCreationResponse.Response,
                                                                   userCreationResponse.Success);

            dtoUserCreationResponse.User = userCreationResponse.User.ToDtoObject(false);
            return dtoUserCreationResponse;
        }
    }
}