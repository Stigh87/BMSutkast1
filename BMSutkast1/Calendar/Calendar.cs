using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BMSutkast1
{
    public class Calendar
    {
        /*Sette opp kalender
         *  ukeplan?
         *  mndplan?
         *  dags syklus
         *  været? hhv temp og lys
         *
         * Sette opp "people" som random (rooms.count/2, rooms.count) -> "findAvaliableOffice()?" man-fredag 8-4
         *                                                                Kommer og går?() / simulere bevegelse i bygg
         *
         * Objekter i liste med: tid, lys/lux, temp som liste i et dags objekt?
         */
        private Day[] Week { get; }
        private Day CurrentDay;
        private int CurrentDayIndex;

        public Calendar()
        {
            Week = new Day[]
            {new ("Monday", 8, 16, 0, 1),
                new ("Tuesday", 8, 16,1, 3),
                new ("Wednesday", 8, 16,2, 4),
                new ("Thursday", 8, 16,3, 4),
                new ("Friday", 8, 16,4, 5),
                new ("Saturday", 0, 0,5, 5),
                new ("Sunday", 0, 0,6, 6),
            };
            CurrentDayIndex = 0;
            CurrentDay = Week[CurrentDayIndex];
        }

        public Day GetCurrentDay()
        {
            return CurrentDay;
        }
        public void DayChanger()
        {
            if (CurrentDayIndex == Week.Length -1) CurrentDayIndex = 0;
            else CurrentDayIndex++;
            CurrentDay = Week[CurrentDayIndex];
        }

        public async Task StartWeek()
        {
            var running = true;
            while (running)
            {
               // Console.WriteLine($"****{CurrentDay.GetDayName()}****");
                await CurrentDay.StartDay();
                DayChanger();
                await Task.Delay(15000);
                //Thread.Sleep(3500);
            }

        }

        public string GetCalendarInfo(string wantedInfo)
        {
            if (wantedInfo == "CurrentDay") return GetCurrentDay().GetDayName();
            if (wantedInfo == "currentTime") return CurrentDay.PrintCurrentHour();
            if (wantedInfo == "temperature") return CurrentDay.GetCurrentHour().GetTemp().ToString();
            if (wantedInfo == "lux") return CurrentDay.GetCurrentHour().GetLux().ToString();
            if (wantedInfo == "workingHours") return CurrentDay.PrintWorkingHours();
            return " ";
        }

    }


}
