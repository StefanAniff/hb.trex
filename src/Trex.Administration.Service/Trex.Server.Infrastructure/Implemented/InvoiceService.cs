#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects;
using System.Linq;
using Aspose.Words;
using StructureMap;
using Test_InvoiceBuilder;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.Server.Infrastructure.Exceptions;
using Trex.ServiceContracts;
using Trex.ServiceContracts.Model;

#endregion

namespace Trex.Server.Infrastructure.Implemented
{
    public class InvoiceService : LogableBase, IInvoiceService
    {
        private readonly ITrexContextProvider _trexContextProvider;

        public InvoiceService(ITrexContextProvider trexContextProvider)
        {
            _trexContextProvider = trexContextProvider;
        }

        #region IInvoiceService Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoicedata"></param>
        /// <returns></returns>
        public ServerResponse SaveInvoiceChanges(InvoiceListItemView invoicedata)
        {
            using (var db = _trexContextProvider.TrexEntityContext)
            {
                var invoice = (from i in db.Invoices
                               where i.ID == invoicedata.ID
                               select i).First();
                
                invoice.InvoiceDate = invoicedata.InvoiceDate;
                invoice.EndDate = invoicedata.EndDate;
                invoice.DueDate = invoicedata.DueDate;
                invoice.Regarding = invoicedata.Regarding;
                invoice.StartDate = invoicedata.StartDate;
                invoice.Delivered = invoicedata.Delivered;
                invoice.DeliveredDate = invoicedata.DeliveredDate;
                invoice.Closed = invoicedata.Closed;
                invoice.Attention = invoicedata.attention;
                invoice.AmountPaid = invoicedata.AmountPaid;
                db.Invoices.ApplyChanges(invoice);
                db.SaveChanges();

                return new ServerResponse("Invoice succesfully saved", true);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public Invoice SaveInvoice(Invoice invoice)
        {
            using (var db = _trexContextProvider.TrexEntityContext)
            {
                db.Invoices.ApplyChanges(invoice);
                db.SaveChanges();
            }

            return invoice;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoiceLine"></param>
        /// <returns></returns>
        public InvoiceLine SaveInvoiceLine(InvoiceLine invoiceLine)
        {
            invoiceLine.ChangeTracker.ChangeTrackingEnabled = true;
            using (var db = _trexContextProvider.TrexEntityContext)
            {
                if(invoiceLine.ID == 0)
                    invoiceLine.ChangeTracker.State = ObjectState.Added;
                else
                    invoiceLine.ChangeTracker.State = ObjectState.Modified;
                db.InvoiceLines.ApplyChanges(invoiceLine);
                db.SaveChanges();
            }
            return invoiceLine;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="custumerInvoiceGroupId"></param>
        /// <returns></returns>
        public List<TimeEntry> GetTimeEntriesForInvoicing(DateTime startDate, DateTime endDate, int custumerInvoiceGroupId)
        {
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            using (var db = _trexContextProvider.TrexEntityContext)
            {
                var notBookedTimeEntries = (from te in db.TimeEntries
                                            where te.Billable
                                                  && te.StartTime >= startDate.Date
                                                  && te.EndTime <= endDate
                                                  && te.Task.Project.CustomerInvoiceGroupID == custumerInvoiceGroupId
                                                  && te.InvoiceId == null
                                            select te);

            return notBookedTimeEntries.ToList();
        }

    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        public ObservableCollection<InvoiceListItemView> GetInvoicesByCustomerId(ObservableCollection<int> customerIds)
        {
            string tmpList = "";
            foreach (var id in customerIds)
            {
                tmpList += id.ToString() + ",";
            }

            using (var db = _trexContextProvider.TrexEntityContext)
            {
                var tmp = db.InvoiceListItemView(tmpList);

                var temp = new ObservableCollection<InvoiceListItemView>();

                foreach (var invoiceListItemView in tmp)
                {
                    temp.Add(invoiceListItemView);
                }

                return temp;
            }

        }

        /// <summary>
        /// Recalculate an invoice, updating it time entries and invoicelines
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public Invoice ReCalculateInvoice(Invoice invoice)
        {
            //Remove any previous invoice lines with hours
            for (int i = invoice.InvoiceLines.Count - 1; i >= 0; i--)
            {
                if (invoice.InvoiceLines[i].UnitType == (int)UnitTypes.Hours)
                {
                    invoice.InvoiceLines[i].MarkAsDeleted();
                }
            }
            using (var db = _trexContextProvider.TrexEntityContext)
            {
                db.Invoices.ApplyChanges(invoice);
                db.SaveChanges();

                return _trexContextProvider.TrexEntityContext.Invoices
                    .Include("TimeEntries")
                    .Include("InvoiceLines")
                    .SingleOrDefault(i => i.ID == invoice.ID);
            }
        }

        /// <summary>
        /// Returns an invoice's TimeEntries, Tasks and Projects
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <returns>List of TimeEntries and navigation properties to Task and Project</returns>
        public List<TimeEntry> GetInvoiceDataByInvoiceId(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var invoice = (from te in entity.TimeEntries.Include("Task").Include("Task.Project")
                               where te.InvoiceId == invoiceId
                               select te);

                return invoice.ToList();
            }
        }

        /// <summary>
        /// Get a list with all the invoices with a duedate smaller then today, and are open
        /// </summary>
        /// <returns>A List of InvoiceListItemView</returns>
        public List<InvoiceListItemView> GetDebitList()
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var b = (from c in entity.Customers
                         where c.Inactive == false
                         select c.CustomerID);

                var t = Enumerable.Aggregate(b, "", (current, customerId) => current + (customerId.ToString() + ','));

                var i = entity.InvoiceListItemView(t);
                var g = i.Where(d => d.DueDate < DateTime.Now && d.Closed == false);

                return g.ToList();
            }
        }

        /// <summary>
        /// Returns data from CreditNote for a finalized invoice
        /// </summary>
        /// <param name="invoiceId">The invoice/credit note's ID</param>
        /// <returns>List of CreditNote with navigation properties to TimeEntries, Task and Project</returns>
        public List<CreditNote> GetFinalizedInvoiceDataByInvoiceId(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var invoice = (from cn in entity.CreditNote.Include("TimeEntries.Task.Project")
                               where cn.InvoiceID == invoiceId
                               select cn);

                return invoice.ToList();
            }
        }

        /// <summary>
        /// Returns a invoice, from a invoice id, with customerInvoiceGroup, Customer and InvoiceLiens
        /// </summary>
        /// <param name="invoiceId">The invoices Invoice.ID</param>
        /// <returns>The invoice with the param invoice.ID</returns>
        public Invoice GetInvoiceById(int invoiceId)
        {
            try
            {
                using (var entity = _trexContextProvider.TrexEntityContext)
                {
                    return entity.Invoices
                        .Include("CustomerInvoiceGroup")
                        .Include("CustomerInvoiceGroup.Customer")
                        .Include("InvoiceLines")
                        .SingleOrDefault(i => i.ID == invoiceId);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// Finds all InvoiceLines for the invoice and deletes them, and sets all TimeEntries InvoiceID = null and DocumentType = 1
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        public void ReleaseTimeEntries(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var lines = (from il in entity.InvoiceLines
                             where il.InvoiceID == invoiceId && il.UnitType == 0 //Autogenerated
                             select il);

                foreach (var invoiceLine in lines)
                {
                    entity.InvoiceLines.DeleteObject(invoiceLine);
                }
                entity.SaveChanges();

                var entries = (from te in entity.TimeEntries
                               where te.InvoiceId == invoiceId
                               select te);

                foreach (var timeEntry in entries)
                {
                    timeEntry.InvoiceId = null;
                    timeEntry.DocumentType = 1;
                    entity.TimeEntries.ApplyCurrentValues(timeEntry);
                }
                entity.SaveChanges();
            }
        }

        /// <summary>
        /// Updates the TimeEntries to a specific InvoiceId within a periode for a CustomerInvoiceGroup
        /// </summary>
        /// <param name="invoiceId">Wanted InvoiceId</param>
        /// <param name="startDate">Start periode</param>
        /// <param name="endDate">End periode</param>
        /// <param name="customerInvoiceGroupId">CustomerInvoiceGroup ID to assign TimeEntries to</param>
        public void UpdateTimeEntries(int invoiceId, DateTime startDate, DateTime endDate, int customerInvoiceGroupId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                entity.UpdateTimeEntriesInvoiceId(customerInvoiceGroupId, startDate, endDate, invoiceId);
            }
        }

        /// <summary>
        /// Returns an invoice's InvoiceLines
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <returns>Returns a list of InvoiceLines</returns>
        public List<InvoiceLine> GetInvoiceLinesByInvoiceId(int invoiceId)
        {
            try
            {
                using (var entity = _trexContextProvider.TrexEntityContext)
                {
                    var invoiceLines = entity.InvoiceLines.Include("Invoice").Where(i => i.InvoiceID == invoiceId);
                    return invoiceLines.ToList();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Get inherited data for a CustomerInvoiceGroup
        /// </summary>
        /// <param name="cigId">The CustomerInvoiceGroup's ID</param>
        /// <returns>Returns the inherited data as CustomerInvoiceGroup</returns>
        public CustomerInvoiceGroup GetInvoiceMetaData(int cigId)
        {
            try
            {
                using (var entity = _trexContextProvider.TrexEntityContext)
                {
                    var group = (from cig in entity.CustomerInvoiceGroups
                                 join c in entity.Customers on cig.CustomerID equals c.CustomerID
                                 where (cig.CustomerInvoiceGroupID == cigId)
                                 select new { CustomerInvoiceGroup = cig, cig.Customer }).First();

                    var tmp = new CustomerInvoiceGroup();
                    tmp.Address1 = (group.CustomerInvoiceGroup.Address1 ?? group.Customer.StreetAddress);
                    tmp.Address2 = (group.CustomerInvoiceGroup.Address2 ?? group.Customer.Address2);

                    tmp.Attention = (group.CustomerInvoiceGroup.Attention ?? group.Customer.ContactName);

                    tmp.City = (group.CustomerInvoiceGroup.City ?? group.Customer.City);
                    tmp.Country = (group.CustomerInvoiceGroup.Country ?? group.Customer.Country);
                    tmp.CustomerID = group.CustomerInvoiceGroup.CustomerID;
                    tmp.ZipCode = group.CustomerInvoiceGroup.ZipCode ?? group.Customer.ZipCode;

                    return tmp;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes an InvoiceLine if it is not autogenerated
        /// </summary>
        /// <param name="invoiceLineId">InvoiceLine's ID</param>
        public void DeleteInvoiceLine(int invoiceLineId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var lines = (from il in entity.InvoiceLines
                             where il.ID == invoiceLineId && il.UnitType != 0
                             select il);

                foreach (var il in lines)
                {
                    entity.InvoiceLines.DeleteObject(il);
                }
                entity.SaveChanges();
            }
        }

        /// <summary>
        /// Generates InvoiceLines for an invoice
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        public List<FixedProjects> GenerateInvoiceLines(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                return entity.GenerateInvoiceLines(invoiceId, true, "decimal hours").ToList();
            }
        }

        /// <summary>
        /// Creates InvoiceLines marked as 'manual' for an invoice
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <param name="VAT">Amount of VAT on the InvoiceLine</param>
        public void CreateNewInvoiceLine(int invoiceId, double VAT)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var invoiceLine = new InvoiceLine
                                      {
                                          Text = "",
                                          PricePrUnit = 0,
                                          InvoiceID = invoiceId,
                                          Units = 0,
                                          UnitType = 1,
                                          SortIndex = 0,
                                          IsExpense = false,
                                          VatPercentage = VAT,
                                          Unit = ""
                                      };

                entity.InvoiceLines.ApplyChanges(invoiceLine);
                entity.SaveChanges();
            }
        }

        /// <summary>
        /// Returns the CustomerInvoiceGroup's data with a specific SendFormat for InvoiceTemplate for an invoice 
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <param name="format">The send format</param>
        /// <returns>CustomerInvoiceGroup and navigation properties to Invoice and Customer</returns>
        public CustomerInvoiceGroup GetInvoiceTemplateByInvoiceId(int invoiceId, int format)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var tmp = (from cig in entity.CustomerInvoiceGroups
                           join i in entity.Invoices on cig.CustomerInvoiceGroupID equals i.CustomerInvoiceGroupId
                           join c in entity.Customers on cig.CustomerID equals c.CustomerID
                           where i.ID == invoiceId && c.SendFormat == format
                           select cig).First();
                return tmp;
            }
        }

        /// <summary>
        /// Returns an InvoiceTemplate by its ID
        /// </summary>
        /// <param name="templateId">The template's ID</param>
        /// <returns>Return an InvoiceTemplate</returns>
        public InvoiceTemplate GetTemplateById(int templateId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                return entity.InvoiceTemplates.SingleOrDefault(it => it.TemplateId == templateId);
            }
        }

        /// <summary>
        /// Returns a CustomerInvoiceGroup with inherited template data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <returns>CustomerInvoiceGroup with inherited template data</returns>
        public CustomerInvoiceGroup GetCustomerInvoiceGroupsTemplateData(int id, int format)
        {
            try
            {
                using (var entity = _trexContextProvider.TrexEntityContext)
                {
                    var data = (from cig in entity.CustomerInvoiceGroups
                                join i in entity.Invoices on cig.CustomerInvoiceGroupID equals i.CustomerInvoiceGroupId
                                join c in entity.Customers on cig.CustomerID equals c.CustomerID
                                where i.ID == id
                                select cig).First();

                    ValidateFiles(format);

                    var tmp = new CustomerInvoiceGroup();
                    tmp = data;

                    if (format == 1)
                    {
                        var standardInvoiceToMail = entity.InvoiceTemplates.First(x => x.StandardInvoiceMail).TemplateId;
                        tmp.InvoiceTemplateIdMail = (data.InvoiceTemplateIdMail ?? standardInvoiceToMail);
                    }

                    if (format == 2)
                    {
                        var standardInvoiceToPrint = entity.InvoiceTemplates.First(x => x.StandardInvoicePrint).TemplateId;
                        tmp.InvoiceTemplateIdPrint = (data.InvoiceTemplateIdPrint ?? standardInvoiceToPrint);
                    }

                    if (format == 3)
                    {
                        var standardSpecificationToMail = entity.InvoiceTemplates.First(x => x.StandardSpecification).TemplateId;
                        tmp.SpecificationTemplateIdMail = (data.SpecificationTemplateIdMail ?? standardSpecificationToMail);
                    }

                    if (format == 4)
                    {
                        var standardCreditNoteToMail = entity.InvoiceTemplates.First(x => x.StandardCreditNoteMail).TemplateId;
                        tmp.CreditNoteTemplateIdMail = (data.CreditNoteTemplateIdMail ?? standardCreditNoteToMail);
                    }
                    if (format == 5)
                    {
                        var standardCreditNoteToPrint = entity.InvoiceTemplates.First(x => x.StandardCreditNotePrint).TemplateId;
                        tmp.CreditNoteTemplateIdPrint = (data.CreditNoteTemplateIdPrint ?? standardCreditNoteToPrint);
                    }

                    return tmp;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Validates if the system has all assigned templates.
        /// <exception cref="InvoiceMailMissing"></exception>
        /// <exception cref="InvoicePrintMissing"></exception>
        /// <exception cref="SpecificationPrintMissing"></exception>
        /// <exception cref="CreditNoteMailMissing"></exception>
        /// <exception cref="CreditNotePrintMissing"></exception>
        /// </summary>
        private void ValidateFiles(int format)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                bool invoicePrint = entity.InvoiceTemplates.Any(x => x.StandardInvoicePrint == true);
                bool invoiceMail = entity.InvoiceTemplates.Any(x => x.StandardInvoiceMail == true);
                bool SpecificationPrint = entity.InvoiceTemplates.Any(x => x.StandardSpecification == true);
                bool CreditNoteMail = entity.InvoiceTemplates.Any(x => x.StandardCreditNoteMail == true);
                bool CreditNotePrint = entity.InvoiceTemplates.Any(x => x.StandardCreditNotePrint == true);

                if (!invoiceMail && format == 1)
                    throw new InvoiceMailMissing();

                if (!invoicePrint && format == 2)
                    throw new InvoicePrintMissing();

                if (!SpecificationPrint && format == 3)
                    throw new SpecificationPrintMissing();

                if (!CreditNoteMail && format == 4)
                    throw new CreditNoteMailMissing();

                if (!CreditNotePrint && format == 5)
                    throw new CreditNotePrintMissing();
            }
        }

        #endregion

        #region Generate files

        /// <summary>
        /// Returns the specification project data (ProjectID, ProjectName and sum billable time of the project) for a project
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <param name="fixedProject">Take projects with fixed prices</param>
        /// <returns>Returns a list of the specifications project data</returns>
        public IEnumerable<GetSpecificationData_Project_Result> GetSpecificationDataProject(int invoiceId, bool fixedProject)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                return entity.GetSpecificationData_Project(invoiceId, true, fixedProject).ToList();
            }
        }

