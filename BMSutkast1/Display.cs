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

        private async Task DisplayPrint()
        {
            var calendar = MyBuilding.Calendar; //funksjon getCalendar()?
            await calendar.StartWeek();

        }
        private static int GetInput()
        {
           // var input = Convert.ToInt32(Console.ReadKey(true).KeyChar);
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
            Console.WriteLine($"BUILDING MANAGEMENT SYSTEM - SIMULATOR");
            Console.WriteLine($"1. Building");
            Console.WriteLine($"2. Floors");
            Console.WriteLine($"3. Weather");
            Console.WriteLine($"4. Calendar");
            var command = GetInput();
            if (command == 1) PrintBuildingMenu();
            else if (command == 2) await PrintFloorsMenu();
            else if (command == 3) ;
            else if (command == 4) DisplayPrint();
            else
            {
                MainMenu();
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
                Console.WriteLine($"Choose a floor number to access its options. Press '0' to go back");
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
            floor.PrintFloorInfo();
            Console.WriteLine($"1. Room options - 2. change floor state - 3. Set temperature - 0. to go back,");
            var command = GetInput();
            if (command == 0) await PrintFloorsMenu();
            else if (command == 2) await FloorStateChanger(floor);
            else if (command == 1)
            {
                floor.PrintRoomOverview();
                Console.WriteLine($"Choose a room number to access options (1 - {floor.GetRoomCount()}) - 0. to go back");
                command = GetInput();
                if (1 <= command && command <= floor.GetRoomCount()) await PrintRoomOptions(floor, command);
                else await PrintFloorMenu(floor);
            }
        }

        private async Task PrintRoomOptions(Floor floor, int roomNr)
        {
            var room = floor.GetRoom(roomNr);
            var controller = room.GetController();
            floor.PrintFloorInfo();
            await controller.PrintRoomInfo();
            Console.WriteLine($"\n 1. Change state - 2. SetTemp - 3. SetLux - 4. Refresh View- '0' to go back");
            var command = GetInput();
            if (command == 0) await PrintFloorMenu(floor);
            if (command == 1) await ChangeRoomState(floor, room);
            if (command == 2) ChangeRoomSetValue("Temp", room); //settemp aktuelt rom
            if (command == 3) ChangeRoomSetValue("Lux", room); //setLux aktuelt rom
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

        private void ChangeRoomSetValue(string type, Room room)
        {
            room.PrintRoomInfo();
            Console.WriteLine($"\n 1. Set new value - '0' to go back");
            var value = int.Parse(Console.ReadLine());
            var controller = room.GetController();
            controller.ChangeSetValue(type, value);
           
            // <--------------------------Fortsett her + evt. sjekke StateChanger (Rom>etasje>bygg? eller andre veien?)
        }

        private async Task FloorStateChanger(Floor floor)
        {
            floor.PrintFloorInfo();
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


