#region

using System;
using System.Configuration;
using System.Data.EntityClient;
using System.IO;
using StructureMap;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using System.Linq;
using log4net.Config;

#endregion

namespace TrexSL.Web.StructureMap
{
    public class BootStrapper : IBootStrapper
    {
        #region IBootStrapper Members

        public void Setup()
        {
            //ValidateDBVersion();

            ConfigureLogging();
            ObjectFactory.Initialize(fac =>
                                         {
                                             fac.UseDefaultStructureMapConfigFile = false;
                                             fac.AddRegistry<StructureMapRegistry>();
                                         }
                );
        }

        private void ConfigureLogging()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "log4net.config");
            if (File.Exists(path))
            {
                XmlConfigurator.ConfigureAndWatch(new FileInfo(path));
            }
        }

        //private void ValidateDBVersion()
        //{
        //    var ef = ConfigurationManager.AppSettings["defaultEFConnectionString"];
        //    var trex = ConfigurationManager.ConnectionStrings["VersionCheck"].ConnectionString;

        //    var entityConnectionStringBuilder = new EntityConnectionStringBuilder(string.Format(ef, trex));
        //    var entityConnection = new EntityConnection(entityConnectionStringBuilder.ToString());

        //    using (var db = new TrexEntities(entityConnection))
        //    {
        //        var ver = (from version in db.Versions.OrderBy(v => v.Version_number)
        //                   select version).ToList().Last();

        //        var dbNumber = int.Parse(ver.Versions_number.Replace(".", ""));
        //        var localNumber = int.Parse(ConfigurationManager.AppSettings["Version"].Replace(".", ""));

        //        if (dbNumber < localNumber)
        //        {
        //            throw new Exception("database version out of date");
        //        }
        //        if (dbNumber > localNumber)
        //        {
        //            throw new Exception("Application version out of date");
        //        }
        //    }
        //}
        #endregion
    }
}