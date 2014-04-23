using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class XmlPermissionService : IPermissionService
    {
        #region IPermissionService Members

        public List<string> GetPermissionsForRoles(List<string> roles, string permissionFilePath)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(permissionFilePath);

            var roleNodes = xmlDoc.SelectNodes("//roles/role");

            var permissionList = new List<string>();
            foreach (XmlNode roleNode in roleNodes)
            {
                var role = roleNode.Attributes["name"].Value;

                foreach (var userRole in roles)
                {
                    if (userRole.Equals(role))
                    {
                        var permissions = xmlDoc.SelectNodes("//role[@name='" + role.Trim() + "']/permissions/permission");

                        if (permissions != null && permissions.Count > 0)
                        {
                            foreach (XmlNode permission in permissions)
                            {
                                var permissionValue = permission.Attributes["name"].Value;
                                if (permissionList.SingleOrDefault(r => r.Equals(permissionValue)) == null)
                                {
                                    permissionList.Add(permissionValue);
                                }
                            }
                        }
                    }
                }
            }
            return permissionList;
        }

        public List<string> GetRoles(string permissionFilePath)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(permissionFilePath);

            var roleNodes = xmlDoc.SelectNodes("//role");
            var returnList = new List<string>();

            foreach (XmlNode roleNode in roleNodes)
            {
                returnList.Add(roleNode.Attributes["name"].Value);
            }
            return returnList;
        }

        #endregion
    }
}