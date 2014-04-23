using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
   public class DBVersionMap : ClassMap<DBVersion>
    {
        public DBVersionMap()
        {
            Id(x => x.DBVersionId);
            Map(x => x.VersionNumber);
            Map(x => x.CreateDate);
            Map(x => x.Creator);
            Map(x => x.Description);
        }
    }
}
