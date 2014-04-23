using System;
using System.Collections.ObjectModel;
using Trex.ServiceContracts;

namespace Trex.Core.Services
{
    public interface IUserRepository
    {
        ObservableCollection<User> Users { get; }
        event EventHandler DataLoaded;
        bool IsDataLoaded { get; }
        User GetById(int userId);
        void Initialize();
        
    }
}
