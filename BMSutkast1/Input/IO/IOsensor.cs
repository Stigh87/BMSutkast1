using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Sensor
{
    public class IOsensor : Sensor
    {
        public bool Open;


        public IOsensor(Guid ControllerId) : base(ControllerId)
        {
            Open = false;
        }

        public void Trigger()
        {
            Open = true;
        }
    
    }
}
