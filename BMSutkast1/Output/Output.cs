using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1.Output
{
    public class Output
    {
        protected Guid _id { get; }
        //public int RoomNr;
        public Guid RoomControllerId;
        public string Type;

        public bool On;

        public Output(Guid ControllerId)
        {
            RoomControllerId = ControllerId;
            _id = Guid.NewGuid();
            //RoomNr = roomId;
            On = false;
        }

        //Fellesfunksjon false->true og motsatt();
    }
}
