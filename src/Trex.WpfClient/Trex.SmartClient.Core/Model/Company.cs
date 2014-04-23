using System.Collections.Generic;

namespace Trex.SmartClient.Core.Model
{
    public class Company
    {        
        public string Name { get; set; }
        public int Id { get; set; }
        public bool InheritsTimeEntryTypes { get; set; }
        public bool Inactive { get; set; }

        private Company()
        {

        }

        public static Company Create(string name, int id, bool inheritsTimeEntryTypes,bool inactive)
        {
            return
                new Company
                              {
                                  Name = name,
                                  Id = id,
                                  InheritsTimeEntryTypes = inheritsTimeEntryTypes,
                                  Inactive = inactive
                              };
        }


        #region Equality

        protected bool Equals(Company other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Company) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        #endregion

    }
}
