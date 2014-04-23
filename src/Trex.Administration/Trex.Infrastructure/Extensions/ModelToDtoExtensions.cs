using System.Collections.Generic;
using System.Linq;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.Infrastructure.TrexSLService;
using Invoice = Trex.Infrastructure.TrexSLService.Invoice;
using Project = Trex.Infrastructure.TrexSLService.Project;
using Task = Trex.Infrastructure.TrexSLService.Task;
using TimeEntry = Trex.Infrastructure.TrexSLService.TimeEntry;
using TimeEntryType = Trex.Infrastructure.TrexSLService.TimeEntryType;
using User = Trex.Infrastructure.TrexSLService.User;
using UserCustomerInfo = Trex.Infrastructure.TrexSLService.UserCustomerInfo;
using DtoCustomer = Trex.Infrastructure.TrexSLService.Customer;
using Customer = Trex.Core.Model.Customer;

namespace Trex.Infrastructure.Extensions
{
    public static class ModelToDtoExtensions
    {
        public static TimeEntry ToDtoObject(this Core.Model.TimeEntry modelTimeEntry)
        {
            var dtoTimeEntry = new TimeEntry();
            //if (modelTimeEntry.Task != null)
            //dtoTimeEntry.Task = modelTimeEntry.Task.ToDtoObject();
            dtoTimeEntry.TaskID = modelTimeEntry.TaskId;
            dtoTimeEntry.Guid = modelTimeEntry.Guid;
            //dtoTimeEntry.T = modelTimeEntry.TaskGuid;
            //dtoTimeEntry.TaskId = modelTimeEntry.TaskId;
            dtoTimeEntry.TimeEntryID = modelTimeEntry.Id;
            //dtoTimeEntry.Price = modelTimeEntry.Price;
            dtoTimeEntry.StartTime = modelTimeEntry.StartTime;
            dtoTimeEntry.EndTime = modelTimeEntry.EndTime;
            dtoTimeEntry.TimeSpent = modelTimeEntry.TimeSpent;
            dtoTimeEntry.Billable = modelTimeEntry.Billable;
            dtoTimeEntry.BillableTime = modelTimeEntry.BillableTime;
            dtoTimeEntry.Description = modelTimeEntry.Description;
            dtoTimeEntry.User = modelTimeEntry.User.ToDtoObject();
            dtoTimeEntry.TimeEntryType = modelTimeEntry.TimeEntryType.ToDtoObject();
            dtoTimeEntry.InvoiceId = modelTimeEntry.InvoiceId;

            return dtoTimeEntry;
        }

        public static Task ToDtoObject(this Core.Model.Task modelTask)
        {
            var dtoTask = new Task();
            dtoTask.Guid = modelTask.Guid;
            dtoTask.TaskID = modelTask.Id;
            dtoTask.TaskName = modelTask.Name;
            dtoTask.Description = modelTask.Description;
            //dtoTask.ParentTaskId = modelTask.ParentTaskId;
            dtoTask.ProjectID = modelTask.ProjectId;
            dtoTask.TimeEstimated = modelTask.TimeEstimated;
            dtoTask.TimeLeft = modelTask.TimeLeft;
            dtoTask.BestCaseEstimate = modelTask.BestCaseEstimate;
            dtoTask.WorstCaseEstimate = modelTask.WorstCaseEstimate;
            dtoTask.RealisticEstimate = modelTask.RealisticEstimate;
            dtoTask.Closed = modelTask.Closed;
            dtoTask.CreateDate = modelTask.CreateDate;
            dtoTask.CreatedByUser = modelTask.CreatedBy.ToDtoObject();
            return dtoTask;
        }

        public static Project ToDtoObject(this Core.Model.Project modelProject)
        {
            var dtoProject = new Project();
            //if (modelProject.Customer != null)
            //dtoProject.Customer = modelProject.Customer.ToModelObject();
            dtoProject.CreateDate = modelProject.CreateDate;
            // dtoProject.CreatedBy = modelProject.CreatedBy.ToDtoObject();
            dtoProject.Guid = modelProject.Guid;
            dtoProject.ProjectID = modelProject.Id;
            dtoProject.CustomerID = modelProject.CustomerId;
            dtoProject.Inactive = modelProject.Inactive;
            dtoProject.ProjectName = modelProject.Name;
            //dtoProject.Tasks = modelProject.Tasks.ToModelObjects();
            dtoProject.IsEstimatesEnabled = modelProject.IsEstimatesEnabled;

            return dtoProject;
        }

        public static User ToDtoObject(this Core.Model.User modelUser)
        {
            var user = new User();
            user.UserName = modelUser.UserName;
            user.UserID = modelUser.Id;
            user.Name = modelUser.Name;
            user.Inactive = modelUser.Inactive;
            user.Price = modelUser.Price;
            user.Email = modelUser.Email;
            if (modelUser.CustomerInfo != null)
            {
                // user.UserCustomerInfo = modelUser.CustomerInfo.ToDtoObject();
            }
            //user.Roles = modelUser.Roles;
            //user.Permissions = modelUser.Permissions;

            //user.Projects = modelUser.Projects.ToModelObjects();)
            return user;
        }

