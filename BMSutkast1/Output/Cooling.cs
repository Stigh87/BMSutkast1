using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1.Output
{
    public class Cooling : Output
    {
        internal bool Interlock;
        internal bool On;
        public Cooling(Guid ControllerId) : base(ControllerId)
        {
            Type = "Cooling";
            Interlock = false;
            On = false;
        }
    }
}
