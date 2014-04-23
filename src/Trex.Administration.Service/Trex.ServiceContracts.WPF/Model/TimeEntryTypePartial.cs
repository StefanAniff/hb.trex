namespace Trex.ServiceContracts
{
    public partial class TimeEntryType
    {
        public static TimeEntryType Create(int id, bool isDefault, bool isBillableByDefault, string name, int? customerId)
        {
            var timeEntryType = new TimeEntryType()
            {
                TimeEntryTypeId = id,
                IsDefault = isDefault,
                IsBillableByDefault = isBillableByDefault,
                Name = name,
                CustomerId = customerId
            };

            return timeEntryType;
        }
    }
}
