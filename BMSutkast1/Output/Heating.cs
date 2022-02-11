using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1.Output
{
    public class Heating : Output
    {
        public bool Interlock;
        //public bool OnOff;
        public Heating(Guid ControllerId) : base(ControllerId)
        {
            Type = "Heater";
            //fåes fra romcontroller - eller bare av/på "signal"?
            Interlock = false; // "Forigling mot cooling" <-
           // OnOff = false;
        }
    }
}
