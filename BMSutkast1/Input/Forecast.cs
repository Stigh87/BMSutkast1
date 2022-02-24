using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BMS.Input.WeatherStation;

namespace BMS.Input
{
    public class Forecast
    {
        private double OutdoorTemp;
        private int OutdoorLux;

        public Forecast(double temp, int lux)
        {
            OutdoorTemp = temp;
            OutdoorLux = lux;
        }

        public double GetTemp()
        {
            return OutdoorTemp;
        }
        public int GetLux()
        {
            return OutdoorLux;
        }
    }
}
