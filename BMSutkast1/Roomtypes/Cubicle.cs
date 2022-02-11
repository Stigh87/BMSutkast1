namespace BMSutkast1
{
    internal class Cubicle : Room
    {
        public Cubicle(int area, int roomIndex, int floorNr) : base(area, roomIndex, floorNr)
        {
            RoomType = "Cubicle";
        }
    }
}