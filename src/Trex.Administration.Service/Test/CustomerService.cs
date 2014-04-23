using System;
using NUnit.Framework;
using Test;
using System.Linq;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;

namespace CustomerService_Test
{
    [TestFixture]
    class GetCustomerByCustomerId
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _customerServices = new CustomerServices(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _customerServices = null;
        }
        #endregion

        private DatabaseSetup _databaseSetup;
        private CustomerServices _customerServices;

        [Test]
        public void CustomerId15_CustomerWithId15()
        {
            _databaseSetup.CreateCustomer(15, "New test Customer", "test@test.dk", "Me", "Street", "City", "Country",
                                          "ZipCode");

            var newCustomer = _customerServices.GetCustomerById(15, false, false, false, false, false);

            Assert.AreEqual(15, newCustomer.CustomerID);
        }

        [Test]
        public void CustomerId15_NotIncludeProjects()
        {
            _databaseSetup.CreateCustomer(15, "New test Customer", "test@test.dk", "Me", "Street", "City", "Country",
                                          "ZipCode");
            _databaseSetup.CreateProject(15, 15, "testproj", 1, "01-01-2012", false, null, 1);

            var newCustomer = _customerServices.GetCustomerById(15, false, false, false, false, false);

            Assert.IsEmpty(newCustomer.Projects);
        }

        [Test]
        public void CustomerId15_IncludeProjects()
        {
            _databaseSetup.CreateCustomer(15, "New test Customer", "test@test.dk", "Me", "Street", "City", "Country",
                                          "ZipCode");
            _databaseSetup.CreateProject(15, 15, "testproj", 1, "01-01-2012", false, null, 1);

            var newCustomer = _customerServices.GetCustomerById(15, false, false, true, false, false);

            Assert.IsNotEmpty(newCustomer.Projects);
        }

        [Test]
        public void CustomerId15_IncludeProjects_Tasks()
        {
            _databaseSetup.CreateCustomer(15, "New test Customer", "test@test.dk", "Me", "Street", "City", "Country",
                                          "ZipCode");
            _databaseSetup.CreateProject(15, 15, "testproj", 1, "01-01-2012", false, null, 1);
            _databaseSetup.CreateTask(15, 15, 1, "01-01-2012", "New Task");

            var newCustomer = _customerServices.GetCustomerById(15, false, false, true, false, false);

            Assert.IsNotEmpty(newCustomer.Projects.Select(t => t.Tasks));
        }

