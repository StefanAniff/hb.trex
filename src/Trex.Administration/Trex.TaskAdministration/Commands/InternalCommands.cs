using Microsoft.Practices.Prism.Commands;

namespace Trex.TaskAdministration.Commands
{
    public static class InternalCommands
    {
        public static CompositeCommand UserFilterChanged = new CompositeCommand();
        public static CompositeCommand UserSelected = new CompositeCommand();
        public static CompositeCommand UserDeselected = new CompositeCommand();
        public static CompositeCommand ItemSelected = new CompositeCommand();

        public static CompositeCommand TimeEntryEditStart = new CompositeCommand();
        public static CompositeCommand TimeEntryEditCompleted = new CompositeCommand();
        public static CompositeCommand TimeEntryDeleteStart = new CompositeCommand();
        public static CompositeCommand TimeEntryDeleteCompleted = new CompositeCommand();
        public static CompositeCommand TimeEntryAddStart = new CompositeCommand();
        public static CompositeCommand TimeEntryAddCompleted = new CompositeCommand();

        public static CompositeCommand TaskAddStart = new CompositeCommand();
        public static CompositeCommand TaskAddCompleted = new CompositeCommand();
        public static CompositeCommand TaskEditStart = new CompositeCommand();
        public static CompositeCommand TaskEditCompleted = new CompositeCommand();
        public static CompositeCommand TaskDeleteStart = new CompositeCommand();
        public static CompositeCommand TaskDeleteCompleted = new CompositeCommand();

        public static CompositeCommand ProjectEditStart = new CompositeCommand();
        public static CompositeCommand ProjectEditCompleted = new CompositeCommand();
        public static CompositeCommand ProjectAddStart = new CompositeCommand();
        public static CompositeCommand ProjectAddCompleted = new CompositeCommand();
        public static CompositeCommand ProjectDeleteStart = new CompositeCommand();
        public static CompositeCommand ProjectDeleteCompleted = new CompositeCommand();

        public static CompositeCommand CustomerEditStart = new CompositeCommand();
        public static CompositeCommand CustomerEditCompleted = new CompositeCommand();
        public static CompositeCommand CustomerDeleteStart = new CompositeCommand();
        public static CompositeCommand CustomerDeleteCompleted = new CompositeCommand();
        public static CompositeCommand CustomerAddStart = new CompositeCommand();
        public static CompositeCommand CustomerAddCompleted = new CompositeCommand();
        public static CompositeCommand CustomerInvoiceGroupAddStart = new CompositeCommand();
        public static CompositeCommand CustomerInvoiceGroupComplete = new CompositeCommand();

        public static CompositeCommand ExcelExportStart = new CompositeCommand();

        public static CompositeCommand EditTimeEntryTypeStart = new CompositeCommand();
        public static CompositeCommand EditTimeEntryTypeCompleted = new CompositeCommand();
        public static CompositeCommand CreateTimeEntryTypeStart = new CompositeCommand();
        public static CompositeCommand CreateTimeEntryTypeCompleted = new CompositeCommand();

        public static CompositeCommand EditCustomerTimeEntryTypesStart = new CompositeCommand();
        public static CompositeCommand EditCustomerTimeEntryTypesCompleted = new CompositeCommand();

        public static CompositeCommand MoveEntityRequest = new CompositeCommand();

        public static CompositeCommand TaskSearchCompleted = new CompositeCommand();

        public static CompositeCommand GetInvoiceTemplates = new CompositeCommand(); 

        public static CompositeCommand UpdateCustomerInvoiceGroupList = new CompositeCommand();

        public static CompositeCommand CigCanExecuteSave = new CompositeCommand();

        public static CompositeCommand ReloadView = new CompositeCommand();
    }
}