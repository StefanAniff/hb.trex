using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using Moq;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.Implemented;
using Trex.Server.Infrastructure.ServiceBehavior;
using Trex.ServiceContracts;

namespace Test
{
    public class DatabaseSetup
    {
        private Mock<IDatabaseConnectionStringProvider> _databaseConnectionStringProvider;
        private EFConnectionStringProvider _connectionStringProvider;
        private ITrexContextProvider _trexContextProvider;

        public DatabaseSetup()
        {
            _databaseConnectionStringProvider = new Mock<IDatabaseConnectionStringProvider>();
            _databaseConnectionStringProvider.Setup(x => x.DatabaseConnectionString)
                .Returns("data source=localhost;initial catalog=Trex_LocalTest;integrated security=True;");

            _connectionStringProvider = new EFConnectionStringProvider(_databaseConnectionStringProvider.Object);
            _trexContextProvider = new TrexContextProvider(_connectionStringProvider);
        }

        public ITrexContextProvider GetTrexConnection
        {
            get { return _trexContextProvider; }
        }

        public TrexEntities EntityContext
        {
            get { return _trexContextProvider.TrexEntityContext; }
        }

        public void CleanDatabase()
        {
            var db = _trexContextProvider.TrexEntityContext;
            List<string> entityList = new List<string>();
            //The following must be added in an order where the FK no longer are connected
            //entityList.Add("Version"); // No need to clean this
            entityList.Add("Roles");
            entityList.Add("PermissionsInRoles");
            entityList.Add("BugtrackerTimeEntryExport");
            entityList.Add("BugtrackerTaskImport");
            entityList.Add("BugtrackerIntegrationProject");
            entityList.Add("BugtrackerIntegrationTask");
            entityList.Add("BugtrackerIntegrationUser");
            entityList.Add("InvoiceTemplateFiles");
            entityList.Add("InvoiceTemplates");
            entityList.Add("TaskTree");
            entityList.Add("CreditNote");
            entityList.Add("InvoiceLines");
            entityList.Add("TimeEntries");
            entityList.Add("DocumentType");
            entityList.Add("TimeEntryTypes");
            entityList.Add("Tasks");
            entityList.Add("InvoiceFiles");
            entityList.Add("UsersCustomers");
            entityList.Add("UsersProjects");
            entityList.Add("Invoices");
            entityList.Add("Tags");
            entityList.Add("Projects");
            entityList.Add("CustomerInvoiceGroup");
            entityList.Add("Customers");
            entityList.Add("Users");

            foreach (var table in entityList)
            {
                db.ExecuteStoreCommand("DELETE FROM [" + table + "]");
            }
            db.SaveChanges();

            foreach (var table in entityList)
            {
                try
                {
                    //Will reseed the tables with 'Identity'. Those without will throw exception and you don't care.
                    db.ExecuteStoreCommand("DBCC CHECKIDENT('" + table + "', RESEED, 0)");
                }
                catch
                {

                }
            }
            db.SaveChanges();
        }

