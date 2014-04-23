using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class Repository : IRepository
    {
        private readonly TrexEntities entityContext;

        public Repository(ITrexContextProvider contextProvider)
        {
            entityContext = contextProvider.TrexEntityContext;
        }

        public Customer GetCustomerById(int customerId)
        {
            var cust = (from c in entityContext.Customers
                        select c);

            return cust.SingleOrDefault(c => c.CustomerID == customerId);
        }

        public List<Project> GetProjectsWithCustomerIdAndTasks(List<Task> ts, int customerId)
        {
            //Find project belonging to given customerId
            var projects = (from p in entityContext.Projects
                            where p.CustomerID == customerId
                            select p).ToList();

            var tmp = new List<Project>();

            //Filter away those projects who does not have the listed tasks
            foreach (var pro in projects)
            {
                //Check if each task has the project's ID
                foreach (var task in ts)
                {
                    if (task.ProjectID == pro.ProjectID && !tmp.Contains(pro))
                    {
                        tmp.Add(pro);
                    }
                }
            }
            return tmp;
        }

        public List<Task> GetTasks(List<TimeEntry> timeEntries)
        {
            //Find all tasks
            var tasks = (from t in entityContext.Tasks
                         select t).ToList();

            var tmp = new List<Task>();

            //Check all tasks
            foreach (var task in tasks)
            {
                //Check all TimeEntries for maching TaskId for current task
                foreach (var timeEntry in timeEntries)
                {
                    if (task.TaskID == timeEntry.TaskID && !tmp.Contains(task))
                    {
                        tmp.Add(task);
                    }
                }
            }

            return tmp;
        }

        public List<TimeEntry> GetTimeEntries(DateTime startDate, DateTime endDate)
        {
            return entityContext.TimeEntries.Where(
                i => i.StartTime >= startDate
                     && i.EndTime <= endDate
                     && i.InvoiceId == null
                     && i.Billable == true).ToList();
        }

        public List<TimeEntry> GetTimeEntriesByInvoiceAndTimespan(Invoice invoice, int projectID, DateTime startDate, DateTime endDate)
        {
            //Find Project connected to Invoice
            var projects = (from project in entityContext.Projects
                            where project.CustomerInvoiceGroupID == invoice.CustomerInvoiceGroupId && projectID == project.ProjectID
                            select project);

            //Find Tasks connected to Invoice
            var tasks = (from task in entityContext.Tasks
                         join pro in projects on task.ProjectID equals pro.ProjectID
                         select task);

            //Find TimeEntries connected to Invoice
            var te = (from tiEn in entityContext.TimeEntries
                      join t in tasks on tiEn.TaskID equals t.TaskID
                      where tiEn.InvoiceId == null
                      && tiEn.Billable == true
                      && tiEn.StartTime >= startDate
                      && tiEn.EndTime <= endDate
                      select tiEn);

            return te.ToList();
        }

        public void SetTimeEntrysInvoiceId(TimeEntry entry, int invoiceId)
        {
            var te = entityContext.TimeEntries.SingleOrDefault(x => x.TimeEntryID == entry.TimeEntryID);

            te.InvoiceId = invoiceId;

            entityContext.TimeEntries.ApplyChanges(te);
            entityContext.SaveChanges();
        }

        public void InsertInvoiceInDatabase(Invoice input)
        {
            entityContext.Invoices.ApplyChanges(input);
            entityContext.SaveChanges();
        }

        public void InsertInvoiceLinesInDatabase(Invoice input, List<TimeEntry> timeEntries, int projectId)
        {
            //Beregn hvor mange forskellige priser der er
            var allETs = GetTimeEntriesByInvoice(input);

            //Opret ny invoiceLine for hver af dem
            var ETLines = new Dictionary<double, double>();

            //Add distinct prices to a dictionary
            foreach (var te in allETs)
            {
                if (!ETLines.ContainsKey(te.Price))
                    ETLines.Add(te.Price, te.BillableTime);

                else
                    ETLines[te.Price] += te.BillableTime;
            }

            //Indsæt data i dem og bind til relevant Invoice
            foreach (var distincts in ETLines)
            {
                var newLine = new InvoiceLine
                                  {
                                      InvoiceID = input.ID,
                                      PricePrUnit = distincts.Key,
                                      Units = distincts.Value,
                                      Unit = "timer",
                                      UnitType = 0,
                                      VatPercentage = input.VAT,
                                      IsExpense = true,
                                      Text = string.Empty
                                  };

                entityContext.InvoiceLines.ApplyChanges(newLine);
                entityContext.SaveChanges();
            }

        }

        public List<TimeEntry> GetTimeEntriesByInvoice(Invoice input)
        {
            return (from ts in entityContext.TimeEntries
                    where ts.InvoiceId == input.ID
                    select ts).ToList();
        }
    }
}