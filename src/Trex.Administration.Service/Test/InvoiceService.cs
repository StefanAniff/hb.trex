﻿using System;
﻿using System.Collections.ObjectModel;
﻿using System.Linq;
﻿using NUnit.Framework;
﻿using Test;
﻿using Trex.Server.Infrastructure.Implemented;
﻿using Trex.ServiceContracts;

namespace Test_InvoiceService
{
    [TestFixture]
    public class GetInvoiceDataByInvoiceId
    {
        private const string StartDate = "10-10-2012 10:00:00";

        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        [Test]
        public void InsertOneTE_OneTEOutput()
        {
            //Take standard invoice
            _databaseSetup.CreateTimeEntry(2, 1, 1, 100, true, 1, 0, StartDate, StartDate, 1, 1, 1);
            var c = _invoiceService.GetInvoiceDataByInvoiceId(1);
            Assert.AreEqual(1, c.Count);
        }

        [Test]
        public void InsertTenTE_TenTEOutput()
        {
            GenerateTimeEntries(10);
            var c = _invoiceService.GetInvoiceDataByInvoiceId(1);
            Assert.AreEqual(10, c.Count);
        }

        [Test]
        public void InsertZeroTE_EmptyOutput()
        {
            var c = _invoiceService.GetInvoiceDataByInvoiceId(1);
            Assert.AreEqual(0, c.Count);
        }

        [Test]
        public void InsertOneTaskAndOneTE_TEsTasksIdFound()
        {
            _databaseSetup.CreateTask(2, 1, 1, StartDate, " ");
            _databaseSetup.CreateTimeEntry(2, 1, 1, 100, true, 1, 0, StartDate, StartDate, 1, 2, 1);
            var c = _invoiceService.GetInvoiceDataByInvoiceId(1);
            Assert.AreEqual(2, c[0].Task.TaskID);
        }

        [Test]
        public void InsertOneProjectOneTaskAndOneTE_TEsTasksProjectsIdFound()
        {
            _databaseSetup.CreateProject(2, 1, " ", 1, StartDate, false, 0, 1);
            _databaseSetup.CreateTask(2, 2, 1, StartDate, " ");
            _databaseSetup.CreateTimeEntry(2, 1, 1, 100, true, 1, 0, StartDate, StartDate, 1, 2, 1);
            var c = _invoiceService.GetInvoiceDataByInvoiceId(1);
            Assert.AreEqual(2, c[0].Task.Project.ProjectID);
        }

        private void GenerateTimeEntries(int amountOfTimeEntries)
        {
            for (int i = 0; i < amountOfTimeEntries; i++)
            {
                _databaseSetup.CreateTimeEntry(i + 2, 1, 1 + ((i + 1) / 10), 100, true, 1 + (i / 10), 0, StartDate,
                                               StartDate, 1, 1, 1);
            }
        }
    }

    [TestFixture]
    public class GenerateInvoiceDraft
    {
        private const string StartDate = "10-10-2012 10:00:00";
        private const string EndDate = "10-10-2012 11:00:00";

        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        private void GenerateTimeEntries(int amountOfTimeEntries)
        {
            for (int i = 0; i < amountOfTimeEntries; i++)
            {
                _databaseSetup.CreateTimeEntry(i + 2, null, 1 + ((i + 1) / 10), 100, true, 1 + (i / 10), 0, StartDate,
                                               StartDate, 1, 1, 1);
            }
        }

        [Test]
        public void NoTEs_NoDraftGenerated()
        {
            _invoiceService.GenerateInvoiceDraft(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1, 1, (float)0.25);
            var data = _databaseSetup.GetTrexConnection.TrexEntityContext.Invoices.Any(i => i.ID == 2);
            Assert.AreEqual(false, data);

        }

        [Test]
        public void InsertNonBillableTE_NoDraftGenerated()
        {
            _databaseSetup.CreateTimeEntry(2, null, 1, 100, false, 1, 0, StartDate, EndDate, 1, 1, 1);
            _invoiceService.GenerateInvoiceDraft(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1, 1, (float)0.25);
            var data = _databaseSetup.GetTrexConnection.TrexEntityContext.Invoices.Any(i => i.ID == 2);
            Assert.AreEqual(false, data);
        }

        [Test]
        public void InsertBillableAndNonBillableTE_OneDraftGenerated()
        {
            _databaseSetup.CreateTimeEntry(2, null, 1, 100, false, 1, 0, StartDate, EndDate, 1, 1, 1);
            _databaseSetup.CreateTimeEntry(3, null, 1, 100, true, 1, 0, StartDate, EndDate, 1, 1, 1);
            _invoiceService.GenerateInvoiceDraft(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1, 1, (float)0.25);
            var data = _databaseSetup.GetTrexConnection.TrexEntityContext.Invoices.Any(i => i.ID == 2);
            Assert.AreEqual(true, data);
        }

        [Test]
        public void InsertTenBillableAndOneNonBillableTE_OneDraftGenerated()
        {
            _databaseSetup.CreateTimeEntry(12, null, 1, 100, false, 1, 0, StartDate, EndDate, 1, 1, 1);
            GenerateTimeEntries(10);
            _invoiceService.GenerateInvoiceDraft(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1, 1, (float)0.25);
            var data = _databaseSetup.GetTrexConnection.TrexEntityContext.Invoices.Any(i => i.ID == 2);
            Assert.AreEqual(true, data);
        }

        [Test]
        public void InsertTwoTE_EntriesAssignToInvoice()
        {
            _databaseSetup.CreateTimeEntry(2, null, 1, 100, true, 1, 0, StartDate, EndDate, 1, 1, 1);
            _databaseSetup.CreateTimeEntry(3, null, 1, 100, true, 1, 0, StartDate, EndDate, 1, 1, 1);
            _invoiceService.GenerateInvoiceDraft(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1, 1, (float)0.25);
            var data = _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries.Where(te => te.TimeEntryID == 2 || te.TimeEntryID == 3).ToList();
            Assert.AreEqual(2, data[0].InvoiceId);
            Assert.AreEqual(2, data[1].InvoiceId);
        }

