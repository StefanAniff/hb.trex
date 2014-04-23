using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Trex.ServiceContracts;

namespace Trex.Server.Core.Services
{
    public interface IInvoiceService
    {
        List<TimeEntry> GetInvoiceDataByInvoiceId(int invoiceId);
        List<InvoiceLine> GetInvoiceLinesByInvoiceId(int invoiceId);
        List<string> ValidateFinalize(int invoiceId);
        ObservableCollection<InvoiceListItemView> GetInvoicesByCustomerId(ObservableCollection<int> customerIds);
        CustomerInvoiceGroup GetInvoiceMetaData(int cigId);
        List<CreditNote> GetFinalizedInvoiceDataByInvoiceId(int invoiceId);
        int CalculateNextInvoiceId(int invoiceId, bool isPreview);
        void DeleteInvoiceLine(int invoiceLineId);
        Invoice SaveInvoice(Invoice invoice);
        ServerResponse SaveInvoiceChanges(InvoiceListItemView invoicedata);
        void CreateNewInvoiceLine(int invoiceId, double VAT);
        void GenerateInvoiceDraft(DateTime startDate, DateTime endDate, int customerId, int userId, float VAT);
        List<FixedProjects> GenerateInvoiceLines(int invoiceId);
        List<TimeEntry> GetTimeEntriesForInvoicing(DateTime startDate, DateTime endDate, int custumerInvoiceGroupId);
        IEnumerable<GetSpecificationData_Project_Result> GetSpecificationDataProject(int invoiceId, bool fixedProject);
        IEnumerable<GetSpecificationData_Tasks_Result> GetSpecificationDataTasks(int invoiceId, bool fixedProject);
        CustomerInvoiceGroup GetInvoiceTemplateByInvoiceId(int invoiceId, int format);
        CustomerInvoiceGroup GetCustomerInvoiceGroupsTemplateData(int id, int invoiceId);
        InvoiceLine SaveInvoiceLine(InvoiceLine invoiceLine);
        InvoiceTemplate GetTemplateById(int templateId);
        Invoice GetInvoiceById(int invoiceId);
        bool InvoiceFileCreated(int id);
        bool InvoiceNumberGiven(int invoiceId);
        void RollBack(int invoiceId);
        void DeleteInvoiceById(int invoiceId);
        void ReleaseTimeEntries(int invoiceId);
        void UpdateTimeEntries(int invoiceId, DateTime startDate, DateTime endDate, int customerInvoiceGroupId);
        Invoice ReCalculateInvoice(Invoice invoice);
        bool ValidateCreditNote(int invoiceId);
        int GenerateCreditNote(int oldInvoiceId, int userId);
        double? UpdateExclVAT(int invoiceId);
        List<int?> GetAllInvoiceIDs();
        InvoiceListItemView GetInvoiceByInvoiceId(int invoiceId);
        void ResetInvoiceId(int invoiceId);
        void CopyTimeEntries(int invoiceId);
        void UpdateDeliveredDate(int invoiceId);
        void UpdateTimeEntriesPricePrHour(IEnumerable<FixedProjects> fixedProjects, int invoiceId);
        List<InvoiceListItemView> GetDebitList();
        CustomerInvoiceGroup GetCustomerInvoiceGroupByInvoiceId(int id);
        Customer GetCustomerByInvoiceId(int invoiceId);
        void UpdateInvoiceAttention(int invoiceId, int customerInvoiceGroupId);
        ServerResponse SaveInvoiceComment(string comment, int invoiceID, int userID);
        List<InvoiceComment> LoadInvoiceComment(int invoiceId);
    }
}
