#region

using System;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.BaseClasses;

#endregion

namespace Trex.Server.Infrastructure.Implemented
{
    public abstract class ComposerBase : LogableBase, IComposerBase
    {
        private readonly IBuilderBase _builder;
        private readonly IGatherData _gatherData;
        private readonly ISavePDF _savePDF;

        protected ComposerBase(IBuilderBase builder, IGatherData gatherData, ISavePDF savePDF)
        {
            Aspose.Words.License license = new Aspose.Words.License();
            license.SetLicense("Aspose.Words.lic");
            _builder = builder;
            _gatherData = gatherData;
            _savePDF = savePDF;
        }

        #region IComposerBase Members

        public void UseTemplate(int invoiceId, int format)
        {
            CreatePdf(invoiceId, format);
        }

        public abstract void TemplateValidation(int invoiceId, IGatherData data, int format);

        #endregion

        private void CreatePdf(int invoiceID, int format)
        {
            try
            {
                TemplateValidation(invoiceID, _gatherData, format);

                var builder = _gatherData.DocumentBuilder;

                var invoice = _gatherData.GetInvoiceData(invoiceID);

                var invoiceLines = _gatherData.GetInvoiceLines(invoiceID);

                _builder.InsertInvoiceData(invoice, builder);
                builder.MoveToDocumentEnd();
                _builder.InsertInvoiceLines(invoiceLines, invoiceID, builder);
                    _savePDF.SavePDF(_gatherData.DocumentBuilder.Document, invoiceID, format);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw ex;
            }
        }
    }
}