        /// <summary>
        /// Returns the specification task data (ProjectID, TaskName and sum billable time for each task) for a project
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <param name="fixedProject">Take projects with fixed prices</param>
        /// <returns>Returns a list of the specifications task data</returns>
        public IEnumerable<GetSpecificationData_Tasks_Result> GetSpecificationDataTasks(int invoiceId, bool fixedProject)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                return entity.GetSpecificationData_Tasks(invoiceId, true, fixedProject).ToList();
            }
        }

        /// <summary>
        /// Finds the next InvoiceID for an invoice/credit note and updates the invoice with the new ID
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <param name="isPreview">Indicates whether to give a new ID or not</param>
        /// <returns>Will return null or the same InvoiceID if <see cref="isPreview"/> = true, will return the next InvoiceID if <see cref="isPreview"/> = false and save it in the database</returns>
        public int CalculateNextInvoiceId(int invoiceId, bool isPreview)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var lastInvoiceId = (from i in entity.Invoices
                                     select i).Max(i => i.InvoiceID);

                if ((isPreview && InvoiceFileCreated(invoiceId)) || isPreview)
                    return (from i in entity.Invoices
                            where i.ID == invoiceId
                            select i).First().ID;

                if (lastInvoiceId == null)
                    lastInvoiceId = 0;

                else
                    lastInvoiceId++;

                var invoice = (from i in entity.Invoices
                               where i.ID == invoiceId
                               select i).First();

                invoice.InvoiceID = lastInvoiceId;
                entity.Invoices.ApplyChanges(invoice);
                entity.SaveChanges();

                return (int)lastInvoiceId;
            }
        }

        /// <summary>
        /// Reset the invoice's InvoiceId
        /// </summary>
        /// <param name="invoiceId">The invoice's id</param>
        public void ResetInvoiceId(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var invoice = entity.Invoices.First(i => i.ID == invoiceId);
                invoice.InvoiceID = null;
                entity.Invoices.ApplyCurrentValues(invoice);
                entity.SaveChanges();
            }
        }

        /// <summary>
        /// Validates if a file is already created and if the invoice has an InvoiceID
        /// </summary>
        /// <param name="id">The invoice's ID</param>
        /// <returns>True if it is created, false if either isn't true</returns>
        public bool InvoiceFileCreated(int id)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                return (from file in entity.InvoiceFiles
                        where file.InvoiceID == id
                        select file).Any();
            }
        }

        /// <summary>
        /// Returns whether or not the invoice's InvoiceId exists
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <returns>Finalized or not</returns>
        public bool InvoiceNumberGiven(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                return entity.Invoices.Any(x => x.ID == invoiceId && x.InvoiceID != null);
            }
        }

        /// <summary>
        /// Returns whether the invoice is a credit not or not
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <returns>Returns whether the invoice is a credit not or not</returns>
        public bool ValidateCreditNote(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                return (from i in entity.Invoices
                        where i.ID == invoiceId
                        select i).First().IsCreditNote;
            }
        }

        /// <summary>
        /// Generates a draft if any TimeEntries can be found within the specified dates for the customer
        /// </summary>
        /// <param name="startDate">Start of periode. This will be shortned to the date only</param>
        /// <param name="endDate">End of periode. This will be to the end of that day, eg: 23-01-2000 10:30 => 23-01-2000 23:59</param>
        /// <param name="customerId">Customers ID</param>
        /// <param name="userId">User ID to note who created the draft</param>
        /// <param name="VAT">The VAT for the invoice</param>
        public void GenerateInvoiceDraft(DateTime startDate, DateTime endDate, int customerId, int userId, float VAT)
        {
            var startDateShort = startDate.Date;
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var cigData = (from cig in entity.CustomerInvoiceGroups
                               where cig.CustomerID == customerId
                               select cig).ToList();

                foreach (var groups in cigData)
                {
                    bool foundTimeEntries =
                        entity.FindTimeEntriesForInvoice(groups.CustomerInvoiceGroupID, startDateShort, endDate).Any();

                    if (foundTimeEntries)
                    {
                        var invoiceData = entity.GenerateNewInvoiceDraft(groups.CustomerInvoiceGroupID, userId, VAT, startDateShort, endDate);

                        int invoiceId = invoiceData.First().ID;

                        entity.UpdateTimeEntriesInvoiceId(groups.CustomerInvoiceGroupID, startDateShort, endDate, invoiceId);

                        var fixedProjects = GenerateInvoiceLines(invoiceId);

                        UpdateTimeEntriesPricePrHour(fixedProjects, invoiceId);
                    }
                }
            }
        }

        /// <summary>
        /// Generates a Credit note (invoice) copies the old invoice's InvoiceLines and CreditNote lines and reverse them ( x*(-1)) and set DocumentType for all affected TimeEntries = 3
        /// </summary>
        /// <param name="oldInvoiceId">The invoice ID the credit note is generated from</param>
        /// <param name="userId">The user ID</param>
        /// <returns>Returns the new id for the credit note</returns>
        public int GenerateCreditNote(int oldInvoiceId, int userId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var oldInvoice = (from i in entity.Invoices
                                  where i.ID == oldInvoiceId
                                  select i).First();

                entity.Invoices.AddObject(new Invoice
                {
                    Closed = true,
                    CreateDate = DateTime.Now,
                    CreatedBy = userId,
                    CustomerInvoiceGroupId = oldInvoice.CustomerInvoiceGroupId,
                    Delivered = false,
                    VAT = oldInvoice.VAT,
                    StartDate = oldInvoice.StartDate,
                    EndDate = oldInvoice.EndDate,
                    IsCreditNote = true,
                    Regarding = oldInvoice.Regarding,
                    InvoiceDate = DateTime.Now,
                    Guid = Guid.NewGuid(),
                    FooterText = oldInvoice.FooterText,
                    DueDate = DateTime.Now,
                    InvoiceID = null,
                    InvoiceLinkId = oldInvoiceId
                });
                entity.SaveChanges();

                var newId = (from i in entity.Invoices
                             select i).Max(i => i.ID);

                oldInvoice.InvoiceLinkId = newId;

                if(!oldInvoice.Delivered)
                {
                    oldInvoice.Delivered = true;
                    oldInvoice.DeliveredDate = DateTime.Now;
                }
                oldInvoice.Closed = true;

                entity.Invoices.ApplyCurrentValues(oldInvoice);
                entity.SaveChanges();

                entity.GenerateCreditNoteLines(oldInvoiceId, newId);
                entity.ReplicateCreditNoteLines(oldInvoiceId);

                return newId;
            }
        }

        /// <summary>
        /// Finds invoice's sum exclusive VAT from its InvoiceLines
        /// </summary>
        /// <param name="invoiceId">The invoice's ID to calculate</param>
        /// <returns>Sum of InvoiceLines, <strong>0</strong> if no invoiceLines are found</returns>
        public double? UpdateExclVAT(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var t = (from te in entity.InvoiceLines
                         where te.InvoiceID == invoiceId
                         select te);

                if (!t.Any())
                    return 0;
                //var g = t.Where(il => il.UnitType == 2).Sum(i => i.PricePrUnit) + t.Where(il => il.UnitType != 2).Sum(r => r.PricePrUnit * r.Units);
                return t.Sum(r => r.PricePrUnit * r.Units);
            }
        }

        /// <summary>
        /// Finds all invoice's ID
        /// </summary>
        /// <returns>Returns a list invoice's ID</returns>
        public List<int?> GetAllInvoiceIDs()
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                return (from ID in entity.Invoices
                        select ID.InvoiceID).ToList();
            }
        }

        /// <summary>
        /// Finds an invoice and associated data by the invoice's ID
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <returns>Associated data for Invoice</returns>
        public InvoiceListItemView GetInvoiceByInvoiceId(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var t = GetCustomerByInvoiceInvoiceId(invoiceId);

                return entity.InvoiceListItemView(t.CustomerID.ToString()).First(i => i.InvoiceID == invoiceId);
            }
        }

        /// <summary>
        /// Updates DeliveredDate to DateTime.Now
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        public void UpdateDeliveredDate(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var invoice = (from i in entity.Invoices
                               where i.ID == invoiceId
                               select i).First();

                entity.Invoices.Attach(invoice);
                invoice.Delivered = true;
                invoice.DeliveredDate = DateTime.Now;
                entity.Invoices.ApplyCurrentValues(invoice);
                entity.SaveChanges();
            }
        }

        public void UpdateTimeEntriesPricePrHour(IEnumerable<FixedProjects> fixedProjects, int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var ii = 0;
                foreach (var fixedProjectse in fixedProjects.Select(p => p.InvoiceLineId).Distinct())
                {

                    var inl = (from il in entity.InvoiceLines
                               where il.ID == fixedProjectse && il.UnitType == 2
                               select il).ToList();

                    if (inl.Count != 0)
                    {
                        var invoiceLines = inl.First();

                        var q = fixedProjects.Select(pr => pr.ProjectID).Distinct().ToList()[ii];

                        var newPrice = invoiceLines.PricePrUnit / invoiceLines.Units;
                        entity.UpdateTimeEntriesHourPrice(q, newPrice, invoiceLines.ID);
                        ii++;
                    }
                }
            }
        }

        /// <summary>
        /// Undo finalize actions: Delete .pdf-files and sets the invoice's ID = null (draft)
        /// </summary>
        /// <param name="invoiceId">Invoice's ID</param>
        public void RollBack(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var files = (from IF in entity.InvoiceFiles
                             where IF.InvoiceID == invoiceId
                             select IF);

                foreach (var invoiceFile in files)
                {
                    entity.InvoiceFiles.DeleteObject(invoiceFile);
                }
                entity.SaveChanges();

                var invoice = entity.Invoices.First(i => i.ID == invoiceId);
                invoice.InvoiceID = null;
                entity.Invoices.ApplyChanges(invoice);
                entity.SaveChanges();
            }
        }

        /// <summary>
        /// Validates an invoice's customer data and CustomerInvoiceGroup data to create a pdf file.
        /// If the CustomerInvoiceGroup have anything different from the customer, the CustomerInvoiceGroup's data will be selected.
        /// If the Customer does not have any data either, the criteria will be added to a list and returned
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <returns>Returns a list of missing items</returns>
        public List<string> ValidateFinalize(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var tmp = new List<string>();

                var group = (from cig in entity.CustomerInvoiceGroups
                             join c in entity.Customers on cig.CustomerID equals c.CustomerID
                             join i in entity.Invoices on cig.CustomerInvoiceGroupID equals i.CustomerInvoiceGroupId
                             where i.ID == invoiceId
                             select new { CustomerInvoiceGroup = cig, cig.Customer, Invoice = i }).First();


                var address = string.IsNullOrEmpty(group.Customer.StreetAddress)
                                ? group.CustomerInvoiceGroup.Address1
                                : group.Customer.StreetAddress;

                var attention = string.IsNullOrEmpty(group.Customer.ContactName)
                                ? group.CustomerInvoiceGroup.Attention
                                : group.Customer.ContactName;

                var city = string.IsNullOrEmpty(group.Customer.City)
                                ? group.CustomerInvoiceGroup.City
                                : group.Customer.City;

                var country = string.IsNullOrEmpty(group.Customer.Country)
                                ? group.CustomerInvoiceGroup.Country
                                : group.Customer.Country;

                var mail = string.IsNullOrEmpty(group.Customer.Email)
                                ? group.CustomerInvoiceGroup.Email
                                : group.Customer.Email;

                var zip = string.IsNullOrEmpty(group.Customer.ZipCode)
                                ? group.CustomerInvoiceGroup.ZipCode
                                : group.Customer.ZipCode;


                //var address = (group.Customer.StreetAddress ?? group.CustomerInvoiceGroup.Address1);
                //var attention = (group.Customer.ContactName ?? group.CustomerInvoiceGroup.Attention);
                //var city = (group.Customer.City ?? group.CustomerInvoiceGroup.City);
                //var country = (group.Customer.Country ?? group.CustomerInvoiceGroup.Country);
                //var mail = (group.Customer.Email ?? group.CustomerInvoiceGroup.Email);
                //var zip = (group.Customer.ZipCode ?? group.CustomerInvoiceGroup.ZipCode);

                if (string.IsNullOrEmpty(address))
                    tmp.Add("Street Adress");
                if (string.IsNullOrEmpty(attention))
                    tmp.Add("Attention");
                if (string.IsNullOrEmpty(city))
                    tmp.Add("City");
                if (string.IsNullOrEmpty(country))
                    tmp.Add("Country");
                if (group.Customer.SendFormat != 2)
                    if (string.IsNullOrEmpty(mail))
                        tmp.Add("mail");
                if (string.IsNullOrEmpty(zip))
                    tmp.Add("zip");

                return tmp;
            }

        }

        /// <summary>
        /// Deletes an invoice's InvoiceLines, InvoiceFiles, CreditNotes, sets TimeEntries's DocumentType affected to old value and removes the Invoice
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        public void DeleteInvoiceById(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                entity.DeleteInvoice(invoiceId);
            }
        }

        /// <summary>
        /// Get a customer by an invoice's ID
        /// </summary>
        /// <param name="invoiceId">The Invoice's ID</param>
        /// <returns>Returns the customer</returns>
        public Customer GetCustomerByInvoiceId(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var cus = (from i in entity.Invoices
                           join cig in entity.CustomerInvoiceGroups on i.CustomerInvoiceGroupId equals cig.CustomerInvoiceGroupID
                           join c in entity.Customers on cig.CustomerID equals c.CustomerID
                           where i.ID == invoiceId
                           select c).First();
                return cus;
            }
        }

        public void UpdateInvoiceAttention(int invoiceId, int customerInvoiceGroupId)
        {
            using (var db = _trexContextProvider.TrexEntityContext)
            {
                var invoice = (from inv in db.Invoices
                               where inv.ID == invoiceId
                               select inv).First();

                invoice.Attention =
                    (from customerInvoiceGroup in db.CustomerInvoiceGroups
                     where customerInvoiceGroup.CustomerInvoiceGroupID == customerInvoiceGroupId
                     select customerInvoiceGroup.Attention).First();

                db.Invoices.Attach(invoice);
                db.Invoices.ApplyChanges(invoice);
                db.SaveChanges();

            }

        }

        public List<InvoiceComment> LoadInvoiceComment(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                var comments = (from invoiceComment in entity.InvoiceComments
                                where invoiceComment.InvoiceID == invoiceId
                                select invoiceComment).ToList();

                return comments;
            }
            
        }

        public ServerResponse SaveInvoiceComment(string comment, int invoiceID, int userID)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                entity.InvoiceComments.AddObject(new InvoiceComment
                                                     {
                                                         Comment = comment,
                                                         InvoiceID = invoiceID,
                                                         TimeStamp = DateTime.Now,
                                                         UserID = userID
                                                     });
                entity.SaveChanges();
            }
            return new ServerResponse("Success", true);
        }

        /// <summary>
        /// Returns a customer located from an invoice's InvoiceID
        /// </summary>
        /// <param name="invoiceId">The invoice's InvoiceID</param>
        /// <returns>The customer linked to the invoice</returns>
        private Customer GetCustomerByInvoiceInvoiceId(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                return (from i in entity.Invoices
                        join cig in entity.CustomerInvoiceGroups on i.CustomerInvoiceGroupId equals cig.CustomerInvoiceGroupID
                        join c in entity.Customers on cig.CustomerID equals c.CustomerID
                        where i.InvoiceID == invoiceId
                        select c).SingleOrDefault();
            }
        }

        /// <summary>
        /// Copies the TimeEntries to CreditNote and set the TimeEntries's DocumentType = 2
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        public void CopyTimeEntries(int invoiceId)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                entity.CopyTimeEntriesToCreditNote(invoiceId);
            }
        }

        #endregion

        public int GetCustomerInvoiceGroupIDByCustomerID(int id)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                return entity.CustomerInvoiceGroups.First(r => r.CustomerID == id).CustomerInvoiceGroupID;
            }
        }

        public CustomerInvoiceGroup GetCustomerInvoiceGroupByInvoiceId(int id)
        {
            using (var entity = _trexContextProvider.TrexEntityContext)
            {
                return (from cig in entity.CustomerInvoiceGroups
                        join i in entity.Invoices on cig.CustomerInvoiceGroupID equals i.CustomerInvoiceGroupId
                        where i.ID == id
                        select cig).SingleOrDefault();
            }
        }
    }
}
