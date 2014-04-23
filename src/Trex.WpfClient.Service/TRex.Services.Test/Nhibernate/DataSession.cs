using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using Trex.Server.Infrastructure;
using Trex.Server.Infrastructure.Mappings2;

namespace TRex.Services.Test.Nhibernate
{
    public class DataSession
    {
        private readonly IPersistenceConfigurer _dbType;
        public DataSession(IPersistenceConfigurer dbType)
        {
            _dbType = dbType;
            CreateSessionFactory();
        }

        private ISessionFactory _sessionFactory;
        private Configuration _configuration;

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        public Configuration Configuration
        {
            get { return _configuration; }
        }

        private void CreateSessionFactory()
        {
            _sessionFactory = Fluently
                .Configure()
                .Database(_dbType)
                .Mappings(m => m.FluentMappings
                                .Add<UserMap>()
                                .Add<CompanyMap>()
                                .Add<ProjectMap>()
                                .Add<CompanyMap>()
                                .Add<TaskMap>()
                                .Add<TimeEntryMap>()
                                .Add<TimeEntryTypeMap>()
                                .Add<InvoiceLineMap>()
                                .Add<InvoiceMap>()
                                .Add<TagMap>()
                                .Add<UserCustomerInfoMap>()
                                .Add<ForecastTypeMap>()
                                .Add<ForecastMap>()
                                .Add<ForecastProjectHoursMap>()
                                .Add<HolidayMap>()
                                .Add<ForecastMonthMap>()
                                .Conventions.AddFromAssemblyOf<FluentNhibernateConfig>())
                .ExposeConfiguration(cfg => _configuration = cfg)
                .BuildSessionFactory();
        }
    }
}
