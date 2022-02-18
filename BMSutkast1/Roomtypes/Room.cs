using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using BMSutkast1.Sensor;

namespace BMSutkast1
{
    public class Room
    {
        private int Area; //areal sendes til romkontroller? avgjøre oppvarming og evt strømforbruk
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
            RoomNr = roomNr;  //CreateRoomNr(roomNr);
            Controller = new RoomController(RoomType, RoomId, Area);
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

        //sycklus() få inn:
        //                  outdoortemp +/- inndoortemp i en while loop ->heat() ->cool()
        //                  outdoor lux? +/- dimming - (N, S, Ø, V ?)
        //      ELLER SELVE ROOM.SYCKLUS I EN WHILE?

        // noen "steg" i temp fra metode- det blir fortere kaldt / varmt med høye/lave temp
        
    }

}