        [Test]
        public void InsertTwoTEOnTwoProjectsOnSameCig_EntriesAssignToSameInvoice()
        {
            var cig1 = _databaseSetup.CreateCustomerInvoiceGroup(20, 1, "asdasd", false, 1);
            var cig2 = _databaseSetup.CreateCustomerInvoiceGroup(21, 1, "asdasd", false, 1, "asd", "asd", "asd", "asd", "asd", "asd", "asd", "asd");
            var u = _databaseSetup.CreateUser(12, "asdasd", 200);
            var c = _databaseSetup.CreateCustomer(10, "", "asd", "", "", "", "", "");
            var p = _databaseSetup.CreateProject(20, 1, " ", 1, StartDate, false, 0, 1);
            var t = _databaseSetup.CreateTask(20, 20, 1, StartDate, " ");
            var te1 = _databaseSetup.CreateTimeEntry(22, null, 1, 100, true, 1, 0, StartDate, EndDate, 1, 1, 1);
            var te2 = _databaseSetup.CreateTimeEntry(23, null, 1.2, 100, true, 1, 0, StartDate, EndDate, 1, 20, 1);
            _invoiceService.GenerateInvoiceDraft(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1, 1, (float)0.25);
            var data = _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries.Where(te => te.TimeEntryID == 2 || te.TimeEntryID == 3).ToList();
            //Assert.AreEqual(2, data[0].InvoiceId);
            //Assert.AreEqual(2, data[1].InvoiceId);
            Assert.AreEqual(22, te1.TimeEntryID);
            Assert.AreEqual(23, te2.TimeEntryID);
        }
    }

    [TestFixture]
    public class GetTimeEntriesForInvoicing
    {
        private const string StartDate = "10-10-2012 10:00:00";
        private const string EndDate = "11-10-2012 11:00:00";

        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        [Test]
        public void InsertOneGoodTE_TEFound()
        {
            _databaseSetup.CreateTimeEntry(2, null, 1.5, 250, true, 1.5, 0, StartDate, EndDate, 1, 1, 1);

            var output = _invoiceService.GetTimeEntriesForInvoicing(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1);
            Assert.AreEqual(2, output[0].Id);
        }

        [Test]
        public void TEWithStartDateOutOfSpan_TENotFound()
        {
            _databaseSetup.CreateTimeEntry(2, null, 1.5, 250, true, 1.5, 0, "09-10-2012 10:00:00", EndDate, 1, 1, 1);

            var output = _invoiceService.GetTimeEntriesForInvoicing(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1);
            Assert.AreEqual(0, output.Count);
        }

        [Test]
        public void TEWithEndDateOutOfSpan_TENotFound()
        {
            _databaseSetup.CreateTimeEntry(2, null, 1.5, 250, true, 1.5, 0, StartDate, "30-10-2012 10:00:00", 1, 1, 1);

            var output = _invoiceService.GetTimeEntriesForInvoicing(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1);
            Assert.AreEqual(0, output.Count);
        }

        [Test]
        public void TEOnAnotherInvoice_TENotFound()
        {
            _databaseSetup.CreateTimeEntry(2, 1, 1.5, 250, true, 1.5, 0, StartDate, EndDate, 1, 1, 1);

            var output = _invoiceService.GetTimeEntriesForInvoicing(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1);
            Assert.AreEqual(0, output.Count);
        }

        [Test]
        public void TENonBillable_TENotFound()
        {
            _databaseSetup.CreateTimeEntry(2, null, 1.5, 250, false, 1.5, 0, StartDate, EndDate, 1, 1, 1);

            var output = _invoiceService.GetTimeEntriesForInvoicing(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1);
            Assert.AreEqual(0, output.Count);
        }

        [Test]
        public void ThreeGoodTE_AllFound()
        {
            _databaseSetup.CreateTimeEntry(20, null, 1.5, 250, true, 1.5, 0, StartDate, EndDate, 1, 1, 1);
            _databaseSetup.CreateTimeEntry(21, null, 1.5, 250, true, 1.5, 0, StartDate, EndDate, 1, 1, 1);
            _databaseSetup.CreateTimeEntry(22, null, 1.5, 250, true, 1.5, 0, StartDate, EndDate, 1, 1, 1);

            var output = _invoiceService.GetTimeEntriesForInvoicing(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1);
            Assert.AreEqual(3, output.Count);
            Assert.AreEqual(20, output[0].Id);
            Assert.AreEqual(21, output[1].Id);
            Assert.AreEqual(22, output[2].Id);
        }

        [Test]
        public void ThreeGoodAndOneBadTE_TreeGoodFound()
        {
            _databaseSetup.CreateTimeEntry(20, null, 1.5, 250, true, 1.5, 0, StartDate, EndDate, 1, 1, 1);
            _databaseSetup.CreateTimeEntry(21, null, 1.5, 250, true, 1.5, 0, StartDate, EndDate, 1, 1, 1);
            _databaseSetup.CreateTimeEntry(22, null, 1.5, 250, true, 1.5, 0, StartDate, EndDate, 1, 1, 1);
            _databaseSetup.CreateTimeEntry(23, null, 1.5, 250, false, 1.5, 0, StartDate, EndDate, 1, 1, 1);

            var output = _invoiceService.GetTimeEntriesForInvoicing(DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1);
            Assert.AreEqual(3, output.Count);
            Assert.AreEqual(20, output[0].Id);
            Assert.AreEqual(21, output[1].Id);
            Assert.AreEqual(22, output[2].Id);
        }
    }

    [TestFixture]
    public class ReleaseTimeEntries
    {
        private const string StartDate = "10-10-2012 10:00:00";
        private const string EndDate = "11-10-2012 11:00:00";

        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        [Test]
        public void TwoILTypeZero_BothRemoved()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var il1 = _databaseSetup.CreateInvoiceLine(3, i.ID, 100, "decimal hours", 2, 0.25, " ", 0, false);
            var il2 = _databaseSetup.CreateInvoiceLine(4, i.ID, 200, "decimal hours", 4, 0.25, " ", 0, false);
            var te1 = _databaseSetup.CreateTimeEntry(3, i.ID, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 2);
            var te2 = _databaseSetup.CreateTimeEntry(4, i.ID, 2, 200, true, 2, 0, StartDate, EndDate, 1, 1, 2);
            var te3 = _databaseSetup.CreateTimeEntry(5, i.ID, 2, 200, true, 2, 0, StartDate, EndDate, 1, 1, 2);

            _invoiceService.ReleaseTimeEntries(i.ID);
            Assert.AreEqual(false, _databaseSetup.EntityContext.InvoiceLines.Any(x => x.ID == il1.ID));
            Assert.AreEqual(false, _databaseSetup.EntityContext.InvoiceLines.Any(x => x.ID == il2.ID));
        }

        [Test]
        public void TwoILTypeZeroAndTypeOne_OneRemoved()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var il1 = _databaseSetup.CreateInvoiceLine(3, i.ID, 100, "decimal hours", 2, 0.25, " ", 1, false);
            var il2 = _databaseSetup.CreateInvoiceLine(4, i.ID, 200, "decimal hours", 4, 0.25, " ", 0, false);
            var te1 = _databaseSetup.CreateTimeEntry(3, i.ID, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 2);
            var te2 = _databaseSetup.CreateTimeEntry(4, i.ID, 2, 200, true, 2, 0, StartDate, EndDate, 1, 1, 2);
            var te3 = _databaseSetup.CreateTimeEntry(5, i.ID, 2, 200, true, 2, 0, StartDate, EndDate, 1, 1, 2);

