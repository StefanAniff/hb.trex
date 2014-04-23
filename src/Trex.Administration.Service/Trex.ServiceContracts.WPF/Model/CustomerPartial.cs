

using System.IO;
using System.Runtime.Serialization;

namespace Trex.ServiceContracts
{
    public partial class Customer : IEntity
    {
        public bool IsValidChild(IEntity entity)
        {
            return entity is Project;
        }

        public int Id
        {
            get { return CustomerID; }
            set { CustomerID = value; }
        }

        public override bool Equals(object obj)
        {
            var customer = obj as Customer;

            if (customer == null)
                return false;

            if (CustomerID == 0)
                return Guid == customer.Guid;

            return customer.CustomerID == CustomerID;

        }

        public override int GetHashCode()
        {
            return CustomerID.GetHashCode();
        }

        public static Customer Create(string name, int id, bool inheritsTimeEntryTypes)
        {
            return
                new Customer
                {
                    CustomerName = name,
                    Id = id,
                    InheritsTimeEntryTypes = inheritsTimeEntryTypes
                };
        }

        public void CancelChanges()
        {
            ChangeTracker.SetParentObject(this);
            ChangeTracker.CancelChanges();
        }
    }

    //Required using System.Runtime.Serialization;
    //- Silverlight doesn't have it in its plugin, have to add reference to your project
    public static class ExtensionMethods
    {
        public static T DeepCopy<T>(this T theSource)
        {
            T theCopy;
            DataContractSerializer theDataContactSerializer = new DataContractSerializer(typeof(T));
            using (MemoryStream memStream = new MemoryStream())
            {
                theDataContactSerializer.WriteObject(memStream, theSource);
                memStream.Position = 0;
                theCopy = (T)theDataContactSerializer.ReadObject(memStream);
            }
            return theCopy;
        }
    }

}
