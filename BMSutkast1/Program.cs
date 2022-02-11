using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace BMSutkast1
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            var building = new Building();
            
            //var room = new Office(6,1,1);
            //room.PrintRoomInfo();
            //room.ChangeState(Status.Awake);
            //Console.ReadLine();
            // room.ChangeState(Status.Standby);
            //Console.ReadLine();
            // room.ChangeState(Status.Sleep);
/*
            var office = new Office(6,1,1);
            var mingle = new Mingle(40, 1, 1);
            var resep = new Reception(10, 1, 1);
            var cube = new Cubicle(25, 1, 1);

            office.Controller.State = Status.Awake;
            office.Controller.ChangeState();
            Thread.Sleep(7000);
            office.Controller.State = Status.Sleep;
            office.Controller.ChangeState();
 */
            foreach (var floor in building.Floors)
            {
                
                Console.WriteLine(@$"***Floor: {floor.FloorNr} - Rooms: {floor.Rooms.Count}***");
               
                foreach (var room in floor.Rooms)
                {
                    room.Controller.State = Status.Awake;
                    room.Controller.ChangeState();
                    Console.WriteLine($"RomNr: {room.RoomNr} - Type: {room.RoomType}");
                    Console.WriteLine($"State: {room.Controller.State} - Actual temp: {room.Controller.Temp.ActualTemperature}");
                    room.Controller.State = Status.Standby;
                    room.Controller.ChangeState();
                }
            }
            foreach (var floor in building.Floors)
            {
                Console.WriteLine($"***Floor: {floor.FloorNr} - Rooms: {floor.Rooms.Count}***");

                foreach (var room in floor.Rooms)
                {
                    Console.WriteLine($"RomNr: {room.RoomNr} - Type: {room.RoomType}");
                    Console.WriteLine($"State: {room.Controller.State} - Actual temp: {room.Controller.Temp.ActualTemperature}");
                    
                }
            }

            //Await
            //Public/protected/internal
            //refactoring? / mer oppdeling

        }

    }
}
