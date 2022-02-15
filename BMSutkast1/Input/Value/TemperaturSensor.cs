using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1.Sensor
{
    public class TemperaturSensor : ValueSensor
    {
        //public double SetTemperature; //max 22 min 16 <- Lagt i romcontroller
        
        public double ActualTemperature; //lage en while loop async Task som kjører 0,1++ 0,1-- avhengig av utendørstemp?
        public TemperaturSensor(Guid controllerId) : base(controllerId)
        {
            Type = "TempSensor";
            ActualTemperature = 15; //startverdi
        }

        public double GetTemp()
        {
            return ActualTemperature;
        }
    }
}
