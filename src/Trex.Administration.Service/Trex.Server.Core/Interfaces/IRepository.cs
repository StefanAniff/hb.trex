using System;
using System.Collections.Generic;
using System.Linq;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public interface IRepository
    {
        Customer GetCustomerById(int customerId);
        List<Project> GetProjectsWithCustomerIdAndTasks(List<Task> ts, int customerId);
        List<Task> GetTasks(List<TimeEntry> timeEntries);
        List<TimeEntry> GetTimeEntries(DateTime startDate, DateTime endDate);
        List<TimeEntry> GetTimeEntriesByInvoiceAndTimespan(Invoice invoiceId, int projectID, DateTime startDate, DateTime endDate);
        List<TimeEntry> GetTimeEntriesByInvoice(Invoice input);

        void SetTimeEntrysInvoiceId(TimeEntry entry, int invoiceId);
        void InsertInvoiceInDatabase(Invoice input);
        void InsertInvoiceLinesInDatabase(Invoice input, List<TimeEntry> timeEntries, int projectId);
    }
}