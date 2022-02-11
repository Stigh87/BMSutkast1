using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1
{
    internal class Reception : Mingle
    {
        public Reception(int area, int roomIndex, int floorNr) : base(area, roomIndex, floorNr)
        {
            RoomType = "Reseption";
        }
    }
}
