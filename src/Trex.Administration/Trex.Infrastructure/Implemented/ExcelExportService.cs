using System;
using System.Linq;
using System.Windows.Browser;
using Trex.Core.EventArgs;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.Core.Services;

namespace Trex.Infrastructure.Implemented
{
    public class ExcelExportService : IExcelExportService
    {
        private readonly IDataService _dataService;

        public ExcelExportService(IDataService dataService)
        {
            _dataService = dataService;
        }

        #region IExcelExportService Members

        public void Export(ISelectionFilter selectionFilter, ITimeEntryFilter timeEntryFilter)
        {
            var entityTransferObject = GetTransferObject(selectionFilter, timeEntryFilter);
            _dataService.ExcelExportCompleted += _dataService_ExcelExportCompleted;
            _dataService.CreateExcelSheet(entityTransferObject);
        }

        #endregion

        private void _dataService_ExcelExportCompleted(object sender, ExcelCompletedEventArgs e)
        {
            _dataService.ExcelExportCompleted -= _dataService_ExcelExportCompleted;
            HtmlPage.Window.Navigate(new Uri(HtmlPage.Document.DocumentUri, String.Format("ExcelHandler.ashx?type={0}&exportid={1}", "xls", e.ExportId)), "_blank");
        }

        private ISearchFilterTransferObject GetTransferObject(ISelectionFilter selectionFilter, ITimeEntryFilter timeEntryFilter)
        {
            var entityTransferObject = new SearchFilterTransferObject();
            entityTransferObject.CustomerIds = selectionFilter.Customers.Select(c => c.Id).ToList();
            entityTransferObject.ProjectIds = selectionFilter.Projects.Select(p => p.Id).ToList();
            entityTransferObject.TaskIds = selectionFilter.Tasks.Select(t => t.Id).ToList();
            if (timeEntryFilter != null)
            {
                entityTransferObject.Users = timeEntryFilter.Users.Select(u => u.Id).ToList();
                entityTransferObject.DateFrom = timeEntryFilter.DateFrom;
                entityTransferObject.DateTo = timeEntryFilter.DateTo;
            }

            return entityTransferObject;
        }
    }
}