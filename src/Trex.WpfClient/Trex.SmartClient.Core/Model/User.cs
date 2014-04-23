using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Core.Model
{
    public class User 
    {

        private User(){}

        public string UserName { get; set; }
        public string Name { get; set; }

        public int Id { get; set; }
        public string Password { get; set; }
        public List<string > Roles { get; set; }
        public List<string > Permissions { get; set; }
        

        public static User Create(string userName,string name, int id, List<string> roles, List<string> permissions)
        {
            return new User() {Id = id, 
                Name = name, 
                UserName = userName,
                Permissions = permissions,
                Roles = roles
            };
        }
    }
}
