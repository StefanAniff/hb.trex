using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trex.Server.Core.Model;
using Task = Trex.Server.Core.Model.Task;
using TaskFactory = Trex.Server.Infrastructure.Implemented.Factories.TaskFactory;

namespace TRex.Services.Test.Nhibernate
{
    public static class DataGenerator
    {
        public static Task GetTask(Project _project, User _user)
        {
            var _taskFactory = new TaskFactory();
            var newTask = _taskFactory.Create(Guid.NewGuid(),
                                              DateTime.Now,
                                              null,
                                              "dbtest_CanSaveNewTask",
                                              string.Empty,
                                              _user,
                                              _project,
                                              null,
                                              null,
                                              0,
                                              0,
                                              0,
                                              0,
                                              9);
            return newTask;
        }
        public static Project GetProject(User user, Company company)
        {
            return new Project("testproject", company, user, DateTime.Now, null, false, true);
        }
        public static Company GetCustomer(User createdBy)
        {
            return new Company("testCustomer", DateTime.Now, null, createdBy, null, null, null, null, null, true, 1, true, null);
        }
    
        public static User GetUser()
        {
            var user = new User("testuser", "test", "test", 0);
            //var userCustomerInfo = UserCustomerInfo();
            //user.AddCustomerInfo(userCustomerInfo);
            return user;
        }

        //public static UserCustomerInfo UserCustomerInfo()
        //{
        //    var userCustomerInfo = new UserCustomerInfo();
        //    return userCustomerInfo;
        //}
        public static TimeEntry GetTimeEntry(Task task, User user, TimeEntryType timeEntryType)
        {
            return new TimeEntry(Guid.NewGuid(), DateTime.Now, DateTime.Now, timeEntryType, string.Empty, 0, 0, 0, true, 0, task, user, 0);
        }
        public static TimeEntryType GetTimeEntryType(Company company)
        {
         return   new TimeEntryType(company, null, true, true);
        }
    }
}
