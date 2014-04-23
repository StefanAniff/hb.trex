using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aspose.Words;
using Aspose.Words.Tables;
using Test_InvoiceBuilder;
using Trex.Server.Core.Interfaces;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public abstract class BuilderBase : LogableBase, IBuilderBase
    {
        public void InsertInvoiceData(Invoice invoice, DocumentBuilder builder)
        {
            MapDataInput = new Dictionary<string, string>();
            var data = MapData(invoice);

            foreach (var d in data)
            {
                bool succes = WriteAtBookMark(d.Key, d.Value, builder);
                if (!succes)
                {
                    throw new ArgumentException("Setting the value of " + d.Key + " failed. The bookmark properly doesn't exist");
                }
            }
        }

        /// <summary>
        /// Validate if a book mark has data to insert
        /// </summary>
        /// <param name="a">Name of book mark</param>
        /// <param name="b">Data to insert on bookmark</param>
        public void ValidateMapInput(string a, string b)
        {
            //TODO: Hvis dit firma, dit navn, adresse eller lignende indeholder 'null' brokker dette sig...
            if (b == null || b.ToUpper().Contains("NULL"))
            {
                throw new MissingCustomerAttributeException(a + " was not set for the current custommer");
            }
            MapDataInput.Add(a, b);
        }

        public Dictionary<string, string> MapDataInput;

        public abstract Dictionary<string, string> MapData(Invoice invoice);
        public abstract void InsertInvoiceLines(List<InvoiceLine> invoiceLineData, int invoiceId, DocumentBuilder builder);

        public bool WriteAtBookMark(string bookMarkName, string value, DocumentBuilder builder)
        {
            try
            {
                bool found = builder.MoveToBookmark(bookMarkName);

                if (found)
                {
                    builder.Write(value);
                    return true;
                }
            }
            catch (ArgumentNullException ex)
            {
                LogError(ex);
                throw;
            }
            return false;
        }

        public bool SetBookMark(string bookMarkName, DocumentBuilder builder)
        {
            bool found = builder.MoveToBookmark(bookMarkName);

            if (found)
                return true;

            return false;
        }

        /// <summary>
        /// Set the builders settings
        /// </summary>
        /// <param name="builder">the builder you use</param>
        /// <param name="prefferedWidth">Width in percent</param>
        /// <param name="lineStyle">Linestyle used</param>
        /// <param name="alignment">text alignment</param>
        /// <returns>The DocumentBuilder</returns>
        public DocumentBuilder BuilderSettings(DocumentBuilder builder, double prefferedWidth, LineStyle lineStyle, ParagraphAlignment alignment)
        {
            var point = (builder.PageSetup.PageWidth - builder.PageSetup.LeftMargin - builder.PageSetup.RightMargin) * (prefferedWidth / 100);
            builder.CellFormat.Width = point;
            builder.CellFormat.Borders.LineStyle = lineStyle;
            builder.CurrentParagraph.ParagraphFormat.Alignment = alignment;
            return builder;
        }

        public int CalculateCellHeigth(string text, double cellSizeInPercent, DocumentBuilder builder)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 1;

            var fontSize = (float)builder.Font.Size;
            var fontName = builder.Font.Name;

            System.Drawing.Font newFont = new System.Drawing.Font(fontName, fontSize);
            Bitmap bmp = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(bmp);

            SizeF stringSize = graphics.MeasureString(text, newFont);
            bmp = new Bitmap(bmp, (int)stringSize.Width, (int)stringSize.Height);
            graphics = Graphics.FromImage(bmp);

            var data = graphics.MeasureString(text, newFont);
            var point = 6.8 * cellSizeInPercent;
            var rows = (int)Math.Ceiling(data.Width / point);

            return rows;
        }
    }
}