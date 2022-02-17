using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BMSutkast1.Input;

namespace BMSutkast1
{
    public class Day
    {
        private string Name { get; }
        private readonly Hour[] Hours;
        private int CurrentHourIndex;
        private Hour CurrentHour;
        private int WorkHourStart;
        private int WorkHourEnd;
        private int DayIndex;
        private WeatherStation _weatherstation;

        public Day(string name, int workStart, int workEnd, int dayIndex, int startTemp)
        {
            _weatherstation = new WeatherStation(startTemp);
            Name = name;
            DayIndex = dayIndex;
            WorkHourStart = workStart;
            WorkHourEnd = workEnd;
            CurrentHourIndex = 0;
            Hours = new Hour[24];
            FillHourArray();
            CurrentHour = Hours[CurrentHourIndex];
           
        }

        private void FillHourArray()
        {
            for (int i = 0; i < Hours.Length; i++)
            {
                //value = time, getLux(), getTemp() <-- fra weatherfoorecast?
                Hours[i] = new Hour(i, CheckWorkTime(i), getForecast(i)); //<----------TESTVERDIER
            }
        }

        private Forecast getForecast(int time)
        {
            return _weatherstation._forecasts[time];
        }


        private bool CheckWorkTime(int time)
        {
            if (WorkHourEnd == 0 && WorkHourStart == 0) return false;
            return time >= WorkHourStart && time <= WorkHourEnd;
        }

        public Hour GetCurrentHour()
        {
            return Hours[CurrentHourIndex];
        }

        public void HourChanger()
        {
           if (CurrentHourIndex == Hours.Length -1) CurrentHourIndex = 0;
           else CurrentHourIndex++;
           CurrentHour = Hours[CurrentHourIndex];
        }

        public string GetDayName()
        {
            return Name;
        }
        public int GetHourValue()
        {
            return Hours[CurrentHourIndex].Value;
        }

        public async Task StartDay()
        {
            foreach (var hour in Hours)
            {
                Console.WriteLine($"{hour.Value} - Lux: {hour.GetLux()} - Temp: {hour.GetTemp()}");
                HourChanger();
                //Task.Delay(1000);
                Thread.Sleep(500);
            }
        }

    }
}
