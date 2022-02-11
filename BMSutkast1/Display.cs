using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BMSutkast1
{
    public class Display
    {
        /*
         *Få inn hele building
         *  -lese ut dataverdier fra sensorer              
         *          Bygningsnivå
         *          Etaasjenivå
         *          Romnivå
         *
         *Justere "ønsket" verider
         *  -Sende ut verider til roomcontrol
         *
         *
         * eks allRooms() listes opp: ID, Navn/type, aktivt, ønsket temp, faktisk temp, heat/cool, ventilasjon, ønsket lux, faktisk lux, lysstyrke/av/på.
         *
         *
         */
        public Building MyBuilding = new Building();

        private void DisplayPrint()
        {
            Console.WriteLine();
        }

        public void MainMenu()
        {
            Console.WriteLine($"BUILDING MANAGEMENT SYSTEM - SIMULATOR");
            Console.WriteLine($"1. Building");
            Console.WriteLine($"2. Floors");
            Console.WriteLine($"3. Weather");
            Console.WriteLine($"4. Calendar");
            var command = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(command);
            if (command == 1) ;
            else if (command == 2) PrintFloorsMenu();
            else if (command == 3) ;
            else if (command == 4) ;
            else
            {
                Console.Clear();
                MainMenu();
            }
        }

        private void PrintFloorsMenu()
        {
            foreach (var floor in MyBuilding.Floors)
            {
                Console.WriteLine($"Floor: {floor.FloorNr} - State: {floor.State} - Rooms: {floor.Rooms.Count}");
            }

            Console.WriteLine($"Choose a floor number to access options. Press '0' to go back");
            var command = Convert.ToInt32(Console.ReadLine());

            if (command == 0)
            {
                Console.Clear();
                MainMenu();
            }
            else if (1 <= command && command <= 4)
            {
                var floorIndex = MyBuilding.Floors.FindIndex(x => x.FloorNr == command);
                PrintFloorMenu(floorIndex);
            }
            else
            {
                Console.Clear();
                PrintFloorsMenu();
            }
        }

        private async Task PrintFloorMenu(int floorIndex)
        {
            var i = 1;
            var floor = MyBuilding.Floors[floorIndex];
            Console.WriteLine($"Floor number: {floor.FloorNr} - State: {floor.State}");
            foreach (var room in floor.Rooms)
            {
                var c = room.Controller;
                string occupied = c.Motion.Open ? "YES" : "NO";
                Console.WriteLine(
                    $"Nr: {i} - Type: {room.RoomType}({room.Area}m2) - State: {c.State} - Temperature: {c.Temp.ActualTemperature}c/{c.SetTemperature}c - Occupied: {occupied} - Power consumption: ???Kw/h");
                i++;
            }

            Console.WriteLine($"1. Room options - 2. to change floor state(temp) - 0. to go back,");
            var command = Convert.ToInt32(Console.ReadLine());
            if (command == 0) PrintFloorsMenu();
            else if (command == 2) FloorStateChanger(floorIndex);
            else if (command == 1)
            {
                Console.WriteLine($"Choose a room number to access options.");
                command = Convert.ToInt32(Console.ReadLine());
                if (1 <= command && command <= floor.Rooms.Count) PrintRoomOptions(floorIndex, command);
            }
        }

        private async Task PrintRoomOptions(int floorIndex, int RoomNr)
        {
            var roomIndex = RoomNr - 1;
            var roomController = MyBuilding.Floors[floorIndex].Rooms[roomIndex].Controller;
            await roomController.Printer();
            Console.WriteLine($"1. Change state - 2. SetTemp - 3. SetLux - '0' to go back");
            var command = Convert.ToInt32(Console.ReadLine());
            if (command == 0) await PrintFloorMenu(floorIndex);
            if (command == 1) await RoomStateChanger(floorIndex, roomIndex);
            if (command == 2) ; //settemp aktuelt rom
            if (command == 3) ; //setLux aktuelt rom

        }

        private async Task RoomStateChanger(int floorIndex, int roomIndex)
        {
            var floor = MyBuilding.Floors[floorIndex];
            var room = floor.Rooms[roomIndex];
            Console.WriteLine();
            Console.WriteLine($"Room: {room.RoomType}{roomIndex+1} - Current State: {room.Controller.State}");
            Console.WriteLine(
                $"1. {Status.Awake} - 2. {Status.Standby} - 3. {Status.Sleep} - 4. {Status.Wakeup}, '0' to go back");
            var command = Convert.ToInt32(Console.ReadLine());
            var state = StateChanger(command);
            await RoomState(room, state);
            floor.State = state;
            await PrintRoomOptions(floorIndex, roomIndex +1);
        }

        private async Task RoomState(Room room, Status state)
        {
            room.Controller.State = state;
            await room.Controller.ChangeState();
        }

        private async Task FloorStateChanger(int floorIndex)
        {
            var floor = MyBuilding.Floors[floorIndex];
            Console.WriteLine($"Floor number: {floor.FloorNr} - Current State: {floor.State}");
            Console.WriteLine(
                $"1. {Status.Awake} - 2. {Status.Standby} - 3. {Status.Sleep} - 4. {Status.Wakeup}, '0' to go back");
            var command = Convert.ToInt32(Console.ReadLine());
            if (command == 0) await PrintFloorMenu(floorIndex);
            var state = StateChanger(command);
            floor.State = state;
            foreach (var room in floor.Rooms)
            {
                await RoomState(room, state);
            }
            await PrintFloorMenu(floorIndex);
        }

        private Status StateChanger(int command)
        {
            var state = command switch
            {
                1 => Status.Awake,
                2 => Status.Standby,
                3 => Status.Sleep,
                _ => Status.Wakeup
            };
            return state;
        }


    }
}


