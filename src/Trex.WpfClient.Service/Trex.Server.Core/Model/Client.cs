using System;

namespace Trex.Server.Core.Model
{
    public class Client : EntityBase
    {
        public virtual int Id { get; set; }

        public virtual string CompanyName { get; set; }

        public virtual string VatNumber { get; set; }

        public virtual string Country { get; set; }

        public virtual string Address1 { get; set; }

        public virtual string Address2 { get; set; }

        public virtual string Address3 { get; set; }

        public virtual string Address4 { get; set; }

        public virtual string Address5 { get; set; }

        public virtual string CreatorUserName { get; set; }

        public virtual string CreatorFullName { get; set; }

        public virtual string CreatorPhone { get; set; }

        public virtual string CreatorEmail { get; set; }

        public virtual string CustomerId { get; set; }

        public virtual string ConnectionString { get; set; }

        public virtual bool Inactive { get; set; }

        public virtual DateTime? InactiveDate { get; set; }

        public virtual bool IsLockedOut { get; set; }

        public virtual DateTime? LockedOutDate { get; set; }

        public virtual DateTime CreateDate { get; set; }

        public virtual bool IsActivated { get; set; }

        public virtual string ActivationId { get; set; }

        public override int EntityId
        {
            get { return Id; }
        }
    }
}
