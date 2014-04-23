using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Services
{
    public interface IConnectionContext
    {
        string ConnectionString { get; set; }
    }
}
