using System;
using System.Collections.Generic;
using Trex.Core.Interfaces;

namespace Trex.Core.Implemented
{
    public class SearchFilterTransferObject : ISearchFilterTransferObject
    {
        #region ISearchFilterTransferObject Members

        public List<int> CustomerIds { get; set; }
        public List<int> ProjectIds { get; set; }
        public List<int> TaskIds { get; set; }
        public List<int> Users { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        #endregion
    }
}