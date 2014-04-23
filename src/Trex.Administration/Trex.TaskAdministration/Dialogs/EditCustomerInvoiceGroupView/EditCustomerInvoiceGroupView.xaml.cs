using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Printing;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;
using Trex.Core.Interfaces;

namespace Trex.TaskAdministration.Dialogs.EditCustomerInvoiceGroupView
{
    public partial class EditCustomerInvoiceGroupView : ChildWindow, IView
    {
        public EditCustomerInvoiceGroupView()
        {
            InitializeComponent();
            CustomerInvoiceGroups.PropertyChanged += CustomerInvoiceGroupsOnPropertyChanged;
        }

        private void CustomerInvoiceGroupsOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "SelectedItem")
            {
                Dispatcher.BeginInvoke(new Action(() =>
                                                      {
                                                          if (CustomerInvoiceGroups.Items.Count - 1 != 0)
                                                          {
                                                              CustomerInvoiceGroups.CurrentCellInfo = new GridViewCellInfo(CustomerInvoiceGroups.Items[CustomerInvoiceGroups.Items.Count - 1],
                                                                                                                           CustomerInvoiceGroups.Columns["Label"]);
                                                              CustomerInvoiceGroups.Focus();

                                                              CustomerInvoiceGroups.CurrentCell.IsInEditMode = true;
                                                              
                                                          }
                                                      }));
            }
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }


    }


}

