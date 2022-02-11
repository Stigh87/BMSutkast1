using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1.Sensor
{
    public class ValueSensor : Sensor
    {
        public double ActualValue;
        public double SetValue;
        public ValueSensor(Guid ControllerId) : base(ControllerId)
        {
            ActualValue = 0.0;
            SetValue = 0.0;
        }


        public void ChangeValue(double newValue)
        {
            SetValue = newValue;
            //send "beskjed" til romcontroller ??
        }
    }
}
