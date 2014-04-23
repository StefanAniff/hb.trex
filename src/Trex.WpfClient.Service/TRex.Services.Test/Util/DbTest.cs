using System.IO;
using System.Text;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using TRex.Services.Test.Nhibernate;

namespace TRex.Services.Test.Util
{
    public abstract class DbTest
    {
        private DataSession _dataSession;
        protected ISession Session;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //Use In Memory database, open session and Build Schema inside In Memory database
            _dataSession = new DataSession(SQLiteConfiguration.Standard.InMemory());
            Session = _dataSession.SessionFactory.OpenSession();
            BuildSchema(Session, _dataSession.Configuration);
            OnTestFixtureSetUp();
        }
         
        protected virtual void OnTestFixtureSetUp()
        {
            // Specialized hookup
        }

        public void BuildSchema(ISession session, Configuration configuration)
        {
            var export = new SchemaExport(configuration);
            var sb = new StringBuilder();
            TextWriter writer = new StringWriter(sb);
            export.Execute(true, true, false, session.Connection, writer);
            writer.Dispose(); //set breakpoint here to get schema
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            //Clean up after tests have run
            Session.Close();
            _dataSession.SessionFactory.Close();
            OnTestFixtureTearDown();
        }

        protected virtual void OnTestFixtureTearDown()
        {
            // Specialized hookup
        }
    }
}