using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        #region IUserRepository Members

        /// <summary>
        /// Gets the user by the specified username
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The found user</returns>
        /// <exception cref="UserNotFoundException"></exception>
        public User GetByUserName(string userName)
        {
            var session = GetSession();
            var user = from users in session.Linq<User>()
                       where users.UserName == userName
                       select users;

            if (user.Count() == 0)
            {
                throw new UserNotFoundException("The user " + userName + " was not found in the User table");
            }

            return user.Single();
        }

        /// <summary>
        /// Gets the user by id
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public User GetByUserID(int userID)
        {
            var session = GetSession();
            var user = session.Get<User>(userID);
            if (user == null)
            {
                throw new UserNotFoundException("The user was not found in the User table");
            }

            return user;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public IList<User> GetAllUsers()
        {
            var session = GetSession();

            var allUsers = from users in session.Linq<User>()
                           select users;

            return allUsers.ToList();
        }

        /// <summary>
        /// Saves the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Save(User user)
        {
            var session = GetSession();

            session.Save(user);
            session.Flush();
        }

        /// <summary>
        /// Creates the specified user
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public User Create(string userName, string name, string email, double price)
        {
            if (UserExists(userName))
            {
                throw new UserAlreadyExistsException(string.Format("Cannot create user. User with username {0} already exists", userName));
            }

            var session = GetSession();
            var user = new User(userName, name, email, price);

            session.Save(user);
            session.Flush();
            return user;
        }

        public void Update(User user)
        {
            var session = GetSession();
            session.Update(user);
            session.Flush();
        }

        public void Delete(User user)
        {
            var session = GetSession();
            foreach (var customerInfo in user.CustomerInfo)
            {
                DeleteCustomerInfo(customerInfo);
            }

            session.Delete(user);
            session.Flush();
        }

        public void DeleteCustomerInfo(UserCustomerInfo customerInfo)
        {
            var session = GetSession();

            var queryString = "delete from UserCustomerInfo where UserId = :userId and CustomerId = :customerId";

            var query = session.CreateQuery(queryString);
            query.SetInt32("userId", customerInfo.UserId);
            query.SetInt32("customerId", customerInfo.CustomerId);
            query.ExecuteUpdate();
        }

        /// <summary>
        /// Checks if a given user the exists.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool UserExists(string userName)
        {
            var session = GetSession();
            var user = from users in session.Linq<User>()
                       where users.UserName == userName
                       select users;

            return (user.ToList().Count > 0);
        }

        #endregion
    }
}