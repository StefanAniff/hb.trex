namespace Trex.Core.Model
{
    public class UserCustomerInfo
    {
        public UserCustomerInfo() {}

        public UserCustomerInfo(User user, Customer customer, double pricePrHour)
        {
            UserId = user.Id;
            CustomerId = customer.Id;
            PricePrHour = pricePrHour;
        }

        public virtual int CustomerId { get; set; }

        public virtual int UserId { get; set; }

        public virtual double PricePrHour { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var compareObj = (UserCustomerInfo) obj;

            if (UserId.CompareTo(compareObj.UserId) != 0)
            {
                return false;
            }
            if (CustomerId.CompareTo(compareObj.CustomerId) != 0)
            {
                return false;
            }

            return true;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}