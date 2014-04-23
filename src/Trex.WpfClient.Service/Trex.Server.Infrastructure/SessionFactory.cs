using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Practices.Unity;
using NHibernate;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using Trex.Server.Core;
using Trex.Server.Infrastructure.Implemented;
using Trex.Server.Infrastructure.Mappings2;
using Configuration = NHibernate.Cfg.Configuration;

namespace Trex.Server.Infrastructure
{
    public interface INHibernateSessionFactory
    {
        ISession CreateSession();
    }

    public class MsSqlSessionFactory : INHibernateSessionFactory
    {
        private Configuration _config;
        private readonly ISessionFactory _sessionFactory;
        private readonly SchemaExport _schema;

        public MsSqlSessionFactory(IUnityContainer unityContainer)
        {
            //string connectionstring = @"Initial Catalog=trex.dk;user id=trexUser;Password=ch33seD1pper;";

            //String hostName = Environment.MachineName;
            //switch (hostName)
            //{
            //    case "otherpc":
            //        connectionstring = @"";
            //        break;
            //    case "WIN-B8C38AQEHEF":
            //    case "WIN-5F68BM93R0K":
            //        connectionstring = @"Server=.;Database=Trex_test;Trusted_Connection=true;";
            //        break;
            //}

            if (TenantConnectionProvider.DynamicString == null)
            {
                TenantConnectionProvider.DynamicString = ConfigurationManager.ConnectionStrings["TrexBase"].ConnectionString;
            }

            _config = Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.ConnectionString(TenantConnectionProvider.DynamicString)
                                                                      .ShowSql().Provider<TenantConnectionProvider>())
                              .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TaskMap>()
                                              .Conventions.AddFromAssemblyOf<FluentNhibernateConfig>())
                              .BuildConfiguration();
            _config.SetListener(ListenerType.PostInsert, new GenericEntityChangeTracker(unityContainer));
            _config.SetListener(ListenerType.PostUpdate, new GenericEntityChangeTracker(unityContainer));
            _sessionFactory = _config.BuildSessionFactory();

            _schema = new SchemaExport(_config);

            CreateSchema();
        }



        public ISession CreateSession()
        {
            return _sessionFactory.OpenSession();
        }

        private void CreateSchema()
        {
            _schema.Execute(false, false, false);
        }
    }
}
