using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Interfaces
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}
