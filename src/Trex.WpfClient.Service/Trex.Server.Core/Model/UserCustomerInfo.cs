namespace Trex.Server.Core.Model
{
    public class UserCustomerInfo
    {
        public virtual int CustomerID { get; protected set; }
        public virtual int UserID { get; protected set; }
        public virtual double PricePrHour { get; set; }

        public UserCustomerInfo()
        {
            
        }
        public UserCustomerInfo(int userID, int customerId)
        {
            CustomerID = customerId;
            UserID = userID;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var compareObj = (UserCustomerInfo) obj;

            if (this.UserID.CompareTo(compareObj.UserID) != 0)
                return false;
            if (this.CustomerID.CompareTo(compareObj.CustomerID) != 0)
                return false;

            return true;

        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}