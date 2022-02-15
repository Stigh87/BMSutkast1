using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMSutkast1
{
    public class Floor
    {
        public int FloorNr { get; }

        //public Status FloorState;
        private readonly List<Room> _rooms;
        private readonly int _officeCount;
        private readonly int _cubicleCount;
        private readonly int _mingleCount;
        public Status State;
        protected internal Floor(int floorNr)
        {
            State = Status.Sleep;
            FloorNr = floorNr;
            
            //FloorState = Status.Sleep; ////skal arves (reversert?) fra room, og sende til building
            _rooms = new List<Room>();

            _officeCount = 10; //fåes som parameter i new floor?
            _cubicleCount = 5; // fåes som parameter i new floor?
            _mingleCount = 2; // fåes som parameter i new floor?
            CreateOffices(_officeCount);
            CreateCubicles(_cubicleCount);
            CreateMingle(_mingleCount);
            CreateReception();

        }
        private void CreateOffices(int officeCount)
        {
            for (int i = 0; i < officeCount; i++)
            {
                _rooms.Add(new Office(6, _rooms.Count+1, FloorNr)); //6m2 minimum
            }
        }
        private void CreateCubicles(int cubicleCount)
        {
            for (int i = 0; i < cubicleCount; i++)
            {
                _rooms.Add(new Cubicle(50, _rooms.Count+1, FloorNr));
            }
        }
        private void CreateMingle(int mingleCount)
        {
            for (int i = 0; i < mingleCount; i++)
            {
                _rooms.Add(new Mingle(30, _rooms.Count+1, FloorNr));
            }
        }
        private void CreateReception()
        {
            if (FloorNr == 1) _rooms.Add(new Reception(10, _rooms.Count + 1, FloorNr));
        }

        public void PrintFloorInfo()
        {
            Console.WriteLine($"Floor: {FloorNr} - State: {State} - Rooms: {_rooms.Count}");
        }

        public void PrintRoomOverview()
        {
            foreach (var room in _rooms)
            {
                room.PrintRoomInfo();
            }
        }
        public int GetRoomCount()
        {
            return _rooms.Count;
        }

        public Room GetRoom(int roomNr)
        {
            return _rooms.Find(x => x.RoomNr == roomNr);
        }

        public async Task ChangeState(Status state)
        {
            State = state;
            foreach (var room in _rooms)
            {
                if (State == Status.Sleep) await room.ChangeState(State);
                else await room.ChangeState(Status.Standby);  //Hindre at alle rom blir satt til awake - må gjøres manuelt
            }
        }
    }
}