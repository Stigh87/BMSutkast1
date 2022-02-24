using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Output
{
    public class Heating : Output
    {
        internal bool Interlock;
        internal bool On;
        public Heating(Guid ControllerId) : base(ControllerId)
        {
            Type = "Heater";
            Interlock = false; 
            On = false;  
        }
    }
}
