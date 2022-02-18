using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BMSutkast1
{
    public class Display
    {
        /*          TO DO'S:
            Endre set temp etasjer
            Lage værmelding view
            
            LAGE ALKORYTME FOR NEHANDLING AV TEMPERATUR OG LYS PÅVIRKNING

        */
        public Building MyBuilding = new Building();

        private async Task DisplayPrint(Calendar cal)
        {
            
        }
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
            var calendar = MyBuilding.Calendar; //funksjon getCalendar()?
            MyBuilding.StartWeek();
            Console.WriteLine($"BUILDING MANAGEMENT SYSTEM - SIMULATOR \n 1. Building \n 2. Floors \n 3. Weather \n 4. Calendar");
            var command = GetInput();
            if (command == 1) PrintBuildingMenu();
            else if (command == 2) await PrintFloorsMenu();
            else if (command == 3) ;
            else if (command == 4) await DisplayPrint(calendar);
            else
            {
                await MainMenu();
            }
        }

        private void PrintBuildingMenu()
        {
            MyBuilding.PrintBuildingOverview();
            Console.WriteLine($"\n 1. Floor options - 2. Change building state - 0. To go back,");
            var command = GetInput();
            if (command == 0) MainMenu();
            if (command == 1) PrintFloorsMenu();
            if (command == 2)
            {
                var state = StateChanger(MyBuilding.State);
                MyBuilding.ChangeState(state);
                PrintBuildingMenu();   //While loop heller?
            }
        }

        private async Task PrintFloorsMenu()
        {
            while (true)
            {
                MyBuilding.PrintFloorsOverview();
                Console.WriteLine($"\n Choose a floor number to access its options. Press '0' to go back");
                var command = GetInput();

                if (command == 0) await MainMenu();
                else if (1 <= command && command <= MyBuilding.GetFloorCount())
                {
                    var floor = MyBuilding.GetFloor(command);
                    await PrintFloorMenu(floor);
                }
                else continue;
                break;
            }
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
                MyBuilding.PrintCalendarOverview();
                floor.PrintRoomOverview();
                Console.WriteLine($"\n Choose a room number to access options (1 - {floor.GetRoomCount()}) - 0. to go back");
                command = GetInput();
                if (1 <= command && command <= floor.GetRoomCount()) await PrintRoomOptions(floor, command);
                else await PrintFloorMenu(floor);
            }
            else if (command == 3) ChangeSetValue(floor);
        }

        

        private async Task PrintRoomOptions(Floor floor, int roomNr)
        {
            var room = floor.GetRoom(roomNr);
            var controller = room.GetController();
            MyBuilding.PrintCalendarOverview();
            MyBuilding.PrintFloorInfo(floor);
            await controller.PrintRoomInfo();
            Console.WriteLine($"\n 1. Change state - 2. SetTemp - 3. SetLux - 4. Refresh View- '0' to go back");
            var command = GetInput();
            if (command == 0) await PrintFloorMenu(floor);
            if (command == 1) await ChangeRoomState(floor, room);
            if (command == 2) ChangeSetValue("Temp", room); //settemp aktuelt rom
            if (command == 3) ChangeSetValue("Lux", room); //setLux aktuelt rom
            //if (command == 4) PrintRoomOptions(floor, roomNr);
            PrintRoomOptions(floor, roomNr); //Legge en while-loop rundt heller?
        }
        private async Task ChangeRoomState(Floor floor, Room room)
        {
            var state = StateChanger(room.GetRoomState());
            if (state == Status.Awake)
            {
                MyBuilding.ChangeState(state);
                floor.ChangeState(state);
            }
            await room.ChangeState(state);
            await PrintRoomOptions(floor, room.RoomNr);
        }

        private void ChangeSetValue(string type, Room room)
        {
            room.PrintRoomInfo();
            Console.WriteLine($"\n 1. Set new {type} value - '0' to go back");
            var value = int.Parse(Console.ReadLine());
            Clear();
            var controller = room.GetController();
            controller.ChangeSetValue(type, value);
           
            // <--------------------------Fortsett her + evt. sjekke StateChanger (Rom>etasje>bygg? eller andre veien?)
        }
        private void ChangeSetValue(Floor floor)
        {
            floor.PrintFloorInfo();
            Console.WriteLine($"\n 1. Set new temperature value - '0' to go back");
        }

        private async Task FloorStateChanger(Floor floor)
        {
            MyBuilding.PrintFloorInfo(floor);
            var state = StateChanger(floor.State);
            await floor.ChangeState(state);
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


