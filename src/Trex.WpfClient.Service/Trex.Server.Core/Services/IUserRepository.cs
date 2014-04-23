using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUserName(string userName);
        User GetByUserID(int userID);

        /// <summary>
        /// Gets all users where inactive is false
        /// </summary>
        /// <returns></returns>
        List<User> GetActiveUsers();        
    }
}
