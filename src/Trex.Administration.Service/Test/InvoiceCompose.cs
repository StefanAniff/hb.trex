using Moq;
using NUnit.Framework;
using Test;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Exceptions;
using Trex.Server.Infrastructure.Implemented;

namespace Test_GatherData
{
    [TestFixture]
    public class ConvertBinaryToDocument
    {
        #region SetUp / TearDown

        private DatabaseSetup _databaseSetup;
        private IGatherData _gather;
        private IInvoiceService _invoiceServiceMock;
        [SetUp]
        public void Initialize()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceServiceMock = new InvoiceService(_databaseSetup.GetTrexConnection);
            _gather = new GatherData(_invoiceServiceMock, _databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void Dispose()
        {
            _databaseSetup = null;
            _gather = null;
            _invoiceServiceMock = null;
        }
        #endregion

        [Test]
        public void NonExcistingTemplateIdZero_InvoiceFileNoteFoundException()
        {
            try
            {
                _gather.ConvertBinaryToDocument(0);
            }
            catch (InvoiceFileNotFound efnf)
            {
                Assert.AreEqual("Template with ID " + 0 + " not found", efnf.Message);
            }
        }

        [Test]
        public void GetMetaData()
        {
            _databaseSetup.CreateCustomerInvoiceGroup(15, 1, "Laban", true, 1, "mail", "ccmail", "attention", "address1",
                                                      "adress2", "City", "Country", "ZIP");

            var cig = _gather.GetMetaData(15);

            Assert.AreEqual("Country", cig.Country);
            Assert.AreEqual("attention", cig.Attention);

        }
    }
    
}
