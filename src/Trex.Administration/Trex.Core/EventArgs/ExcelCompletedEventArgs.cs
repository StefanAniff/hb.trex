using System;

namespace Trex.Core.EventArgs
{
    public class ExcelCompletedEventArgs : System.EventArgs
    {
        public ExcelCompletedEventArgs(Guid exportId)
        {
            ExportId = exportId;
        }

        public Guid ExportId { get; set; }
    }
}