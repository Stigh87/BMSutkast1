using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BMS.Sensor
{
    public class MotionSensor : IOsensor
    {
        public int Timer;
        private bool Open;
        public MotionSensor(Guid controllerId) : base(controllerId)
        {
            Type = "Motion";
            Timer = 15; //? "min" ?
            Open = true;
        }
        public void DetectMotion()
        {
            Open = false;
            StartTimer();
        }
        public void TimeOut()
        {
            Open = true;
        }
        private void StartTimer()
        {
            //eventuell timer funksjon for åpning av kontakt.
        }
    }
}
