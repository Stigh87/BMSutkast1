using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Sensor
{
    public class ValueSensor : Sensor
    {
        protected double ActualValue;
        protected double SetValue;
        public ValueSensor(Guid ControllerId) : base(ControllerId)
        {
            ActualValue = 0.0;
            SetValue = 0.0;
        }


        
    }
}
