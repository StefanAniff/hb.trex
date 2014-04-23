using System;
using System.Collections.ObjectModel;
using System.Linq;
using Trex.Core.Services;
using Trex.ServiceContracts;

namespace Trex.Infrastructure.Implemented
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataService _dataService;
        private readonly IExceptionHandlerService _exceptionHandlerService;

        public UserRepository(IDataService dataService, IExceptionHandlerService exceptionHandlerService)
        {
            _dataService = dataService;
            _exceptionHandlerService = exceptionHandlerService;
        }

        public ObservableCollection<User> Users { get; private set; }


        public event EventHandler DataLoaded;

        public bool IsDataLoaded { get; private set; }

        public User GetById(int userId)
        {
            return Users.SingleOrDefault(u => u.UserID == userId);
        }

        public void Initialize()
        {
            _dataService.GetAllUsers().Subscribe(users =>
                                                     {
                                                         Users = users;
                                                         OnDataLoaded();
                                                     }
                 
                                                 , _exceptionHandlerService.OnError
                );
        }

        private void OnDataLoaded()
        {
            if (DataLoaded != null)
            {
                DataLoaded(this, null);
            }
            IsDataLoaded = true;
        }
    }
}
