using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Trex.ServiceContracts
{
    public partial class User
    {
        public User()
        {
            Roles = new List<string>();
        }

        [DataMember]
        public List<string> Roles { get; set; }
        [DataMember]
        public List<string> Permissions { get; set; }

        public override bool Equals(object obj)
        {
            var user = obj as User;

            if (user == null)
                return false;

            return user.UserID == UserID;
        }

        public override int GetHashCode()
        {
            return UserID.GetHashCode();
        }

        public double GetRateForCustomer(Customer customer)
        {
            var userCustomerInfo = this.UsersCustomers.SingleOrDefault(
                    uc => uc.UserID == this.UserID && uc.CustomerID == customer.CustomerID);

            if (userCustomerInfo == null)
                return this.Price;

            return userCustomerInfo.Price;
        }

        public virtual void AddCustomerInfo(UsersCustomer customerInfo)
        {
            //Update existing info item
            if (this.UsersCustomers.Contains<UsersCustomer>(customerInfo))
            {
                var foundInfo = UsersCustomers.Single(ci => ci.Equals(customerInfo));
                foundInfo.Price = customerInfo.Price;
            }
            else
            {
                UsersCustomers.Add(customerInfo);
            }
        }

        public virtual void RemoveCustomerInfo(UsersCustomer customerInfo)
        {
            if (UsersCustomers.Contains<UsersCustomer>(customerInfo))
            {
                var foundInfo = UsersCustomers.Single(ci => ci == customerInfo);
                UsersCustomers.Remove(foundInfo);
            }
        }

    }
}
