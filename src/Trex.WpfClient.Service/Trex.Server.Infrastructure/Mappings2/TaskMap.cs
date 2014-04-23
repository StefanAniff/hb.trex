using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class TaskMap : ClassMap<Task>
    {
        public TaskMap()
        {
            Id(x => x.TaskID);
            References(x => x.ParentTask).Column("ParentID").Nullable();
            Map(x => x.Guid);
            References(x => x.CreatedBy).Column("CreatedBy");
            References(x => x.ChangedBy).Column("ChangedBy").Nullable();

            Map(x => x.CreateDate);
            Map(x => x.TaskName);
            Map(x => x.Description).Nullable();
            Map(x => x.TimeLeft);
            Map(x => x.TimeEstimated);
            Map(x => x.WorstCaseEstimate);
            Map(x => x.BestCaseEstimate);
            Map(x => x.RealisticEstimate);
            Map(x => x.Inactive).Not.Nullable();
            Map(x => x.TimeRegistrationTypeId);
            References(x => x.Project);
            Map(x => x.ChangeDate).Nullable();
            References(x => x.Tag).Nullable();

            Table(ObjectNames.TableTask);
        }
    }
}