        public void CreateStandardDatabase()
        {
            CreateTimeEntryType(1, "Regular", true, true);
            CreateTimeEntryType(2, "Activable", false, false);

            var docType1 = CreateDocumentType(1, "Non-Credited");
            var docType2 = CreateDocumentType(2, "Invoiced");
            var docType3 = CreateDocumentType(3, "Credit note");

            for (int u = 0; u < 1; u++)
            {
                var user = CreateUser(u + 1, "User" + (u + 1), 500);

                for (int c = 0; c < 1; c++)
                {
                    var customer = CreateCustomer(c + 1, "Customer " + (c + 1), "test" + (c + 1) + "@t.com", "Name of a Customer",
                                                  "Test Street " + (c + 1), "Test City", "Test Land", "1234");

                    for (int cig = 0; cig < 1; cig++)
                    {
                        var cigroup = CreateCustomerInvoiceGroup(cig + 1, customer.CustomerID, "Default", true, 1);
                        var invoice = CreateInvoice(1, "01-01-2012", user.UserID, 0.25, "01-01-2012", "31-01-2012",
                                                    "14-02-2012", null, cigroup.CustomerInvoiceGroupID, false,
                                                    null, false, null);

                        for (int p = 0; p < 1; p++)
                        {
                            var proj = CreateProject(p + 1, customer.CustomerID, "Project " + (p + 1), user.UserID, "01-01-2012", false, 0, cigroup.CustomerInvoiceGroupID);

                            for (int t = 0; t < 1; t++)
                            {
                                var task = CreateTask(t + 1, proj.ProjectID, user.UserID, "01-01-2012", "task" + (t + 1));

                                for (int te = 0; te < 1; te++)
                                {
                                    CreateTimeEntry(te+1, null, 1, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 11:00:00", user.UserID, task.TaskID, docType1.ID);
                                }
                            }
                        }
                    }
                }
            }
        }

        public Invoice CreateInvoice(int id, string invoiceDate, int userId, double vat, string startDate, string endDate, string dueDate, int? invoiceId, int cigId, bool delivered, string deliveredDate, bool isCreditNote, int? invoiceLink)
        {
            var db = _trexContextProvider.TrexEntityContext;

            var input = string.Format(CultureInfo.InvariantCulture, "SET IDENTITY_INSERT Invoices ON " +
                "INSERT INTO Invoices (Closed, CreateDate, CreatedBy, CustomerInvoiceGroupId, Delivered, DeliveredDate, DueDate, EndDate, " +
                                      "Guid, ID, InvoiceDate, InvoiceID, StartDate, VAT, attention) " +
                "VALUES({0}, N'{1}', {2}, {3}, {4}, {5}, N'{6}', N'{7}', {8}, {9}, N'{10}', {11}, N'{12}', {13}, {14}) " +
                "SET IDENTITY_INSERT Invoices OFF",
                0,
                DateTime.Parse("01-01-2012"),
                userId,
                cigId,
                (delivered == false ? 0 : 1),
                (deliveredDate != null ? string.Format("N'{0}'", DateTime.Parse(deliveredDate)) : "NULL"),
                DateTime.Parse(dueDate),
                DateTime.Parse(endDate),
                "NEWID()",
                id,
                DateTime.Parse(invoiceDate),
                (invoiceId != null ? string.Format("'{0}'", invoiceId) : "NULL"),
                DateTime.Parse(startDate),
                vat,
                "NULL"
                );

            db.ExecuteStoreCommand(input);

            return db.Invoices.First(i => i.ID == id);
        }

        public InvoiceLine CreateInvoiceLine(int id, int invoiceId, double pricePerUnit, string unit, double units, double vatPercent, string text, int unitType, bool isExpense)
        {
            var db = _trexContextProvider.TrexEntityContext;

            var input = string.Format(CultureInfo.InvariantCulture, "SET IDENTITY_INSERT InvoiceLines ON " +
                "INSERT INTO InvoiceLines (ID, InvoiceID, PricePrUnit, Unit, Units, VatPercentage, Text, UnitType, IsExpense, SortIndex) " +
                "VALUES({0}, {1}, {2}, '{3}', {4}, {5}, '{6}', {7}, {8}, {9}) " +
                "SET IDENTITY_INSERT InvoiceLines OFF",
                id,
                invoiceId,
                pricePerUnit,
                unit,
                units,
                vatPercent,
                text,
                unitType,
                (isExpense == false ? 0 : 1),
                1
                );

            db.ExecuteStoreCommand(input);

            return db.InvoiceLines.First(il => il.ID == id);
        }

        public DocumentType CreateDocumentType(int id, string name)
        {
            var db = _trexContextProvider.TrexEntityContext;

            var input = string.Format("SET IDENTITY_INSERT DocumentType ON " +
                "INSERT INTO DocumentType (ID, Name) " +
                "VALUES({0}, '{1}') " +
                "SET IDENTITY_INSERT DocumentType OFF",
                id,
                name);

            db.ExecuteStoreCommand(input);

            return db.DocumentType.First(dt => dt.ID == id);
        }

        public TimeEntryType CreateTimeEntryType(int teTypeId, string name, bool isDefault, bool isBillableByDefault)
        {
            var db = _trexContextProvider.TrexEntityContext;

            var input = string.Format("SET IDENTITY_INSERT TimeEntryTypes ON " +
                "INSERT INTO TimeEntryTypes (TimeEntryTypeId, Name, IsDefault, IsBillableByDefault) " +
                "VALUES({0}, '{1}', {2}, {3}) " +
                "SET IDENTITY_INSERT TimeEntryTypes OFF",
                teTypeId, name, (isDefault == false ? 0 : 1), (isBillableByDefault == false ? 0 : 1));

            db.ExecuteStoreCommand(input);

            return db.TimeEntryTypes.First(tet => tet.TimeEntryTypeId == teTypeId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeEntryId"></param>
        /// <param name="invoiceId">can be 'null'</param>
        /// <param name="timeSpent"></param>
        /// <param name="price"></param>
        /// <param name="billable"></param>
        /// <param name="billableTime"></param>
        /// <param name="pauseTime"></param>
        /// <param name="startTime">ex "20-10-2012 10:30:00"</param>
        /// <param name="endTime">ex "20-10-2012 10:30:00"</param>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        /// <param name="docTypeId">1 = Non-credit, 2 = Invoiced, 3 = Credit note</param>
        /// <returns></returns>
        public TimeEntry CreateTimeEntry(int timeEntryId, int? invoiceId, double timeSpent, double price, bool billable, double billableTime, double pauseTime, string startTime, string endTime, int userId, int taskId, int docTypeId)
        {
            var db = _trexContextProvider.TrexEntityContext;

            var input = string.Format(CultureInfo.InvariantCulture, "SET IDENTITY_INSERT TimeEntries ON " +
                "INSERT INTO TimeEntries (TimeEntryID, TaskID, UserID, StartTime, EndTime, PauseTime, BillableTime, " +
                                      "Billable, Price, TimeSpent, InvoiceId, Guid, TimeEntryTypeId, ClientSourceId, DocumentType) " +
                "VALUES({0}, {1}, {2}, N'{3}', N'{4}', {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}) " +
                "SET IDENTITY_INSERT TimeEntries OFF",
                timeEntryId,
                taskId,
                userId,
                DateTime.Parse(startTime),
                DateTime.Parse(endTime),
                pauseTime,
                billableTime,
                (billable == false ? 0 : 1),
                price,
                timeSpent,
                (invoiceId == null ? "null" : invoiceId.ToString()),
                "NEWID()",
                1,
                1,
                docTypeId);

            db.ExecuteStoreCommand(input);

            return db.TimeEntries.First(te => te.TimeEntryID == timeEntryId);
        }

        public Task CreateTask(int taskId, int projectId, int userId, string createdDate, string taskName)
        {
            var db = _trexContextProvider.TrexEntityContext;

            var input = string.Format(CultureInfo.InvariantCulture, "SET IDENTITY_INSERT Tasks ON " +
                "INSERT INTO Tasks (TaskID, Guid, ProjectID, CreatedBy, ModifyDate, CreateDate, TaskName, TimeEstimated, TimeLeft, Closed, WorstCaseEstimate, BestCaseEstimate, RealisticEstimate, Inactive) " +
                "VALUES({0}, {1}, {2}, {3}, N'{4}', N'{5}', '{6}', {7}, {8}, {9}, {10}, {11}, {12}, {13}) " +
                "SET IDENTITY_INSERT Tasks OFF",
                taskId, "NEWID()", projectId, userId, DateTime.Parse(createdDate), DateTime.Parse(createdDate), taskName, 10, 2, 0, 20, 5, 10, 0);

            db.ExecuteStoreCommand(input);

            return db.Tasks.First(t => t.TaskID == taskId);
        }

        public Project CreateProject(int projId, int customerId, string name, int userId, string createdDate, bool fixedProject, double? fixedPrice, int cigId)
        {
            var db = _trexContextProvider.TrexEntityContext;
            string input = null;
            if (fixedPrice != null)
            {
                input = string.Format("SET IDENTITY_INSERT Projects ON " +
                    "INSERT INTO Projects (ProjectID, Guid, CustomerID, ProjectName, CreatedBy, CreateDate, FixedPriceProject, FixedPrice, CustomerInvoiceGroupID, Inactive) " +
                    "VALUES({0}, {1}, {2}, '{3}', {4}, N'{5}', {6}, {7}, {8}, {9}) " +
                    "SET IDENTITY_INSERT Projects OFF",
                    projId, "NEWID()", customerId, name, userId, DateTime.Parse(createdDate), (fixedProject == false ? 0 : 1), fixedPrice, cigId, 0);
            }
            else
            {
                input = string.Format("SET IDENTITY_INSERT Projects ON " +
                "INSERT INTO Projects (ProjectID, Guid, CustomerID, ProjectName, CreatedBy, CreateDate, FixedPriceProject, CustomerInvoiceGroupID, Inactive) " +
                "VALUES({0}, {1}, {2}, '{3}', {4}, N'{5}', {6}, {7}, {8}) " +
                "SET IDENTITY_INSERT Projects OFF",
                projId, "NEWID()", customerId, name, userId, DateTime.Parse(createdDate), (fixedProject == false ? 0 : 1), cigId, 0);
            }


            db.ExecuteStoreCommand(input);

            return db.Projects.First(c => c.ProjectID == projId);
        }

        public CustomerInvoiceGroup CreateCustomerInvoiceGroup(int id, int customerId, string label, bool defaultCig, int sendFormat)
        {
            var db = _trexContextProvider.TrexEntityContext;

            var input = string.Format("SET IDENTITY_INSERT CustomerInvoiceGroup ON " +
                "INSERT INTO CustomerInvoiceGroup (CustomerInvoiceGroupID, CustomerID, Label, DefaultCig, SendFormat) " +
                "VALUES({0}, {1}, '{2}', {3}, '{4}') " +
                "SET IDENTITY_INSERT CustomerInvoiceGroup OFF",
                id, customerId, label, (defaultCig == false ? 0 : 1), sendFormat);

            db.ExecuteStoreCommand(input);

            return db.CustomerInvoiceGroups.First(c => c.CustomerInvoiceGroupID == id);
        }

        public CustomerInvoiceGroup CreateCustomerInvoiceGroup(int id, int customerId, string label, bool defaultCig, int sendFormat, string email, string emailCC, string attention, string address1, string address2, string city, string country, string zipcode)
        {
            var db = _trexContextProvider.TrexEntityContext;

            var input = string.Format("SET IDENTITY_INSERT CustomerInvoiceGroup ON " +
                                      "INSERT INTO CustomerInvoiceGroup (CustomerInvoiceGroupID, CustomerID, Label, DefaultCig, SendFormat, " +
                                      "Email, EmailCC, Attention, Address1, Address2, City, Country, ZipCode) " +
                                      "VALUES({0}, {1}, '{2}', {3}, '{4}', {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}) " +
                                      "SET IDENTITY_INSERT CustomerInvoiceGroup OFF",
                                      id, customerId, label, (defaultCig == false ? 0 : 1), sendFormat,
                                      (email != null ? string.Format("'{0}'", email) : "NULL"),
                                      (emailCC != null ? string.Format("'{0}'", emailCC) : "NULL"),
                                      (attention != null ? string.Format("'{0}'", attention) : "NULL"),
                                      (address1 != null ? string.Format("'{0}'", address1) : "NULL"),
                                      (address2 != null ? string.Format("'{0}'", address2) : "NULL"),
                                      (city != null ? string.Format("'{0}'", city) : "NULL"),
                                      (country != null ? string.Format("'{0}'", country) : "NULL"),
                                      (zipcode != null ? string.Format("'{0}'", zipcode) : "NULL"));

            db.ExecuteStoreCommand(input);

            return db.CustomerInvoiceGroups.First(c => c.CustomerInvoiceGroupID == id);
        }

        public User CreateUser(int userId, string userName, double price)
        {
            var db = _trexContextProvider.TrexEntityContext;

            var input = string.Format(CultureInfo.InvariantCulture, "SET IDENTITY_INSERT Users ON " +
                        "INSERT INTO Users (UserID, Price, UserName, Inactive)" +
                        "VALUES({0}, {1}, '{2}', {3}) " +
                        "SET IDENTITY_INSERT Users OFF",
                        userId,
                        price,
                        userName,
                        0);

            db.ExecuteStoreCommand(input);

            return db.Users.First(u => u.UserID == userId);
        }

        public Customer CreateCustomer(int customerId, string customerName, string email, string contactName, string streetName, string city, string country, string zipCode)
        {
            var db = _trexContextProvider.TrexEntityContext;
            var input = string.Format("SET IDENTITY_INSERT Customers ON " +
                "INSERT INTO Customers (CustomerID, Email, Guid, StreetAddress, City, ZipCode, Country, " +
                "ContactName, CreateDate, CreatedBy, CustomerName, Inactive, InheritsTimeEntryTypes, PaymentTermsIncludeCurrentMonth, " +
                "PaymentTermsNumberOfDays, SendFormat, EMailCC, UserId, Internal) VALUES({0}, {1}, {2}, '{3}', '{4}', '{5}', '{6}', '{7}', N'{8}', {9}, '{10}', {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}) " +
                "SET IDENTITY_INSERT Customers OFF",
                customerId,
                (email != null ? string.Format("'{0}'", email) : "NULL"),
                "NEWID()",
                streetName,
                city,
                zipCode,
                country,
                contactName,
                DateTime.Parse("01-01-2012"),
                1,
                customerName,
                0,
                1,
                0,
                14,
                1,
                "NULL",
                1,
                1);

            db.ExecuteStoreCommand(input);
            var g = db.Customers.First(c => c.CustomerID == customerId);
            return g;
        }

        public UsersCustomer CreateUserCustomerConnection(int customerId, int userId, double price)
        {
            var db = _trexContextProvider.TrexEntityContext;

            var input = string.Format(CultureInfo.InvariantCulture, "SET IDENTITY_INSERT UsersCustomers ON " +
                        "INSERT INTO UsersCustomers (CustomerID, Price, UserID)" +
                        "VALUES({0}, {1}, {2}) " +
                        "SET IDENTITY_INSERT UsersCustomers OFF",
                        customerId,
                        price,
                        userId);

            db.ExecuteStoreCommand(input);

            return db.UsersCustomers.First(u => u.UserID == userId);
        }
    }
}