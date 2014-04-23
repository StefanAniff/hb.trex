using System.Collections.Generic;
using System.Linq;
using Trex.Core.Model;
using Trex.Infrastructure.TrexSLService;
using CustomerStats = Trex.Core.Model.CustomerStats;
using Invoice = Trex.Core.Model.Invoice;
using InvoiceLine = Trex.Core.Model.InvoiceLine;
using Project = Trex.Core.Model.Project;
using Task = Trex.Core.Model.Task;
using TimeEntry = Trex.Core.Model.TimeEntry;
using TimeEntryType = Trex.Core.Model.TimeEntryType;
using User = Trex.Core.Model.User;
using UserCustomerInfo = Trex.Core.Model.UserCustomerInfo;
using Customer = Trex.Core.Model.Customer;
using DtoCustomer = Trex.Infrastructure.TrexSLService.Customer;

namespace Trex.Infrastructure.Extensions
{
    public static class DtoToModelExtensions
    {
        public static List<Invoice> ToModelObjects(this IEnumerable<TrexSLService.Invoice> dtoInvoices)
        {
            if (dtoInvoices != null)
            {
                return dtoInvoices.Select(p => p.ToModelObject()).ToList();
            }

            return new List<Invoice>();
        }

        public static List<InvoiceLine> ToModelObjects(this IEnumerable<TrexSLService.InvoiceLine> dtoInvoiceLines)
        {
            if (dtoInvoiceLines != null)
            {
                return dtoInvoiceLines.Select(p => p.ToModelObject()).ToList();
            }

            return new List<InvoiceLine>();
        }

        public static List<Customer> ToModelObjects(this IEnumerable<DtoCustomer> dtoCompanies)
        {
            if (dtoCompanies != null)
            {
                return dtoCompanies.Select(c => c.ToModelObject()).ToList();
            }

            return new List<Customer>();
        }

        public static List<Project> ToModelObjects(this IEnumerable<TrexSLService.Project> dtoProjects)
        {
            if (dtoProjects != null)
            {
                return dtoProjects.Select(p => p.ToModelObject()).ToList();
            }

            return new List<Project>();
        }

        public static List<Task> ToModelObjects(this IEnumerable<TrexSLService.Task> dtoTasks)
        {
            if (dtoTasks != null)
            {
                return dtoTasks.Select(t => t.ToModelObject()).ToList();
            }

            return new List<Task>();
        }

        public static List<TimeEntry> ToModelObjects(this IEnumerable<TrexSLService.TimeEntry> dtoTimeEntries)
        {
            if (dtoTimeEntries != null)
            {
                return dtoTimeEntries.Select(t => t.ToModelObject()).ToList();
            }

            return new List<TimeEntry>();
        }

        public static List<User> ToModelObjects(this IEnumerable<TrexSLService.User> dtoUsers)
        {
            if (dtoUsers != null)
            {
                return dtoUsers.Select(u => u.ToModelObject()).ToList();
            }
            return new List<User>();
        }

        public static List<TimeEntryType> ToModelObjects(this IEnumerable<TrexSLService.TimeEntryType> dtoTimeEntryTypes)
        {
            if (dtoTimeEntryTypes != null)
            {
                return dtoTimeEntryTypes.Select(t => t.ToModelObject()).ToList();
            }
            return new List<TimeEntryType>();
        }

        public static List<UserCustomerInfo> ToModelObjects(this IEnumerable<TrexSLService.UserCustomerInfo> dtoUserCustomerinfos)
        {
            if (dtoUserCustomerinfos != null)
            {
                return dtoUserCustomerinfos.Select(u => u.ToModelObject()).ToList();
            }
            return new List<UserCustomerInfo>();
        }

        public static InvoiceLine ToModelObject(this TrexSLService.InvoiceLine dtoInvoiceLine)
        {
            var invoiceLine = new InvoiceLine
                                  {
                                      ID = dtoInvoiceLine.ID,
                                      InvoiceId = dtoInvoiceLine.InvoiceID,
                                      IsExpense = dtoInvoiceLine.IsExpense,
                                      PricePrUnit = dtoInvoiceLine.PricePrUnit,
                                      SortIndex = dtoInvoiceLine.SortIndex,
                                      Text = dtoInvoiceLine.Text,
                                      Unit = dtoInvoiceLine.Unit,
                                      Units = dtoInvoiceLine.Units,
                                      VatPercentage = dtoInvoiceLine.VatPercentage
                                  };

            return invoiceLine;
        }

