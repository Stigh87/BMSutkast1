using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMSutkast1.Sensor;

namespace BMSutkast1.Input
{
    public class WeatherStation
    {
        private double Temp;
        private double StartTemp;
        public int Lux;
        internal Forecast[] _forecasts;

        public WeatherStation(double Starttemp)
        {
            Temp = StartTemp = Starttemp;
            Lux = 1;
            _forecasts = new Forecast[24];
            AddForecasts();
        }

        private void AddForecasts()
        {
            for (var i = 0; i < _forecasts.Length; i++)
            {
                if (i is >= 0 and < 8) Night();
                if (i is >= 8 and < 11) Sunrise();
                if (i is >= 11 and < 16) MidDay();
                if (i is >= 16 and < 21) Sunset();
                if (i is >= 21 and < 24) Night();
                _forecasts[i] = new Forecast(Temp, Lux);
            }
        }
        private void Night()
        {
            Temp = StartTemp;
            Lux = 1;
        }

        private void Sunset()
        {
            Temp -= 2;
            Lux -= 10000; 
        }
        private void MidDay()
        {
            
        }
        private void Sunrise()
        {
            Temp = Temp > 0 ? Temp += 4 : Temp = 1;
            Lux += 30000;
        }
    }
}
