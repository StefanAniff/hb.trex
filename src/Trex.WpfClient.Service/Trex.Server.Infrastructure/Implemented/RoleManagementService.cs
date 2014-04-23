using System;
using System.Data;
using System.Data.SqlClient;
using Trex.Server.Core.Services;
using log4net;

namespace Trex.Server.Infrastructure.Implemented
{
    public class RoleManagementService : IRoleManagementService
    {
        private static readonly ILog Log = LogManager.GetLogger("TRex." + typeof(RoleManagementService).Name);

        public void CreateRole(string roleName, string conn)
        {
            try
            {
                using (var connection = new SqlConnection(conn))
                {
                    using (var cmd = new SqlCommand("spCreateRole", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@role", SqlDbType.NVarChar, 100);
                        cmd.Parameters["@role"].Value = roleName;

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }



        public void DeleteRole(string roleName, string conn)
        {
            try
            {
                using (var connection = new SqlConnection(conn))
                {
                    using (var cmd = new SqlCommand("spDeleteRole", connection) {CommandType = CommandType.StoredProcedure})
                    {
                        cmd.Parameters.Add("@role", SqlDbType.NVarChar, 100);
                        cmd.Parameters["@role"].Value = roleName;

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
