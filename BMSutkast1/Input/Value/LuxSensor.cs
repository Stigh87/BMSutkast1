using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1.Sensor
{
    public class LuxSensor : ValueSensor
    {
        public double ActualLux; //endres fra calender tid på døgnet.
        public LuxSensor(Guid ControllerId) : base(ControllerId)
        {
            Type = "LuxSensor";
            ActualLux = 0; //startverdi
        }
    }
}
