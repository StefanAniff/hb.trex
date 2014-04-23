using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class ExcelExportService : IExcelExportService
    {
        private readonly IAppSettings _appSettings;

        public ExcelExportService(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        #region IExcelExportService Members

        public Guid CreateWorkSheet(ISelectionFilter selectionFilter)
        {
            try
            {
                var dataSet = CreateXmlData(selectionFilter);

                var excelStream = CreateExcelStream(dataSet);
                var exportId = Guid.NewGuid();
                var serverMapPath = HttpContext.Current.Server.MapPath(_appSettings.ExcelExportPath);
                var excelFilePath = string.Concat(serverMapPath, "\\", exportId, ".xls");

                using (var fileWriter = new StreamWriter(excelFilePath, false, Encoding.Unicode))
                {
                    fileWriter.Write(excelStream);
                }
                return exportId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        private string CreateExcelStream(DataSet ds)
        {
            var xmlDataDoc = new XmlDataDocument(ds);

            var xslTransform = new XslCompiledTransform();
            //var names = typeof (ExcelExportService).Assembly.GetManifestResourceNames();
            var stream = typeof (ExcelExportService).Assembly.GetManifestResourceStream("Trex.Server.Infrastructure.Excel.Excel.xsl");
            if (stream == null)
            {
                return null;
            }
            var xslStreamReader = new StreamReader(stream);
            var xslReader = new XmlTextReader(xslStreamReader);
            xslTransform.Load(xslReader);

            var returnStringWriter = new StringWriter();
            var xmlTextWriter = new XmlTextWriter(returnStringWriter);
            xslTransform.Transform(xmlDataDoc, xmlTextWriter);

            var returnString = returnStringWriter.ToString();
            //Hack: Could´nt get this fucker in the xml in any other way
            return string.Concat("<?xml version=\"1.0\" encoding=\"utf-16\" standalone=\"yes\"?>", returnString);
        }

        private void GetSubTasks(Task parentTask, List<TimeEntry> timeEntries, ISelectionFilter filter)
        {
            foreach (var subTask in parentTask.SubTasks)
            {
                if (filter.HasTask(subTask))
                {
                    foreach (var timeEntry in subTask.TimeEntries)
                    {
                        if (filter.HasTimeEntry(timeEntry))
                        {
                            timeEntries.Add(timeEntry);
                        }
                    }
                }
                GetSubTasks(subTask, timeEntries, filter);
            }
        }

        private DataSet CreateXmlData(ISelectionFilter filter)
        {
            var dataSet = new DataSet();
            var timeEntries = new List<TimeEntry>();

            var customers = filter.Customers;

            foreach (var customer in customers)
            {
                foreach (var project in customer.Projects)
                {
                    if (filter.HasProject(project))
                    {
                        foreach (var task in project.Tasks)
                        {
                            if (filter.HasTask(task))
                            {
                                foreach (var timeEntry in task.TimeEntries)
                                {
                                    if (filter.HasTimeEntry(timeEntry))
                                    {
                                        timeEntries.Add(timeEntry);
                                    }
                                }
                            }
                            GetSubTasks(task, timeEntries, filter);
                        }
                    }
                }
            }

            using (var stream = new StringWriter())
            {
                using (var writer = new XmlTextWriter(stream))
                {
                    writer.WriteStartDocument(true);
                    writer.WriteStartElement("data");

                    foreach (var timeEntry in timeEntries)
                    {
                        writer.WriteStartElement("Table");
                        writer.WriteElementString("StartTime", String.Format("{0} {1}", timeEntry.StartTime.ToShortDateString(), timeEntry.StartTime.ToShortTimeString()));
                        writer.WriteElementString("EndTime", String.Format("{0} {1}", timeEntry.EndTime.ToShortDateString(), timeEntry.EndTime.ToShortTimeString()));
                        writer.WriteElementString("Customer", timeEntry.Task.Project.Customer.Name);
                        writer.WriteElementString("Project", timeEntry.Task.Project.Name);
                        writer.WriteElementString("Task", timeEntry.Task.Name);
                        writer.WriteElementString("Consultant", timeEntry.User.Name);
                        writer.WriteElementString("Billable", timeEntry.Billable.ToString());
                        writer.WriteElementString("Description", timeEntry.Description);
                        writer.WriteElementString("Timespent", timeEntry.TimeSpent.ToString("N2"));
                        writer.WriteElementString("BillableTime", timeEntry.BillableTime.ToString("N2"));
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                    //stream.Position = 0;
                    //var xml = System.Text.Encoding.UTF8.GetString(stream.GetBuffer());
                    var stringReader = new StringReader(stream.ToString());

                    dataSet.ReadXml(stringReader);

                    return dataSet;
                }
            }
        }
    }
}