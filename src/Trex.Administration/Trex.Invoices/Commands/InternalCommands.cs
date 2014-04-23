#region

using Microsoft.Practices.Prism.Commands;

#endregion

namespace Trex.Invoices.Commands
{
    public static class InternalCommands
    {
        public static CompositeCommand RefreshCustomers = new CompositeCommand();
        public static CompositeCommand InvoiceEditStart = new CompositeCommand();
        public static CompositeCommand InvoiceAddStart = new CompositeCommand();
        public static CompositeCommand InvoiceLinesEditStart = new CompositeCommand();
        public static CompositeCommand CustomerSelected = new CompositeCommand();
        public static CompositeCommand InvoiceSelected2 = new CompositeCommand();
        public static CompositeCommand CustomerListFocused = new CompositeCommand();
        public static CompositeCommand ApplyFilter = new CompositeCommand();
        public static CompositeCommand RemoveInvoiceFromInvoicesList = new CompositeCommand();
        public static CompositeCommand CloseInvoice = new CompositeCommand();
        public static CompositeCommand AutoInvoice = new CompositeCommand();
        public static CompositeCommand ManageInvoiceLinesStart = new CompositeCommand();
        public static CompositeCommand ManageInvoiceLinesCompleted = new CompositeCommand();
        public static CompositeCommand InvoiceLineEditStart = new CompositeCommand();
        public static CompositeCommand InvoiceLineEditCompleted = new CompositeCommand();
        public static CompositeCommand InvoiceLineAddStart = new CompositeCommand();
        public static CompositeCommand InvoiceLineAddCompleted = new CompositeCommand();
        public static CompositeCommand UpdateScreens = new CompositeCommand();
        public static CompositeCommand TimeEntryChanged = new CompositeCommand();
        public static CompositeCommand InvoiceLineChanged = new CompositeCommand();
        public static CompositeCommand EditInvoiceCompleted = new CompositeCommand();
        public static CompositeCommand CreateNewInvoiceTemplateStart = new CompositeCommand();
        public static CompositeCommand FileUploaded = new CompositeCommand();

        public static CompositeCommand PreviewInvoice = new CompositeCommand();
        public static CompositeCommand PreviewSpecification = new CompositeCommand();

        public static CompositeCommand GetInvoicesFromMany = new CompositeCommand();
        public static CompositeCommand GenerateInvoices = new CompositeCommand();
        public static CompositeCommand SendInvoiceEmail = new CompositeCommand();

        public static CompositeCommand LoadInvoiceLines = new CompositeCommand();
        public static CompositeCommand LoadTimeEntries = new CompositeCommand();
        public static CompositeCommand GenerateInvoiceLines = new CompositeCommand();

        public static CompositeCommand FinalizeInvoice = new CompositeCommand();

        public static CompositeCommand GetAllDrafts = new CompositeCommand();
        public static CompositeCommand InvoiceSelected = new CompositeCommand();

        public static CompositeCommand UpdateInvoiceList = new CompositeCommand();
        public static CompositeCommand UpdateSelectedInvoiceItems = new CompositeCommand();
        public static CompositeCommand UpdateInvoiceListAfterFinalize = new CompositeCommand();

        public static CompositeCommand AddDummyInvoiceLines = new CompositeCommand();

        public static CompositeCommand CloseAddEditInvoiceWindow = new CompositeCommand();
        
        public static CompositeCommand ReloadInvoices = new CompositeCommand();

        public static CompositeCommand SeeAllInvoices = new CompositeCommand();

        public static CompositeCommand ResetSelectedInvoices = new CompositeCommand();

        public static CompositeCommand PrintInvoice = new CompositeCommand();

        public static CompositeCommand GenerateCreditnote = new CompositeCommand();
        public static CompositeCommand UpdateExclVAT = new CompositeCommand();

        public static CompositeCommand ReselectedCustomer = new CompositeCommand();
        public static CompositeCommand InvoiceIDSelected = new CompositeCommand();
        public static CompositeCommand FindInvoiceFromID = new CompositeCommand();

        public static CompositeCommand ReloadInvoiceSearchFilter = new CompositeCommand();

        public static CompositeCommand RaiseSendEmailCanExecute = new CompositeCommand();
        public static CompositeCommand SeeDebitList = new CompositeCommand();

        public static CompositeCommand RegenerateInvoiceFiles = new CompositeCommand();
        public static CompositeCommand SaveInvoiceComment = new CompositeCommand();

        public static CompositeCommand SeeCommentsStart = new CompositeCommand();
        public static CompositeCommand ReloadInvoiceCommentField = new CompositeCommand();

        public static CompositeCommand SendComment = new CompositeCommand();


        public static CompositeCommand ExcludeTimeEntry = new CompositeCommand();
    }
}