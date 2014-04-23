namespace Trex.Server.Reports
{
    partial class CustomerStatus
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter5 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter6 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.CustomerDB = new Telerik.Reporting.SqlDataSource();
            this.ProjectDataSource = new Telerik.Reporting.SqlDataSource();
            this.UserDataSource = new Telerik.Reporting.SqlDataSource();
            this.CustomerStatusDB = new Telerik.Reporting.SqlDataSource();
            this.labelsGroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.taskCaptionTextBox = new Telerik.Reporting.TextBox();
            this.timeSpentCaptionTextBox = new Telerik.Reporting.TextBox();
            this.userNameCaptionTextBox = new Telerik.Reporting.TextBox();
            this.startDateCaptionTextBox = new Telerik.Reporting.TextBox();
            this.labelsGroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroup = new Telerik.Reporting.Group();
            this.projectGroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.projectDataTextBox = new Telerik.Reporting.TextBox();
            this.projectGroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.timeSpentSumFunctionTextBox = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.projectGroup = new Telerik.Reporting.Group();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.reportNameTextBox = new Telerik.Reporting.TextBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.customerDataTextBox = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.taskDataTextBox = new Telerik.Reporting.TextBox();
            this.timeSpentDataTextBox = new Telerik.Reporting.TextBox();
            this.userNameDataTextBox = new Telerik.Reporting.TextBox();
            this.startDateDataTextBox = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // CustomerDB
            // 
            this.CustomerDB.ConnectionString = "Data Source=WIN7DEV2010;Initial Catalog=trex_drift;Integrated Security=True";
            this.CustomerDB.Name = "CustomerDB";
            this.CustomerDB.ProviderName = "System.Data.SqlClient";
            this.CustomerDB.SelectCommand = "SELECT        Customers.*\r\nFROM            Customers";
            // 
            // ProjectDataSource
            // 
            this.ProjectDataSource.ConnectionString = "Data Source=WIN7DEV2010;Initial Catalog=trex_drift;Integrated Security=True";
            this.ProjectDataSource.Name = "ProjectDataSource";
            this.ProjectDataSource.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@CustomerId", System.Data.DbType.Int32, "=Parameters.Customer.Value")});
            this.ProjectDataSource.ProviderName = "System.Data.SqlClient";
            this.ProjectDataSource.SelectCommand = "SELECT        ProjectID, Guid, CustomerID, ProjectName, CreatedBy, CreateDate, In" +
                "active, IsEstimatesEnabled, ChangeDate, ChangedBy\r\nFROM            Projects\r\nWHE" +
                "RE        (CustomerId = @CustomerId)";
            // 
            // UserDataSource
            // 
            this.UserDataSource.ConnectionString = "Data Source=WIN7DEV2010;Initial Catalog=trex_drift;Integrated Security=True";
            this.UserDataSource.Name = "UserDataSource";
            this.UserDataSource.ProviderName = "System.Data.SqlClient";
            this.UserDataSource.SelectCommand = "SELECT        UserID, UserName, Name\r\nFROM            Users";
            // 
            // CustomerStatusDB
            // 
            this.CustomerStatusDB.ConnectionString = "Data Source=WIN7DEV2010;Initial Catalog=trex_drift;Integrated Security=True";
            this.CustomerStatusDB.Name = "CustomerStatusDB";
            this.CustomerStatusDB.ProviderName = "System.Data.SqlClient";
            this.CustomerStatusDB.SelectCommand = "SELECT        RoundedTimeSpentPrDayPrUser.*\r\nFROM            RoundedTimeSpentPrDa" +
                "yPrUser";
            // 
            // labelsGroupHeader
            // 
            this.labelsGroupHeader.Height = new Telerik.Reporting.Drawing.Unit(0.71437495946884155D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.labelsGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.taskCaptionTextBox,
            this.timeSpentCaptionTextBox,
            this.userNameCaptionTextBox,
            this.startDateCaptionTextBox});
            this.labelsGroupHeader.Name = "labelsGroupHeader";
            this.labelsGroupHeader.PrintOnEveryPage = true;
            // 
            // taskCaptionTextBox
            // 
            this.taskCaptionTextBox.CanGrow = true;
            this.taskCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(2.2518069744110107D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.00010012308484874666D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.taskCaptionTextBox.Name = "taskCaptionTextBox";
            this.taskCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(7.3481931686401367D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.taskCaptionTextBox.StyleName = "Caption";
            this.taskCaptionTextBox.Value = "Task";
            // 
            // timeSpentCaptionTextBox
            // 
            this.timeSpentCaptionTextBox.CanGrow = true;
            this.timeSpentCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.562560081481934D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.00010012308484874666D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.timeSpentCaptionTextBox.Name = "timeSpentCaptionTextBox";
            this.timeSpentCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.1986904144287109D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.timeSpentCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.timeSpentCaptionTextBox.StyleName = "Caption";
            this.timeSpentCaptionTextBox.TextWrap = false;
            this.timeSpentCaptionTextBox.Value = "Time Spent";
            // 
            // userNameCaptionTextBox
            // 
            this.userNameCaptionTextBox.CanGrow = true;
            this.userNameCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(9.6002006530761719D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.00010012308484874666D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.userNameCaptionTextBox.Name = "userNameCaptionTextBox";
            this.userNameCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.962158203125D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.userNameCaptionTextBox.StyleName = "Caption";
            this.userNameCaptionTextBox.Value = "Consultant";
            // 
            // startDateCaptionTextBox
            // 
            this.startDateCaptionTextBox.CanGrow = true;
            this.startDateCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.startDateCaptionTextBox.Name = "startDateCaptionTextBox";
            this.startDateCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.1986904144287109D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.startDateCaptionTextBox.StyleName = "Caption";
            this.startDateCaptionTextBox.Value = "Date";
            // 
            // labelsGroupFooter
            // 
            this.labelsGroupFooter.Height = new Telerik.Reporting.Drawing.Unit(0.71437495946884155D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.labelsGroupFooter.Name = "labelsGroupFooter";
            this.labelsGroupFooter.Style.Visible = false;
            // 
            // labelsGroup
            // 
            this.labelsGroup.GroupFooter = this.labelsGroupFooter;
            this.labelsGroup.GroupHeader = this.labelsGroupHeader;
            this.labelsGroup.Name = "labelsGroup";
            // 
            // projectGroupHeader
            // 
            this.projectGroupHeader.Height = new Telerik.Reporting.Drawing.Unit(0.71437495946884155D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.projectGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.projectDataTextBox});
            this.projectGroupHeader.Name = "projectGroupHeader";
            // 
            // projectDataTextBox
            // 
            this.projectDataTextBox.CanGrow = true;
            this.projectDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.projectDataTextBox.Name = "projectDataTextBox";
            this.projectDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(15.708332061767578D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.projectDataTextBox.Style.Font.Bold = true;
            this.projectDataTextBox.StyleName = "Data";
            this.projectDataTextBox.Value = "=Fields.Project";
            // 
            // projectGroupFooter
            // 
            this.projectGroupFooter.Height = new Telerik.Reporting.Drawing.Unit(0.71437495946884155D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.projectGroupFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.timeSpentSumFunctionTextBox,
            this.textBox1});
            this.projectGroupFooter.Name = "projectGroupFooter";
            // 
            // timeSpentSumFunctionTextBox
            // 
            this.timeSpentSumFunctionTextBox.CanGrow = true;
            this.timeSpentSumFunctionTextBox.Format = "{0:N2}";
            this.timeSpentSumFunctionTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.562560081481934D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.timeSpentSumFunctionTextBox.Name = "timeSpentSumFunctionTextBox";
            this.timeSpentSumFunctionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.19869065284729D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.timeSpentSumFunctionTextBox.Style.Font.Bold = true;
            this.timeSpentSumFunctionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.timeSpentSumFunctionTextBox.StyleName = "Data";
            this.timeSpentSumFunctionTextBox.Value = "=Sum(Fields.TimeSpent)";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Format = "{0:N2}";
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(4.4502973556518555D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.StyleName = "Data";
            this.textBox1.Value = "Total";
            // 
            // projectGroup
            // 
            this.projectGroup.GroupFooter = this.projectGroupFooter;
            this.projectGroup.GroupHeader = this.projectGroupHeader;
            this.projectGroup.Grouping.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.Project")});
            this.projectGroup.Name = "projectGroup";
            // 
            // pageHeader
            // 
            this.pageHeader.Height = new Telerik.Reporting.Drawing.Unit(0.71437495946884155D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.reportNameTextBox});
            this.pageHeader.Name = "pageHeader";
            // 
            // reportNameTextBox
            // 
            this.reportNameTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.reportNameTextBox.Name = "reportNameTextBox";
            this.reportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(15.708333015441895D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.reportNameTextBox.StyleName = "PageInfo";
            this.reportNameTextBox.Value = "Status Report";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = new Telerik.Reporting.Drawing.Unit(0.71437495946884155D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.currentTimeTextBox,
            this.pageInfoTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(7.8277082443237305D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(7.9335417747497559D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(7.8277082443237305D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = new Telerik.Reporting.Drawing.Unit(2.7058331966400146D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox,
            this.customerDataTextBox});
            this.reportHeader.Name = "reportHeader";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(15.814167022705078D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "Billable time ";
            // 
            // customerDataTextBox
            // 
            this.customerDataTextBox.CanGrow = true;
            this.customerDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.00010012308484874666D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.0002000331878662D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.customerDataTextBox.Name = "customerDataTextBox";
            this.customerDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(7.8277082443237305D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.customerDataTextBox.Style.Font.Bold = true;
            this.customerDataTextBox.StyleName = "Data";
            this.customerDataTextBox.Value = "=Fields.Customer";
            // 
            // detail
            // 
            this.detail.Height = new Telerik.Reporting.Drawing.Unit(0.71437495946884155D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.taskDataTextBox,
            this.timeSpentDataTextBox,
            this.userNameDataTextBox,
            this.startDateDataTextBox});
            this.detail.Name = "detail";
            // 
            // taskDataTextBox
            // 
            this.taskDataTextBox.CanGrow = true;
            this.taskDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(2.2518069744110107D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.taskDataTextBox.Name = "taskDataTextBox";
            this.taskDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(7.3481931686401367D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.taskDataTextBox.StyleName = "Data";
            this.taskDataTextBox.Value = "=Fields.Task";
            // 
            // timeSpentDataTextBox
            // 
            this.timeSpentDataTextBox.CanGrow = true;
            this.timeSpentDataTextBox.Format = "{0:N2}";
            this.timeSpentDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.562560081481934D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.timeSpentDataTextBox.Name = "timeSpentDataTextBox";
            this.timeSpentDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.1986904144287109D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.timeSpentDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.timeSpentDataTextBox.StyleName = "Data";
            this.timeSpentDataTextBox.Value = "=Fields.TimeSpent";
            // 
            // userNameDataTextBox
            // 
            this.userNameDataTextBox.CanGrow = true;
            this.userNameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(9.6002006530761719D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.userNameDataTextBox.Name = "userNameDataTextBox";
            this.userNameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.9094436168670654D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.userNameDataTextBox.StyleName = "Data";
            this.userNameDataTextBox.Value = "= Fields.Name";
            // 
            // startDateDataTextBox
            // 
            this.startDateDataTextBox.CanGrow = true;
            this.startDateDataTextBox.Format = "{0:d}";
            this.startDateDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052917070686817169D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.startDateDataTextBox.Name = "startDateDataTextBox";
            this.startDateDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.1986904144287109D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.startDateDataTextBox.StyleName = "Data";
            this.startDateDataTextBox.Value = "=Fields.StartDate";
            // 
            // CustomerStatus
            // 
            this.DataSource = this.CustomerStatusDB;
            this.Filters.AddRange(new Telerik.Reporting.Data.Filter[] {
            new Telerik.Reporting.Data.Filter("=Fields.CustomerID", Telerik.Reporting.Data.FilterOperator.Equal, "=Parameters.Customer.Value"),
            new Telerik.Reporting.Data.Filter("=Fields.StartDate", Telerik.Reporting.Data.FilterOperator.GreaterOrEqual, "=Parameters.Startdate.Value"),
            new Telerik.Reporting.Data.Filter("=Fields.StartDate", Telerik.Reporting.Data.FilterOperator.LessOrEqual, "=Parameters.EndDate.Value"),
            new Telerik.Reporting.Data.Filter("=Fields.ProjectID", Telerik.Reporting.Data.FilterOperator.In, "=Parameters.Project.Value"),
            new Telerik.Reporting.Data.Filter("=Fields.UserID", Telerik.Reporting.Data.FilterOperator.In, "=Parameters.Consultant.Value"),
            new Telerik.Reporting.Data.Filter("=Fields.Billable", Telerik.Reporting.Data.FilterOperator.Equal, "= Parameters(\"IsBillable\")")});
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            this.labelsGroup,
            this.projectGroup});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.labelsGroupHeader,
            this.labelsGroupFooter,
            this.projectGroupHeader,
            this.projectGroupFooter,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.detail});
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins.Bottom = new Telerik.Reporting.Drawing.Unit(2.5399999618530273D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Left = new Telerik.Reporting.Drawing.Unit(2.5399999618530273D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Right = new Telerik.Reporting.Drawing.Unit(2.5399999618530273D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Top = new Telerik.Reporting.Drawing.Unit(2.5399999618530273D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AvailableValues.DataSource = this.CustomerDB;
            reportParameter1.AvailableValues.DisplayMember = "CustomerName";
            reportParameter1.AvailableValues.ValueMember = "CustomerID";
            reportParameter1.Name = "Customer";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Visible = true;
            reportParameter2.AvailableValues.DataSource = this.ProjectDataSource;
            reportParameter2.AvailableValues.DisplayMember = "ProjectName";
            reportParameter2.AvailableValues.ValueMember = "= ProjectID";
            reportParameter2.MultiValue = true;
            reportParameter2.Name = "Project";
            reportParameter2.Text = "Project";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter2.Visible = true;
            reportParameter3.AllowBlank = false;
            reportParameter3.Name = "Startdate";
            reportParameter3.Text = "Start date";
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter3.Visible = true;
            reportParameter4.AllowBlank = false;
            reportParameter4.Name = "EndDate";
            reportParameter4.Text = "End date";
            reportParameter4.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter4.Visible = true;
            reportParameter5.AvailableValues.DataSource = this.UserDataSource;
            reportParameter5.AvailableValues.DisplayMember = "= Name";
            reportParameter5.AvailableValues.ValueMember = "UserID";
            reportParameter5.MultiValue = true;
            reportParameter5.Name = "Consultant";
            reportParameter5.Text = "Consultant";
            reportParameter5.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter5.Visible = true;
            reportParameter6.AllowBlank = false;
            reportParameter6.Name = "IsBillable";
            reportParameter6.Text = "Billable";
            reportParameter6.Type = Telerik.Reporting.ReportParameterType.Boolean;
            reportParameter6.Value = "= True";
            reportParameter6.Visible = true;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(reportParameter4);
            this.ReportParameters.Add(reportParameter5);
            this.ReportParameters.Add(reportParameter6);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule1.Style.BackgroundColor = System.Drawing.Color.Empty;
            styleRule1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(189)))), ((int)(((byte)(64)))));
            styleRule1.Style.Font.Name = "Tahoma";
            styleRule1.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(18D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule2.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(189)))), ((int)(((byte)(64)))));
            styleRule2.Style.Color = System.Drawing.Color.White;
            styleRule2.Style.Font.Bold = true;
            styleRule2.Style.Font.Italic = false;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule2.Style.Font.Strikeout = false;
            styleRule2.Style.Font.Underline = false;
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule3.Style.Color = System.Drawing.Color.Black;
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule4.Style.Color = System.Drawing.Color.Black;
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(8D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = new Telerik.Reporting.Drawing.Unit(15.814167022705078D, Telerik.Reporting.Drawing.UnitType.Cm);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource CustomerStatusDB;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeader;
        private Telerik.Reporting.TextBox taskCaptionTextBox;
        private Telerik.Reporting.TextBox timeSpentCaptionTextBox;
        private Telerik.Reporting.TextBox userNameCaptionTextBox;
        private Telerik.Reporting.TextBox startDateCaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooter;
        private Telerik.Reporting.Group labelsGroup;
        private Telerik.Reporting.GroupHeaderSection projectGroupHeader;
        private Telerik.Reporting.TextBox projectDataTextBox;
        private Telerik.Reporting.GroupFooterSection projectGroupFooter;
        private Telerik.Reporting.TextBox timeSpentSumFunctionTextBox;
        private Telerik.Reporting.Group projectGroup;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.TextBox reportNameTextBox;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.TextBox customerDataTextBox;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox taskDataTextBox;
        private Telerik.Reporting.TextBox timeSpentDataTextBox;
        private Telerik.Reporting.TextBox startDateDataTextBox;
        private Telerik.Reporting.SqlDataSource CustomerDB;
        private Telerik.Reporting.SqlDataSource ProjectDataSource;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.SqlDataSource UserDataSource;
        private Telerik.Reporting.TextBox userNameDataTextBox;

    }
}