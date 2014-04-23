using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Trex.Server.Core.Services;
using Trex.ServiceContracts;
using TrexSL.Web.Exceptions;

namespace Trex.Server.Infrastructure.Implemented
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly ITrexContextProvider _contextProvider;
        private readonly IMembershipService _membershipService;

        public RoleManagementService(ITrexContextProvider contextProvider, IMembershipService membershipService)
        {
            _contextProvider = contextProvider;
            _membershipService = membershipService;
        }


        public List<Role> GetAllRoles()
        {
            return _contextProvider.TrexEntityContext.Roles.ToList();
        }

        public void CreateRole(string roleName)
        {
            if (!Roles.RoleExists(roleName))
            {
                _membershipService.CreateRole(roleName);
                var context = _contextProvider.TrexEntityContext;
                context.Roles.AddObject(new Role() {Title = roleName});

                context.SaveChanges();
            }
            else
            {
                throw new RoleException("Role name is already in use");
            }
        }



        public void DeleteRole(string roleName)
        {
            if (!Roles.RoleExists(roleName))
                throw new RoleException("Role does not exist");

            if (Roles.GetUsersInRole(roleName).Any())
                throw new RoleException("Unable to delete role because it has users assigned to it");


            _membershipService.DeleteRole(roleName);
            var context = _contextProvider.TrexEntityContext;
            var role = context.Roles.SingleOrDefault(r => r.Title == roleName);
            context.Roles.Attach(role);
            context.Roles.DeleteObject(role);
            context.SaveChanges();

        }






    }

}
