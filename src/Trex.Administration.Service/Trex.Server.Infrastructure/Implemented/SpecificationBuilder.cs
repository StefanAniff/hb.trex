using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aspose.Words;
using Aspose.Words.Tables;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Exceptions;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class SpecificationBuilder : BuilderBase, ISpecificationBuilder
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ITrexContextProvider _contextProvider;

        public SpecificationBuilder(IInvoiceService invoiceService, ITrexContextProvider contextProvider)
        {
            _invoiceService = invoiceService;
            MapDataInput = new Dictionary<string, string>();
            _contextProvider = contextProvider;
        }

        #region String names

        private const string Attention = "Attention";
        private const string City = "City_Zip";
        private const string Country = "Country";
        private const string CustomerID = "CustomerId";
        private const string CustomerName = "CustomerName";
        private const string DataList = "SpecData";
        private const string DueDate = "DueDate";
        private const string InvoiceDate = "InvoiceDate";
        private const string InvoiceId = "InvoiceId";
        private const string StreetName = "StreetName";

        #endregion

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            var hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            var f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            var v = Convert.ToInt32(value);
            var p = Convert.ToInt32(value * (1 - saturation));
            var q = Convert.ToInt32(value * (1 - f * saturation));
            var t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            switch (hi)
            {
                case 0:
                    return Color.FromArgb(255, v, t, p);
                case 1:
                    return Color.FromArgb(255, q, v, p);
                case 2:
                    return Color.FromArgb(255, p, v, t);
                case 3:
                    return Color.FromArgb(255, p, q, v);
                case 4:
                    return Color.FromArgb(255, t, p, v);
                default:
                    return Color.FromArgb(255, v, p, q);
            }
        }

        #region ISpecificationBuilder Members

        public override Dictionary<string, string> MapData(Invoice invoice)
        {
            const string format = "dd/MM/yyyy";
            var metaData = _invoiceService.GetInvoiceMetaData(invoice.CustomerInvoiceGroupId);

            ValidateMapInput(Attention, metaData.Attention);
            ValidateMapInput(City, metaData.ZipCode + " " + metaData.City);
            ValidateMapInput(Country, metaData.Country);
            ValidateMapInput(CustomerID, metaData.CustomerID.ToString());
            ValidateMapInput(CustomerName, invoice.CustomerInvoiceGroup.Customer.CustomerName);

            if (invoice.DueDate != null)
                ValidateMapInput(DueDate, invoice.DueDate.Value.ToString(format));
            ValidateMapInput(InvoiceDate, invoice.InvoiceDate.ToString(format));
            ValidateMapInput(InvoiceId, invoice.InvoiceID.ToString());
            ValidateMapInput(StreetName, metaData.Address1);

            return MapDataInput;
        }

        public override void InsertInvoiceLines(List<InvoiceLine> invoiceLineData, int invoiceId, DocumentBuilder builder)
        {
            try
            {
                const string timeSpecifier = "0.00";

                if (invoiceLineData.Count == 0)
                    throw new NoInvoiceLines();

                var projects = _invoiceService.GetSpecificationDataProject(invoiceLineData.Select(i => i.InvoiceID).First(), false);

                var invoiceData = _invoiceService.GetInvoiceById(invoiceLineData.First().InvoiceID);
                var startDate = invoiceData.StartDate;
                var endDate = invoiceData.InvoiceDate;


                SetBookMark(DataList, builder);
                double padding = 2;

                Aspose.Words.Font font1 = builder.Font;
                font1.Size = 11;
                font1.Bold = true;
                font1.Color = Color.White;

                
                builder.InsertCell();
                Color original = Color.FromArgb(145, 200, 65);

                double hue;
                double saturation;
                double value;
                ColorToHSV(original, out hue, out saturation, out value);

                Color mainBar = ColorFromHSV(hue, saturation, value);

                // Compare that to the HSL values that the .NET framework provides: 

                original.GetHue();
                original.GetSaturation();
                original.GetBrightness();

                builder.PageSetup.RightMargin = 50;
                BuilderSettings(builder, 80, LineStyle.None, ParagraphAlignment.Left);
                builder.CellFormat.Shading.BackgroundPatternColor = mainBar;
                builder.CellFormat.BottomPadding = padding;
                builder.CellFormat.TopPadding = padding;

                builder.RowFormat.Height = 18;
                builder.RowFormat.HeightRule = HeightRule.Exactly;

                builder.Write("Time statement for period ");
                builder.Write(startDate.ToShortDateString() + " - " + endDate.ToShortDateString());


                builder.InsertCell();
                BuilderSettings(builder, 20, LineStyle.None, ParagraphAlignment.Right);

                builder.Write("Hours");

                builder.EndRow();

                InsertProjects(builder, projects, invoiceLineData, timeSpecifier);

                bool anyFound = false;
                foreach (var b in invoiceLineData)
                {
                    if (b.UnitType == 2)
                        anyFound = true;
                }

                if (anyFound)
                {
                    projects = _invoiceService.GetSpecificationDataProject(invoiceLineData.Select(i => i.InvoiceID).First(), true);
                    InsertProjects(builder, projects, invoiceLineData, timeSpecifier);
                }
                Aspose.Words.Font font4 = builder.Font;
                font4.Size = 11;
                font4.Bold = true;
                font4.Color = Color.Black;

                builder.InsertCell();
                BuilderSettings(builder, 80, LineStyle.None, ParagraphAlignment.Left);
                builder.CellFormat.Shading.BackgroundPatternColor = mainBar;
                builder.CellFormat.WrapText = true;
                builder.CellFormat.BottomPadding = padding;
                builder.CellFormat.TopPadding = padding;

                builder.Write("Total");

                builder.InsertCell();
                BuilderSettings(builder, 20, LineStyle.None, ParagraphAlignment.Right);

                var projectData = _invoiceService.GetSpecificationDataProject(invoiceLineData.Select(i => i.InvoiceID).First(), false);
                double val = projectData.Sum(p => p.TimeUsed).Value;

                builder.RowFormat.Height = 18;
                builder.RowFormat.HeightRule = HeightRule.Exactly;

                builder.Write(val.ToString("F"));

                builder.EndRow();

                builder.EndTable();
            }
            catch (NoInvoiceLines)
            {
                var line = GenerateEmptyInvoiceLine(invoiceId);
                InsertInvoiceLines(line, invoiceId, builder);
            }
        }

        private List<InvoiceLine> GenerateEmptyInvoiceLine(int invoiceId)
        {
            using (var entity = _contextProvider.TrexEntityContext)
            {
                int ID = (from i in entity.Invoices
                          where i.ID == invoiceId
                          select i).SingleOrDefault().ID;

                var line = new InvoiceLine
                           {
                               InvoiceID = ID,
                               IsExpense = false,
                               PricePrUnit = 0,
                               SortIndex = 0,
                               Unit = "decimal hours",
                               Text = "",
                               UnitType = 0,
                               Units = 0,
                               VatPercentage = 0
                           };
                var list = new List<InvoiceLine>();
                list.Add(line);
                return list;
            }
        }

        private void InsertProjects(DocumentBuilder builder, IEnumerable<GetSpecificationData_Project_Result> projectResults, List<InvoiceLine> invoiceLineData, string timeSpecifier)
        {
            try
            {
                Color original = Color.FromArgb(228, 244, 221);

                double hue;
                double saturation;
                double value;
                ColorToHSV(original, out hue, out saturation, out value);

                Color taskBar = ColorFromHSV(hue, saturation, value);

                // Compare that to the HSL values that the .NET framework provides: 
                original.GetHue();        // 212.0
                original.GetSaturation(); // 0.6
                original.GetBrightness(); // 0.490196079

                foreach (var p in projectResults)
                {
                    builder.InsertCell();
                    builder.PageSetup.RightMargin = 50;
                    builder.RowFormat.Height = 18;
                    builder.RowFormat.HeightRule = HeightRule.Exactly;
                    BuilderSettings(builder, 80, LineStyle.None, ParagraphAlignment.Left);
                    builder.CellFormat.HorizontalMerge = CellMerge.First;
                    builder.CellFormat.Shading.BackgroundPatternColor = taskBar;
                    builder.CellFormat.WrapText = true;

                    Aspose.Words.Font font2 = builder.Font;
                    font2.Size = 11;
                    font2.Bold = true;
                    font2.Color = Color.Black;
                    builder.Write(p.ProjectName);

                    builder.InsertCell();
                    BuilderSettings(builder, 20, LineStyle.None, ParagraphAlignment.Right);
                    builder.CellFormat.HorizontalMerge = CellMerge.None;
                    builder.CellFormat.Shading.BackgroundPatternColor = taskBar;

                    if (p.FixedPriceProject == false)
                        builder.Write(p.TimeUsed.Value.ToString(timeSpecifier));

                    builder.EndRow();
                    builder.EndTable();

                    InsertTasks(builder, invoiceLineData, p.ProjectID.Value, timeSpecifier, p.FixedPriceProject == true);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        private void InsertTasks(DocumentBuilder builder, List<InvoiceLine> invoiceLineData, int projectId, string timeSpecifier, bool fixedProject)
        {
            var tasks = _invoiceService.GetSpecificationDataTasks(invoiceLineData.Select(i => i.InvoiceID).First(), fixedProject);
            
            foreach (var t in tasks.Where(x => projectId == x.ProjectID))
            {
                builder.InsertCell();
                BuilderSettings(builder, 2, LineStyle.None, ParagraphAlignment.Left);
                builder.CellFormat.Shading.BackgroundPatternColor = Color.White;

                Aspose.Words.Font font3 = builder.Font;
                font3.Size = 11;
                font3.Bold = false;
                font3.Color = Color.Black;
                builder.PageSetup.RightMargin = 50;

                builder.InsertCell();
                builder.CellFormat.WrapText = true;
                builder.CellFormat.Shading.BackgroundPatternColor = Color.White;
                BuilderSettings(builder, 88, LineStyle.None, ParagraphAlignment.Left);
                builder.RowFormat.Height = CalculateCellHeigth(t.TaskName, 88, builder) * 15;

                builder.Write(t.TaskName);

                builder.InsertCell();
                BuilderSettings(builder, 10, LineStyle.None, ParagraphAlignment.Right);
                builder.Write(t.TimeUsed.Value.ToString(timeSpecifier));

                builder.EndRow();
            }
        }
        #endregion
    }
}