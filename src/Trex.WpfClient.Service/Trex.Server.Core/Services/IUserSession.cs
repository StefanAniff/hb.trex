using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IUserSession
    {
        bool ValidateCredentials(string username, string password);
    }
}
