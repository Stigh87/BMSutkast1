namespace BMS
{
    internal class Office : Room
    {
        public Office(int area, int roomNr, int floorNr) : base(area, roomNr, floorNr)
        {
            RoomType = "Office";
        }
    }
}