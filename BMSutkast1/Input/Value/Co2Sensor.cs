using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Sensor
{
    public class Co2Sensor : ValueSensor
    {
        public Co2Sensor(Guid ControllerId) : base(ControllerId)
        {
            Type = "Co2Sensor";
        }
    }
}
