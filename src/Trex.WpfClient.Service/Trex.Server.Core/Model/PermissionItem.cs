namespace Trex.Server.Core.Model
{
    public class PermissionItem
    {

        public PermissionItem() { }

        public PermissionItem(int id, string permissionName, bool isEnabled)
        {
            Id = id;
            PermissionName = permissionName;
            IsEnabled = isEnabled;
        }

        public virtual int Id { get; set; }

        public virtual string PermissionName { get; set; }

        public virtual bool IsEnabled { get; set; }
    }
}