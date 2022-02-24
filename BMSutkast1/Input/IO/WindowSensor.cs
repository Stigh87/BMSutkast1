using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Sensor
{
    public class WindowSensor : IOsensor
    {
        public WindowSensor(Guid ControllerId) : base(ControllerId)
        {
            Type = "Window";
        }
    }
}
