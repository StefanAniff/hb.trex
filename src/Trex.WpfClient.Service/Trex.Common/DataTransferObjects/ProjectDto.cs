namespace Trex.Common.DataTransferObjects
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CompanyDto CompanyDto { get; set; }

        public bool Inactive { get; set; }

        #region Equalitymembers

        protected bool Equals(ProjectDto other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProjectDto) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        #endregion

    }
}