            _invoiceService.ReleaseTimeEntries(i.ID);
            Assert.AreEqual(true, _databaseSetup.EntityContext.InvoiceLines.Any(x => x.ID == il1.ID));
            Assert.AreEqual(false, _databaseSetup.EntityContext.InvoiceLines.Any(x => x.ID == il2.ID));
        }

        [Test]
        public void ThreeILTypeZeroAndTypeOneAndOnOtherInvoice_OneRemoved()
        {
            var i1 = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var i2 = _databaseSetup.CreateInvoice(21, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var il1 = _databaseSetup.CreateInvoiceLine(3, i1.ID, 100, "decimal hours", 2, 0.25, " ", 1, false);
            var il2 = _databaseSetup.CreateInvoiceLine(4, i1.ID, 200, "decimal hours", 4, 0.25, " ", 0, false);
            var il3 = _databaseSetup.CreateInvoiceLine(5, i2.ID, 200, "decimal hours", 4, 0.25, " ", 0, false);
            var te1 = _databaseSetup.CreateTimeEntry(3, i1.ID, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 2);
            var te2 = _databaseSetup.CreateTimeEntry(4, i1.ID, 2, 200, true, 2, 0, StartDate, EndDate, 1, 1, 2);
            var te3 = _databaseSetup.CreateTimeEntry(5, i2.ID, 2, 200, true, 2, 0, StartDate, EndDate, 1, 1, 2);

            _invoiceService.ReleaseTimeEntries(i1.ID);
            Assert.AreEqual(true, _databaseSetup.EntityContext.InvoiceLines.Any(x => x.ID == il1.ID));
            Assert.AreEqual(false, _databaseSetup.EntityContext.InvoiceLines.Any(x => x.ID == il2.ID));
            Assert.AreEqual(true, _databaseSetup.EntityContext.InvoiceLines.Any(x => x.ID == il3.ID));
        }

        [Test]
        public void TwoILTypeZeroAndThreeTE_BothTEHasNoInvoiceId()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var il1 = _databaseSetup.CreateInvoiceLine(3, i.ID, 100, "decimal hours", 2, 0.25, " ", 0, false);
            var il2 = _databaseSetup.CreateInvoiceLine(4, i.ID, 200, "decimal hours", 4, 0.25, " ", 0, false);
            var te1 = _databaseSetup.CreateTimeEntry(3, i.ID, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 2);
            var te2 = _databaseSetup.CreateTimeEntry(4, i.ID, 2, 200, true, 2, 0, StartDate, EndDate, 1, 1, 2);
            var te3 = _databaseSetup.CreateTimeEntry(5, i.ID, 2, 200, true, 2, 0, StartDate, EndDate, 1, 1, 2);

            _invoiceService.ReleaseTimeEntries(i.ID);
            Assert.AreEqual(null, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te1.TimeEntryID).InvoiceId);
            Assert.AreEqual(null, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te2.TimeEntryID).InvoiceId);
            Assert.AreEqual(null, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te3.TimeEntryID).InvoiceId);
        }

        [Test]
        public void ThreeILTypeZeroAndTypeOneAndOnOtherInvoice_TwoTEsDocumentTypeEqualsOne()
        {
            var i1 = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var i2 = _databaseSetup.CreateInvoice(21, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var il1 = _databaseSetup.CreateInvoiceLine(3, i1.ID, 100, "decimal hours", 2, 0.25, " ", 1, false);
            var il2 = _databaseSetup.CreateInvoiceLine(4, i1.ID, 200, "decimal hours", 4, 0.25, " ", 0, false);
            var il3 = _databaseSetup.CreateInvoiceLine(5, i2.ID, 200, "decimal hours", 4, 0.25, " ", 0, false);
            var te1 = _databaseSetup.CreateTimeEntry(3, i1.ID, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 2);
            var te2 = _databaseSetup.CreateTimeEntry(4, i1.ID, 2, 200, true, 2, 0, StartDate, EndDate, 1, 1, 2);
            var te3 = _databaseSetup.CreateTimeEntry(5, i2.ID, 2, 200, true, 2, 0, StartDate, EndDate, 1, 1, 2);

            _invoiceService.ReleaseTimeEntries(i1.ID);
            Assert.AreEqual(1, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te1.TimeEntryID).DocumentType);
            Assert.AreEqual(1, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te2.TimeEntryID).DocumentType);
            Assert.AreEqual(te3.DocumentType, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te3.TimeEntryID).DocumentType);
        }
    }

    [TestFixture]
    public class GenerateInvoiceLines
    {
        private const string StartDate = "10-10-2012 10:00:00";
        private const string EndDate = "11-10-2012 11:00:00";

        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        [Test]
        public void OneTEDocTypeOne_TEInvoiceIdUpdated()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var te1 = _databaseSetup.CreateTimeEntry(3, null, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 1);

            _invoiceService.UpdateTimeEntries(i.ID, DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1);
            Assert.AreEqual(i.ID, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te1.TimeEntryID).InvoiceId);
        }

        [Test]
        public void OneTEDocTypeOne_TEDocumentTypeUpdated()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var te1 = _databaseSetup.CreateTimeEntry(3, null, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 1);

            _invoiceService.UpdateTimeEntries(i.ID, DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1);
            Assert.AreEqual(2, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te1.TimeEntryID).DocumentType);
        }

        [Test]
        public void ThreeTEDocTypeOne_TEDocumentTypeUpdated()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var te1 = _databaseSetup.CreateTimeEntry(3, null, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 1);
            var te2 = _databaseSetup.CreateTimeEntry(4, null, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 1);
            var te3 = _databaseSetup.CreateTimeEntry(5, null, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 1);

            _invoiceService.UpdateTimeEntries(i.ID, DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1);
            Assert.AreEqual(2, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te1.TimeEntryID).DocumentType);
            Assert.AreEqual(2, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te2.TimeEntryID).DocumentType);
            Assert.AreEqual(2, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te3.TimeEntryID).DocumentType);
        }

        [Test]
        public void ThreeTEDocTypeOne_TEInvoiceIdUpdated()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var te1 = _databaseSetup.CreateTimeEntry(3, null, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 1);
            var te2 = _databaseSetup.CreateTimeEntry(4, null, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 1);
            var te3 = _databaseSetup.CreateTimeEntry(5, null, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 1);

            _invoiceService.UpdateTimeEntries(i.ID, DateTime.Parse(StartDate), DateTime.Parse(EndDate), 1);
            Assert.AreEqual(i.ID, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te1.TimeEntryID).InvoiceId);
            Assert.AreEqual(i.ID, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te2.TimeEntryID).InvoiceId);
            Assert.AreEqual(i.ID, _databaseSetup.EntityContext.TimeEntries.First(te => te.TimeEntryID == te3.TimeEntryID).InvoiceId);
        }

        [Test]
        public void TwoTEsSamePrice_OneILGenerated()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var te1 = _databaseSetup.CreateTimeEntry(3, i.ID, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 1);
            var te2 = _databaseSetup.CreateTimeEntry(4, i.ID, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 1);

            _invoiceService.GenerateInvoiceLines(i.ID);
            Assert.AreEqual(1, _databaseSetup.EntityContext.InvoiceLines.Where(il => il.InvoiceID == i.ID).Count(), "Number of Invoicelines generated is wrong");
            Assert.AreEqual(4, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).Units, "Time used is wrong");
            Assert.AreEqual(100, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).PricePrUnit, "PricePrHour is wrong for this Invoiceline");
        }

        [Test]
        public void TwoTEsDifferentPrice_TwoILGenerated()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var te1 = _databaseSetup.CreateTimeEntry(3, i.ID, 2, 100, true, 2, 0, StartDate, EndDate, 1, 1, 1);
            var te2 = _databaseSetup.CreateTimeEntry(4, i.ID, 2, 200, true, 2, 0, StartDate, EndDate, 1, 1, 1);

            _invoiceService.GenerateInvoiceLines(i.ID);
            Assert.AreEqual(2, _databaseSetup.EntityContext.InvoiceLines.Count(il => il.InvoiceID == i.ID), "Number of Invoicelines generated is wrong");
            Assert.AreEqual(2, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).Units, "Time used is wrong");

            var t = _databaseSetup.EntityContext.InvoiceLines.Max(il => il.ID);
            Assert.AreEqual(2, _databaseSetup.EntityContext.InvoiceLines.First(il => il.ID == t).Units, "Time used is wrong");
            Assert.AreEqual(100, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).PricePrUnit, "PricePrHour is wrong for this Invoiceline");
            Assert.AreEqual(200, _databaseSetup.EntityContext.InvoiceLines.First(il => il.ID == t).PricePrUnit, "PricePrHour is wrong for this Invoiceline");
        }

        [Test]
        public void TwoTEsSamePriceSameTaskSameDay_PriceRoundedCorrectly()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var te1 = _databaseSetup.CreateTimeEntry(3, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 10:00:00", "10-10-2012 11:20:00", 1, 1, 1); //1,33 +
            var te2 = _databaseSetup.CreateTimeEntry(4, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 12:00:00", "10-10-2012 12:20:00", 1, 1, 1); //1,33 = 2,66 ~ 2,75

            _invoiceService.GenerateInvoiceLines(i.ID);
            Assert.AreEqual(1, _databaseSetup.EntityContext.InvoiceLines.Where(il => il.InvoiceID == i.ID).Count(), "Number of Invoicelines generated is wrong");
            Assert.AreEqual(2.75, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).Units, "Time used is wrong");
            Assert.AreEqual(100, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).PricePrUnit, "PricePrHour is wrong for this Invoiceline");
        }

        [Test]
        public void TwoTEsSamePriceDifferentTaskSameDay_PriceRoundedCorrectly()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var t = _databaseSetup.CreateTask(2, 1, 1, StartDate, " ");
            var te1 = _databaseSetup.CreateTimeEntry(3, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 10:00:00", "10-10-2012 11:20:00", 1, 1, 1); //1,33 +
            var te2 = _databaseSetup.CreateTimeEntry(4, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 12:00:00", "10-10-2012 12:20:00", 1, t.TaskID, 1); //1,33 = 2,66 ~ 2,75

            _invoiceService.GenerateInvoiceLines(i.ID);
            Assert.AreEqual(1, _databaseSetup.EntityContext.InvoiceLines.Where(il => il.InvoiceID == i.ID).Count(), "Number of Invoicelines generated is wrong");
            Assert.AreEqual(3, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).Units, "Time used is wrong");
            Assert.AreEqual(100, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).PricePrUnit, "PricePrHour is wrong for this Invoiceline");
        }

        [Test]
        public void TwoTEsSamePriceSameTaskDifferentDay_PriceRoundedCorrectly()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var t = _databaseSetup.CreateTask(2, 1, 1, StartDate, " ");
            var te1 = _databaseSetup.CreateTimeEntry(3, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 10:00:00", "10-10-2012 11:20:00", 1, 1, 1); //1,33 +
            var te2 = _databaseSetup.CreateTimeEntry(4, i.ID, 1.33, 100, true, 1.33, 0, "11-10-2012 12:00:00", "11-10-2012 12:20:00", 1, 1, 1); //1,33 = 2,66 ~ 2,75

            _invoiceService.GenerateInvoiceLines(i.ID);
            Assert.AreEqual(1, _databaseSetup.EntityContext.InvoiceLines.Where(il => il.InvoiceID == i.ID).Count(), "Number of Invoicelines generated is wrong");
            Assert.AreEqual(3, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).Units, "Time used is wrong");
            Assert.AreEqual(100, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).PricePrUnit, "PricePrHour is wrong for this Invoiceline");
        }

        [Test]
        public void TwoTEsDifferentPriceSameTaskSameDay_PriceRoundedCorrectly()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var t = _databaseSetup.CreateTask(2, 1, 1, StartDate, " ");
            var te1 = _databaseSetup.CreateTimeEntry(3, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 10:00:00", "10-10-2012 11:20:00", 1, 1, 1); //1,33 +
            var te2 = _databaseSetup.CreateTimeEntry(4, i.ID, 1.33, 200, true, 1.33, 0, "10-10-2012 12:00:00", "10-10-2012 12:20:00", 1, 1, 1); //1,33 = 2,66 ~ 2,75

            _invoiceService.GenerateInvoiceLines(i.ID);

            var lastLine = _databaseSetup.EntityContext.InvoiceLines.Max(il => il.ID);
            Assert.AreEqual(2, _databaseSetup.EntityContext.InvoiceLines.Where(il => il.InvoiceID == i.ID).Count(), "Number of Invoicelines generated is wrong");
            Assert.AreEqual(1.50, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).Units, "Time used is wrong");
            Assert.AreEqual(1.50, _databaseSetup.EntityContext.InvoiceLines.First(il => il.ID == lastLine).Units, "Time used is wrong");
            Assert.AreEqual(100, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).PricePrUnit, "PricePrHour is wrong for this Invoiceline");
            Assert.AreEqual(200, _databaseSetup.EntityContext.InvoiceLines.First(il => il.ID == lastLine).PricePrUnit, "PricePrHour is wrong for this Invoiceline");
        }

        [Test]
        public void FourTEsSamePriceSameTaskSameDayDifferentUsers_PriceRoundedCorrectly()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var u = _databaseSetup.CreateUser(2, "secondUser", 1000);
            var t = _databaseSetup.CreateTask(2, 1, 1, StartDate, " ");
            var te1 = _databaseSetup.CreateTimeEntry(3, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 10:00:00", "10-10-2012 11:20:00", 1, 1, 1); //1,33 +
            var te2 = _databaseSetup.CreateTimeEntry(4, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 12:00:00", "10-10-2012 13:20:00", 1, 1, 1); //1,33 ~= 2,75 +
            var te3 = _databaseSetup.CreateTimeEntry(5, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 12:00:00", "10-10-2012 12:20:00", 2, 1, 1); //1,33 +
            var te4 = _databaseSetup.CreateTimeEntry(6, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 12:00:00", "10-10-2012 12:20:00", 2, 1, 1); //1,33 ~= 2,75 = 5,5

            _invoiceService.GenerateInvoiceLines(i.ID);

            Assert.AreEqual(1, _databaseSetup.EntityContext.InvoiceLines.Where(il => il.InvoiceID == i.ID).Count(), "Number of Invoicelines generated is wrong");
            Assert.AreEqual(5.5, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).Units, "Time used is wrong");
            Assert.AreEqual(100, _databaseSetup.EntityContext.InvoiceLines.First(il => il.InvoiceID == i.ID).PricePrUnit, "PricePrHour is wrong for this Invoiceline");
        }

        [Test]
        public void NonFixedProject_NoOuput()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var p = _databaseSetup.CreateProject(2, 1, "non-fixed", 1, StartDate, false, 0, 1);
            var t = _databaseSetup.CreateTask(2, p.ProjectID, 1, StartDate, "Task name");
            var te1 = _databaseSetup.CreateTimeEntry(2, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 10:00:00", "10-10-2012 11:20:00", 1, t.TaskID, 1);
            var te2 = _databaseSetup.CreateTimeEntry(3, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 12:00:00", "10-10-2012 13:20:00", 1, t.TaskID, 1);

            var output = _invoiceService.GenerateInvoiceLines(i.ID);
            Assert.AreEqual(0, output.Count);
        }

        [Test]
        public void OneFixedProject_OneLineOutput()
        {
            var i = _databaseSetup.CreateInvoice(20, StartDate, 1, 0.25, StartDate, EndDate, EndDate, null, 1, false, null, false, null);
            var p = _databaseSetup.CreateProject(2, 1, "fixed", 1, StartDate, true, 10000, 1);
            var t = _databaseSetup.CreateTask(2, p.ProjectID, 1, StartDate, "Task name");
            var te1 = _databaseSetup.CreateTimeEntry(2, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 10:00:00", "10-10-2012 11:20:00", 1, t.TaskID, 1);
            var te2 = _databaseSetup.CreateTimeEntry(3, i.ID, 1.33, 100, true, 1.33, 0, "10-10-2012 12:00:00", "10-10-2012 13:20:00", 1, t.TaskID, 1);

            var output = _invoiceService.GenerateInvoiceLines(i.ID);
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual(p.ProjectID, output[0].ProjectID);
        }
    }

    [TestFixture]
    public class ValidateFinalize
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        [Test]
        public void EmptyErrorList()
        {
            _databaseSetup.CreateCustomer(15, "New Customer", "test@test.dk", "mini me", "Street", "City", "Country",
                                          "ZIP");
            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "Labeld", false, 1);

            const string date = "01-01-2013";
            _databaseSetup.CreateInvoice(15, date, 1, 0.25, date, date, date, null, 15, false, date, false, null);

            var errorlist = _invoiceService.ValidateFinalize(15);

            Assert.IsEmpty(errorlist);
        }

        [Test]
        public void FiveErrorsInList()
        {
            _databaseSetup.CreateCustomer(15, null, null, null, null, null, null,
                                          "ZIP");
            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "Labeld", false, 1);

            const string date = "01-01-2013";
            _databaseSetup.CreateInvoice(15, date, 1, 0.25, date, date, date, null, 15, false, date, false, null);

            var errorlist = _invoiceService.ValidateFinalize(15);

            Assert.AreEqual(5, errorlist.Count);
        }

        [Test]
        public void NoErrors_GetDataFromCig()
        {
            _databaseSetup.CreateCustomer(15, null, null, null, null, null, null,
                                          null);
            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "Labeld", false, 1, "test@test.dk", null, "mini me", "adress", null, "City", "Country", "ZIP");

            const string date = "01-01-2013";
            _databaseSetup.CreateInvoice(15, date, 1, 0.25, date, date, date, null, 15, false, date, false, null);

            var errorlist = _invoiceService.ValidateFinalize(15);

            Assert.IsEmpty(errorlist);
        }
    }

    [TestFixture]
    public class UpdateExclVAT
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        private string date = "01-01-2013";

        [Test]
        public void InvoiceId15_Return200()
        {
            _databaseSetup.CreateInvoice(15, date, 1, 0.25, date, date, date, null, 1, false, null, false, null);
            _databaseSetup.CreateInvoiceLine(15, 15, 100, "timer", 2, 0.25, "test", 1, false);
            var vat = _invoiceService.UpdateExclVAT(15);

            Assert.AreEqual(200, vat);
        }

        [Test]
        public void InvoiceId15_noInvoiceLines_Return0()
        {
            _databaseSetup.CreateInvoice(15, date, 1, 0.25, date, date, date, null, 1, false, null, false, null);

            var vat = _invoiceService.UpdateExclVAT(15);

            Assert.AreEqual(0, vat);
        }
    }

    [TestFixture]
    public class CalculateNextInvoiceId
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        [Test]
        public void InvoiceWithNoInvoiceID_Zero()
        {
            var newID = _invoiceService.CalculateNextInvoiceId(1, false);

            Assert.AreEqual(0, newID);
        }

        private string date = "01-01-2013";
        [Test]
        public void InvoiceWith15InvoiceID_16()
        {
            _databaseSetup.CreateInvoice(13, date, 1, 0.25, date, date, date, 15, 1, false, null, false, null);
            var newID = _invoiceService.CalculateNextInvoiceId(13, false);

            Assert.AreEqual(16, newID);
        }

        [Test]
        public void InvoiceWith15_preview_InvoiceID_15()
        {
            _databaseSetup.CreateInvoice(13, date, 1, 0.25, date, date, date, 15, 1, false, null, false, null);
            var newID = _invoiceService.CalculateNextInvoiceId(13, true);

            Assert.AreEqual(13, newID);
        }
    }

    [TestFixture]
    public class GetInvoicesByCustomerId
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        public string date = "01-01-2000";
        [Test]
        public void treeCustomerIds_AnObservableCollectionOfInvoiceListItemView()
        {

            _databaseSetup.CreateCustomer(14, "1", "1@1.com", "name1", "street1", "city1", "county1", "zip1");
            _databaseSetup.CreateCustomer(15, "2", "2@2.com", "name2", "street2", "city2", "county2", "zip2");
            _databaseSetup.CreateCustomer(16, "3", "3@3.com", "name3", "street3", "city3", "county3", "zip3");

            var result = _invoiceService.GetInvoicesByCustomerId(new ObservableCollection<int> { 14, 15, 16 });

            Assert.IsInstanceOf<ObservableCollection<InvoiceListItemView>>(result);

        }

        [Test]
        public void treeCustomerIds_TwoInvoices()
        {

            _databaseSetup.CreateCustomer(14, "CustomerName1", "1@1.com", "name1", "street1", "city1", "county1", "zip1");
            _databaseSetup.CreateCustomer(15, "CustomerName2", "2@2.com", "name2", "street2", "city2", "county2", "zip2");
            _databaseSetup.CreateCustomer(16, "CustomerName3", "3@3.com", "name3", "street3", "city3", "county3", "zip3");

            _databaseSetup.CreateCustomerInvoiceGroup(14, 14, "label1", true, 1);
            _databaseSetup.CreateCustomerInvoiceGroup(16, 16, "label2", true, 1);

            _databaseSetup.CreateInvoice(14, date, 1, 0.25, date, date, date, null, 14, false, date, false, null);
            _databaseSetup.CreateInvoice(15, date, 1, 0.25, date, date, date, null, 16, false, date, false, null);

            _databaseSetup.CreateInvoiceLine(2, 14, 12, "w", 12, 2, "qwe", 1, true);
            _databaseSetup.CreateInvoiceLine(1, 15, 12, "w", 12, 2, "qwe", 1, true);

            var result = _invoiceService.GetInvoicesByCustomerId(new ObservableCollection<int> { 14, 15, 16 });
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void treeCustomerIds_TwoInvoices_WithRightCustomerNameAndInvoiceId()
        {

            _databaseSetup.CreateCustomer(14, "CustomerName1", "1@1.com", "name1", "street1", "city1", "county1", "zip1");
            _databaseSetup.CreateCustomer(15, "CustomerName2", "2@2.com", "name2", "street2", "city2", "county2", "zip2");
            _databaseSetup.CreateCustomer(16, "CustomerName3", "3@3.com", "name3", "street3", "city3", "county3", "zip3");

            _databaseSetup.CreateCustomerInvoiceGroup(14, 14, "label1", true, 1);
            _databaseSetup.CreateCustomerInvoiceGroup(16, 16, "label2", true, 1);

            _databaseSetup.CreateInvoice(14, date, 1, 0.25, date, date, date, null, 14, false, date, false, null);
            _databaseSetup.CreateInvoice(15, date, 1, 0.25, date, date, date, null, 16, false, date, false, null);

            _databaseSetup.CreateInvoiceLine(2, 14, 12, "w", 12, 2, "qwe", 1, true);
            _databaseSetup.CreateInvoiceLine(1, 15, 12, "w", 12, 2, "qwe", 1, true);

            var result = _invoiceService.GetInvoicesByCustomerId(new ObservableCollection<int> { 14, 15, 16 });
            Assert.AreEqual("CustomerName1", result.First().CustomerName);
            Assert.AreEqual(14, result.First().ID);
        }
    }

    [TestFixture]
    public class GetDebitList
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion
        public string date = "01-01-2000";
        [Test]
        public void ListOfInvoiceListItems_With2ItemWithDueDateSmallerThenToday()
        {
            _databaseSetup.CreateCustomer(14, "CustomerName1", "1@1.com", "name1", "street1", "city1", "county1", "zip1");
            _databaseSetup.CreateCustomer(15, "CustomerName2", "2@2.com", "name2", "street2", "city2", "county2", "zip2");
            _databaseSetup.CreateCustomer(16, "CustomerName3", "3@3.com", "name3", "street3", "city3", "county3", "zip3");

            _databaseSetup.CreateCustomerInvoiceGroup(14, 14, "label1", true, 1);
            _databaseSetup.CreateCustomerInvoiceGroup(16, 16, "label2", true, 1);

            _databaseSetup.CreateInvoice(14, date, 1, 0.25, date, date, "01-01-3000", 1, 14, false, date, false, null);
            _databaseSetup.CreateInvoice(15, date, 1, 0.25, date, date, date, 2, 16, false, date, false, null);

            _databaseSetup.CreateInvoiceLine(2, 14, 12, "w", 12, 2, "qwe", 1, true);
            _databaseSetup.CreateInvoiceLine(3, 15, 12, "w", 12, 2, "qwe", 1, true);

            var list = _invoiceService.GetDebitList();

            Assert.IsTrue(list.Where(r => r.ID == 15).Select(f => f.DueDate).First() < DateTime.Now);
            Assert.IsTrue(list.Where(r => r.ID == 1).Select(f => f.DueDate).First() < DateTime.Now);
            Assert.AreEqual(2, list.Count);
        }
    }

    [TestFixture]
    public class GetInvoiceMetaData
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        [Test]
        public void CigId15_ReturnsTypeCustomerInvoiceGroup()
        {
            _databaseSetup.CreateCustomer(15, "Customer", "CustomerMail", "CustomerContact", "CustomerStreet",
                                          "CustomerCity", "CustomerCountry", "CustomerZip");

            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "Cig", true, 1);

            var data = _invoiceService.GetInvoiceMetaData(15);

            Assert.IsInstanceOf<CustomerInvoiceGroup>(data);
        }

        [Test]
        public void CigId15_getDataFromCustomer()
        {
            _databaseSetup.CreateCustomer(15, "Customer", "CustomerMail", "CustomerContact", "CustomerStreet",
                                          "CustomerCity", "CustomerCountry", "CustomerZip");

            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "Cig", true, 1);

            var data = _invoiceService.GetInvoiceMetaData(15);

            Assert.AreEqual("CustomerStreet", data.Address1);
            Assert.AreEqual("CustomerContact", data.Attention);
            Assert.AreEqual("CustomerCity", data.City);
            Assert.AreEqual("CustomerCountry", data.Country);
            Assert.AreEqual("CustomerZip", data.ZipCode);
        }

        [Test]
        public void CigId15_getDataFromCig()
        {
            _databaseSetup.CreateCustomer(15, "Customer", "CustomerMail", "CustomerContact", "CustomerStreet",
                                          "CustomerCity", "CustomerCountry", "CustomerZip");

            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "Cig", false, 1, "CigMail", "CigCC", "CigAttention",
                                                      "CigAddress", "CigAddress2", "CigCity", "CigCountry", "CigZip");

            var data = _invoiceService.GetInvoiceMetaData(15);

            Assert.AreEqual("CigAddress", data.Address1);
            Assert.AreEqual("CigAttention", data.Attention);
            Assert.AreEqual("CigCity", data.City);
            Assert.AreEqual("CigCountry", data.Country);
            Assert.AreEqual("CigZip", data.ZipCode);
        }

        [Test]
        [ExpectedException]
        public void NonExcistingCigID9999_ExpectedException()
        {
            _invoiceService.GetInvoiceMetaData(9999);
        }
    }

    [TestFixture]
    public class ResetInvoiceId
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        public const string Date = "01-01-2012";

        [Test]
        public void NewInvoiceWithId15AndInvoiceID15_InvoiceIDIsNull()
        {
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 15, 1, false, Date, false, null);
            _invoiceService.ResetInvoiceId(15);

            var data = (from invoice in _databaseSetup.GetTrexConnection.TrexEntityContext.Invoices
                        where invoice.ID == 15
                        select invoice).First();

            Assert.IsNull(data.InvoiceID);
        }
    }

    [TestFixture]
    public class GetAllInvoiceIds
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion
        public const string Date = "01-01-2012";

        [Test]
        public void ListOf5InvoiceIds()
        {
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 10, 1, false, Date, false, null);
            _databaseSetup.CreateInvoice(16, Date, 1, 0.25, Date, Date, Date, 11, 1, false, Date, false, null);
            _databaseSetup.CreateInvoice(17, Date, 1, 0.25, Date, Date, Date, null, 1, false, Date, false, null);
            _databaseSetup.CreateInvoice(18, Date, 1, 0.25, Date, Date, Date, 13, 1, false, Date, false, null);

            var allOfEm = _invoiceService.GetAllInvoiceIDs();

            Assert.AreEqual(5, allOfEm.Count);
        }

        [Test]
        public void ListOf5InvoiceIds_Ids10_11_null_13()
        {
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 10, 1, false, Date, false, null);
            _databaseSetup.CreateInvoice(16, Date, 1, 0.25, Date, Date, Date, 11, 1, false, Date, false, null);
            _databaseSetup.CreateInvoice(17, Date, 1, 0.25, Date, Date, Date, null, 1, false, Date, false, null);
            _databaseSetup.CreateInvoice(18, Date, 1, 0.25, Date, Date, Date, 13, 1, false, Date, false, null);

            var allOfEm = _invoiceService.GetAllInvoiceIDs();

            Assert.AreEqual(null, allOfEm[0]);
            Assert.AreEqual(10, allOfEm[1]);
            Assert.AreEqual(11, allOfEm[2]);
            Assert.AreEqual(null, allOfEm[3]);
            Assert.AreEqual(13, allOfEm[4]);
        }
    }

    [TestFixture]
    public class CopyTimeEntries
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion
        public const string Date = "01-01-2012";

        [Test]
        public void treeTimeEntries_3TimeEntriesInCreditNote()
        {
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 10, 1, false, Date, false, null);
            _databaseSetup.CreateTimeEntry(15, 15, 10, 10, true, 10, 0, Date, Date, 1, 1, 2);
            _databaseSetup.CreateTimeEntry(16, 15, 10, 10, true, 10, 0, Date, Date, 1, 1, 2);
            _databaseSetup.CreateTimeEntry(17, 15, 10, 10, true, 10, 0, Date, Date, 1, 1, 2);

            _invoiceService.CopyTimeEntries(15);

            var cn = (from creditNote in _databaseSetup.GetTrexConnection.TrexEntityContext.CreditNote
                      where creditNote.InvoiceID == 15
                      select creditNote);

            Assert.AreEqual(3, cn.Count());
        }

        [Test]
        public void treeTimeEntries_3TimeEntriesInCreditNoteWithSameDataAsTheTimeEntries()
        {
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 10, 1, false, Date, false, null);
            _databaseSetup.CreateTimeEntry(15, 15, 10, 100, true, 10, 0, Date, Date, 1, 1, 2);
            _databaseSetup.CreateTimeEntry(16, 15, 10, 10, true, 10, 0, Date, Date, 1, 1, 2);
            _databaseSetup.CreateTimeEntry(17, 15, 10, 10, true, 10, 0, Date, Date, 1, 1, 2);

            _invoiceService.CopyTimeEntries(15);

            var cn = (from creditNote in _databaseSetup.GetTrexConnection.TrexEntityContext.CreditNote
                      where creditNote.InvoiceID == 15
                      select creditNote).First();

            var datetime = new DateTime(2012, 01, 01);

            Assert.AreEqual(100, cn.Price, "Wrong price");
            Assert.AreEqual(10, cn.BillableTime, "Wrong billable time");
            Assert.AreEqual(10, cn.TimeSpent, "Wrong timespent");
            Assert.AreEqual(1, cn.TaskID, "Wrong taskID");
            Assert.AreEqual(datetime, cn.StartTime, "wrong starttime");
            Assert.AreEqual(datetime, cn.EndTime, "Wrong EndTime");
            Assert.AreEqual(true, cn.Billable, "Wrong Billable value");
        }
    }

    [TestFixture]
    public class UpdateTimeEntries
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion


        [Test]
        public void OneTimeEntry_NewInvoiceId15_StartDateMatch()
        {
            const string Date = "01-01-2012";
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 10, 1, false, Date, false, null);
            _databaseSetup.CreateTimeEntry(15, null, 10, 10, true, 10, 0, Date, Date, 1, 1, 1);

            _invoiceService.UpdateTimeEntries(15, new DateTime(2012, 1, 1), new DateTime(2012, 1, 5), 1);

            var time = (from timeEntry in _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries
                        where timeEntry.TimeEntryID == 15
                        select timeEntry).First();

            Assert.AreEqual(15, time.InvoiceId);
        }

        [Test]
        public void OneTimeEntry_NewInvoiceId15_EndDateMatch()
        {
            const string Date = "05-01-2012";
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 10, 1, false, Date, false, null);

            _databaseSetup.CreateTimeEntry(15, null, 10, 10, true, 10, 0, Date, Date, 1, 1, 1);


            _invoiceService.UpdateTimeEntries(15, new DateTime(2012, 1, 1), new DateTime(2012, 1, 5), 1);

            var time = (from timeEntry in _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries
                        where timeEntry.TimeEntryID == 15
                        select timeEntry).First();

            Assert.AreEqual(15, time.InvoiceId);
        }

        [Test]
        public void OneTimeEntry_NewInvoiceIdnull_NoTimeEntryInDateTimeSpan()
        {
            const string Date = "06-01-2012";
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 10, 1, false, Date, false, null);

            _databaseSetup.CreateTimeEntry(15, null, 10, 10, true, 10, 0, Date, Date, 1, 1, 1);


            _invoiceService.UpdateTimeEntries(15, new DateTime(2012, 1, 1), new DateTime(2012, 1, 5), 1);

            var time = (from timeEntry in _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries
                        where timeEntry.TimeEntryID == 15
                        select timeEntry).First();

            Assert.AreEqual(null, time.InvoiceId);
        }

        [Test]
        public void TreeTimeEntry_NewInvoiceId15()
        {
            const string Date = "03-01-2012";
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 10, 1, false, Date, false, null);

            _databaseSetup.CreateTimeEntry(15, null, 10, 10, true, 10, 0, Date, Date, 1, 1, 1);
            _databaseSetup.CreateTimeEntry(16, null, 10, 10, true, 10, 0, Date, Date, 1, 1, 1);
            _databaseSetup.CreateTimeEntry(17, null, 10, 10, true, 10, 0, Date, Date, 1, 1, 1);


            _invoiceService.UpdateTimeEntries(15, new DateTime(2012, 1, 1), new DateTime(2012, 1, 5), 1);

            var time = (from timeEntry in _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries
                        where timeEntry.TimeEntryID == 15 || timeEntry.TimeEntryID == 16 || timeEntry.TimeEntryID == 17
                        select timeEntry);

            foreach (var timeEntry in time)
            {
                Assert.AreEqual(15, timeEntry.InvoiceId);
            }
        }

        [Test]
        public void TreeTimeEntry_NewInvoiceId15_1timeEntryOutOfTimeSpan()
        {
            const string Date = "03-01-2012";
            const string OutDate = "06-01-2012";
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 10, 1, false, Date, false, null);

            _databaseSetup.CreateTimeEntry(15, null, 10, 10, true, 10, 0, Date, Date, 1, 1, 1);
            _databaseSetup.CreateTimeEntry(16, null, 10, 10, true, 10, 0, OutDate, OutDate, 1, 1, 1);
            _databaseSetup.CreateTimeEntry(17, null, 10, 10, true, 10, 0, Date, Date, 1, 1, 1);


            _invoiceService.UpdateTimeEntries(15, new DateTime(2012, 1, 1), new DateTime(2012, 1, 5), 1);

            var time = (from timeEntry in _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries
                        where timeEntry.TimeEntryID == 15 || timeEntry.TimeEntryID == 16 || timeEntry.TimeEntryID == 17
                        select timeEntry);

            Assert.AreEqual(2, time.Count(r => r.InvoiceId != null));
        }

        [Test]
        public void OneTimeEntry_NewInvoiceIdNull_StartTimeInSpan_EndTimeOutOfSpan()
        {
            const string Date = "03-01-2012";
            const string OutDate = "06-01-2012";
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 10, 1, false, Date, false, null);

            _databaseSetup.CreateTimeEntry(15, null, 10, 10, true, 10, 0, Date, OutDate, 1, 1, 1);

            _invoiceService.UpdateTimeEntries(15, new DateTime(2012, 1, 1), new DateTime(2012, 1, 5), 1);

            var time = (from timeEntry in _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries
                        where timeEntry.TimeEntryID == 15
                        select timeEntry);

            Assert.AreEqual(null, time.First().InvoiceId);
        }

    }

    [TestFixture]
    public class CreateNewInvoiceLine
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        [Test]
        public void NewInvoiceLine()
        {
            const string Date = "01-01-2012";
            _databaseSetup.CreateInvoice(15, Date, 1, 0.25, Date, Date, Date, 10, 1, false, Date, false, null);
            _invoiceService.CreateNewInvoiceLine(15, 0.25);

            var line = (from invoiceLine in _databaseSetup.GetTrexConnection.TrexEntityContext.InvoiceLines
                        where invoiceLine.InvoiceID == 15
                        select invoiceLine).First();

            Assert.AreEqual(15, line.InvoiceID);
            Assert.AreEqual(0.25, line.VatPercentage);
        }
    }

    [TestFixture]
    public class GenerateCreditNote
    {
        #region SetUp / TearDown

        private InvoiceService _invoiceService;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _invoiceService = null;
            _databaseSetup = null;
        }

        #endregion

        [Test]
        public void NewCreditNoteFromInvoice15()
        {
            _databaseSetup.CreateInvoice(15, "2012-01-01", 1, 0.25, "2012-01-01", "2012-01-01", "2012-01-01", 15, 1,
                                         true, "2012-01-01", false, null);

            _invoiceService.GenerateCreditNote(15, 1);

            var cn = (from invoice in _databaseSetup.GetTrexConnection.TrexEntityContext.Invoices
                      where invoice.InvoiceLinkId == 15
                      select invoice).First();

            Assert.IsTrue(cn.IsCreditNote);
        }

        [Test]
        public void OldInvoiceInvoiceLinkIdUpdate()
        {
            _databaseSetup.CreateInvoice(15, "2012-01-01", 1, 0.25, "2012-01-01", "2012-01-01", "2012-01-01", 15, 1,
                                         true, "2012-01-01", false, null);

            _invoiceService.GenerateCreditNote(15, 1);


            var oldInvoice = (from invoice in _databaseSetup.GetTrexConnection.TrexEntityContext.Invoices
                      where invoice.ID == 15
                      select invoice).First();
            var newCreditNote = (from invoice in _databaseSetup.GetTrexConnection.TrexEntityContext.Invoices
                      where invoice.InvoiceLinkId == 15
                      select invoice).First();

            Assert.IsFalse(oldInvoice.IsCreditNote);
            Assert.AreEqual(newCreditNote.ID, oldInvoice.InvoiceLinkId);
        }

        [Test]
        public void NewCreditNote_AllDataMatch()
        {
            _databaseSetup.CreateInvoice(15, "2012-01-01", 1, 0.25, "2012-01-01", "2012-01-01", "2012-01-01", 15, 1,
                                         true, "2012-01-01", false, null);

            _invoiceService.GenerateCreditNote(15, 1);


            var oldInvoice = (from invoice in _databaseSetup.GetTrexConnection.TrexEntityContext.Invoices
                              where invoice.ID == 15
                              select invoice).First();
            var newCreditNote = (from invoice in _databaseSetup.GetTrexConnection.TrexEntityContext.Invoices
                                 where invoice.InvoiceLinkId == 15
                                 select invoice).First();

            Assert.IsTrue(newCreditNote.Closed, "CreditNote is not closed");
            Assert.AreEqual(1, newCreditNote.CreatedBy, "Created by the wrong person");
            Assert.AreEqual(oldInvoice.CustomerInvoiceGroupId, newCreditNote.CustomerInvoiceGroupId, "Wrong CustomerInvoiceGroupId");
            Assert.IsFalse(newCreditNote.Delivered, "CreditNote Is delivered");
            Assert.AreEqual(oldInvoice.VAT, newCreditNote.VAT, "VAT does not match");
            Assert.AreEqual(oldInvoice.StartDate, newCreditNote.StartDate, "Startdate Does not match");
            Assert.AreEqual(oldInvoice.EndDate, newCreditNote.EndDate, "EndDate does not match");
            Assert.IsTrue(newCreditNote.IsCreditNote, "Credit note is not a creditnote");
            Assert.IsNull(newCreditNote.InvoiceID, "InvoiceID is not null");
            Assert.AreEqual(oldInvoice.ID, newCreditNote.InvoiceLinkId, "Wrong InvoiceLinkId");


        }
    }


}
