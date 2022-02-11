using System.Collections.Generic;

namespace BMSutkast1
{
    public class Floor
    {
        public int FloorNr;

        //public Status FloorState;
        public List<Room> Rooms;
        private int OfficeCount;
        private int CubicleCount;
        private int MingleCount;
        public Status State;
        public Floor(int floorNr)
        {
            State = Status.Sleep;
            FloorNr = floorNr;
            
            //FloorState = Status.Sleep; ////skal arves (reversert?) fra room, og sende til building
            Rooms = new List<Room>();

            OfficeCount = 10; //fåes som parameter i new floor?
            CubicleCount = 5; // fåes som parameter i new floor?
            MingleCount = 2; // fåes som parameter i new floor?
            CreateOffices(OfficeCount);
            CreateCubicles(CubicleCount);
            CreateMingle(MingleCount);
            CreateReseption();

        }

        private void CreateReseption()
        {
            if (FloorNr == 1) Rooms.Add(new Reception(10, 1, FloorNr));
        }


        private void CreateOffices(int officeCount)
        {
            for (int i = 0; i < officeCount; i++)
            {
                Rooms.Add(new Office(6, i, FloorNr)); //6m2 minimum
            }
        }
        private void CreateCubicles(int cubicleCount)
        {
            for (int i = 0; i < cubicleCount; i++)
            {
                Rooms.Add(new Cubicle(50, i, FloorNr));
            }
        }
        private void CreateMingle(int mingleCount)
        {
            for (int i = 0; i < mingleCount; i++)
            {
                Rooms.Add(new Mingle(30, i, FloorNr));
            }
        }
    }
}