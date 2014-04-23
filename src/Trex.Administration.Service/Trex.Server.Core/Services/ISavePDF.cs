using Aspose.Words;

namespace Trex.Server.Core.Services
{
    public interface ISavePDF
    {
        void SavePDF(Document document, string name, string location);
        void SavePDF(Document document, int invoiceId, int type);
    }
}
