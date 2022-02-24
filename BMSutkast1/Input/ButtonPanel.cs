using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Sensor
{
    public class ButtonPanel : ValueSensor
    {
        public ButtonPanel(Guid ControllerId) : base(ControllerId)
        {
            Type = "Button";
        }
    }
}
