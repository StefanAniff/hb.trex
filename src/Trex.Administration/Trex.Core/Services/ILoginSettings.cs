using System;

namespace Trex.Core.Services
{
    public interface ILoginSettings
    {
        DateTime CreateDate { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string CustomerId { get; set; }
        bool PersistLogin { get; set; }
    }
}