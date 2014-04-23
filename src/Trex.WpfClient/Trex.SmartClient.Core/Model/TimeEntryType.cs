namespace Trex.SmartClient.Core.Model
{
    public class TimeEntryType
    {
        public virtual int Id { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual bool IsBillableByDefault { get; set; }
        public virtual string Name { get; set; }
        public virtual int? CustomerId { get; set; }

        public static TimeEntryType Create(int id,bool isDefault, bool isBillableByDefault, string name, int? customerId)
        {
            var timeEntryType = new TimeEntryType()
                                    {
                                        Id = id,
                                        IsDefault = isDefault,
                                        IsBillableByDefault = isBillableByDefault,
                                        Name = name,
                                        CustomerId = customerId
                                    };

            return timeEntryType;
        }
    }
}
