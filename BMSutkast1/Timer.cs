using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS
{
    public static class Timer
    {
        public static int DayDelay;
        public static int HourDelay;
        public static int CheckDelay;

        static Timer()
        {
            DayDelay = 5000;
            HourDelay = 4000; //endres i presentasjon (5000 -> + 00)
            CheckDelay = 1000; 
        }
    }
}