        [Test]
        public void CustomerId15_IncludeProjects_Tasks_TimeEntries()
        {
            _databaseSetup.CreateCustomer(15, "New test Customer", "test@test.dk", "Me", "Street", "City", "Country",
                                          "ZipCode");
            _databaseSetup.CreateProject(15, 15, "testproj", 1, "01-01-2012", false, null, 1);
            _databaseSetup.CreateTask(15, 15, 1, "01-01-2012", "New Task");
            _databaseSetup.CreateTimeEntry(15, null, 10, 10, true, 10, 0, "01-01-2012", "01-01-2012", 1, 15, 1);

            var newCustomer = _customerServices.GetCustomerById(15, false, false, true, false, false);

            Assert.IsNotEmpty(newCustomer.Projects.Select(t => t.Tasks.Select(te => te.TimeEntries)));
        }
    }

    [TestFixture]
    public class GetAllCustomers
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _customerServices = new CustomerServices(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _customerServices = null;
        }
        private DatabaseSetup _databaseSetup;
        private CustomerServices _customerServices;
        #endregion

        [Test]
        public void Return_Two_Customers()
        {
            _databaseSetup.CreateCustomer(15, "New Test Customer", "test@test.dk", "Me", "Street", "City", "Country",
                                          "Zip");

            var newCustomerList = _customerServices.GetAllCustomers(false, false, false, false, false);

            Assert.AreEqual(2, newCustomerList.Count);
        }
    }

    [TestFixture]
    public class GetCustomerInvoiceViews
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();

            _customerServices = new CustomerServices(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _customerServices = null;
        }
        private DatabaseSetup _databaseSetup;
        private CustomerServices _customerServices;
        #endregion

        [Test]
        public void DatetimeNow_Return1Customer()
        {
            var newlist = _customerServices.GetCustomerInvoiceViews(DateTime.Now, DateTime.Now);

            Assert.AreEqual(1, newlist.Count);
        }

        [Test]
        public void DatetimeNow_Return2Customer()
        {
            _databaseSetup.CreateCustomer(15, "TestC", "mail", "me", "testvej", "testby", "testLand", "0911");
            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "labal", true, 1);
            var newlist = _customerServices.GetCustomerInvoiceViews(DateTime.Now, DateTime.Now);

            Assert.AreEqual(2, newlist.Count);
        }

        [Test]
        public void DatetimeNow_ReturnTwoCustomerWithOneDraftEach()
        {
            _databaseSetup.CreateCustomer(15, "TestC", "mail", "me", "testvej", "testby", "testLand", "0911");
            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "labal", true, 1);
            var newlist = _customerServices.GetCustomerInvoiceViews(DateTime.Now, DateTime.Now);

            Assert.AreEqual(2, newlist.Select(c => c.Drafts).Count());
        }

        [Test]
        public void DatetimeNow_FirstDateNowInvoiced_01012000_TimeEntryDocTypeOne()
        {
            _databaseSetup.CreateCustomer(15, "TestC", "mail", "me", "testvej", "testby", "testLand", "0911");
            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "labal", true, 1);
            _databaseSetup.CreateProject(15, 15, "testcustomerone", 1, "01-01-2012", false, null, 15);
            _databaseSetup.CreateTask(15, 15, 1, "01-01-2012", "testtas");
            _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "01-01-2000", "01-01-2001", 1, 15, 1);
            var newlist = _customerServices.GetCustomerInvoiceViews(DateTime.Now, DateTime.Now);

            Assert.AreEqual(new DateTime(2000, 01, 01), newlist.Select(c => c.FirstDateNotInvoiced).Last());
        }

        [Test]
        public void DatetimeNow_FirstDateNowInvoiced_01012000_TimeEntryDocTypeTwo_ContainsNothing()
        {
            _databaseSetup.CreateCustomer(15, "TestC", "mail", "me", "testvej", "testby", "testLand", "0911");
            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "labal", true, 1);
            _databaseSetup.CreateProject(15, 15, "testcustomerone", 1, "01-01-2012", false, null, 15);
            _databaseSetup.CreateTask(15, 15, 1, "01-01-2012", "testtas");
            _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "01-01-2000", "01-01-2001", 1, 15, 2);
            var newlist = _customerServices.GetCustomerInvoiceViews(DateTime.Now, DateTime.Now);

            Assert.IsNull(newlist.Select(c => c.FirstDateNotInvoiced).Last());
        }

        [Test]
        public void DatetimeNow_FirstDateNowInvoiced_01012000_TimeEntryDocTypeTree()
        {
            _databaseSetup.CreateCustomer(15, "TestC", "mail", "me", "testvej", "testby", "testLand", "0911");
            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "labal", true, 1);
            _databaseSetup.CreateProject(15, 15, "testcustomerone", 1, "01-01-2012", false, null, 15);
            _databaseSetup.CreateTask(15, 15, 1, "01-01-2012", "testtas");
            _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "01-01-2000", "01-01-2001", 1, 15, 3);
            var newlist = _customerServices.GetCustomerInvoiceViews(DateTime.Now, DateTime.Now);

            Assert.AreEqual(new DateTime(2000, 01, 01), newlist.Select(c => c.FirstDateNotInvoiced).Last());
        }

        //[Test]
        //public void DatetimeNow_One_DistinctPrices()
        //{
        //    _databaseSetup.CreateCustomer(15, "TestC", "mail", "me", "testvej", "testby", "testLand", "0911");
        //    _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "labal", true, 1);
        //    _databaseSetup.CreateProject(15, 15, "testcustomerone", 1, "01-01-2012", false, null, 15);
        //    _databaseSetup.CreateTask(15, 15, 1, "01-01-2012", "testtas");
        //    _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "01-01-2000", "01-01-2001", 1, 15, 1);

        //    var newlist = _customerServices.GetCustomerInvoiceViews(new DateTime(2000, 01, 01), new DateTime(2001, 01, 01));

        //    var tmp = newlist.Where(c => c.CustomerID == 15).Select(c => c.DistinctPrices);
        //    Assert.AreEqual(1, tmp.First());
        //}

        //[Test]
        //public void DatetimeNow_Two_DistinctPrices()
        //{
        //    _databaseSetup.CreateCustomer(15, "TestC", "mail", "me", "testvej", "testby", "testLand", "0911");
        //    _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "labal", true, 1);
        //    _databaseSetup.CreateProject(15, 15, "testcustomerone", 1, "01-01-2012", false, null, 15);
        //    _databaseSetup.CreateTask(15, 15, 1, "01-01-2012", "testtas");
        //    _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "01-01-2000", "01-01-2001", 1, 15, 1);
        //    _databaseSetup.CreateTimeEntry(16, null, 10, 1200, true, 10, 0, "01-01-2000", "01-01-2001", 1, 15, 1);

        //    var newlist = _customerServices.GetCustomerInvoiceViews(new DateTime(2000, 01, 01), new DateTime(2001, 01, 01));
        //    var tmp = newlist.Where(c => c.CustomerID == 15).Select(c => c.DistinctPrices);
        //    Assert.AreEqual(2, tmp.First());
        //}
        [Test]
        public void DatetimeNow_1300_InventoryValue()
        {
            _databaseSetup.CreateCustomer(15, "TestC", "mail", "me", "testvej", "testby", "testLand", "0911");
            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "labal", true, 1);
            _databaseSetup.CreateProject(15, 15, "testcustomerone", 1, "01-01-2012", false, null, 15);
            _databaseSetup.CreateTask(15, 15, 1, "01-01-2012", "testtas");
            _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "01-01-2000", "01-01-2001", 1, 15, 1);
            _databaseSetup.CreateTimeEntry(16, null, 10, 1200, true, 10, 0, "01-01-2000", "01-01-2001", 1, 15, 1);

            var newlist = _customerServices.GetCustomerInvoiceViews(new DateTime(2000, 01, 01), new DateTime(2001, 01, 01));
            var tmp = newlist.Where(c => c.CustomerID == 15).Select(c => c.InventoryValue);
            Assert.AreEqual(13000.0d, tmp.First());
        }

        [Test]
        public void DatetimeNow_10_NonBillableTime()
        {
            _databaseSetup.CreateCustomer(15, "TestC", "mail", "me", "testvej", "testby", "testLand", "0911");
            _databaseSetup.CreateCustomerInvoiceGroup(15, 15, "labal", true, 1);
            _databaseSetup.CreateProject(15, 15, "testcustomerone", 1, "01-01-2012", false, null, 15);
            _databaseSetup.CreateTask(15, 15, 1, "01-01-2012", "testtas");
            _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "01-01-2000", "01-01-2001", 1, 15, 1);
            _databaseSetup.CreateTimeEntry(16, null, 20, 1200, false, 20, 0, "01-01-2000", "01-01-2001", 1, 15, 1);

            var newlist = _customerServices.GetCustomerInvoiceViews(new DateTime(2000, 01, 01), new DateTime(2001, 01, 01));
            var tmp = newlist.Where(c => c.CustomerID == 15).Select(c => c.NonBillableTime);
            Assert.AreEqual(20, tmp.First());
        }

        [TestFixture]
        public class SaveCustomer
        {
            #region Setup/Teardown
            [SetUp]
            public void Setup()
            {
                _databaseSetup = new DatabaseSetup();
                _databaseSetup.CleanDatabase();
                _databaseSetup.CreateStandardDatabase();

                _customerServices = new CustomerServices(_databaseSetup.GetTrexConnection);
            }

            [TearDown]
            public void TearDown()
            {
                _databaseSetup = null;
                _customerServices = null;
            }
            private DatabaseSetup _databaseSetup;
            private CustomerServices _customerServices;
            #endregion

            [Test]
            public void SaveNewCustomer()
            {
                Customer customer = new Customer
                                        {
                                            CustomerName = "New Customer to save",
                                            CreateDate = DateTime.Now,
                                            CreatedBy = 1,
                                            City = "Wierd city"
                                        };
                _customerServices.SaveCustomer(customer);

                var c = (from customer1 in _databaseSetup.GetTrexConnection.TrexEntityContext.Customers
                         where customer1.City == "Wierd city"
                         select customer1).First();

                Assert.AreEqual("New Customer to save", c.CustomerName);
            }

            [Test]
            public void SaveChangeToExsitingCustomer()
            {
                var customer = (from customer1 in _databaseSetup.GetTrexConnection.TrexEntityContext.Customers
                                where customer1.CustomerID == 1
                                select customer1).First();

                customer.CustomerName = "This is a change";

                _customerServices.SaveCustomer(customer);

                var changedcustomer = (from customer1 in _databaseSetup.GetTrexConnection.TrexEntityContext.Customers
                                where customer1.CustomerID == 1
                                select customer1).First();

                Assert.AreEqual("This is a change", changedcustomer.CustomerName);

            }
        }
        
    }
}
