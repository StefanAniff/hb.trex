using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using log4net;


namespace Trex.Server.Infrastructure.Implemented
{
    public class PermissionService : IPermissionService
    {
        private static readonly ILog Log = LogManager.GetLogger("TRex." + typeof (PermissionService).Name);

        public List<PermissionItem> GetAllPermissions(int clientApplicationId)
        {
            var permissions = new List<PermissionItem>();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TrexBase"].ConnectionString))
                {
                    using (var cmd = new SqlCommand("spGetAllPermissions", connection) {CommandType = CommandType.StoredProcedure})
                    {
                        cmd.Parameters.Add("@ClientApplicationID", SqlDbType.Int);
                        cmd.Parameters["@ClientApplicationID"].Value = clientApplicationId;

                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var permissionId = (int) reader[0];
                                var permissionName = (string) reader[1];
                                permissions.Add(new PermissionItem(permissionId, permissionName, false));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
            return permissions;
        }

        private static string BuildGetRolesPermissionsSqlQuery(IEnumerable<string> roleNames, int clientAppTypeId)
        {
            const string sql = @"
                                SELECT distinct 
	                                PermissionID, 
	                                ps.Permission 
                                from 
	                                PermissionsInRoles as pr
	                                join Roles as r on r.ID = pr.RoleID
	                                join dbo.[Permissions] as ps on ps.ID = pr.PermissionID
                                where 
	                                r.Title in ({0}) 
	                                and ps.ClientApplicationID = {1}";

            var sqlQuery = string.Format(sql, roleNames.Aggregate(string.Empty, (current, roleName) => (!string.IsNullOrEmpty(current)
                                                                                                    ? current + ',' + "'" + roleName + "'"
                                                                                                    : "'" + roleName + "'"))
                                    , clientAppTypeId);

            return sqlQuery;
        }

        public List<PermissionItem> GetPermissions(IEnumerable<string> roles, string conn, int clientApplicationType)
        {
            try
            {
                List<PermissionItem> permissions;
                using (var connection = new SqlConnection(conn))
                {
                    // Get all existing permissions for current client
                    permissions = GetAllPermissions(clientApplicationType);

                    // Get applied permissions for roles
                    using (var cmd = new SqlCommand(BuildGetRolesPermissionsSqlQuery(roles, clientApplicationType), connection))
                    {
                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var permissionName = ((string)reader[1]);
                                foreach (var n in permissions.Where(n => n.PermissionName == permissionName))
                                {
                                    n.IsEnabled = true;
                                }
                            }
                        }
                    }
                }

                return permissions;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        //DONE
        public void AddPermission(int permissionId, string roleName, string conn)
        {
            try
            {
                using (var connection = new SqlConnection(conn))
                {
                    using (var cmd = new SqlCommand("spAddPermission", connection) {CommandType = CommandType.StoredProcedure})
                    {
                        cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 100);
                        cmd.Parameters["@RoleName"].Value = roleName;
                        cmd.Parameters.Add("@PermissionID", SqlDbType.Int);
                        cmd.Parameters["@PermissionID"].Value = permissionId;

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


        //NOT DONE
        public void RemovePermission(int permissionId, string roleName, string conn)
        {
            try
            {
                using (var connection = new SqlConnection(conn))
                {
                    using (var cmd = new SqlCommand("spRemovePermission", connection) {CommandType = CommandType.StoredProcedure})
                    {
                        cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 100);
                        cmd.Parameters["@RoleName"].Value = roleName;
                        cmd.Parameters.Add("@PermissionID", SqlDbType.Int);
                        cmd.Parameters["@PermissionID"].Value = permissionId;

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
