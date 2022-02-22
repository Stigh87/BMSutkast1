using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMSutkast1.Output;

namespace BMSutkast1.Sensor
{
    public class LuxSensor : ValueSensor
    {
        protected internal double ActualLux; //endres fra calender tid på døgnet.
        protected internal int OutdoorLux;
        protected internal double windowBlending;
        public LuxSensor(Guid ControllerId) : base(ControllerId)
        {
            Type = "LuxSensor";
            ActualLux = 0; 
            OutdoorLux = 0;
            windowBlending = 0.009;
        }

        public async Task SetUpdateOutdoorValue(int currentOutdoorLux, Light light)
        {
            OutdoorLux = currentOutdoorLux;
            UpdateActualLux(light);
        }

        public async Task UpdateActualLux(Light light)
        {
            ActualLux = (OutdoorLux * windowBlending) + (light.Value * 10);
        }

        public string GetLuxString()
        {
            var tempString = Math.Round(ActualLux).ToString();
            return tempString;
        }
    }
}
