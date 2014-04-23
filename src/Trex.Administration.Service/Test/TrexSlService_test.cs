using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using Test;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;
using TrexSL.Web;

namespace TrexSlService_Test
{
    [TestFixture]
    internal class UpdateExclVAT
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSender = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSender.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSender = null;
        }



        private DatabaseSetup _databaseSetup;
        private IInvoiceService _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSender; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void InvoiceID1ExVatIs5500()
        {
            _databaseSetup.CreateInvoiceLine(1, 1, 100, "hours", 5, 0.25, "test", 0, false);
            _databaseSetup.CreateInvoiceLine(2, 1, 500, "hours", 10, 0.25, "test2", 0, false);
            Assert.AreEqual(5500, _trexSLService.UpdateExclVAT(1));
        }

        [Test]
        public void NonExsitingInvoiceId_ExVatIs0()
        {
            Assert.AreEqual(0, _trexSLService.UpdateExclVAT(10));
        }
    }

    [TestFixture]
    internal class SetStandardInvoiceMailTemplate
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();

            _invoiceService = new Mock<IInvoiceService>();
            _templateService = new TemplateService(_databaseSetup.GetTrexConnection);
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSender = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService.Object,
                                               _templateService,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSender.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSender = null;
        }



        private DatabaseSetup _databaseSetup;
        private ITemplateService _templateService;

        private Mock<IInvoiceService> _invoiceService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSender; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void NonExcistingTemplateId_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => _trexSLService.SetStandardInvoiceMailTemplate(1337));
        }
    }

    [TestFixture]
    internal class CreateNewInvoiceLine
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSender = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSender.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSender = null;
        }



        private DatabaseSetup _databaseSetup;
        private IInvoiceService _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSender; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void InvoiceId1Vat50_dbContainsInvoicelineWithInvoiceid5AndVat50()
        {
            _trexSLService.CreateNewInvoiceLine(1, 50);

            var b = (from il in _databaseSetup.EntityContext.InvoiceLines
                     where il.InvoiceID == 1 && il.VatPercentage == 50
                     select il);

            Assert.AreEqual(1, b.Count());
            using (var entity = _databaseSetup.EntityContext)
            {
                foreach (var invoiceLine in b)
                {
                    entity.InvoiceLines.Attach(invoiceLine);
                    entity.InvoiceLines.DeleteObject(invoiceLine);

                }
                entity.SaveChanges();
            }
        }

        [Test]
        public void InvoiceId1337Vat50_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => _trexSLService.CreateNewInvoiceLine(1337, 50));
        }

        [Test]
        public void InvoiceId1337Vat50_IsNotCreatedinDB()
        {
            _trexSLService.CreateNewInvoiceLine(1337, 50);

            var b = (from il in _databaseSetup.EntityContext.InvoiceLines
                     where il.InvoiceID == 1337 && il.VatPercentage == 50
                     select il);

            Assert.AreEqual(0, b.Count());
        }
    }

    [TestFixture]
    internal class GenerateInvoiceLines
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();

            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSender = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSender.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSender = null;
        }

        private DatabaseSetup _databaseSetup;
        private IInvoiceService _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSender; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void InvoiceId1_dbContainsInvoicelineWithInvoiceid1andPrice100()
        {

            _databaseSetup.CreateTimeEntry(2, 1, 1, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 11:00:00", 1, 1,
                                           1);
            _trexSLService.GenerateInvoiceLines(1);

            var b = (from il in _databaseSetup.EntityContext.InvoiceLines
                     where il.InvoiceID == 1 && il.PricePrUnit == 100
                     select il);

            Assert.AreEqual(1, b.Count());

            using (var entity = _databaseSetup.EntityContext)
            {
                foreach (var invoiceLine in b)
                {
                    entity.InvoiceLines.Attach(invoiceLine);
                    entity.InvoiceLines.DeleteObject(invoiceLine);

                }
                entity.SaveChanges();
            }
        }

        [Test]
        public void InvoiceId1_dbContainsInvoicelineWithInvoiceid1andPrice2187_5()
        {

            _databaseSetup.CreateTimeEntry(2, 1, 1, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(3, 1, 1.37, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(4, 1, 1.55, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(5, 1, 1.1, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(6, 1, 1.74, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(7, 1, 1.76, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);

            _databaseSetup.CreateTimeEntry(8, 1, 1, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(9, 1, 1.37, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(10, 1, 1.55, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(11, 1, 1.1, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(12, 1, 1.74, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(13, 1, 1.76, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);

            _trexSLService.GenerateInvoiceLines(1);

            var g = (from il in _databaseSetup.EntityContext.InvoiceLines
                     where il.InvoiceID == 1
                     select il);


            var total = g.Select(p => p.PricePrUnit * p.Units * (1 + p.VatPercentage));
            Assert.AreEqual(2187.5, total.First());
        }

        [Test]
        public void InvoiceId1_dbContainsInvoicelineWithInvoiceid1andPrice3281_25()
        {
            _databaseSetup.CreateUser(2, "testman", 100);

            _databaseSetup.CreateTimeEntry(2, 1, 1, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(3, 1, 1.37, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(4, 1, 1.55, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(5, 1, 1.1, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(6, 1, 1.74, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(7, 1, 1.76, 100, true, 1, 0, "01-01-2012 12:00:00", "01-01-2012 13:00:00", 1, 1,
                                           1);

            _databaseSetup.CreateTimeEntry(8, 1, 1, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(9, 1, 1.37, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(10, 1, 1.55, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(11, 1, 1.1, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(12, 1, 1.74, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(13, 1, 1.76, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 1, 1,
                                           1);

            _databaseSetup.CreateTimeEntry(14, 1, 1, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 2, 1,
                               1);
            _databaseSetup.CreateTimeEntry(15, 1, 1.37, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 2, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(16, 1, 1.55, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 2, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(17, 1, 1.1, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 2, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(18, 1, 1.74, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 2, 1,
                                           1);
            _databaseSetup.CreateTimeEntry(19, 1, 1.76, 100, true, 1, 0, "02-02-2012 12:00:00", "02-02-2012 13:00:00", 2, 1,
                                           1);

            _trexSLService.GenerateInvoiceLines(1);

            var g = (from il in _databaseSetup.EntityContext.InvoiceLines
                     where il.InvoiceID == 1
                     select il);


            var total = g.Select(p => p.PricePrUnit * p.Units * (1 + p.VatPercentage));
            Assert.AreEqual(3281, 25, total.First());
        }
    }

    [TestFixture]
    internal class GetCustomerInvoiceGroupByCustomerId
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new Mock<IInvoiceService>();
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new CustomerInvoiceGroupService(new CustomerInvoiceGroupRepository(_databaseSetup.GetTrexConnection), new Repository(_databaseSetup.GetTrexConnection));
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSender = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService.Object,
                                               _templateService.Object,
                                               _customerInvoiceGroupService,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSender.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSender = null;
        }



        private DatabaseSetup _databaseSetup;
        private Mock<IInvoiceService> _invoiceService;

        private Mock<ITemplateService> _templateService;
        private ICustomerInvoiceGroupService _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSender; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void CustomerId1_2CustomerInvoiceGroups()
        {
            _databaseSetup.CreateCustomerInvoiceGroup(5, 1, "test", false, 1);
            var cigs = _trexSLService.GetCustomerInvoiceGroupByCustomerId(1);

            Assert.AreEqual(2, cigs.Count);
        }

        [Test]
        public void NonExcistingCustomerId_null()
        {
            var cigs = _trexSLService.GetCustomerInvoiceGroupByCustomerId(1337);

            Assert.IsNull(cigs);
        }

        [Test]
        [ExpectedException]
        public void null_null()
        {
            int? i = null;
            var cigs = _trexSLService.GetCustomerInvoiceGroupByCustomerId((int)i);
        }
    }

    [TestFixture]
    internal class GetCustomerInvoiceGroupByCustomerInvoiceFGroupId
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new Mock<IInvoiceService>();
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new CustomerInvoiceGroupService(new CustomerInvoiceGroupRepository(_databaseSetup.GetTrexConnection), new Repository(_databaseSetup.GetTrexConnection));
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSender = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService.Object,
                                               _templateService.Object,
                                               _customerInvoiceGroupService,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSender.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
        }



        private DatabaseSetup _databaseSetup;
        private Mock<IInvoiceService> _invoiceService;

        private Mock<ITemplateService> _templateService;
        private ICustomerInvoiceGroupService _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSender; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void cigId1_ofTypeCustomerInvoiceGroup()
        {
            var cigs = _trexSLService.GetCustomerInvoiceGroupByCustomerInvoiceFGroupId(1);

            Assert.IsInstanceOf<CustomerInvoiceGroup>(cigs);
        }

        [Test]
        public void cigId2_cigWithId2()
        {
            _databaseSetup.CreateCustomerInvoiceGroup(2, 1, "test", false, 1);
            var cigs = _trexSLService.GetCustomerInvoiceGroupByCustomerInvoiceFGroupId(2);


            Assert.AreEqual(2, cigs.CustomerInvoiceGroupID);
        }

        [Test]
        public void NonExcistingId_null()
        {
            _databaseSetup.CreateCustomerInvoiceGroup(2, 1, "test", false, 1);
            var cigs = _trexSLService.GetCustomerInvoiceGroupByCustomerInvoiceFGroupId(1337);

            Assert.AreEqual(null, cigs);
        }
    }

    [TestFixture]
    internal class InsertCustomerInvoiceGroup
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new Mock<IInvoiceService>();
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new CustomerInvoiceGroupService(new CustomerInvoiceGroupRepository(_databaseSetup.GetTrexConnection), new Repository(_databaseSetup.GetTrexConnection));
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSender = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService.Object,
                                               _templateService.Object,
                                               _customerInvoiceGroupService,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSender.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSender = null;
        }



        private DatabaseSetup _databaseSetup;
        private Mock<IInvoiceService> _invoiceService;

        private Mock<ITemplateService> _templateService;
        private ICustomerInvoiceGroupService _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSender; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void groupWith4Cigs_ServerRespone()
        {
            ObservableCollection<CustomerInvoiceGroup> group = new ObservableCollection<CustomerInvoiceGroup>();
            group.Add(_databaseSetup.CreateCustomerInvoiceGroup(15, 1, "test1", false, 1));
            group.Add(_databaseSetup.CreateCustomerInvoiceGroup(16, 1, "test2", false, 1));
            group.Add(_databaseSetup.CreateCustomerInvoiceGroup(17, 1, "test3", false, 1));
            group.Add(_databaseSetup.CreateCustomerInvoiceGroup(18, 1, "test4", false, 1));

            var cigs = _trexSLService.InsertCustomerInvoiceGroup(group);

            Assert.IsInstanceOf<ServerResponse>(cigs);
            Assert.AreEqual(cigs.Response, "Data inserted succesfully");
        }

        [Test]
        public void groupWith4Cigs_dbContains4Cigs()
        {
            ObservableCollection<CustomerInvoiceGroup> group = new ObservableCollection<CustomerInvoiceGroup>();
            group.Add(_databaseSetup.CreateCustomerInvoiceGroup(15, 1, "test1", false, 1));
            group.Add(_databaseSetup.CreateCustomerInvoiceGroup(16, 1, "test2", false, 1));
            group.Add(_databaseSetup.CreateCustomerInvoiceGroup(17, 1, "test3", false, 1));
            group.Add(_databaseSetup.CreateCustomerInvoiceGroup(18, 1, "test4", false, 1));

            var cigs = _trexSLService.InsertCustomerInvoiceGroup(group);

            var ids = from customerInvoiceGroup in _databaseSetup.EntityContext.CustomerInvoiceGroups
                      where customerInvoiceGroup.CustomerInvoiceGroupID == 15 ||
                            customerInvoiceGroup.CustomerInvoiceGroupID == 16 ||
                            customerInvoiceGroup.CustomerInvoiceGroupID == 17 ||
                            customerInvoiceGroup.CustomerInvoiceGroupID == 18
                      select customerInvoiceGroup;

            Assert.AreEqual(4, ids.Count());

        }
    }

    [TestFixture]
    internal class OverWriteCig
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new Mock<IInvoiceService>();
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new CustomerInvoiceGroupService(new CustomerInvoiceGroupRepository(_databaseSetup.GetTrexConnection), new Repository(_databaseSetup.GetTrexConnection));
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSender = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService.Object,
                                               _templateService.Object,
                                               _customerInvoiceGroupService,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSender.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _invoiceSender = null;
            _trexSLService = null;
        }

        private DatabaseSetup _databaseSetup;
        private Mock<IInvoiceService> _invoiceService;

        private Mock<ITemplateService> _templateService;
        private ICustomerInvoiceGroupService _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSender; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void CigWithLabeltest1_ServerResponse()
        {
            var group = new ObservableCollection<CustomerInvoiceGroup>();
            var c = _databaseSetup.CreateCustomerInvoiceGroup(15, 1, "test1", false, 1);
            group.Add(c);
            _trexSLService.InsertCustomerInvoiceGroup(group);

            c.Label = "This have been overwritten";

            var t = _trexSLService.OverWriteCig(c);

            Assert.IsInstanceOf<ServerResponse>(t);
            Assert.AreEqual(t.Response, "Data inserted succesfully");
        }

        [Test]
        public void CigWithLabeltest1_CigWithLabel_This_have_been_overwritten()
        {
            var group = new ObservableCollection<CustomerInvoiceGroup>();
            var c = _databaseSetup.CreateCustomerInvoiceGroup(15, 1, "test1", false, 1);
            group.Add(c);
            _trexSLService.InsertCustomerInvoiceGroup(group);

            c.Label = "This have been overwritten";
            _trexSLService.OverWriteCig(c);

            var label = (from customerInvoiceGroup in _databaseSetup.EntityContext.CustomerInvoiceGroups
                         where customerInvoiceGroup.CustomerInvoiceGroupID == c.CustomerInvoiceGroupID
                         select customerInvoiceGroup.Label).First();

            Assert.AreEqual(label, c.Label);
        }
    }

    [TestFixture]
    internal class DeleteCustomerInvoiceGroup
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new Mock<IInvoiceService>();
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new CustomerInvoiceGroupService(new CustomerInvoiceGroupRepository(_databaseSetup.GetTrexConnection), new Repository(_databaseSetup.GetTrexConnection));
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSender = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService.Object,
                                               _templateService.Object,
                                               _customerInvoiceGroupService,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSender.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _invoiceSender = null;
            _trexSLService = null;
        }

        private DatabaseSetup _databaseSetup;
        private Mock<IInvoiceService> _invoiceService;

        private Mock<ITemplateService> _templateService;
        private ICustomerInvoiceGroupService _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSender; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void DeleteCigWithId15_ServerResponse()
        {
            var group = new ObservableCollection<CustomerInvoiceGroup>();
            var c = _databaseSetup.CreateCustomerInvoiceGroup(15, 1, "test1", false, 1);
            group.Add(c);

            var t = _trexSLService.DeleteCustomerInvoiceGroup(15);

            Assert.IsInstanceOf<ServerResponse>(t);
            Assert.AreEqual(t.Response, "CustomerInvoiceGroup deleted");
        }

        [Test]
        public void nonExcistingCigId_ServerResponse()
        {
            var group = new ObservableCollection<CustomerInvoiceGroup>();
            var c = _databaseSetup.CreateCustomerInvoiceGroup(15, 1, "test1", false, 1);
            group.Add(c);

            var t = _trexSLService.DeleteCustomerInvoiceGroup(1337);

            Assert.IsInstanceOf<ServerResponse>(t);
            Assert.AreEqual(t.Response, "Could not delete CustomerInvoiceGroup");
        }

        [Test]
        public void CigWithAttachedInvoice_ServerResponse()
        {
            var group = new ObservableCollection<CustomerInvoiceGroup>();
            var c = _databaseSetup.CreateCustomerInvoiceGroup(15, 1, "test1", false, 1);
            group.Add(c);
            _databaseSetup.CreateInvoice(2, "01-01-2012", 1, 0.25, "01-01-2012", "01-01-2012", "01-01-2012", null, 15,
                             false, null, false, null);
            var t = _trexSLService.DeleteCustomerInvoiceGroup(15);

            Assert.IsInstanceOf<ServerResponse>(t);
            Assert.AreEqual("You can't delete this cig, there are still projects and/or invoices that uses it \n" +
                                          "delete them, before you can delete this cig", t.Response);
        }

        [Test]
        public void CigWithAttachedProject_ServerResponse()
        {
            var group = new ObservableCollection<CustomerInvoiceGroup>();
            var c = _databaseSetup.CreateCustomerInvoiceGroup(15, 1, "test1", false, 1);
            group.Add(c);
            _databaseSetup.CreateProject(2, 1, "test", 1, "01-01-2012", false, 0, 15);


            var t = _trexSLService.DeleteCustomerInvoiceGroup(15);

            Assert.IsInstanceOf<ServerResponse>(t);
            Assert.AreEqual("You can't delete this cig, there are still projects and/or invoices that uses it \n" +
                              "delete them, before you can delete this cig", t.Response);
        }

        [Test]
        public void DeleteCigWithId15_dbDoesNotContainCigWithId15()
        {
            var group = new ObservableCollection<CustomerInvoiceGroup>();
            var c = _databaseSetup.CreateCustomerInvoiceGroup(15, 1, "test1", false, 1);
            group.Add(c);

            _trexSLService.DeleteCustomerInvoiceGroup(15);

            var label = (from customerInvoiceGroup in _databaseSetup.EntityContext.CustomerInvoiceGroups
                         where customerInvoiceGroup.CustomerInvoiceGroupID == 15
                         select customerInvoiceGroup);

            Assert.IsEmpty(label);
        }
    }

    [TestFixture]
    internal class GetInvoiceById
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSernder = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSernder.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSernder = null;
        }



        private DatabaseSetup _databaseSetup;
        private IInvoiceService _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSernder; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void InvoiceWithId15_InvoiceWithId15()
        {
            var inv = _databaseSetup.CreateInvoice(15, "01-01-2012", 1, 0.25, "01-01-2012", "01-01-2012", "01-01-2012", null, 1, false, "01-01-2012", false, null);

            var i = _invoiceService.GetInvoiceById(inv.ID);

            Assert.AreEqual(15, i.ID);
        }

        [Test]
        public void nonExcistingInvoiceId30_null()
        {
            var inv = _databaseSetup.CreateInvoice(15, "01-01-2012", 1, 0.25, "01-01-2012", "01-01-2012", "01-01-2012", null, 1, false, "01-01-2012", false, null);

            var i = _invoiceService.GetInvoiceById(inv.ID + 15);

            Assert.AreEqual(null, i);
        }
    }

    [TestFixture]
    internal class GenerateInvoicesFromCustomerID
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSernder = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSernder.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSernder = null;
        }



        private DatabaseSetup _databaseSetup;
        private IInvoiceService _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSernder; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void generateNewDraft_newDraftWithId2()
        {
            _databaseSetup.CreateTimeEntry(15, null, 10, 200, true, 10, 0, "01-01-2012", "01-01-2012", 1, 1, 1);

            _trexSLService.GenerateInvoicesFromCustomerID(DateTime.Parse("01-01-2012"), DateTime.Parse("01-01-2012"), 1,
                                                          1, (float)0.25);

            var drf = (from invoice in _databaseSetup.EntityContext.Invoices
                       where invoice.ID == 2
                       select invoice).First();

            Assert.AreEqual(DateTime.Parse("01-01-2012"), drf.StartDate);
            Assert.AreEqual(DateTime.Parse("01-01-2012"), drf.EndDate);
            Assert.AreEqual(null, drf.InvoiceID);
            Assert.AreEqual(2, drf.ID);

        }

        [Test]
        public void timeEntryWithId15_TimeEntryInvoiceIdhaveBeenUpdated()
        {
            var te = _databaseSetup.CreateTimeEntry(15, null, 10, 200, true, 10, 0, "01-01-2012", "01-01-2012", 1, 1, 1);

            _trexSLService.GenerateInvoicesFromCustomerID(DateTime.Parse("01-01-2012"), DateTime.Parse("01-01-2012"), 1,
                                                          1, (float)0.25);

            var drf = (from invoice in _databaseSetup.EntityContext.Invoices
                       where invoice.ID == 2
                       select invoice).First();

            var te2 = (from time in _databaseSetup.EntityContext.TimeEntries
                       where time.TimeEntryID == te.TimeEntryID
                       select time).First();

            Assert.AreEqual(drf.ID, te2.InvoiceId);


        }

        [Test]
        public void _5timeEntries_TimeEntriesInvoiceIdhaveBeenUpdated()
        {
            var te1 = _databaseSetup.CreateTimeEntry(15, null, 10, 200, true, 10, 0, "01-01-2012", "01-01-2012", 1, 1, 1);
            var te2 = _databaseSetup.CreateTimeEntry(16, null, 10, 200, true, 10, 0, "01-01-2012", "01-01-2012", 1, 1, 1);
            var te3 = _databaseSetup.CreateTimeEntry(17, null, 10, 200, true, 10, 0, "01-01-2012", "01-01-2012", 1, 1, 1);
            var te4 = _databaseSetup.CreateTimeEntry(18, null, 10, 200, true, 10, 0, "01-01-2012", "01-01-2012", 1, 1, 1);
            var te5 = _databaseSetup.CreateTimeEntry(19, null, 10, 200, true, 10, 0, "01-01-2012", "01-01-2012", 1, 1, 1);

            _trexSLService.GenerateInvoicesFromCustomerID(DateTime.Parse("01-01-2012"), DateTime.Parse("01-01-2012"), 1,
                                                          1, (float)0.25);

            var drf = (from invoice in _databaseSetup.EntityContext.Invoices
                       where invoice.ID == 2
                       select invoice).First();

            var te6 = (from time in _databaseSetup.EntityContext.TimeEntries
                       where time.TimeEntryID == te1.TimeEntryID
                       select time).First();
            var te7 = (from time in _databaseSetup.EntityContext.TimeEntries
                       where time.TimeEntryID == te2.TimeEntryID
                       select time).First();
            var te8 = (from time in _databaseSetup.EntityContext.TimeEntries
                       where time.TimeEntryID == te3.TimeEntryID
                       select time).First();
            var te9 = (from time in _databaseSetup.EntityContext.TimeEntries
                       where time.TimeEntryID == te4.TimeEntryID
                       select time).First();
            var te10 = (from time in _databaseSetup.EntityContext.TimeEntries
                        where time.TimeEntryID == te5.TimeEntryID
                        select time).First();

            Assert.AreEqual(drf.ID, te6.InvoiceId);
            Assert.AreEqual(drf.ID, te7.InvoiceId);
            Assert.AreEqual(drf.ID, te8.InvoiceId);
            Assert.AreEqual(drf.ID, te9.InvoiceId);
            Assert.AreEqual(drf.ID, te10.InvoiceId);
        }

        [Test]
        public void TimeEntryWithDateOutsideInvoiceDates_TimeEntryInvoiceIdhaveNOTBeenUpdated()
        {
            var te1 = _databaseSetup.CreateTimeEntry(15, null, 10, 200, true, 10, 0, "01-01-2012", "01-01-2012", 1, 1, 1);
            var te2 = _databaseSetup.CreateTimeEntry(16, null, 10, 200, true, 10, 0, "03-01-2012", "03-01-2012", 1, 1, 1);

            _trexSLService.GenerateInvoicesFromCustomerID(DateTime.Parse("01-01-2012"), DateTime.Parse("01-01-2012"), 1,
                                                          1, (float)0.25);

            var drf = (from invoice in _databaseSetup.EntityContext.Invoices
                       where invoice.ID == 2
                       select invoice).First();

            var te3 = (from time in _databaseSetup.EntityContext.TimeEntries
                       where time.TimeEntryID == te1.TimeEntryID
                       select time).First();

            var te4 = (from time in _databaseSetup.EntityContext.TimeEntries
                       where time.TimeEntryID == te2.TimeEntryID
                       select time).First();

            Assert.AreEqual(drf.ID, te3.InvoiceId);
            Assert.AreEqual(null, te4.InvoiceId);


        }
    }

    [TestFixture]
    internal class GenerateCreditnote
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSernder = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSernder.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSernder = null;
        }



        private DatabaseSetup _databaseSetup;
        private IInvoiceService _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSernder; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void InvoiceWithId15_CreditnoteWithId16()
        {
            var i = _databaseSetup.CreateInvoice(15, "01-01-2012", 1, 0.25, "01-01-2012", "01-01-2012", "01-01-2012", 15,
                                                 1, true, "01-01-2012", false, null);
            _trexSLService.GenerateCreditnote(i.ID, 1);

            var cn = (from invoice in _databaseSetup.EntityContext.Invoices
                      where invoice.ID == 16
                      select invoice).First();

            Assert.AreEqual(true, cn.IsCreditNote);

        }

        [Test]
        public void InvoiceWithId15_CreditnoteWithId16AndInvoiceLinkId15()
        {
            var i = _databaseSetup.CreateInvoice(15, "01-01-2012", 1, 0.25, "01-01-2012", "01-01-2012", "01-01-2012", 15,
                                                 1, true, "01-01-2012", false, null);
            _trexSLService.GenerateCreditnote(i.ID, 1);

            var cn = (from invoice in _databaseSetup.EntityContext.Invoices
                      where invoice.ID == 16
                      select invoice).First();

            Assert.AreEqual(true, cn.IsCreditNote);
            Assert.AreEqual(15, cn.InvoiceLinkId);

        }

        [Test]
        public void InvoiceWithId15_InvoiceWithId16AndInvoiceLinkId16()
        {
            var i = _databaseSetup.CreateInvoice(15, "01-01-2012", 1, 0.25, "01-01-2012", "01-01-2012", "01-01-2012", 15,
                                                 1, true, "01-01-2012", false, null);
            _trexSLService.GenerateCreditnote(i.ID, 1);

            var cn = (from invoice in _databaseSetup.EntityContext.Invoices
                      where invoice.ID == 16
                      select invoice).First();

            var inv = (from invoice in _databaseSetup.EntityContext.Invoices
                       where invoice.ID == 15
                       select invoice).First();

            Assert.AreEqual(15, cn.InvoiceLinkId);
            Assert.AreEqual(16, inv.InvoiceLinkId);

        }
    }

    [TestFixture]
    internal class GetTimeEntriesForInvoicing
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new InvoiceService(_databaseSetup.GetTrexConnection);
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSernder = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSernder.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSernder = null;
        }



        private DatabaseSetup _databaseSetup;
        private IInvoiceService _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSernder; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void _3TimeEntries_ListOfTimeEntries()
        {
            _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "02-01-2012", "02-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(16, null, 10, 100, true, 10, 0, "02-01-2012", "02-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(17, null, 10, 100, true, 10, 0, "02-01-2012", "02-01-2012", 1, 1, 1);

            _databaseSetup.CreateTimeEntry(18, null, 10, 100, true, 10, 0, "03-01-2012", "03-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(19, null, 10, 100, true, 10, 0, "03-01-2012", "03-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(20, null, 10, 100, true, 10, 0, "03-01-2012", "03-01-2012", 1, 1, 1);

            var list = _trexSLService.GetTimeEntriesForInvoicing(DateTime.Parse("02, 01, 2012"), DateTime.Parse("03, 01, 2012"), 1);

            Assert.IsInstanceOf<List<TimeEntry>>(list);
        }

        [Test]
        public void _6TimeEntries_3TimeEntries()
        {
            _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "02-01-2012", "02-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(16, null, 10, 100, true, 10, 0, "02-01-2012", "02-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(17, null, 10, 100, true, 10, 0, "02-01-2012", "02-01-2012", 1, 1, 1);

            _databaseSetup.CreateTimeEntry(18, null, 10, 100, true, 10, 0, "03-01-2012", "03-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(19, null, 10, 100, true, 10, 0, "03-01-2012", "03-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(20, null, 10, 100, true, 10, 0, "03-01-2012", "03-01-2012", 1, 1, 1);

            var list = _trexSLService.GetTimeEntriesForInvoicing(DateTime.Parse("02, 01, 2012"), DateTime.Parse("02, 01, 2012"), 1);

            Assert.AreEqual(3, list.Count);
        }

        [Test]
        public void _6TimeEntries_6TimeEntries()
        {
            _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "02-01-2012", "02-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(16, null, 10, 100, true, 10, 0, "02-01-2012", "02-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(17, null, 10, 100, true, 10, 0, "02-01-2012", "02-01-2012", 1, 1, 1);

            _databaseSetup.CreateTimeEntry(18, null, 10, 100, true, 10, 0, "03-01-2012", "03-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(19, null, 10, 100, true, 10, 0, "03-01-2012", "03-01-2012", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(20, null, 10, 100, true, 10, 0, "03-01-2012", "03-01-2012", 1, 1, 1);

            var list = _trexSLService.GetTimeEntriesForInvoicing(DateTime.Parse("02, 01, 2012"), DateTime.Parse("03, 01, 2012"), 1);

            Assert.AreEqual(6, list.Count);
        }
    }

    [TestFixture]
    internal class SaveTimeEntry
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new Mock<IInvoiceService>();
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new TimeEntryService(_databaseSetup.GetTrexConnection);
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSernder = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService.Object,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSernder.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSernder = null;
        }



        private DatabaseSetup _databaseSetup;
        private Mock<IInvoiceService> _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private ITimeEntryService _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSernder; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void TimeEntryPrice100_ChangePriceTo500_Save_GetTimeentryWithPrice500()
        {
            var te1 = _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "02-01-2012", "02-01-2012", 1, 1, 1);

            te1.Price = 500;
            _trexSLService.SaveTimeEntry(te1);

            var tee1 = (from timeEntry in _databaseSetup.EntityContext.TimeEntries
                        where timeEntry.TimeEntryID == te1.Id
                        select timeEntry).First();

            Assert.AreEqual(500, tee1.Price);
        }
    }

    [TestFixture]
    public class SaveTask
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new Mock<IInvoiceService>();
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new TaskService(_databaseSetup.GetTrexConnection);
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSernder = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService.Object,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSernder.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSernder = null;
        }



        private DatabaseSetup _databaseSetup;
        private Mock<IInvoiceService> _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private ITaskService _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSernder; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void TasnametestAfSaveTask_ChangePriceToNewTextToTest_Save_TaskWithNameNewTextToTest()
        {
            var t1 = _databaseSetup.CreateTask(15, 1, 1, "01-01-2012", "testAfSaveTask");

            t1.TaskName = "NewTextToTest";

            var t = _trexSLService.SaveTask(t1);
            t.AcceptChanges();

            var ta1 = (from timeEntry in _databaseSetup.EntityContext.Tasks
                       where timeEntry.TaskID == t1.TaskID
                       select timeEntry).First();

            Assert.AreEqual("NewTextToTest", ta1.TaskName);
        }
    }

    [TestFixture]
    internal class SaveProject
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new Mock<IInvoiceService>();
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new ProjectService(_databaseSetup.GetTrexConnection);
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSernder = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService.Object,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSernder.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _invoiceSernder = null;
            _trexSLService = null;
        }



        private DatabaseSetup _databaseSetup;
        private Mock<IInvoiceService> _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private IProjectService _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSernder; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void ProjectWithNameTestProj_ChangeNameToNewTestProj_Save_GetTProjectWithNameNewTestProj()
        {
            var p1 = _databaseSetup.CreateProject(15, 1, "Test Proj", 1, "01-01-2012", false, null, 1);

            p1.ProjectName = "New Test Project";

            _trexSLService.SaveProject(p1);

            var pee1 = (from proj in _databaseSetup.EntityContext.Projects
                        where proj.ProjectID == p1.Id
                        select proj).First();

            Assert.AreEqual("New Test Project", pee1.ProjectName);
        }
    }

    [TestFixture]
    internal class SaveCustomer
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new Mock<IInvoiceService>();
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new CustomerServices(_databaseSetup.GetTrexConnection);
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new Mock<ITimeEntryService>();
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSernder = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService.Object,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService.Object,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSernder.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSernder = null;
        }



        private DatabaseSetup _databaseSetup;
        private Mock<IInvoiceService> _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private ICustomerService _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private Mock<ITimeEntryService> _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSernder; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void CustomerNameTestCustomer_ChangenameToNewtestCustomer_Save_GetCustomerWithNameNewtestCustomer()
        {
            var customer = _databaseSetup.CreateCustomer(15, "Test Customer", "Mail", "Me", "TestStreet1", "TestCity",
                                                    "Testistan", "8888");

            customer.CustomerName = "New test Customer";

            _trexSLService.SaveCustomer(customer);

            var CustomerEntry1 = (from timeEntry in _databaseSetup.EntityContext.Customers
                                  where timeEntry.CustomerID == customer.Id
                                  select timeEntry).First();

            Assert.AreEqual("New test Customer", CustomerEntry1.CustomerName);
        }
    }

    [TestFixture]
    internal class GetTimeEntriesByPeriodAndUser
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceService = new Mock<IInvoiceService>();
            _templateService = new Mock<ITemplateService>();
            _customerInvoiceGroupService = new Mock<ICustomerInvoiceGroupService>();
            _customerService = new Mock<ICustomerService>();
            _projectService = new Mock<IProjectService>();
            _taskService = new Mock<ITaskService>();
            _timeEntryService = new TimeEntryService(_databaseSetup.GetTrexConnection);
            _userManagementService = new Mock<IUserManagementService>();
            _membershipService = new Mock<IMembershipService>();
            _invoiceSernder = new Mock<IInvoiceSender>();

            _trexSLService = new TrexSLService(_invoiceService.Object,
                                               _templateService.Object,
                                               _customerInvoiceGroupService.Object,
                                               _customerService.Object,
                                               _projectService.Object,
                                               _taskService.Object,
                                               _timeEntryService,
                                               _userManagementService.Object,
                                               _membershipService.Object,
                                               _invoiceSernder.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _invoiceService = null;
            _templateService = null;
            _customerInvoiceGroupService = null;
            _customerService = null;
            _projectService = null;
            _taskService = null;
            _timeEntryService = null;
            _userManagementService = null;
            _membershipService = null;
            _trexSLService = null;
            _invoiceSernder = null;
        }



        private DatabaseSetup _databaseSetup;
        private Mock<IInvoiceService> _invoiceService;

        private Mock<ITemplateService> _templateService;
        private Mock<ICustomerInvoiceGroupService> _customerInvoiceGroupService;
        private Mock<ICustomerService> _customerService;
        private Mock<IProjectService> _projectService;
        private Mock<ITaskService> _taskService;
        private ITimeEntryService _timeEntryService;
        private Mock<IUserManagementService> _userManagementService;
        private Mock<IMembershipService> _membershipService;
        private Mock<IInvoiceSender> _invoiceSernder; 

        private TrexSLService _trexSLService;

        #endregion

        [Test]
        public void NewUser_3NewTimeEntries_GetOnlyTimeEntriesInPeriodWithNewUSer()
        {
            var tempUser = _databaseSetup.CreateUser(5, "testman", 10);
            _databaseSetup.CreateTimeEntry(14, 1, 10, 10, true, 10, 0, "01-01-2013", "02-01-2013", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(15, 1, 10, 10, true, 10, 0, "01-01-2013", "02-01-2013", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(16, 1, 10, 10, true, 10, 0, "01-01-2013", "02-01-2013", 5, 1, 1);

            DateTime start = new DateTime(2013, 1, 1);
            DateTime end = new DateTime(2013, 1, 2);

            var temp = _trexSLService.GetTimeEntriesByPeriodAndUser(tempUser, start, end);

            Assert.AreEqual(1, temp.Count);
        }
    }

}