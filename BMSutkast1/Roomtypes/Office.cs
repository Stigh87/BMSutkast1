namespace BMSutkast1
{
    internal class Office : Room
    {
        public Office(int area, int roomIndex, int floorNr) : base(area, roomIndex, floorNr)
        {
            RoomType = "Office";
        }
    }
}