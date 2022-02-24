using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Output
{
    public class Ventilation : Output
    {
        public Ventilation(Guid ControllerId) : base(ControllerId)
        {
            Type = "Ventilation";
        }
    }
}
