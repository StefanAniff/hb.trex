namespace Trex.Server.Reports
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    partial class InvoiceSpecification
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.projectGroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.projectDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.projectGroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.projectGroup = new Telerik.Reporting.Group();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.customerNameDataTextBox = new Telerik.Reporting.TextBox();
            this.contactNameDataTextBox = new Telerik.Reporting.TextBox();
            this.streetAddressDataTextBox = new Telerik.Reporting.TextBox();
            this.zipCodeDataTextBox = new Telerik.Reporting.TextBox();
            this.cityDataTextBox = new Telerik.Reporting.TextBox();
            this.countryDataTextBox = new Telerik.Reporting.TextBox();
            this.invoiceNumberDataTextBox = new Telerik.Reporting.TextBox();
            this.invoiceDateCaptionTextBox = new Telerik.Reporting.TextBox();
            this.invoiceDateDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.panel1 = new Telerik.Reporting.Panel();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.detail = new Telerik.Reporting.DetailSection();
            this.taskDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.invoiceSpecDataSet = new Trex.Server.Reports.InvoiceSpecDataSet();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceSpecDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // projectGroupHeader
            // 
            this.projectGroupHeader.Height = new Telerik.Reporting.Drawing.Unit(0.53877496719360352D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.projectGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.projectDataTextBox,
            this.textBox6});
            this.projectGroupHeader.Name = "projectGroupHeader";
            this.projectGroupHeader.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(241)))), ((int)(((byte)(221)))));
            // 
            // projectDataTextBox
            // 
            this.projectDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.05290985107421875D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052910052239894867D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.projectDataTextBox.Name = "projectDataTextBox";
            this.projectDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(11.947089195251465D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.38986873626708984D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.projectDataTextBox.Style.Font.Bold = true;
            this.projectDataTextBox.Style.Font.Name = "Calibri";
            this.projectDataTextBox.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.projectDataTextBox.StyleName = "Data";
            this.projectDataTextBox.Value = "=Fields.Project";
            // 
            // textBox6
            // 
            this.textBox6.Format = "{0:0.00}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(12.053110122680664D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052910052239894867D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.6558833122253418D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.48586508631706238D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.Font.Name = "Calibri";
            this.textBox6.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.StyleName = "Data";
            this.textBox6.Value = "= Sum(Fields.TimeSpent)";
            // 
            // projectGroupFooter
            // 
            this.projectGroupFooter.Height = new Telerik.Reporting.Drawing.Unit(0.13227513432502747D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.projectGroupFooter.Name = "projectGroupFooter";
            // 
            // projectGroup
            // 
            this.projectGroup.Bookmark = null;
            this.projectGroup.GroupFooter = this.projectGroupFooter;
            this.projectGroup.GroupHeader = this.projectGroupHeader;
            this.projectGroup.Grouping.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.Project")});
            // 
            // reportHeader
            // 
            this.reportHeader.Height = new Telerik.Reporting.Drawing.Unit(8.6735448837280273D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox,
            this.customerNameDataTextBox,
            this.contactNameDataTextBox,
            this.streetAddressDataTextBox,
            this.zipCodeDataTextBox,
            this.cityDataTextBox,
            this.countryDataTextBox,
            this.invoiceNumberDataTextBox,
            this.invoiceDateCaptionTextBox,
            this.invoiceDateDataTextBox,
            this.textBox1,
            this.textBox2,
            this.textBox3,
            this.textBox4,
            this.textBox15,
            this.panel1,
            this.shape1});
            this.reportHeader.Name = "reportHeader";
            this.reportHeader.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.reportHeader.Style.BorderWidth.Bottom = new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Point);
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.053111888468265533D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(6.6857142448425293D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(6.12063455581665D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.49508535861968994D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.Style.Font.Name = "Calibri";
            this.titleTextBox.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.titleTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "=\'Bilag faktura \' + Fields.InvoiceNumber";
            // 
            // customerNameDataTextBox
            // 
            this.customerNameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052910052239894867D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.026554936543107033D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.customerNameDataTextBox.Name = "customerNameDataTextBox";
            this.customerNameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(9.8530902862548828D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.33270436525344849D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.customerNameDataTextBox.Style.Font.Bold = true;
            this.customerNameDataTextBox.Style.Font.Name = "Calibri";
            this.customerNameDataTextBox.StyleName = "Data";
            this.customerNameDataTextBox.Value = "=Fields.CustomerName";
            // 
            // contactNameDataTextBox
            // 
            this.contactNameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052910052239894867D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.41236919164657593D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.contactNameDataTextBox.Name = "contactNameDataTextBox";
            this.contactNameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(9.8530902862548828D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.37959426641464233D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.contactNameDataTextBox.Style.Font.Bold = false;
            this.contactNameDataTextBox.Style.Font.Name = "Calibri";
            this.contactNameDataTextBox.StyleName = "Data";
            this.contactNameDataTextBox.Value = "=Fields.ContactName";
            // 
            // streetAddressDataTextBox
            // 
            this.streetAddressDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052910052239894867D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.84507334232330322D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.streetAddressDataTextBox.Name = "streetAddressDataTextBox";
            this.streetAddressDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(9.8530902862548828D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.41418591141700745D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.streetAddressDataTextBox.Style.Font.Bold = false;
            this.streetAddressDataTextBox.Style.Font.Name = "Calibri";
            this.streetAddressDataTextBox.StyleName = "Data";
            this.streetAddressDataTextBox.Value = "=Fields.StreetAddress";
            // 
            // zipCodeDataTextBox
            // 
            this.zipCodeDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052910052239894867D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.3123693466186523D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.zipCodeDataTextBox.Name = "zipCodeDataTextBox";
            this.zipCodeDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(0.92063504457473755D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.37337389588356018D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.zipCodeDataTextBox.Style.Font.Bold = false;
            this.zipCodeDataTextBox.Style.Font.Name = "Calibri";
            this.zipCodeDataTextBox.StyleName = "Data";
            this.zipCodeDataTextBox.Value = "=Fields.ZipCode";
            // 
            // cityDataTextBox
            // 
            this.cityDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(1.0266549587249756D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.312369704246521D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.cityDataTextBox.Name = "cityDataTextBox";
            this.cityDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(8.8793458938598633D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.37337389588356018D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.cityDataTextBox.Style.Font.Bold = false;
            this.cityDataTextBox.Style.Font.Name = "Calibri";
            this.cityDataTextBox.StyleName = "Data";
            this.cityDataTextBox.Value = "=Fields.City";
            // 
            // countryDataTextBox
            // 
            this.countryDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052910052239894867D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.7388536930084229D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.countryDataTextBox.Name = "countryDataTextBox";
            this.countryDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(9.8530902862548828D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.42026400566101074D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.countryDataTextBox.Style.Font.Bold = false;
            this.countryDataTextBox.Style.Font.Name = "Calibri";
            this.countryDataTextBox.StyleName = "Data";
            this.countryDataTextBox.Value = "=Fields.Country";
            // 
            // invoiceNumberDataTextBox
            // 
            this.invoiceNumberDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.326457023620606D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(6.2335124015808105D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.invoiceNumberDataTextBox.Name = "invoiceNumberDataTextBox";
            this.invoiceNumberDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.3825380802154541D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.39423403143882751D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.invoiceNumberDataTextBox.Style.Font.Name = "Calibri";
            this.invoiceNumberDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.invoiceNumberDataTextBox.StyleName = "Data";
            this.invoiceNumberDataTextBox.Value = "=Fields.InvoiceNumber";
            // 
            // invoiceDateCaptionTextBox
            // 
            this.invoiceDateCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(10.746889114379883D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(5.385714054107666D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.invoiceDateCaptionTextBox.Name = "invoiceDateCaptionTextBox";
            this.invoiceDateCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.4735450744628906D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.34734433889389038D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.invoiceDateCaptionTextBox.Style.Font.Name = "Calibri";
            this.invoiceDateCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.invoiceDateCaptionTextBox.StyleName = "Caption";
            this.invoiceDateCaptionTextBox.Value = "Fakturadato";
            // 
            // invoiceDateDataTextBox
            // 
            this.invoiceDateDataTextBox.Format = "{0:d}";
            this.invoiceDateDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.273543357849121D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(5.385714054107666D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.invoiceDateDataTextBox.Name = "invoiceDateDataTextBox";
            this.invoiceDateDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.4354503154754639D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.34734433889389038D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.invoiceDateDataTextBox.Style.Font.Name = "Calibri";
            this.invoiceDateDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.invoiceDateDataTextBox.StyleName = "Data";
            this.invoiceDateDataTextBox.Value = "=Fields.InvoiceDate";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(10.746889114379883D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(6.2335124015808105D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.4735450744628906D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.40596634149551392D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox1.Style.Font.Name = "Calibri";
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "Fakturanr.";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(10.7468900680542D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(5.7861685752868652D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.4735450744628906D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.39423403143882751D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Style.Font.Name = "Calibri";
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox2.StyleName = "Caption";
            this.textBox2.Value = "Forfaldsdato";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(10.746889114379883D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(6.6925888061523438D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.4735450744628906D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.40596634149551392D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox3.Style.Font.Name = "Calibri";
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox3.StyleName = "Caption";
            this.textBox3.Value = "Kundenr.";
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.27354621887207D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(6.7679939270019531D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.3825380802154541D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.33056128025054932D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox4.Style.Font.Name = "Calibri";
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox4.StyleName = "Data";
            this.textBox4.Value = "=Fields.CustomerNumber";
            // 
            // textBox15
            // 
            this.textBox15.Format = "{0:d}";
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.326457023620606D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(5.7861685752868652D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.3825380802154541D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.39423403143882751D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox15.Style.Font.Name = "Calibri";
            this.textBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox15.StyleName = "Data";
            this.textBox15.Value = "=Fields.DueDate";
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox8,
            this.textBox12,
            this.textBox13,
            this.textBox14,
            this.textBox9});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(7.9471664428710938D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(15.841068267822266D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.699999988079071D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(187)))), ((int)(((byte)(89)))));
            // 
            // textBox8
            // 
            this.textBox8.CanGrow = false;
            this.textBox8.CanShrink = true;
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052910052239894867D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.02655513770878315D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox8.Multiline = false;
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(6.6206355094909668D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60195982456207275D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox8.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(187)))), ((int)(((byte)(89)))));
            this.textBox8.Style.Color = System.Drawing.Color.White;
            this.textBox8.Style.Font.Name = "Calibri";
            this.textBox8.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox8.StyleName = "Title";
            this.textBox8.TextWrap = false;
            this.textBox8.Value = "Timeopgørelse i projekter for periode";
            // 
            // textBox12
            // 
            this.textBox12.Format = "{0:d}";
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(6.72665548324585D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.029999999329447746D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox12.Multiline = false;
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.1800003051757812D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox12.Style.Color = System.Drawing.Color.White;
            this.textBox12.Style.Font.Name = "Calibri";
            this.textBox12.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox12.StyleName = "Title";
            this.textBox12.TextWrap = false;
            this.textBox12.Value = "=Fields.StartDate";
            // 
            // textBox13
            // 
            this.textBox13.Format = "{0:d}";
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(9.3384990692138672D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.029999999329447746D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox13.Multiline = false;
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.1400001049041748D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox13.Style.Color = System.Drawing.Color.White;
            this.textBox13.Style.Font.Name = "Calibri";
            this.textBox13.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox13.StyleName = "Title";
            this.textBox13.TextWrap = false;
            this.textBox13.Value = "= Fields.EndDate";
            // 
            // textBox14
            // 
            this.textBox14.Format = "{0:d}";
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(8.95913314819336D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.029999999329447746D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(0.32625633478164673D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox14.Style.Color = System.Drawing.Color.White;
            this.textBox14.Style.Font.Name = "Calibri";
            this.textBox14.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox14.StyleName = "Title";
            this.textBox14.Value = "-";
            // 
            // textBox9
            // 
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(14.12645435333252D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.029999999329447746D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(1.6900007724761963D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox9.Style.Color = System.Drawing.Color.White;
            this.textBox9.Style.Font.Name = "Calibri";
            this.textBox9.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox9.StyleName = "Title";
            this.textBox9.Value = "Timer";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.02655513770878315D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(7.2339096069335938D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(15.841068267822266D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.37354525923728943D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.shape1.Style.LineWidth = new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Point);
            // 
            // detail
            // 
            this.detail.Height = new Telerik.Reporting.Drawing.Unit(0.4489571750164032D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.taskDataTextBox,
            this.textBox5});
            this.detail.Name = "detail";
            // 
            // taskDataTextBox
            // 
            this.taskDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.5264551043510437D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.059088651090860367D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.taskDataTextBox.Name = "taskDataTextBox";
            this.taskDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(11.473545074462891D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.38986873626708984D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.taskDataTextBox.Style.Font.Name = "Calibri";
            this.taskDataTextBox.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.taskDataTextBox.StyleName = "Data";
            this.taskDataTextBox.Value = "=Fields.Task";
            // 
            // textBox5
            // 
            this.textBox5.Format = "{0:0.00}";
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(12.053110122680664D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.02655513770878315D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.6558833122253418D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.38986873626708984D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox5.Style.Font.Name = "Calibri";
            this.textBox5.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox5.StyleName = "Data";
            this.textBox5.Value = "=Fields.TimeSpent";
            // 
            // invoiceSpecDataSet
            // 
            this.invoiceSpecDataSet.DataSetName = "InvoiceSpecDataSet";
            this.invoiceSpecDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = new Telerik.Reporting.Drawing.Unit(0.63474220037460327D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox11,
            this.textBox7});
            this.reportFooterSection1.Name = "reportFooterSection1";
            this.reportFooterSection1.Style.BorderColor.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(187)))), ((int)(((byte)(89)))));
            this.reportFooterSection1.Style.BorderColor.Top = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(187)))), ((int)(((byte)(89)))));
            this.reportFooterSection1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.reportFooterSection1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.026555944234132767D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.4204328060150146D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox11.Style.Font.Bold = true;
            this.textBox11.Style.Font.Name = "Calibri";
            this.textBox11.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox11.StyleName = "Data";
            this.textBox11.Value = "Hovedtotal";
            // 
            // textBox7
            // 
            this.textBox7.Format = "{0:0.00}";
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(12.053111076354981D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.02655513770878315D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.6558833122253418D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox7.Style.BorderColor.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(187)))), ((int)(((byte)(89)))));
            this.textBox7.Style.BorderColor.Top = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(187)))), ((int)(((byte)(89)))));
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.Font.Name = "Calibri";
            this.textBox7.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.StyleName = "Data";
            this.textBox7.Value = "= Sum(Fields.TimeSpent)";
            // 
            // InvoiceSpecification
            // 
            this.DataSource = this.invoiceSpecDataSet;
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            this.projectGroup});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.projectGroupHeader,
            this.projectGroupFooter,
            this.reportHeader,
            this.detail,
            this.reportFooterSection1});
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins.Bottom = new Telerik.Reporting.Drawing.Unit(2.5396826267242432D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Left = new Telerik.Reporting.Drawing.Unit(2.5396826267242432D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Right = new Telerik.Reporting.Drawing.Unit(2.5396826267242432D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Top = new Telerik.Reporting.Drawing.Unit(2.5396826267242432D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Style.BackgroundColor = System.Drawing.Color.White;
            this.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule1.Style.Color = System.Drawing.Color.Black;
            styleRule1.Style.Font.Bold = true;
            styleRule1.Style.Font.Italic = false;
            styleRule1.Style.Font.Name = "Tahoma";
            styleRule1.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(20D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule1.Style.Font.Strikeout = false;
            styleRule1.Style.Font.Underline = false;
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule2.Style.Color = System.Drawing.Color.Black;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = new Telerik.Reporting.Drawing.Unit(15.867623329162598D, Telerik.Reporting.Drawing.UnitType.Cm);
            ((System.ComponentModel.ISupportInitialize)(this.invoiceSpecDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GroupHeaderSection projectGroupHeader;
        private Telerik.Reporting.TextBox projectDataTextBox;
        private GroupFooterSection projectGroupFooter;
        private Group projectGroup;
        private ReportHeaderSection reportHeader;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.TextBox customerNameDataTextBox;
        private Telerik.Reporting.TextBox contactNameDataTextBox;
        private Telerik.Reporting.TextBox streetAddressDataTextBox;
        private Telerik.Reporting.TextBox zipCodeDataTextBox;
        private Telerik.Reporting.TextBox cityDataTextBox;
        private Telerik.Reporting.TextBox countryDataTextBox;
        private Telerik.Reporting.TextBox invoiceNumberDataTextBox;
        private Telerik.Reporting.TextBox invoiceDateDataTextBox;
        private DetailSection detail;
        private Telerik.Reporting.TextBox taskDataTextBox;
        private InvoiceSpecDataSet invoiceSpecDataSet;
        private Telerik.Reporting.TextBox invoiceDateCaptionTextBox;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox textBox13;
        private Telerik.Reporting.TextBox textBox14;
        private ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox15;
        private Telerik.Reporting.Panel panel1;
        private Shape shape1;

    }
}