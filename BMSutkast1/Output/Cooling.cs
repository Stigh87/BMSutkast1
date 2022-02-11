using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1.Output
{
    public class Cooling : Output
    {
        public bool Interlock;
       // public bool OnOff;
        public Cooling(Guid ControllerId) : base(ControllerId)
        {
            Type = "Cooling";
            Interlock = false; // "Forigling mot heating" <-
           // OnOff = false;
        }
    }
}
