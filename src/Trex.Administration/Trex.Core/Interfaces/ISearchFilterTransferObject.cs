using System;
using System.Collections.Generic;

namespace Trex.Core.Interfaces
{
    public interface ISearchFilterTransferObject
    {
        List<int> CustomerIds { get; set; }
        List<int> ProjectIds { get; set; }
        List<int> TaskIds { get; set; }
        List<int> Users { get; set; }
        DateTime? DateFrom { get; set; }
        DateTime? DateTo { get; set; }
    }
}