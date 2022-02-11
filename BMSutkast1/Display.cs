using System;
using System.Threading.Channels;

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
            else {Console.Clear(); MainMenu();}
        }

        private void PrintFloorsMenu()
        {
            foreach (var floor in MyBuilding.Floors)
            {
                Console.WriteLine($"Floor: {floor.FloorNr} - State: {floor.State} - Rooms: {floor.Rooms.Count}");
            }

            Console.WriteLine($"Choose a floor number to access options. Press '0' to go back");
            var command = Convert.ToInt32(Console.ReadLine());
          
            if (command == 0) {Console.Clear(); MainMenu(); }
            else if (1 <= command && command <= 4) PrintFloorMenu(command);
            else { Console.Clear(); PrintFloorsMenu(); }
        }

        private void PrintFloorMenu(int floorNr)
        {
            var foundI = MyBuilding.Floors.FindIndex(x => x.FloorNr == floorNr);
            var i = 1;
            var floor = MyBuilding.Floors[foundI];
            Console.WriteLine($"Floor number: {floor.FloorNr} - State: {floor.State}");
            foreach (var room in floor.Rooms)
            {
                var c = room.Controller;
                string occupied = c.Motion.Open ? "YES" : "NO";
                Console.WriteLine($"Nr: {i} - Type: {room.RoomType}({room.Area}m2) - State: {c.State} - Temperature: {c.Temp.ActualTemperature}c/{c.SetTemperature}c - Occupied: {occupied} - Power consumption: ???Kw/h");
                i++;
            }
            Console.WriteLine($"Choose a room number to access options. Press 'S' to change floor state(temp), '0' to go back,");
            var command = Convert.ToInt32(Console.ReadLine());
            if (command == 0) PrintFloorsMenu();
            else if (command.ToString() == "S") StateChanger(floorNr);
            else if (1 <= command && command <= MyBuilding.Floors[foundI].Rooms.Count) PrintRoomOptions(floorNr, command);

        }

        private void PrintRoomOptions(int floorNr, int command)
        {
            Console.Clear();
            var foundRoomI = MyBuilding.Floors[floorNr].Rooms.FindIndex(x => x.RoomNr == command);
        }

        private void StateChanger(int floorI)
        {
            var floor = MyBuilding.Floors[floorI];
            Console.WriteLine($"Floor number: {floor.FloorNr} - State: {floor.State}");
            Console.WriteLine($"1. {Status.Awake} - 2. {Status.Standby} - 3. {Status.Sleep} - 4. {Status.Wakeup}");
        }
    }
}