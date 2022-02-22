using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BMSutkast1
{
    public class Display
    {
        /*          TO DO'S:
            BONUS: Lage simulering av folk i bygget?
        */
        public Building MyBuilding = new ();

        private static int GetInput()
        {
            var input = int.Parse(Console.ReadKey(true).KeyChar.ToString());
            Clear();
            return input;
        }
        private static void Clear()
        {
            Console.Clear();
        }
        public async Task MainMenu()
        {
            var calendar = MyBuilding.GetCalendar();
            MyBuilding.StartWeekSimulation();
            Console.WriteLine($"BUILDING MANAGEMENT SYSTEM - SIMULATOR \n 1. Building \n 2. Floors \n 3. Calendar&Weather \n");
            var command = GetInput();
            if (command == 1) PrintBuildingMenu();
            else if (command == 2) await PrintFloorsMenu();
            else if (command == 3) await PrintWeekAndWeather(calendar);
            else await MainMenu();

        }

        private async Task PrintWeekAndWeather(Calendar calendar)
        {
            MyBuilding.PrintCalendarOverview();
            calendar.PrintWeekOverview();
            Console.WriteLine($"\n 4. Refresh view - 0. To go back");
            var command = GetInput();
            if (command == 4) PrintWeekAndWeather(calendar);
            else MainMenu();
        }

        private void PrintBuildingMenu()
        {
            MyBuilding.PrintBuildingOverview();
            Console.WriteLine($"\n 1. Floor options - 2. Change building state - 4. Refresh view - 0. To go back");
            var command = GetInput();
            if (command == 0) MainMenu();
            if (command == 1) PrintFloorsMenu();
            if (command == 2)
            {
                MyBuilding.PrintBuildingOverview();
                var state = StateChanger(MyBuilding.State);
                MyBuilding.ChangeState(state);
                PrintBuildingMenu();
            }
            else PrintBuildingMenu();
        }

        private async Task PrintFloorsMenu()
        {
                MyBuilding.PrintFloorsOverview();
                Console.WriteLine($"\n Choose a floor number to access its options. - 4. Refresh view - 0. To go back");
                var command = GetInput();

                if (command == 0) await MainMenu();
                if (command == 4) PrintFloorsMenu();
                else if (1 <= command && command <= MyBuilding.GetFloorCount())
                {
                    var floor = MyBuilding.GetFloor(command);
                    await PrintFloorMenu(floor);
                }
                else PrintFloorsMenu();
        }

        private async Task PrintFloorMenu(Floor floor)
        {
            MyBuilding.PrintCalendarOverview();
            MyBuilding.PrintFloorInfo(floor);
            Console.WriteLine($"\n 1. Room options - 2. change floor state - 3. Set temperature - 0. to go back,");
            var command = GetInput();
            if (command == 0) await PrintFloorsMenu();
            else if (command == 2) await FloorStateChanger(floor);
            else if (command == 1)
            {
                while (true)
                {
                    MyBuilding.PrintCalendarOverview();
                    floor.PrintRoomOverview();
                    Console.WriteLine($"\n Choose a room number to access options (1 - {floor.GetRoomCount()}) - 0. to go back");
                    command = GetInput();
                    if (command == 4) continue;
                    if (1 <= command && command <= floor.GetRoomCount()) await PrintRoomOptions(floor, command);
                    else if (command == 0) await PrintFloorMenu(floor);
                }
            }
            else if (command == 3) ChangeSetValue(floor);
            else PrintFloorMenu(floor);

        }

        private async Task PrintRoomOptions(Floor floor, int roomNr)
        {
            var room = floor.GetRoom(roomNr);
            MyBuilding.PrintCalendarOverview();
            MyBuilding.PrintFloorInfo(floor);
            await room.GetController().PrintRoomInfo();
            Console.WriteLine($"\n 1. Change state - 2. SetTemp - 3. SetLux - 4. Refresh View - '0' to go back");
            var command = GetInput();
            if (command == 0) await PrintFloorMenu(floor);
            if (command == 1) await ChangeRoomState(floor, room);
            if (command == 2) ChangeSetValue("Temp", room, floor, roomNr); 
            if (command == 3) ChangeSetValue("Lux", room, floor, roomNr);
            PrintRoomOptions(floor, roomNr);
        }
        private async Task ChangeRoomState(Floor floor, Room room)
        {
            MyBuilding.PrintCalendarOverview();
            floor.PrintFloorInfo();
            room.GetController().PrintRoomInfo();
            var state = StateChanger(room.GetRoomState());
            if (state == Status.Awake)
            {
                MyBuilding.ChangeState(state);
            }
            await room.ChangeState(state);
            await PrintRoomOptions(floor, room.RoomNr);
        }

        private void ChangeSetValue(string type, Room room, Floor floor, int roomNr)
        {
            MyBuilding.PrintCalendarOverview(); 
            room.GetController().PrintRoomInfo();
            var valueRange = type == "Lux" ? "(Max 1000)" : "(15-28c)";
            Console.WriteLine($"\n 1. Set new {type} value {valueRange}");
            var value = int.Parse(Console.ReadLine());
            Clear();
            room.ChangeSetValue(type, value);
        }
        private void ChangeSetValue(Floor floor)
        {
            MyBuilding.PrintCalendarOverview();
            floor.PrintFloorInfo();
            Console.WriteLine($"\n 1. Set new temperature value (15-28c) - 0. to go back");
            var command = int.Parse(Console.ReadLine());
            Clear();
            if (command != 0) floor.ChangeSetTemp(command);
            PrintFloorMenu(floor);
        }

        private async Task FloorStateChanger(Floor floor)
        {
            MyBuilding.PrintCalendarOverview();
            MyBuilding.PrintFloorInfo(floor);
            var state = StateChanger(floor.State);
            await floor.ChangeState(state, MyBuilding.GetCalendar());
            await PrintFloorMenu(floor);
        }

        private Status StateChanger(Status state)
        {
            StatusExtraction.PrintStatusOptions(state);
            var command = GetInput();
            state = command switch
            {
                1 => Status.Awake,
                2 => Status.Standby,
                3 => Status.Sleep,
                4 => Status.Wakeup,
                _ => state
            };
            return state;
        }
    }
}


