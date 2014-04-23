using System;
using System.IO;
using System.Linq;
using Aspose.Words;
using StructureMap;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class SaveToDB : LogableBase, ISavePDF
    {
        public void SavePDF(Document document, string name, string location)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the invoice or specification on the server
        /// </summary>
        /// <param name="document">Aspose Document to save</param>
        /// <param name="name">Name of pdf</param>
        /// <param name="location">Not used</param>
        public void SavePDF(Document document, int invoiceId, int type)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                document.Save(stream, SaveFormat.Pdf);
                var byteArray2 = stream.ToArray();
                //var byteArray = SaveFile(stream);
                SaveInvoiceOrSpecification(invoiceId, byteArray2, type);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        //public byte[] SaveFile(Stream stream)
        //{
        //    byte[] buffer;

        //    var binaryReader = new BinaryReader(stream);
        //    var totalBytes = stream.Length;

        //    // read entire file into buffer
        //    buffer = binaryReader.ReadBytes((Int32)totalBytes);

        //    binaryReader.Close();

        //    return buffer;
        //}

        /// <summary>
        /// Saves an invoice or a specification
        /// </summary>
        /// <param name="invoiceId">ID of the invoice</param>
        /// <param name="fileStream">Stream of data in byte[]</param>
        /// <param name="type">1 = invoiceMAIL, 2 = invoicePRINT, 3 = specificationMAIL</param>
        public void SaveInvoiceOrSpecification(int invoiceId, byte[] fileStream, int type)
        {
            try
            {
                using (var entity = ObjectFactory.GetInstance<ITrexContextProvider>().TrexEntityContext)
                {
                    bool anyLeft = (from infi in entity.InvoiceFiles
                                    where infi.InvoiceID == invoiceId && infi.FileType == type
                                    select infi).Any();

                    if (anyLeft)
                    {
                        var line = entity.InvoiceFiles.First(x => x.FileType == type && x.InvoiceID == invoiceId);
                        entity.InvoiceFiles.Attach(line);
                        line.File = fileStream;
                        entity.InvoiceFiles.ApplyCurrentValues(line);
                    }
                    else
                        entity.InvoiceFiles.AddObject(
                                new InvoiceFile
                                {
                                    File = fileStream,
                                    InvoiceID = invoiceId,
                                    FileType = type
                                });

                    entity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }
    }
}