using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMSutkast1.Sensor;
using static BMSutkast1.Sensor.LuxSensor;

namespace BMSutkast1.Output
{
    public class Light : Output
    {
        public int Value;
        public Light(Guid ControllerId) : base(ControllerId)
        {
            Type = "Light";
            Value = 0; // 0-100% utifra lux Actual/Set
        }

       
    }
}
