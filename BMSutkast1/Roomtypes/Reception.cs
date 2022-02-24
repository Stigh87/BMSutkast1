using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS
{
    internal class Reception : Mingle
    {
        public Reception(int area, int roomNr, int floorNr) : base(area, roomNr, floorNr)
        {
            RoomType = "Reseption";
        }
    }
}
