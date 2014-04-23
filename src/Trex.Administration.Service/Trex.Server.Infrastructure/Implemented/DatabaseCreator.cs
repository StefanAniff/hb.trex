using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using Trex.Server.Core.Services;


namespace Trex.Server.Infrastructure.Implemented
{
    public class DatabaseCreator : IDatabaseCreator
    {
        private readonly IAppSettings _appSettings;

        public DatabaseCreator(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        private string _dbName = string.Empty;
        public string ConnectionString { get; private set; }

        public void CreateTrex(string customerId)
        {
            var defaultString = _appSettings.DefaultConnectionString; 
            _dbName = string.Concat(Guid.NewGuid(), "-", customerId);
            ConnectionString = string.Format(defaultString,_dbName);

            CreateDatabase();
            CreateScheme();
            CreateData();
           
        }

        private void CreateDatabase()
        {
            var trexCreateDBScriptPath = _appSettings.CreateDBScriptPath; 
            RunScriptFile(trexCreateDBScriptPath);
        }

        private void CreateScheme()
        {
            var schemeFilePath = _appSettings.CreateSchemeScriptPath; 
            RunScriptFile(schemeFilePath);
        }

        private void CreateData()
        {
            var trexDataScriptPath = _appSettings.DataScriptPath; 
            RunScriptFile(trexDataScriptPath);
        }

        private void RunScriptFile(string filePath)
        {
            var sqlSeperator = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            SqlConnection connection = null;

            var file = new FileInfo(filePath);
            using (var fileStream = file.OpenText())
            {
                var tablesScript = fileStream.ReadToEnd();
                tablesScript = tablesScript.Replace("nameofinstance", _dbName);

                var txtSql = tablesScript;
                var sqlLine = sqlSeperator.Split(txtSql);

                try
                {
                    connection = new SqlConnection(_appSettings.TrexBaseConnectionString);
                    connection.Open();

                    var cmd = connection.CreateCommand();
                    cmd.Connection = connection;

                    foreach (var line in sqlLine)
                    {
                        if (line.Length > 0)
                        {
                            cmd.CommandText = line;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                finally
                {
                    if (connection != null) connection.Close();
                }



            }



        }
    }
}
