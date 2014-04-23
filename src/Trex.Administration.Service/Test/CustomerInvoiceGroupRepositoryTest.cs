
﻿using System;
﻿using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
﻿using Test;
﻿using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;

namespace CustomerInvoiceGroupRepository_Test
{
    [TestFixture]
    class GetCustomerIdByCustomerInvoiceGroupId
    {
        #region Setup/Teardown

        private CustomerInvoiceGroupRepository CigRepository;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            CigRepository = new CustomerInvoiceGroupRepository(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            CigRepository = null;
            _databaseSetup = null;
        }
        #endregion

        [Test]
        public void CigId1_CustomerId1()
        {
            var test = CigRepository.GetCustomerIdByCustomerInvoiceGroupId(1);
            Assert.AreEqual(1, test);
        }

        [Test]
        public void NonExsitingCigId1337_returnsNegativ20()
        {
            var test = CigRepository.GetCustomerIdByCustomerInvoiceGroupId(1337);
            Assert.AreEqual(-20, test);
        }
    }

    [TestFixture]
    class GetCustomerInvoiceGroupIDByCustomerID
    {

        #region Setup/Teardown
        private DatabaseSetup _databaseSetup;
        private Customer _customer;

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _cigr = new CustomerInvoiceGroupRepository(_databaseSetup.GetTrexConnection);
            _customer = _databaseSetup.CreateCustomer(2, "testCustomer", "Test@Customer.dk", "CustomerContact", "TestCustomerVej",
                                                      "TestCustomerCity", "TestCustomerLand", "7777");

            _databaseSetup.CreateCustomerInvoiceGroup(2, 2, "TestCig", false, 1, "TestCigMail", "TestCigMailCC", "TestCigAttention",
                                                      "TestCigvej1", "TestCigVej2", "TestCigCity", "TestCigLand", "8888");

            _databaseSetup.CreateCustomerInvoiceGroup(3, 2, "TestCig2", false, 1, null, null, null, null, null, null, null, null);
        }

        [TearDown]
        public void TearDown()
        {
            _cigr = null;
            _databaseSetup = null;
        }

        #endregion
        private CustomerInvoiceGroupRepository _cigr;

        [Test]
        public void nonExcitingCigId1337_CustomerIdNegativ20()
        {
            Assert.AreEqual(-20, _cigr.GetCustomerIdByCustomerInvoiceGroupId(1337));
        }

