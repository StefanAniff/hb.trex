using System;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class LoginSettings:ILoginSettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CustomerId { get; set; }
        public bool PersistLogin { get; set;}
        public int UserId { get; set; }
        public DateTime CreateDate{get;set;}
        public string UserFullName { get; set; }

    }
}