using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1.Sensor
{
    public class Co2Sensor : ValueSensor
    {
        public Co2Sensor(Guid ControllerId) : base(ControllerId)
        {
            Type = "Co2Sensor";
        }
    }
}
