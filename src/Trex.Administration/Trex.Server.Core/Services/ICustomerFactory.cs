using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ICustomerFactory
    {
        Customer Create(string name, User creator, string streetAddress, string zipCode, string country, string contactName, string contactPhone, bool inheritsTimeEntryTypes, int paymentTermsNumberOfDays,
                        bool paymentTermsIncludeCurrentMonth, string address2);
    }
}