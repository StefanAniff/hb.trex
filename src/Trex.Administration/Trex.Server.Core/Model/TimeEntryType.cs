namespace Trex.Server.Core.Model
{
    public class TimeEntryType
    {
        public virtual int Id { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual bool IsBillableByDefault { get; set; }
        public virtual string Name { get; set; }
        public virtual Customer Customer { get; set; }
    }
}