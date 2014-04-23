using System.Collections.Generic;
using NHibernate;
using NHibernate.Connection;

namespace Trex.Server.Infrastructure
{
    public class DynamicConnectionProvider : DriverConnectionProvider
    {
        private string _connectionString;
        public static string DynamicString { get; set; }
        public static int ApplicationType { get; set; }

        public override void Configure(IDictionary<string, string> settings)
        {
            if (!settings.TryGetValue(NHibernate.Cfg.Environment.ConnectionString, out _connectionString))
                _connectionString = GetNamedConnectionString(settings);

            if (_connectionString == null)
            {
                throw new HibernateException();
            }
            ConfigureDriver(settings);
        }

        protected override string ConnectionString
        {
            get { return DynamicString; }
        }
    }
}