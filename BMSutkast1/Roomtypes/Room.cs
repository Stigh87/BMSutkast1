using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using BMS.Sensor;

namespace BMS
{
    public class Room
    {
        private int Area;
        private Guid RoomId;
        protected internal int RoomNr { get; }
        protected string RoomType { get; set; }
        private int _floorNr;

        //  ROOMCONTROLLER:
        protected RoomController Controller;
        public Room(int area, int roomNr, int floorNr)
        {
            RoomId = new Guid();
            _floorNr = floorNr;
            Area = area;
            RoomNr = roomNr;
            Controller = new RoomController(RoomType, RoomId);
        }
        public void PrintRoomInfo()
        {
            var roomtypeAndArea = Convert.ToString($"{RoomType}({Area}m2)");
            Console.WriteLine($"Nr: {RoomNr.ToString().PadLeft(2, ' ')}  Type: {roomtypeAndArea.PadRight(15, ' ')}  {Controller.PrintControllerInfo()}");
        }
        public RoomController GetController()
        {
            return Controller;
        }
        public Status GetRoomState()
        {
            return Controller.State;
        }
        public async Task ChangeState(Status state)
        {
            Controller.State = state;
            await Controller.ChangeState();
        }
        internal async Task SetUpdateOutdoorValues(double currentOutdoorTemperature, int currentOutdoorLux)
        {
            await Controller.SetUpdateOutdoorValues(currentOutdoorTemperature, currentOutdoorLux);
        }
        public void ChangeSetValue(string temp, int value)
        {
            Controller.ChangeSetValue(temp, value);
        }
        public void Populate()
        {
            Controller.Populate();
        }
        public void GoHome()
        {
            Controller.GoHome();
        }
    }

}