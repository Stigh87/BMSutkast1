﻿namespace BMSutkast1
{
    internal class Cubicle : Room
    {
        public Cubicle(int area, int roomNr, int floorNr) : base(area, roomNr, floorNr)
        {
            RoomType = "Cubicle";
        }
    }
}