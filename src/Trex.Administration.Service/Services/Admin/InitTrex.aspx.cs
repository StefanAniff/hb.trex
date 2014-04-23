using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using StructureMap;
using Trex.Server.Core.Services;

namespace TrexSL.Web.Admin
{
    public partial class InitTrex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var permissionService = ObjectFactory.GetInstance<IPermissionService>();


            var permissionFile = ConfigurationManager.AppSettings["clientPermissionConfigFile"];

            permissionFile = HttpContext.Current.Server.MapPath(permissionFile);
            var roles = permissionService.GetRoles(permissionFile);

            foreach (var role in roles)
            {
                if (!Roles.RoleExists(role))
                    Roles.CreateRole(role);
            }
            if (!Page.IsPostBack)
                BindRoles();
        }

        private void BindRoles()
        {
            foreach (var role in Roles.GetAllRoles())
            {
                roleList.Items.Add(new ListItem(role));

            }

        }

        protected void CreateUserClick(object sender, EventArgs e)
        {
            var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
            var newUser = new Trex.Server.Core.Model.User()
            {
                UserName = txtUserName.Text,
                Name = txtFullName.Text,
                Email = txtEmail.Text,

            };

            if (string.IsNullOrEmpty(roleList.SelectedValue))
            {
                status.Text = "Select a role";
                return;
            }

            var userCreationResponse = userManagementService.CreateUser(newUser, txtPassword.Text, "test", "test");

            if (userCreationResponse.Success)
                Roles.AddUserToRole(txtUserName.Text, roleList.SelectedValue);
            status.Text = userCreationResponse.Response;



        }
    }
}