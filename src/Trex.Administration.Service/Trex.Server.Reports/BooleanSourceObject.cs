using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Reports
{
    

    public class BooleanSourceObject
    {

        private bool [] _values = new bool[] {true,false};
        public BooleanSourceObject()
        {
            
        }

        public bool[] Values { get { return _values; } }
    }
}