        public static Invoice ToModelObject(this TrexSLService.Invoice dtoInvoice)
        {
            var invoice = new Invoice
                              {
                                  Address2 = dtoInvoice.Address2,
                                  Attention = dtoInvoice.Attention,
                                  City = dtoInvoice.City,
                                  Closed = dtoInvoice.Closed,
                                  Country = dtoInvoice.Country,
                                  CreateDate = dtoInvoice.CreateDate,
                                  CreatedBy = dtoInvoice.CreatedBy,
                                  CustomerName = dtoInvoice.CustomerName,
                                  CustomerNumber = dtoInvoice.CustomerId,
                                  DueDate = dtoInvoice.DueDate,
                                  EndDate = dtoInvoice.EndDate,
                                  FooterText = dtoInvoice.FooterText,
                                  ID = dtoInvoice.ID,
                                  InvoiceDate = dtoInvoice.InvoiceDate,
                                  Regarding = dtoInvoice.Regarding,
                                  StartDate = dtoInvoice.StartDate,
                                  StreetAddress = dtoInvoice.StreetAddress,
                                  //TotalExclVAT = dtoInvoice.TotalExclVAT,
                                  ZipCode = dtoInvoice.ZipCode
                              };

            return invoice;
        }

        public static CustomerStats ToModelObject(this TrexSLService.CustomerStats dtoCustomerStats)
        {
            var customerStats = new CustomerStats
                                    {
                                        DistinctPrices = dtoCustomerStats.DistinctPrices,
                                        FirstEntry = dtoCustomerStats.FirstEntry,
                                        InventoryValue = dtoCustomerStats.InventoryValue
                                    };

            return customerStats;
        }

        public static Customer ToModelObject(this DtoCustomer dtoCompany)
        {
            var customer = new Customer();
            customer.Id = dtoCompany.CustomerID;
            customer.Name = dtoCompany.CustomerName;
            customer.Guid = dtoCompany.Guid;
            customer.Inactive = dtoCompany.Inactive;
            customer.PhoneNumber = dtoCompany.PhoneNumber;
            customer.StreetAddress = dtoCompany.StreetAddress;
            customer.ZipCode = dtoCompany.ZipCode;
            customer.City = dtoCompany.City;
            customer.CellPhoneNumber = dtoCompany.PhoneNumber;
            customer.ContactName = dtoCompany.ContactName;
            customer.ContactPhone = dtoCompany.ContactPhone;
            customer.Country = dtoCompany.Country;
            customer.CreateDate = dtoCompany.CreateDate;
            //customer.CreatedBy = dtoCompany.CreatedByUser.ToModelObject();
            customer.Email = dtoCompany.Email;
            customer.Projects = dtoCompany.Projects.ToModelObjects();
            customer.InheritsTimeEntryTypes = dtoCompany.InheritsTimeEntryTypes;
            customer.TimeEntryTypes = dtoCompany.TimeEntryTypes.ToModelObjects();
            customer.PaymentTermNumberOfDays = dtoCompany.PaymentTermsNumberOfDays;
            customer.PaymentTermIncludeCurrentMonth = dtoCompany.PaymentTermsIncludeCurrentMonth;
            customer.Address2 = dtoCompany.Address2;
            //customer.TotalNotInvoicedTime = dtoCompany.TotalNotInvoicedTime;
            //customer.DistinctPrices = dtoCompany.DistinctPrices;
            //customer.InventoryValue = dtoCompany.InventoryValue;
            //customer.FirstTimeEntryDate = dtoCompany.FirstTimeEntryDate;

            return customer;
        }

        public static Project ToModelObject(this TrexSLService.Project dtoProject)
        {
            var project = new Project();
            if (dtoProject.Customer != null)
            {
                project.Customer = dtoProject.Customer.ToModelObject();
            }
            project.CreateDate = dtoProject.CreateDate;
            project.CreatedBy = dtoProject.CreatedByUser.ToModelObject();
            project.Guid = dtoProject.Guid;
            project.Id = dtoProject.ProjectID;
            project.CustomerId = dtoProject.CustomerID;
            project.Inactive = dtoProject.Inactive;
            project.Name = dtoProject.ProjectName;
            project.Tasks = dtoProject.Tasks.ToModelObjects();
            project.IsEstimatesEnabled = dtoProject.IsEstimatesEnabled;

            return project;
        }

