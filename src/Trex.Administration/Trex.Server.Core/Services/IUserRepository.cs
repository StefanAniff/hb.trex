using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IUserRepository
    {
        User GetByUserName(string userName);
        User GetByUserID(int userID);
        IList<User> GetAllUsers();
        void Save(User user);
        User Create(string userName, string name, string email, double price);
        void Update(User user);
        void Delete(User user);
        bool UserExists(string userName);
        void DeleteCustomerInfo(UserCustomerInfo userCustomerInfo);
    }
}