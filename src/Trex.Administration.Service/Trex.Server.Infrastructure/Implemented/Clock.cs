using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Server.Core.Interfaces;

namespace Trex.Server.Infrastructure.Implemented
{
    public class Clock : IClock 
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}
