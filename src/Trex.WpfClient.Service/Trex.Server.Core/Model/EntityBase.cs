using System;

namespace Trex.Server.Core.Model
{
    [Serializable]
    public abstract class EntityBase : IEquatable<EntityBase>
    {
        public abstract int EntityId { get; }

        private Guid _internalId { get; set; }

        protected EntityBase()
        {
            _internalId = Guid.NewGuid();
        }

        public virtual bool Equals(EntityBase obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            if (EntityId != 0)
                return obj.EntityId == EntityId;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals((EntityBase)obj);
        }

        public override int GetHashCode()
        {
            if (EntityId != 0)
            {
                return (EntityId.GetHashCode() * 397) ^ GetType().GetHashCode();
            }
            else
            {
                return (_internalId.GetHashCode() * 397) ^ GetType().GetHashCode();
            }
        }

        public static bool operator ==(EntityBase left, EntityBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EntityBase left, EntityBase right)
        {
            return !Equals(left, right);
        }
    }
}