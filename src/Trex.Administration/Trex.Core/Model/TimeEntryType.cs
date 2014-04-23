using System;
using Trex.Core.Interfaces;

namespace Trex.Core.Model
{
    public class TimeEntryType : IEntity
    {
        private Guid _guid;

        public TimeEntryType()
        {
            _guid = Guid.NewGuid();
        }

        public virtual bool IsDefault { get; set; }
        public virtual bool IsBillableByDefault { get; set; }
        public virtual string Name { get; set; }
        public virtual int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public bool IsGlobal
        {
            get { return Customer == null; }
        }

        public Guid Guid
        {
            get
            {
                if (_guid == Guid.Empty)
                {
                    _guid = Guid.NewGuid();
                }
                return _guid;
            }
            set { _guid = value; }
        }

        #region IEntity Members

        public virtual int Id { get; set; }

        public bool IsValidChild(IEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}