using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.ServiceContracts;


namespace Trex.Server.Infrastructure.Implemented
{
    public class PermissionService : IPermissionService
    {
        private readonly ITrexBaseContextProvider _baseContextProvider;
        private readonly ITrexContextProvider _trexContext;


        public PermissionService(ITrexBaseContextProvider contextProvider, ITrexContextProvider trexContext)
        {
            _baseContextProvider = contextProvider;
            _trexContext = trexContext;
        }


        public List<UserPermission> GetAllPermissions(int clientApplicationId)
        {

            return _baseContextProvider.TrexBaseEntityContext.Permissions.Where(p => p.ClientApplicationID == clientApplicationId).
                Select(p => new UserPermission() { Permission = p.Permission1, PermissionID = p.ID }).ToList();

        }


        public List<UserPermission> GetPermissionsForRoles(List<string> roles, int clientApplicationType)
        {

            var returnList = new Dictionary<int, UserPermission>();

            foreach (var role in roles)
            {
                var permissions = _trexContext.TrexEntityContext.GetUserPermission(role, clientApplicationType);

                foreach (var userPermission in permissions)
                {
                    if (!returnList.ContainsKey(userPermission.PermissionID))
                        returnList.Add(userPermission.PermissionID, userPermission);
                }
            }

            return returnList.Values.ToList();

        }

        public void UpdatePermissionsForRole(List<UserPermission> permissions, Role role, int clientId)
        {

            var context = _trexContext.TrexEntityContext;


            var rolePermissions = GetPermissionsForRoles(new List<string>() { role.Title }, clientId);

            var allRolePermissions = context.PermissionsInRoles.Where(p => p.RoleID == role.ID).ToList();

            foreach (var permissionsInRole in allRolePermissions)
            {
                if (rolePermissions.SingleOrDefault(r => r.PermissionID == permissionsInRole.RoleID) != null)
                {
                    context.PermissionsInRoles.Attach(permissionsInRole);
                    context.PermissionsInRoles.DeleteObject(permissionsInRole);
                }


            }
            context.SaveChanges();

            foreach (var userPermission in permissions)
            {
                var newPermission = context.PermissionsInRoles.CreateObject();
                newPermission.RoleID = role.ID;
                newPermission.PermissionID = userPermission.PermissionID;

                 context.PermissionsInRoles.AddObject(newPermission);
            }
            context.SaveChanges();


        }

        //DONE
        public void AddPermission(int permissionId, string roleName)
        {
            //var permissionList = new List<string>();
            //SqlConnection connection = null;

            //try
            //{
            //    connection = new SqlConnection(_connectionString);

            //    var cmd = new SqlCommand("spAddPermission", connection);
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 100);
            //    cmd.Parameters["@RoleName"].Value = roleName;
            //    cmd.Parameters.Add("@PermissionID", SqlDbType.Int);
            //    cmd.Parameters["@PermissionID"].Value = permissionId;

            //    connection.Open();

            //    cmd.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    OnError(ex);
            //}
            //finally
            //{
            //    if (connection != null) connection.Close();
            //}

        }


        //NOT DONE
        public void RemovePermission(int permissionId, string roleName)
        {
            //var permissionList = new List<string>();
            //SqlConnection connection = null;

            //try
            //{
            //    connection = new SqlConnection(_connectionString);

            //    var cmd = new SqlCommand("spRemovePermission", connection);
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 100);
            //    cmd.Parameters["@RoleName"].Value = roleName;
            //    cmd.Parameters.Add("@PermissionID", SqlDbType.Int);
            //    cmd.Parameters["@PermissionID"].Value = permissionId;

            //    connection.Open();

            //    cmd.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    OnError(ex);
            //}
            //finally
            //{
            //    if (connection != null) connection.Close();
            //}
        }

        //private void OnError(Exception ex)
        //{
        //    Logger.LogError();g(ex);
        //}
    }
}
