using System.Linq;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public static class PermissionHelper
    {
        public static bool HasPermission(this User user, Permissions permission)
        {
            if (user != null)
                return user.Permissions.SingleOrDefault(p => p.Equals(permission.ToString())) != null;
            return true;
        }
    }
}
