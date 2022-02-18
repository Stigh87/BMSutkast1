using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1.Sensor
{
    public class TemperaturSensor : ValueSensor
    {
        protected double ActualTemperature; //lage en while loop async Task som kjører 0,1++ 0,1-- avhengig av utendørstemp?
        public TemperaturSensor(Guid controllerId) : base(controllerId)
        {
            Type = "TempSensor";
            ActualTemperature = 15; //startverdi ved new();
        }

        public double GetTemp()
        {
            return Math.Round(ActualTemperature, 2);
        }

        public void AdjustTemperature(bool heatInterlock, bool coolInterlock)
        {   
            var tempChange = heatInterlock && !coolInterlock ? -0.3 : coolInterlock && !heatInterlock ? +0.3 : -0.1; //-0.1 om utendørs temp < actual / +0.1 om motsatt?
            
            ActualTemperature += tempChange;
        }
    }
}
