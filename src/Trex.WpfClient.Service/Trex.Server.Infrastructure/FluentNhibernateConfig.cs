using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Trex.Server.Infrastructure
{
    public class FluentNhibernateConfig
    {
        public class ForeignKeyNameConvention : IHasManyConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                instance.Key.Column(instance.EntityType.Name + "Id");
            }
        }


        public class ForeignKeyNameManyToManyConvention : IHasManyToManyConvention
        {
            public void Apply(IManyToManyCollectionInstance instance)
            {
                instance.Table(string.Format("{0}{1}",
                                             instance.EntityType.Name + "s",
                                             instance.ChildType.Name + "s"));
                instance.Key.Column(instance.EntityType.Name + "Id");
                instance.Relationship.Column(instance.ChildType.Name + "Id");
            }
        }

        public class PrimaryKeyNameConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.Column(instance.EntityType.Name + "ID");
            }
        }

        public class ReferenceConvention : IReferenceConvention
        {
            public void Apply(IManyToOneInstance instance)
            {
                instance.Column(instance.Property.PropertyType.Name + "ID"); //replace with _id when DB is cleaned up
            }
        }

        public class ForeignKeyConstraintNameConvention : IHasManyConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                instance.Key.ForeignKey(string.Format("{0}_{1}_FK", instance.Member.Name, instance.EntityType.Name));
            }
        }


        public class TableNameConvention : IClassConvention, IClassConventionAcceptance
        {
            public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
            {
                criteria.Expect(x => x.TableName, Is.Not.Set);
            }

            public void Apply(IClassInstance instance)
            {
                instance.Table(instance.EntityType.Name + "s");
            }
        }
    }
}
