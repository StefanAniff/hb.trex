namespace Trex.Server.Core.Model
{
    public class TimeEntryType : EntityBase
    {
        public virtual int Id { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual bool IsBillableByDefault { get; set; }
        public virtual string Name { get; set; }
        public virtual Company Company { get; set; }

        public TimeEntryType()
        {

        }
        public TimeEntryType(Company company, string name, bool isBillableByDefault, bool isDefault)
        {
            Company = company;
            Name = name;
            IsBillableByDefault = isBillableByDefault;
            IsDefault = isDefault;
        }

        public override int EntityId
        {
            get { return Id; }
        }
    }
}
