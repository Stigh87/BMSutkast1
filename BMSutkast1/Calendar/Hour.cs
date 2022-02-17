using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMSutkast1.Input;

namespace BMSutkast1
{
    public class Hour
    {
        internal int Value;
        private double OutdoorTemp;
        private int OutdoorLux;
        private bool WorkHour;
        private Forecast _forecast;
        public Hour(int value, bool workHour, Forecast forecast)
        {
            _forecast = forecast;
            WorkHour = workHour;
            Value = value;
            OutdoorLux = GetLux(); //skal sendes/lages en faktor for og taes med i Luxsensorer
            OutdoorTemp = GetTemp(); //skal sendes/lages en faktor for og taes med i tempsensorer
        }

        public double GetTemp()
        {
            return _forecast.GetTemp();
        }

        public int GetLux()
        {
            return _forecast.GetLux();
        }
    }
}
