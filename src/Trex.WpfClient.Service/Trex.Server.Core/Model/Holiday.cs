using System;

namespace Trex.Server.Core.Model
{
    public class Holiday : EntityBase
    {
        private DateTime _date;        

        protected Holiday() {}

        public Holiday(DateTime date, string description)
        {
            Date = date;
            Description = description;
        }

        public virtual int Id { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }        

        public override int EntityId
        {
            get { return  Id; }
        }
    }
}