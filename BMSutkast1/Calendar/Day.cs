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
        protected int WorkHourStart;
        protected int WorkHourEnd;
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
                Hours[i] = new Hour(i, CheckWorkTime(i), getForecast(i)); 
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

        public async Task StartDay(int hourDelay)
        {
            foreach (var hour in Hours)
            {
                HourChanger();
                await Task.Delay(hourDelay);
            }
        }

        internal string ConvertToString(int time)
        {
            var timeString = time.ToString();
            var pad = 2 - timeString.Length;
            return timeString.PadLeft(2, '0');
        }

        internal string PrintWorkingHours()
        {
            var hourStart = WorkHourStart;
            var hourEnd = WorkHourEnd;
            var hourString = ConvertToString(hourStart) +":00" + " - " + ConvertToString(hourEnd) + ":00";
            return hourString;
        }
        internal string PrintCurrentHour()
        {
            var hour = GetCurrentHour().Value;
            var hourString = ConvertToString(hour);
            return hourString + ":00";
        }

        public void PrintDayOverview()
        {
            var linebreak = 0;
            foreach (var hour in Hours)
            {
                //linjeskifte pr 6
                Console.Write($"{ConvertToString(hour.Value).PadLeft(2, ' ')}:00 {hour.GetTemp().ToString().PadLeft(2, ' ')}c ||  ");
                linebreak++;
                if (linebreak == 6) {
                    Console.WriteLine();
                    linebreak = 0;
                }
            }
            
        }
    }
}
