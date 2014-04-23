using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aspose.Words;
using Aspose.Words.Tables;
using StructureMap;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class InvoiceBuilder : BuilderBase, IInvoiceBuilder
    {
        private readonly IInvoiceService _invoiceService;
        private CustomerInvoiceGroup _metaData;
        private readonly ITrexContextProvider _contextProvider;

        public InvoiceBuilder(IInvoiceService invoiceService, ITrexContextProvider contextProvider)
        {
            _invoiceService = invoiceService;
            MapDataInput = new Dictionary<string, string>();
            _contextProvider = contextProvider;

        }

        #region IInvoiceBuilder Members

        public override void InsertInvoiceLines(List<InvoiceLine> invoiceLineData, int invoiceId, DocumentBuilder builder)
        {
            try
            {
                const string moneySpecifier = "N";
                const string timeSpecifier = "0.00";

                List<double> units = invoiceLineData.Select(i => i.Units).ToList();
                List<double> pricePrUnit = invoiceLineData.Select(i => i.PricePrUnit).ToList();
                List<double> vatPercentage = invoiceLineData.Select(i => i.VatPercentage).ToList();
                List<string> description = invoiceLineData.Select(i => i.Text).ToList();
                List<int> unitType = invoiceLineData.Select(i => i.UnitType).ToList();

                IEnumerable<string> invoiceLinesUnit = invoiceLineData.Select(i => i.Unit).ToList();
                var unit = new List<string>();
                foreach (var u in invoiceLinesUnit)
                {
                    unit.Add(string.IsNullOrEmpty(u) ? "decimal hours" : u.ToLower());
                }

                SetBookMark(DataList, builder);
                builder.PageSetup.RightMargin = 48;
                builder.Font.Name = "Calibri";

                double widestsText = 0;
                for (int i = 0; i < units.Count; i++)
                {
                    var width = CalculateCellWidthPercent(
                        units[i].ToString(timeSpecifier) + " " +
                        unit[i] + " of DKK. " +
                        pricePrUnit[i].ToString(moneySpecifier) + " ", builder);

                    if (width > widestsText)
                        widestsText = width;
                }

                for (int i = 0; i < units.Count; i++)
                {
                    if (unitType[i] == 2)
                    {
                        //Without VAT
                        builder.InsertCell();
                        BuilderSettings(builder, 100 - 18 - 17.5, LineStyle.None, ParagraphAlignment.Left);
                        builder.CellFormat.LeftPadding = 0;
                        builder.CellFormat.RightPadding = 0;

                        builder.Write(description[i]); //Description for each line

                        //builder.InsertCell(); //Empty cell needed
                    }
                    else
                    {
                        //Units, of unit in DKK and price/unit
                        builder.InsertCell();
                        builder.CellFormat.WrapText = false;
                        builder.RowFormat.HeightRule = HeightRule.Exactly;

                        BuilderSettings(builder, widestsText, LineStyle.None, ParagraphAlignment.Left);
                        builder.CellFormat.LeftPadding = 0;
                        builder.CellFormat.RightPadding = 0;

                        builder.Write(units[i].ToString(timeSpecifier)); //Unit count
                        builder.Write(" ");
                        builder.Write(unit[i]); //Unit
                        builder.Write(" of DKK. "); //middle text
                        builder.Write(pricePrUnit[i].ToString(moneySpecifier)); //Unit price

                        //Description
                        builder.InsertCell();
                        builder.CellFormat.LeftPadding = 5;
                        var missingWidth = 100 - 18 - 17.5 - widestsText;
                        builder.CellFormat.WrapText = true;
                        builder.RowFormat.Height = CalculateCellHeigth(description[i], missingWidth, builder) * 15;
                        BuilderSettings(builder, missingWidth, LineStyle.None, ParagraphAlignment.Left);

                        builder.Write(description[i]); //Description for each line
                    }

                    //Cell for VAT only
                    builder.InsertCell();
                    BuilderSettings(builder, 18, LineStyle.None, ParagraphAlignment.Right);
                    builder.CellFormat.RightPadding = 0;

                    double vat;
                    if (unitType[i] != 2)
                        vat = units[i]*pricePrUnit[i]*vatPercentage[i];
                    else
                        vat = pricePrUnit[i]*vatPercentage[i];

                    builder.Write(vat.ToString(moneySpecifier));

                    builder.InsertCell();
                    BuilderSettings(builder, 17.5, LineStyle.None, ParagraphAlignment.Right);

                    double noVat;
                    if (unitType[i] != 2)
                        noVat = invoiceLineData[i].PricePrUnit*invoiceLineData[i].Units;
                    else
                        noVat = pricePrUnit[i];

                    builder.Write(noVat.ToString(moneySpecifier));

                    builder.EndRow();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw ex;
            }
        }

        private double CalculateCellWidthPercent(string text, DocumentBuilder builder)
        {
            var fontSize = (float)builder.Font.Size;
            var fontName = builder.Font.Name;

            System.Drawing.Font newFont = new System.Drawing.Font(fontName, fontSize);
            Bitmap bmp = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(bmp);

            SizeF stringSize = graphics.MeasureString(text, newFont);
            bmp = new Bitmap(bmp, (int)stringSize.Width, (int)stringSize.Height);
            graphics = Graphics.FromImage(bmp);

            var data = graphics.MeasureString(text, newFont);
            var percent = (data.Width / (6.8 * 100)) * 100;

            return percent;
        }

        public override Dictionary<string, string> MapData(Invoice invoice)
        {
            const string moneySpecifier = "N";
            const string format = "dd/MM/yyyy";

            _metaData = _invoiceService.GetInvoiceMetaData(invoice.CustomerInvoiceGroupId);

            ValidateMapInput(StartDate, invoice.StartDate.ToString(format));
            ValidateMapInput(EndDate, invoice.EndDate.ToString(format));

            if(invoice.Attention == null)
                ValidateMapInput(Attention, _metaData.Attention); //Contactperson
            else
                ValidateMapInput(Attention, invoice.Attention);

            ValidateMapInput(CustomerAddress, _metaData.Address1);
            ValidateMapInput(City, _metaData.City);
            ValidateMapInput(ZIP, _metaData.ZipCode);
            ValidateMapInput(Country, _metaData.Country);
            ValidateMapInput(CustomerName, invoice.CustomerInvoiceGroup.Customer.CustomerName);

            double ExclVat = CalculateExclVat(invoice);
            double vat = CalculateVat(invoice);

            ValidateMapInput(VATBase, ExclVat.ToString(moneySpecifier));
            ValidateMapInput(VATOfTotal, vat.ToString(moneySpecifier));
            ValidateMapInput(TotalDkk, (ExclVat + vat).ToString(moneySpecifier));

            ValidateMapInput(InvoiceDate, invoice.InvoiceDate.ToShortDateString());
            if (invoice.DueDate != null)
                ValidateMapInput(DueDate, invoice.DueDate.Value.ToShortDateString());
            ValidateMapInput(InvoiceNumber, invoice.InvoiceID.ToString());
            ValidateMapInput(CustomerID, _metaData.CustomerID.ToString());

            if (!invoice.IsCreditNote)
                ValidateMapInput(InvoiceNumberBold, invoice.InvoiceID.ToString());

            ValidateMapInput(Regarding, (string.IsNullOrEmpty(invoice.Regarding) ? "" : invoice.Regarding));

            if (invoice.IsCreditNote)
            {
                using (var entity = _contextProvider.TrexEntityContext)
                {
                    int? id = (from i in entity.Invoices
                               where i.ID == invoice.InvoiceLinkId
                               select i.InvoiceID).First();

                    ValidateMapInput(InvoiceParent, id.ToString());
                }
            }

            return MapDataInput;
        }

        private double CalculateExclVat(Invoice invoice)
        {
            double value = 0;
            foreach (var invoiceLine in invoice.InvoiceLines)
            {
                if (invoiceLine.UnitType != 2)
                {
                    value += invoiceLine.Units * invoiceLine.PricePrUnit;
                }
                else
                {
                    value += invoiceLine.PricePrUnit;
                }
            }
            return value;
        }

        private double CalculateVat(Invoice invoice)
        {
            double value = 0;
            foreach (var invoiceLine in invoice.InvoiceLines)
            {
                if (invoiceLine.UnitType != 2)
                {
                    value += invoiceLine.Units * invoiceLine.PricePrUnit * invoiceLine.VatPercentage;
                }
                else
                {
                    value += invoiceLine.PricePrUnit * invoiceLine.VatPercentage;
                }
            }
            return value;
        }

        #endregion

        #region String names

        private const string Attention = "Attention";
        private const string CustomerName = "CustomerName";
        private const string CustomerAddress = "CustomerAddress";
        private const string ZIP = "ZIP";
        private const string City = "City";
        private const string Country = "Country";

        private const string InvoiceDate = "InvoiceDate";
        private const string DueDate = "DueDate";
        private const string CustomerID = "CustomerId";
        private const string InvoiceNumber = "InvoiceNumber";

        private const string InvoiceNumberBold = "InvoiceNumberBold";
        private const string StartDate = "StartDate";
        private const string EndDate = "EndDate";

        private const string TotalDkk = "Total_DKK";
        private const string VATOfTotal = "VAT";
        private const string VATBase = "VAT_base";

        private const string DataList = "ItemsData";
        private const string Regarding = "Regarding";

        private const string InvoiceParent = "InvoiceParent";

        #endregion
    }
}