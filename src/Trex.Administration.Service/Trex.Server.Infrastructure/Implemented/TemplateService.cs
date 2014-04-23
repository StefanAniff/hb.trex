using System;
using System.Collections.Generic;
using StructureMap;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using System.Linq;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TemplateService : LogableBase, ITemplateService
    {
        private readonly TrexEntities _entityContext;
        public TemplateService(ITrexContextProvider ContextProvider)
        {
            _entityContext = ContextProvider.TrexEntityContext;
        }

        public List<InvoiceTemplate> GetInvoiceTemplates()
        {
            try
            {
                return (from template in _entityContext.InvoiceTemplates
                        select template).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void SetStandardInvoiceMailTemplate(int templateId)
        {
            try
            {
                var invoiceTemplate = (from t in _entityContext.InvoiceTemplates
                                       select t).ToList();

                invoiceTemplate.First(it => it.StandardInvoiceMail).StandardInvoiceMail = false;
                _entityContext.SaveChanges();

                invoiceTemplate.First(it => it.TemplateId == templateId).StandardInvoiceMail = true;
                _entityContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void SetStandardInvoicePrintTemplate(int templateId)
        {
            try
            {
                var invoiceTemplate = (from t in _entityContext.InvoiceTemplates
                                       select t).ToList();

                invoiceTemplate.First(it => it.StandardInvoicePrint).StandardInvoicePrint = false;
                _entityContext.SaveChanges();

                invoiceTemplate.First(it => it.TemplateId == templateId).StandardInvoicePrint = true;
                _entityContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void SetStandardSpecificationTemplate(int templateId)
        {
            try
            {
                var invoiceTemplate = (from t in _entityContext.InvoiceTemplates
                                       select t).ToList();

                invoiceTemplate.First(it => it.StandardSpecification).StandardSpecification = false;
                _entityContext.SaveChanges();

                invoiceTemplate.First(it => it.TemplateId == templateId).StandardSpecification = true;
                _entityContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void SetStandardCreditNotePrintTemplate(int templateId)
        {
            try
            {
                var invoiceTemplate = (from t in _entityContext.InvoiceTemplates
                                       select t).ToList();

                invoiceTemplate.First(it => it.StandardCreditNotePrint).StandardCreditNotePrint = false; //Set old to null
                _entityContext.SaveChanges();

                invoiceTemplate.First(it => it.TemplateId == templateId).StandardCreditNotePrint = true;
                _entityContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void SetStandardCreditNoteMailTemplate(int templateId)
        {
            try
            {
                var invoiceTemplate = (from t in _entityContext.InvoiceTemplates
                                       select t).ToList();

                invoiceTemplate.First(it => it.StandardCreditNoteMail).StandardCreditNoteMail = false; //Set old to null
                _entityContext.SaveChanges();

                invoiceTemplate.First(it => it.TemplateId == templateId).StandardCreditNoteMail = true;
                _entityContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void SaveTemplate(InvoiceTemplate template)
        {
            try
            {
                var found = (from it in _entityContext.InvoiceTemplates
                             where it.TemplateId == template.TemplateId
                             select it);

                if (!found.Any())
                    _entityContext.InvoiceTemplates.AddObject(template);

                else
                {
                    var it = found.First();
                    it.TemplateName = template.TemplateName;

                    if (template.StandardInvoiceMail)
                    {
                        foreach (var x in _entityContext.InvoiceTemplates)
                        {
                            x.StandardInvoiceMail = false;
                        }
                        it.StandardInvoiceMail = true;
                    }

                    if (template.StandardInvoicePrint)
                    {
                        foreach (var x in _entityContext.InvoiceTemplates)
                        {
                            x.StandardInvoicePrint = false;
                        }
                        it.StandardInvoicePrint = true;
                    }

                    if (template.StandardSpecification)
                    {
                        foreach (var x in _entityContext.InvoiceTemplates)
                        {
                            x.StandardSpecification = false;
                        }
                        it.StandardSpecification = true;
                    }

                    if (template.StandardCreditNoteMail)
                    {
                        foreach (var x in _entityContext.InvoiceTemplates)
                        {
                            x.StandardCreditNoteMail = false;
                        }
                        it.StandardCreditNoteMail = true;
                    }

                    if (template.StandardCreditNotePrint)
                    {
                        foreach (var x in _entityContext.InvoiceTemplates)
                        {
                            x.StandardCreditNotePrint = false;
                        }
                        it.StandardCreditNotePrint = true;
                    }
                }

                _entityContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void SaveTemplateFile(int templateId, byte[] fileStream)
        {
            try
            {
                var templateFile = _entityContext.InvoiceTemplateFiles.Any(x => x.InvoiceTemplateId == templateId);

                if (templateFile == false)
                    _entityContext.InvoiceTemplateFiles.AddObject(
                        new InvoiceTemplateFiles
                        {
                            File = fileStream,
                            InvoiceTemplateId = templateId
                        });

                else
                {
                    var template = _entityContext.InvoiceTemplateFiles.First(x => x.InvoiceTemplateId == templateId);
                    _entityContext.InvoiceTemplateFiles.Attach(template);
                    template.File = fileStream;
                }

                _entityContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void DeleteTemplate(InvoiceTemplate template)
        {
            try
            {
                var templateFiles = _entityContext.InvoiceTemplateFiles.Where(x => x.InvoiceTemplateId == template.TemplateId);

                foreach (var itf in templateFiles)
                {
                    _entityContext.InvoiceTemplateFiles.Attach(itf);
                    _entityContext.InvoiceTemplateFiles.DeleteObject(itf);
                }
                _entityContext.SaveChanges();

                _entityContext.InvoiceTemplates.Attach(template);
                _entityContext.InvoiceTemplates.DeleteObject(template);
                _entityContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public byte[] DownloadTemplateFile(int templateId)
        {
            var file = (from inte in _entityContext.InvoiceTemplateFiles
                        where inte.InvoiceTemplateId == templateId
                        select inte).First();

            return file.File;
        }

        public byte[] DownloadPdfFile(Guid invoiceGuid, int format)
        {
            int alternative = 0;
            if (format == 1)
                alternative = 4;
            if (format == 2)
                alternative = 5;

            var invoice = (from infi in _entityContext.InvoiceFiles
                           join i in _entityContext.Invoices on infi.InvoiceID equals i.ID
                           where i.Guid == invoiceGuid
                           select i).First();

            var file = (from inte in _entityContext.InvoiceFiles
                        where inte.InvoiceID == invoice.ID && (inte.FileType == format || inte.FileType == alternative)
                        select inte).First();

            return file.File;
        }

        public List<InvoiceTemplate> GetAllInvoiceTemplates()
        {
            return _entityContext.InvoiceTemplates.ToList();
        }

        public ServerResponse DeleteInvoiceFiles(int invoiceId)
        {
            try
            {
                var files = _entityContext.InvoiceFiles.Where(i => i.InvoiceID == invoiceId);
                foreach (var invoiceFile in files)
                {
                    _entityContext.InvoiceFiles.Attach(invoiceFile);
                    _entityContext.InvoiceFiles.DeleteObject(invoiceFile);
                }
                _entityContext.SaveChanges();

                return new ServerResponse("Files regenerated", true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
