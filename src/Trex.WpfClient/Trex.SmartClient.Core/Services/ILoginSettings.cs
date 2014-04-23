using System;

namespace Trex.SmartClient.Core.Services
{
    public interface ILoginSettings
    {
        DateTime CreateDate { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        bool PersistLogin { get; set; }
        string CustomerId { get; set; }
        int UserId { get; set; }
        string UserFullName { get; set; }
        
    }
}