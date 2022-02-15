using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1
{
    internal class Reception : Mingle
    {
        public Reception(int area, int roomNr, int floorNr) : base(area, roomNr, floorNr)
        {
            RoomType = "Reseption";
        }
    }
}
