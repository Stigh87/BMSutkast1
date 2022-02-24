using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BMS.Sensor
{
    public class Sensor
    {
        public Guid _id { get; }
       // public int RoomNr;
        public Guid RoomControllerId;
        public string Type;

        public Sensor(Guid ControllerId)
        {
            RoomControllerId = ControllerId;
            _id = Guid.NewGuid();
            //RoomNr = roomId;
        }


    }
}
