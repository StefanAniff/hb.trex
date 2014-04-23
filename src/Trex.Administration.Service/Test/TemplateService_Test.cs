using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Test;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;

namespace TemplateService_Test
{
    [TestFixture]
    class SaveTemplate
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceTemplate = new InvoiceTemplate();
            _templateService = new TemplateService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _templateService = null;
            _invoiceTemplate = null;
        }
        private DatabaseSetup _databaseSetup;
        private TemplateService _templateService;
        private InvoiceTemplate _invoiceTemplate;
        #endregion

        [Test]
        public void SaveTemplate_databaseContainsTheTemplate()
        {
            _invoiceTemplate.TemplateId = 1;
            _invoiceTemplate.StandardCreditNoteMail = false;
            _invoiceTemplate.StandardCreditNotePrint = false;
            _invoiceTemplate.StandardSpecification = false;
            _invoiceTemplate.StandardInvoiceMail = false;
            _invoiceTemplate.StandardInvoicePrint = true;
            _invoiceTemplate.CreateDate = DateTime.Now;
            _invoiceTemplate.CreatedBy = "me";
            _invoiceTemplate.Guid = new Guid(1, 23, 14, new byte[]{1, 2, 3, 4, 5, 6, 7, 8});
            _invoiceTemplate.TemplateName = "Test Template";

            _templateService.SaveTemplate(_invoiceTemplate);


            var check = (from i in _databaseSetup.EntityContext.InvoiceTemplates
                         where i.TemplateId == 1 && i.TemplateName == "Test Template"
                         select i).First();

            Assert.AreEqual("me", check.CreatedBy);
            Assert.AreEqual(new Guid(1, 23, 14, new byte[]{1, 2, 3, 4, 5, 6, 7, 8}), check.Guid);

        }
    }

    [TestFixture]
    public class SaveTemplateFile
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _invoiceTemplate = new InvoiceTemplate();
            _templateService = new TemplateService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _templateService = null;
            _invoiceTemplate = null;
        }
        private DatabaseSetup _databaseSetup;
        private TemplateService _templateService;
        private InvoiceTemplate _invoiceTemplate;
        #endregion

        [Test]
        public void SaveTemplateAndtemplateFile()
        {
            _invoiceTemplate.TemplateId = 1;
            _invoiceTemplate.StandardCreditNoteMail = false;
            _invoiceTemplate.StandardCreditNotePrint = false;
            _invoiceTemplate.StandardSpecification = false;
            _invoiceTemplate.StandardInvoiceMail = false;
            _invoiceTemplate.StandardInvoicePrint = true;
            _invoiceTemplate.CreateDate = DateTime.Now;
            _invoiceTemplate.CreatedBy = "me";
            _invoiceTemplate.Guid = new Guid(1, 23, 14, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            _invoiceTemplate.TemplateName = "Test Template";

            _templateService.SaveTemplate(_invoiceTemplate);


            _templateService.SaveTemplateFile(1, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            var file = (from invoiceTemplateFilese in _databaseSetup.EntityContext.InvoiceTemplateFiles
                        where invoiceTemplateFilese.File == new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}
                        select invoiceTemplateFilese).First();

            Assert.IsInstanceOf<InvoiceTemplateFiles>(file);
            Assert.AreEqual(1, file.ID);
            Assert.AreEqual(1, file.InvoiceTemplateId);
            Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, file.File);

        }
    }

}
