using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1.Sensor
{
    public class ButtonPanel : ValueSensor
    {
        public ButtonPanel(Guid ControllerId) : base(ControllerId)
        {
            Type = "Button";
        }
    }
}
