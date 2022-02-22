using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1.Sensor
{
    public class TemperatureSensor : ValueSensor
    {
        protected double ActualTemperature; 
        protected internal double OutdoorTemperature { get; set; }
        public TemperatureSensor(Guid controllerId) : base(controllerId)
        {
            Type = "TempSensor";
            OutdoorTemperature = 0; 
            ActualTemperature = 15; 
            
        }

        public double GetTemp()
        {
            return Math.Round(ActualTemperature, 2);
        }

        public void AdjustTemperature(bool heater, bool cooler, bool vent)
        {
            var tempChange = heater ? +1.3 : cooler ? -1.3 : 0.0;
            tempChange += GetTemperatureChange(vent);
            ActualTemperature += tempChange;
        }

        protected internal double GetTemperatureChange(bool vent)
        {
            // simulering av K-verdi : viduer, dører, vegger tak mm.  PR TIME
            // Legg inn noe random ?
            var change = 0.0;
            var compare = ActualTemperature - OutdoorTemperature;
            var rnd = RandomSim(); //kun for simulering
            if (compare > 10) change = -0.6 - rnd;
            else if (compare > 5) change = -0.4 - rnd;
            else if (compare > 2) change = -0.2 - rnd;
            else if (compare < -10) change = 0.8 + rnd;
            else if (compare < -5) change = 0.6 + rnd;
            else if (compare < -2) change = 0.4 + rnd;
            if (vent) change -= 0.2;

            return change;
        }

        private double RandomSim()
        {
            var Random = new Random();
            return Random.NextDouble() * (0.6 - 0.1) + 0.1;
        }

        internal async Task SetUpdateOutdoorValue(double currentOutdoorTemperature)
        {
            OutdoorTemperature = currentOutdoorTemperature;
        }

        public double GetOutdoorTemp()
        {
            return OutdoorTemperature;
        }
    }
}
