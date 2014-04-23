using System;

namespace Trex.ServiceContracts
{
    public partial class InvoiceTemplate
    {
        public InvoiceTemplate()
        {
            Guid = Guid.NewGuid();
        }
    }
}
