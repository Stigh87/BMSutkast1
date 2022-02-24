using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BMS.Input;

namespace BMS
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

        //hjelpevariabler:
        private int _dayStarted;

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
            _dayStarted = 0;
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
        public bool CheckWorkTime(int time)
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
        public async Task StartDay(List<Floor> floors)
        {
            foreach (var hour in Hours)
            {
                if (hour.Value > 0 && (hour.Value + 2) == WorkHourStart) ChangeFloorState(floors, Status.Standby);
                if (hour.Value > 0 && hour.Value == WorkHourStart) ChangeFloorState(floors, Status.Awake);
                if (hour.Value >= WorkHourStart && hour.Value <= WorkHourEnd) PopulateRooms(floors, true);
                if (hour.Value >= WorkHourEnd) PopulateRooms(floors, false);
                if (hour.Value is >= 16 and < 18) ChangeFloorState(floors, Status.Standby);
                if (hour.Value >= 18) ChangeFloorState(floors, Status.Sleep);
                await Task.Delay(Timer.HourDelay);
                HourChanger();
            }
        }
        private void ChangeFloorState(List<Floor> floors, Status state)
        {
            bool? occupied = null;
            foreach (var floor in floors)
            {
                occupied = floor.CheckOccupied();
                if (state is Status.Standby or Status.Sleep && occupied == true) return;
                floor.ChangeState(state);
            }
        }
        private void PopulateRooms(List<Floor> floors, bool populate)
        {
            foreach (var floor in floors)
            {
                foreach (var room in floor.GetRooms())
                {
                    var random = new Random().Next(0, 6);
                    if (populate && random >= 2 && !room.GetController().CheckOccupied()) room.Populate();
                    if (!populate && random > 1) room.GoHome();
                }
            }
        }
        internal int GetWorkTime(string time)
        {
            return time == "Start" ? WorkHourStart : WorkHourEnd;
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
