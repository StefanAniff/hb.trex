using System;

namespace Trex.Server.Core.Services
{
    public interface IExcelExportService
    {
        Guid CreateWorkSheet(ISelectionFilter selectionFilter);
    }
}