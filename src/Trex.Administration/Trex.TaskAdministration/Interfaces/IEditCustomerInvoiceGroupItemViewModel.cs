using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Trex.ServiceContracts;

namespace Trex.TaskAdministration.Dialogs.EditCustomerInvoiceGroupView
{
    public interface IEditCustomerInvoiceGroupItemViewModel
    {
        CustomerInvoiceGroup CIG { get; set; }

        [Required]
        string Label { get; set; }

        string Attention { get; set; }
        string Email { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string ZipCode { get; set; }
        string City { get; set; }
        string Country { get; set; }
        bool ReadOnly { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        void Update();
        void Close();
    }
}