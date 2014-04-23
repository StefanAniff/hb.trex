#region

using System.Windows.Controls;
using Trex.Core.Interfaces;

#endregion

namespace Trex.Invoices.InvoiceManagementScreen.InvoiceAdministrationMasterView
{
    public partial class InvoiceAdministrationMasterView : UserControl, IView
    {
        public InvoiceAdministrationMasterView()
        {
            InitializeComponent();

            //radHtmlPlaceholder1.SourceUrl = new Uri(Application.Current.Host.Source, "/trexadmin/Invoices/newinvoice.aspx");
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion
    }
}