using Microsoft.Practices.Prism.Commands;

namespace Trex.Administration.Commands
{
    public static class InternalCommands
    {
        public static CompositeCommand CreateNewUserStart = new CompositeCommand();
        public static CompositeCommand CreateNewUserCompleted = new CompositeCommand();
        public static CompositeCommand EditUserStart = new CompositeCommand();
        public static CompositeCommand EditUserCompleted = new CompositeCommand();
        public static CompositeCommand EditUserPricesStart = new CompositeCommand();
        public static CompositeCommand EditUserPricesCompleted = new CompositeCommand();
        public static CompositeCommand DeleteCustomerInfo = new CompositeCommand();
        public static CompositeCommand DeleteUser = new CompositeCommand();
        public static CompositeCommand ActivateUser = new CompositeCommand();
        public static CompositeCommand DeActivateUser = new CompositeCommand();
        public static CompositeCommand InviteUsersStart = new CompositeCommand();
        public static CompositeCommand InviteUsersCompleted = new CompositeCommand();
        public static CompositeCommand ReloadUsers = new CompositeCommand();
        public static CompositeCommand CancelEditPrices = new CompositeCommand();
    }
}