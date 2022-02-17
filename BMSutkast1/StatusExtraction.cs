using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1
{
    static class StatusExtraction
    {
        public static void PrintStatusOptions(Status state)
        {
            Console.WriteLine($"Current state: {state} \n 1. {Status.Awake} - 2. {Status.Standby} - 3. {Status.Sleep} - 4. {Status.Wakeup}, '0' to go back");
        }
    }
}