        [Test]
        public void CustomerWithNonExsitingId_EmptyList()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(new Customer { CustomerID = 1337 });
            Assert.IsEmpty(t);
        }

        [Test]
        public void CustomerWithId2_ListOfCigs()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);

            Assert.IsInstanceOf<List<CustomerInvoiceGroup>>(t);
        }

        [Test]
        public void CustomerWithId2_ListWith2Cig()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);

            Assert.AreEqual(2, t.Count);
        }

        [Test]
        public void CustomerWithId2_CigWithCityTestCigCity()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var city = t.Where(c => c.CustomerID == 2).Select(b => b.City).First();


            Assert.AreEqual("TestCigCity", city);
        }

        [Test]
        public void CustomerWithId2_CigWithTestCigLand()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var Country = t.Where(c => c.CustomerID == 2).Select(b => b.Country).First();


            Assert.AreEqual("TestCigLand", Country);
        }

        [Test]
        public void CustomerWithId2_CigWithAddressTestCigvej1()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var Address1 = t.Where(c => c.CustomerID == 2).Select(b => b.Address1).First();


            Assert.AreEqual("TestCigvej1", Address1);
        }

        [Test]
        public void CustomerWithId2_CigWithAdress2TestCigVej2()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var Address2 = t.Where(c => c.CustomerID == 2).Select(b => b.Address2).First();


            Assert.AreEqual("TestCigVej2", Address2);
        }

        [Test]
        public void CustomerWithId2_CigWithAttentionTestCigAttention()
        {

            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var Attention = t.Where(c => c.CustomerID == 2).Select(b => b.Attention).First();


            Assert.AreEqual("TestCigAttention", Attention);
        }

        [Test]
        public void CustomerWithId2_CigWithZipCode8888()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var ZipCode = t.Where(c => c.CustomerID == 2).Select(b => b.ZipCode).First();


            Assert.AreEqual("8888", ZipCode);
        }

        [Test]
        public void CustomerWithId2_CigWithEmail_TestCigMail()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var Email = t.Where(c => c.CustomerID == 2).Select(b => b.Email).First();


            Assert.AreEqual("TestCigMail", Email);
        }

        [Test]
        public void CustomerWithId2_CigWithEmailCC_TestCigMailCC()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var EmailCC = t.Where(c => c.CustomerID == 2).Select(b => b.EmailCC).First();


            Assert.AreEqual("TestCigMailCC", EmailCC);
        }

        [Test]
        public void CustomerWithId2_CigWithCityTestCustomerCity()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var City = t.Where(c => c.CustomerID == 2).Select(b => b.City).Last();


            Assert.AreEqual("TestCustomerCity", City);
        }

        [Test]
        public void CustomerWithId2_CigWithStreetAddressTestCustomerVej()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var Address1 = t.Where(c => c.CustomerID == 2).Select(b => b.Address1).Last();


            Assert.AreEqual("TestCustomerVej", Address1);
        }

        [Test]
        public void CustomerWithId2_CigWithContactNameCustomerContact()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var Attention = t.Where(c => c.CustomerID == 2).Select(b => b.Attention).Last();


            Assert.AreEqual("CustomerContact", Attention);
        }

        [Test]
        public void CustomerWithId2_CigWithZipCode7777()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var ZipCode = t.Where(c => c.CustomerID == 2).Select(b => b.ZipCode).Last();


            Assert.AreEqual("7777", ZipCode);
        }

        [Test]
        public void CustomerWithId2_CigWithCountryTestCustomerLand()
        {

            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var Country = t.Where(c => c.CustomerID == 2).Select(b => b.Country).Last();


            Assert.AreEqual("TestCustomerLand", Country);
        }

        [Test]
        public void CustomerWithId2_CigWithEmail_TestCustomerdk()
        {
            var t = _cigr.GetCustomerInvoiceGroupIDByCustomerID(_customer);
            var Email = t.Where(c => c.CustomerID == 2).Select(b => b.Email).Last();


            Assert.AreEqual("Test@Customer.dk", Email);
        }
    }

    [TestFixture]
    class GetCustomerInvoiceGroupByCustomerInvoiceGroupID
    {
        #region Setup/Teardown

        private Customer _customer;
        private CustomerInvoiceGroupRepository _cigr;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _cigr = new CustomerInvoiceGroupRepository(_databaseSetup.GetTrexConnection);
            _customer = new Customer
            {
                City = "TestCity",
                StreetAddress = "TestVej",
                Address2 = "TestVej2",
                ContactName = "Lls",
                ZipCode = "8210",
                Country = "TestLand",
                SendFormat = 2,
                Email = "test@d60.dk",
                EmailCC = "Cctest@d60.dk",
                CustomerID = 1
            };
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _customer = null;
            _cigr = null;
        }
        #endregion

        [Test]
        public void CigId1_CigWithId1()
        {

            var t = _cigr.GetCustomerInvoiceGroupByCustomerInvoiceGroupID(1);

            Assert.AreEqual(1, t.CustomerInvoiceGroupID);

        }

        [Test]
        [ExpectedException]
        public void NonExsitingCigId_Exception()
        {

            _cigr.GetCustomerInvoiceGroupByCustomerInvoiceGroupID(1337);
        }
    }

    [TestFixture]
    public class SaveCig
    {
        #region Setup/Teardown

        private CustomerInvoiceGroupRepository _cigr;
        private DatabaseSetup _databaseSetup;

        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _cigr = new CustomerInvoiceGroupRepository(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _cigr = null;
        }
        #endregion

        [Test]
        public void SaveNewCig()
        {
            var cig = new CustomerInvoiceGroup { Label = "New Cig", CustomerID = 1, City = "New cig city" };

            _cigr.InsertInDatabase(cig);

            var cigs =
                (from customerInvoiceGroup in _databaseSetup.GetTrexConnection.TrexEntityContext.CustomerInvoiceGroups
                 where customerInvoiceGroup.Label == "New Cig"
                 select customerInvoiceGroup).First();

            Assert.AreEqual("New cig city", cigs.City);
        }

        [Test]
        public void SaveChangesToExsitingCIG()
        {
            using (var db = _databaseSetup.GetTrexConnection.TrexEntityContext)
            {
                var cigs =
                    (from customerInvoiceGroup in db.CustomerInvoiceGroups
                     where customerInvoiceGroup.CustomerInvoiceGroupID == 1
                     select customerInvoiceGroup).First();

                cigs.Label = "This is a change";

                _cigr.InsertInDatabase(cigs);

                var cigs2 =
                    (from customerInvoiceGroup in db.CustomerInvoiceGroups
                     where customerInvoiceGroup.CustomerInvoiceGroupID == 1
                     select customerInvoiceGroup).First();

                Assert.AreEqual("This is a change", cigs2.Label);
                
            }
        }
    }
}
