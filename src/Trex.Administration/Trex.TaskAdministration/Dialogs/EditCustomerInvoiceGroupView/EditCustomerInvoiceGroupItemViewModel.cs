using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Trex.Core.Implemented;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.Dialogs.EditCustomerInvoiceGroupView
{
    public class EditCustomerInvoiceGroupItemViewModel : ViewModelBase, IEditCustomerInvoiceGroupItemViewModel
    {
        public EditCustomerInvoiceGroupItemViewModel(CustomerInvoiceGroup cig)
        {
            CIG = cig;
            RadioReadOnly = true;
            if (CIG.DefaultCig)
            {
                ReadOnly = true;
                RadioReadOnly = false;
            }
            if (CIG.Label == null)
                CIG.Label = null;

            if (CIG.SendFormat == 1)
            {
                IsMail = true;
                IsPrint = false;
            }
            else
            {
                IsPrint = true;
                IsMail = false  ;
            }

        }
        public CustomerInvoiceGroup CIG { get; set; }

        [Required]
        public string Label
        {
            get
            {
                return CIG.Label;
            }
            set
            {
                CIG.Label = value;
                Validate("Label", value);
                OnPropertyChanged("Label");
            }
        }

        [Required]
        public string Attention
        {
            get { return CIG.Attention; }
            set
            {
                CIG.Attention = value;
                Validate("Attention", value);
                OnPropertyChanged("Attention");
            }
        }

        
        public string Email
        {
            get
            {
                return CIG.Email;
            }
            set
            {
                CIG.Email = value;
                OnPropertyChanged("Email");
                if(IsMail && value == "")
                    throw new ValidationException("The email field is requered");
            }
        }

        public string EmailCC
        {
            get { return CIG.EmailCC; }
            set
            {
                CIG.EmailCC = value;
                OnPropertyChanged("EmailCC");
            }
        }

        [Required]
        public string Address1
        {
            get { return CIG.Address1; }
            set
            {
                CIG.Address1 = value;
                Validate("Address1", value);
                OnPropertyChanged("Address1");
            }
        }

        public string Address2
        {
            get { return CIG.Address2; }
            set
            {
                CIG.Address2 = value;
                OnPropertyChanged("Address2");
            }
        }

        [Required]
        public string ZipCode
        {
            get { return CIG.ZipCode; }
            set
            {
                CIG.ZipCode = value;
                Validate("ZipCode", value);
                OnPropertyChanged("ZipCode");
            }
        }

        [Required]
        public string City
        {
            get { return CIG.City; }
            set
            {
                CIG.City = value;
                Validate("City", value);
                OnPropertyChanged("City");
            }
        }

        [Required]
        public string Country
        {
            get { return CIG.Country; }
            set
            {
                CIG.Country = value;
                Validate("Country", value);
                OnPropertyChanged("Country");
            }
        }

        private bool _isMail;
        public bool IsMail
        {
            get { return _isMail; } 
            set
            {
                _isMail = value;
                if (value)
                    CIG.SendFormat = 1;
                else
                    CIG.SendFormat = 2;
                OnPropertyChanged("IsMail");
                OnPropertyChanged("Email");
            }
        }

        private bool _isPrint;
        public bool IsPrint
        {
            get { return _isPrint; } 
            set
            {
                _isPrint = value;
                if (value)
                    CIG.SendFormat = 2;
                else
                    CIG.SendFormat = 1;

                OnPropertyChanged("IsPrint");
                OnPropertyChanged("Email");
            }
        }

        public bool ReadOnly { get; set; }
        public bool RadioReadOnly { get; set; }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

           InternalCommands.CigCanExecuteSave.Execute(null);
        }
    }
}
