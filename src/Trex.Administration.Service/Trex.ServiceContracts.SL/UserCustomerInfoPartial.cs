namespace Trex.ServiceContracts
{
    public partial class UsersCustomer
    {
        public override bool Equals(object obj)
        {
            var compareObj = obj as UsersCustomer;
            if (compareObj == null)
                return false;

            return this.CustomerID == compareObj.CustomerID && this.UserID == compareObj.UserID;
        }

        public void CancelChanges()
        {
            ChangeTracker.CancelChanges();
        }
    }
}
