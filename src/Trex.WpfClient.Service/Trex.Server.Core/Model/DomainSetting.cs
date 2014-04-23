namespace Trex.Server.Core.Model
{
    /// <summary>
    /// Representation of a single domain/business related setting
    /// </summary>
    public class DomainSetting : EntityBase
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Value { get; protected set; }

        public override int EntityId
        {
            get { return Id; }
        }
    }
}