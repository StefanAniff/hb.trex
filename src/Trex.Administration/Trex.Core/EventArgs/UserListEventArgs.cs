using System.Collections.Generic;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class UserListEventArgs : System.EventArgs
    {
        public UserListEventArgs(List<User> users)
        {
            if (users != null)
            {
                Users = users;
            }
            else
            {
                Users = new List<User>();
            }
        }

        public List<User> Users { get; set; }
    }
}