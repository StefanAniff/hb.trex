using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using NHibernate;
using NHibernate.Connection;
using Trex.Server.Infrastructure.State;

namespace Trex.Server.Core
{
    public class TenantConnectionProvider : DriverConnectionProvider
    {
        private string _connectionString;

        [ThreadStatic]
        private static string _dynamicString;
        public static string DynamicString
        {
            get { return _dynamicString; }
            set { _dynamicString = value; }
        }

        public static int ApplicationType { get; set; }

        public override void Configure(IDictionary<string, string> settings)
        {
            if (!settings.TryGetValue(NHibernate.Cfg.Environment.ConnectionString, out _connectionString))
            {
                _connectionString = GetNamedConnectionString(settings);
            }

            if (_connectionString == null)
            {
                throw new HibernateException();
            }
            ConfigureDriver(settings);
        }


        public override IDbConnection GetConnection()
        {
            var conn = Driver.CreateConnection();
            try
            {
                conn.ConnectionString = DynamicString;
                conn.Open();
            }
            catch (Exception)
            {
                conn.Dispose();
                throw;
            }

            return conn;
        }
    }
}
