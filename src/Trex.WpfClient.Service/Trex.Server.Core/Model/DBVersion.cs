using System;

namespace Trex.Server.Core.Model
{
    public class DBVersion : EntityBase
    {
        public DBVersion() { }
                
        public DBVersion(string version, DateTime createDate, string creator, string description)
        {
            VersionNumber = version;
            CreateDate = createDate;
            Creator = creator;
            Description = description;
        }

        public virtual int DBVersionId { get; set; }
        public virtual string VersionNumber { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual string Creator { get; set; }
        public virtual string Description { get; set; }

        public override int EntityId
        {
            get { return DBVersionId; }
        }
    }
}