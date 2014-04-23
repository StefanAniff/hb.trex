using System;
using Trex.Core.Services;

namespace Trex.Infrastructure.Implemented
{
    public class LoginSettings : ILoginSettings
    {
        #region ILoginSettings Members

        public string UserName { get; set; }
        public string Password { get; set; }
        public string CustomerId { get; set; }
        public bool PersistLogin { get; set; }
        public DateTime CreateDate { get; set; }

        #endregion
    }
}