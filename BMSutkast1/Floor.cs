using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMS
{
    public class Floor
    {
        public int FloorNr { get; }

        private readonly List<Room> _rooms;
        public Status State;
        protected internal Floor(int floorNr)
        {
            State = Status.Sleep;
            FloorNr = floorNr;
            _rooms = new List<Room>();
            const int officeCount = 10; //Hjelpevariabler for oppretting av rom <-- legges i constructor om en ønsker annen str. bygg
            const int cubicleCount = 5;
            const int mingleCount = 2;
            CreateOffices(officeCount);
            CreateCubicles(cubicleCount);
            CreateMingle(mingleCount);
            CreateReception();
        }
        private void CreateOffices(int officeCount)
        {
            for (var i = 0; i < officeCount; i++)
            {
                _rooms.Add(new Office(6, _rooms.Count + 1, FloorNr)); //6m2 minimum
            }
        }
        private void CreateCubicles(int cubicleCount)
        {
            for (int i = 0; i < cubicleCount; i++)
            {
                _rooms.Add(new Cubicle(50, _rooms.Count + 1, FloorNr));
            }
        }
        private void CreateMingle(int mingleCount)
        {
            for (int i = 0; i < mingleCount; i++)
            {
                _rooms.Add(new Mingle(30, _rooms.Count + 1, FloorNr));
            }
        }
        private void CreateReception()
        {
            if (FloorNr == 1) _rooms.Add(new Reception(10, _rooms.Count + 1, FloorNr));
        }
        public void PrintFloorInfo()
        {
            //avg temp? 

            Console.WriteLine(@$"
                    Floor number: {FloorNr} - Current State: {State} - Rooms: {_rooms.Count}");
        }
        private object GetAvgTemp()
        {
            //rooms -> controller -> current temp / count
            throw new NotImplementedException();
        }
        public void PrintRoomOverview()
        {
            Console.WriteLine("\n");
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
                if (state == Status.Awake) room.ChangeState(Status.Standby);
                else room.ChangeState(state);
            }
        }
        public int GetFloorNr()
        {
            return FloorNr;
        }
        public async Task SetUpdateOutdoorValues(double currentOutdoorTemperature, int currentOutdoorLux)
        {
            foreach (var room in _rooms)
            {
                await room.SetUpdateOutdoorValues(currentOutdoorTemperature, currentOutdoorLux);
                await Task.Delay(Timer.HourDelay);
            }
        }
        public void ChangeSetTemp(int value)
        {
            foreach (var room in _rooms)
            {
                room.ChangeSetValue("Temp", value);
            }
        }
        public List<Room> GetRooms()
        {
            return _rooms;
        }

        public bool? CheckOccupied()
        {
            var occupied = false;
            foreach (var room in _rooms)
            {
                if (room.GetController().CheckOccupied())
                {
                    return occupied = true;
                } 
            }
            return occupied;
        }
    }
}