using Trex.Core.Interfaces;
using Trex.Core.Model;

namespace Trex.Core.Services
{
    public interface IExcelExportService
    {
        void Export(ISelectionFilter selectionFilter, ITimeEntryFilter timeEntryFilter);
    }
}