using System.Collections.Generic;
using System.Linq;
using Trex.Server.Core.Model;
using NHibernate;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ISession openSession)
            : base(openSession)
        {
        }

        public UserRepository(IActiveSessionManager activeSessionManager)
            : base(activeSessionManager)
        {
        }

        /// <summary>
        /// Gets the user by the specified username
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The found user</returns>
        /// <exception cref="UserNotFoundException"></exception>
        public User GetByUserName(string userName)
        {
            ISession session = OpenSession ?? ActiveSessionManager.GetActiveSession();
            var user = session.QueryOver<User>().Where(users => users.UserName == userName).List();

            if (!user.Any())
                throw new UserNotFoundException("The user " + userName + " was not found in the User table");


            return user.Single();

        }

        /// <summary>
        /// Gets the user by id
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public User GetByUserID(int userID)
        {
            ISession session = OpenSession ?? ActiveSessionManager.GetActiveSession();
            User user = session.Get<User>(userID);
            if (user == null)
                throw new UserNotFoundException("The user was not found in the User table");

            return user;
        }

        /// <summary>
        /// Gets all users where inactive is false
        /// </summary>
        /// <returns></returns>
        public List<User> GetActiveUsers()
        {
            return Session
                .QueryOver<User>()
                .Where(x => !x.Inactive)
                .List<User>()
                .ToList();
        }        
    }
}