        public static DtoCustomer ToDtoObject(this Customer modelCustomer)
        {
            var company = new DtoCustomer();
            company.Guid = modelCustomer.Guid;
            company.CustomerID = modelCustomer.Id;
            company.CreatedByUser = modelCustomer.CreatedBy.ToDtoObject();
            company.CreateDate = modelCustomer.CreateDate;
            company.Inactive = modelCustomer.Inactive;
            company.CustomerName = modelCustomer.Name;
            company.PhoneNumber = modelCustomer.PhoneNumber;
            company.Email = modelCustomer.Email;
            company.StreetAddress = modelCustomer.StreetAddress;
            company.ZipCode = modelCustomer.ZipCode;
            company.Country = modelCustomer.Country;
            company.ContactName = modelCustomer.ContactName;
            company.ContactPhone = modelCustomer.ContactPhone;
            company.City = modelCustomer.City;
            company.InheritsTimeEntryTypes = modelCustomer.InheritsTimeEntryTypes;
            company.PaymentTermsIncludeCurrentMonth = modelCustomer.PaymentTermIncludeCurrentMonth;
            company.PaymentTermsNumberOfDays = modelCustomer.PaymentTermNumberOfDays;
            company.Address2 = modelCustomer.Address2;

            return company;
        }

        public static TimeEntryType ToDtoObject(this Core.Model.TimeEntryType modelTimeEntryType)
        {
            var timeEntryType = new TimeEntryType();
            timeEntryType.CustomerId = modelTimeEntryType.CustomerId;
            timeEntryType.TimeEntryTypeId = modelTimeEntryType.Id;
            timeEntryType.IsBillableByDefault = modelTimeEntryType.IsBillableByDefault;
            timeEntryType.IsDefault = modelTimeEntryType.IsDefault;
            timeEntryType.Name = modelTimeEntryType.Name;
            //timeEntryType. = modelTimeEntryType.Guid;

            return timeEntryType;
        }

        public static List<DtoCustomer> ToDtoObjects(this List<Customer> customers)
        {
            var returnList = new List<DtoCustomer>();

            foreach (var customer in customers)
            {
                var dtoCustomer = customer.ToDtoObject();

                //dtoCustomer.Projects = customer.Projects.ToDtoObjects();
                returnList.Add(dtoCustomer);
            }
            return returnList;
        }

        public static List<Project> ToDtoObjects(this IEnumerable<Core.Model.Project> projects)
        {
            var returnList = new List<Project>();

            foreach (var project in projects)
            {
                var dtoProject = project.ToDtoObject();
                //dtoProject.Tasks = project.Tasks.ToDtoObjects();
                returnList.Add(dtoProject);
            }
            return returnList;
        }

        public static List<Task> ToDtoObjects(this IEnumerable<Core.Model.Task> tasks)
        {
            var returnList = new List<Task>();

            foreach (var task in tasks)
            {
                var dtoTask = task.ToDtoObject();
                //dtoTask.TimeEntries = task.TimeEntries.ToDtoObjects();

                returnList.Add(dtoTask);
            }
            return returnList;
        }

        public static List<TimeEntry> ToDtoObjects(this IEnumerable<Core.Model.TimeEntry> timeEntries)
        {
            var returnList = new List<TimeEntry>();

            foreach (var timeEntry in timeEntries)
            {
                var dtoTimeEntry = timeEntry.ToDtoObject();
                returnList.Add(dtoTimeEntry);
            }
            return returnList;
        }

        public static UserCustomerInfo ToDtoObject(this Core.Model.UserCustomerInfo userCustomerInfo)
        {
            var dtoCustomerInfo = new UserCustomerInfo();
            dtoCustomerInfo.CustomerID = userCustomerInfo.CustomerId;
            dtoCustomerInfo.UserID = userCustomerInfo.UserId;
            dtoCustomerInfo.Price = userCustomerInfo.PricePrHour;

            return dtoCustomerInfo;
        }

        public static List<UserCustomerInfo> ToDtoObject(this IEnumerable<Core.Model.UserCustomerInfo> userCustomerInfo)
        {
            return userCustomerInfo.Select(u => u.ToDtoObject()).ToList();
        }

        public static SearchFilterTransferObject ToDtoObject(this ISearchFilterTransferObject transferObject)
        {
            var dtoTransfer = new SearchFilterTransferObject();
            dtoTransfer.CustomerIds = transferObject.CustomerIds;
            dtoTransfer.ProjectIds = transferObject.ProjectIds;
            dtoTransfer.TaskIds = transferObject.TaskIds;
            dtoTransfer.Users = transferObject.Users;
            dtoTransfer.DateFrom = transferObject.DateFrom;
            dtoTransfer.DateTo = transferObject.DateTo;

            return dtoTransfer;
        }

        public static Invoice ToDtoObject(this Core.Model.Invoice invoice)
        {
            var dtoInvoice = new Invoice
                                 {
                                     Address2 = invoice.Address2,
                                     Attention = invoice.Attention,
                                     City = invoice.City,
                                     Closed = invoice.Closed,
                                     Country = invoice.Country,
                                     CreateDate = invoice.CreateDate,
                                     CreatedBy = invoice.CreatedBy,
                                     CustomerName = invoice.CustomerName,
                                     CustomerId = invoice.CustomerNumber,
                                     DueDate = invoice.DueDate,
                                     EndDate = invoice.EndDate,
                                     FooterText = invoice.FooterText,
                                     ID = invoice.ID,
                                     InvoiceDate = invoice.InvoiceDate,
                                     Regarding = invoice.Regarding,
                                     StartDate = invoice.StartDate,
                                     StreetAddress = invoice.StreetAddress,
                                     //TotalExclVAT = invoice.TotalExclVAT,
                                     ZipCode = invoice.ZipCode
                                 };

            return dtoInvoice;
        }
    }
}