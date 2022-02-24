using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Sensor
{
    public class DoorSensor : IOsensor
    {
        public DoorSensor(Guid ControllerId) : base(ControllerId)
        {
            Type = "Door";
        }
    }
}