        public static Task ToModelObject(this TrexSLService.Task dtoTask)
        {
            var task = new Task();
            task.Guid = dtoTask.Guid;
            task.Id = dtoTask.TaskID;
            task.Name = dtoTask.TaskName;
            task.Description = dtoTask.Description;
            //if (task.ParentTask != null)
            //{
            //    task.ParentTask = dtoTask.ParentTask.ToModelObject();
            //}
            if (dtoTask.Project != null)
            {
                task.Project = dtoTask.Project.ToModelObject();
            }

           // task.ParentTaskId = dtoTask.ParentTaskId;
            task.ProjectId = dtoTask.ProjectID;
            //task.SubTasks = dtoTask.SubTasks.ToModelObjects();
            task.TimeEntries = dtoTask.TimeEntries.ToModelObjects();
            task.TimeEstimated = dtoTask.TimeEstimated;
            task.TimeLeft = dtoTask.TimeLeft;
            task.BestCaseEstimate = dtoTask.BestCaseEstimate;
            task.WorstCaseEstimate = dtoTask.WorstCaseEstimate;
            task.RealisticEstimate = dtoTask.RealisticEstimate;
            task.Closed = dtoTask.Closed;
            task.CreateDate = dtoTask.CreateDate;
            task.CreatedBy = dtoTask.CreatedByUser.ToModelObject();

            return task;
        }

        public static TimeEntry ToModelObject(this TrexSLService.TimeEntry dtoTimeEntry)
        {
            var timeEntry = new TimeEntry();
            if (dtoTimeEntry.Task != null)
            {
                timeEntry.Task = dtoTimeEntry.Task.ToModelObject();
            }
            timeEntry.Guid = dtoTimeEntry.Guid;
            timeEntry.TaskId = dtoTimeEntry.TaskID;
            timeEntry.TaskGuid = dtoTimeEntry.Task.Guid;
            timeEntry.Id = dtoTimeEntry.TimeEntryID;
            timeEntry.Price = dtoTimeEntry.Price;
            timeEntry.StartTime = dtoTimeEntry.StartTime;
            timeEntry.EndTime = dtoTimeEntry.EndTime;
            timeEntry.TimeSpent = dtoTimeEntry.TimeSpent;
            timeEntry.Billable = dtoTimeEntry.Billable;
            timeEntry.BillableTime = dtoTimeEntry.BillableTime;
            timeEntry.Description = dtoTimeEntry.Description;
            timeEntry.User = dtoTimeEntry.User.ToModelObject();
            timeEntry.TimeEntryType = dtoTimeEntry.TimeEntryType.ToModelObject();
            timeEntry.InvoiceId = dtoTimeEntry.InvoiceId;
            timeEntry.ProjectName = dtoTimeEntry.Task.Project.ProjectName;
            timeEntry.TaskName = dtoTimeEntry.Task.TaskName;

            return timeEntry;
        }

        public static User ToModelObject(this TrexSLService.User dtoUser)
        {
            var user = new User();
            user.UserName = dtoUser.UserName;
            user.Id = dtoUser.UserID;
            user.Name = dtoUser.Name;
            user.Inactive = dtoUser.Inactive;
            user.Price = dtoUser.Price;
            user.Email = dtoUser.Email;
            user.Projects = dtoUser.Projects.ToModelObjects();
            user.CustomerInfo = dtoUser.UserCustomerInfo.ToModelObjects();
            //user.NumOfTimeEntries = dtoUser.NumOfTimeEntries;
            //user.TotalBillableTime = dtoUser.TotalBillableTime;
            //user.TotalTime = dtoUser.TotalTime;
            user.Roles = dtoUser.Roles;
            user.Permissions = dtoUser.Permissions;

            return user;
        }

        public static UserCustomerInfo ToModelObject(this TrexSLService.UserCustomerInfo dtoUserCustomerInfo)
        {
            var userCustomerInfo = new UserCustomerInfo();
            userCustomerInfo.CustomerId = dtoUserCustomerInfo.CustomerID;
            userCustomerInfo.UserId = dtoUserCustomerInfo.UserID;
            userCustomerInfo.PricePrHour = dtoUserCustomerInfo.Price;

            return userCustomerInfo;
        }

        public static TimeEntryType ToModelObject(this TrexSLService.TimeEntryType dtoTimeEntryType)
        {
            var timeEntryType = new TimeEntryType();
            timeEntryType.CustomerId = dtoTimeEntryType.CustomerId;
            timeEntryType.Id = dtoTimeEntryType.TimeEntryTypeId;
            timeEntryType.IsBillableByDefault = dtoTimeEntryType.IsBillableByDefault;
            timeEntryType.IsDefault = dtoTimeEntryType.IsDefault;
            timeEntryType.Name = dtoTimeEntryType.Name;
           // timeEntryType.Guid = dtoTimeEntryType.;

            return timeEntryType;
        }
    }
}