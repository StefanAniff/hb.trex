using System;
using System.IO;
using Aspose.Words;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.BaseClasses;

namespace Trex.Server.Infrastructure.Implemented
{
    public class SaveToPc : LogableBase, ISavePDF
    {
        public void SavePDF(Document document, string name, string location)
        {
            try
            {
                string fileLocation = location + name + ".pdf";
                document.Save(fileLocation);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Not to be called
        /// </summary>
        public void SavePDF(Document document, int invoiceId, int type)
        {
            throw new NotImplementedException();
        }
    }
}