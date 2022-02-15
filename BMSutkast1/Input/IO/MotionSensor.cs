using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BMSutkast1.Sensor
{
    public class MotionSensor : IOsensor
    {
        public int Timer;
        public MotionSensor(Guid controllerId) : base(controllerId)
        {
            Type = "Motion";
            Timer = 15; //? "min" ?
        }

        //public void DetectMotion(){
        // Trenger tilgang til Status->Romcontroller
        // set status - start timer - changestate
        //
        //}
        public bool MotionOpen()
        {
            return Open;
        }
    }
}
