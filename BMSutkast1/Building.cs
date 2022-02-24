using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS
{
    public class Building
    {
        private readonly List<Floor> _floors;
        private readonly Calendar Calendar;
        public Status State { get; set; }

        public int PowerConsumption;

        public Building()
        {
            State = Status.Sleep; 
            Calendar = new Calendar();
            _floors = new List<Floor>
            {
                new(1),
                new(2),
                new(3)
            };
           
        }
        public void PrintFloorsOverview()
        {
            PrintCalendarOverview();
            foreach (var floor in _floors)
            {
                PrintFloorInfo(floor);
            }
        }
        public int GetFloorCount()
        {
            return _floors.Count;
        }
        public Floor GetFloor(int command)
        {
            var floorIndex = _floors.FindIndex(x => x.FloorNr == command);
            return _floors[floorIndex];
        }
        public async Task ChangeState(Status state)
        {
            State = state;
            foreach (var floor in _floors)
            {
                //if(State == Status.Awake) await floor.ChangeState(Status.Awake);
                await floor.ChangeState(State);
            }
        }
        public void PrintBuildingOverview()
        {
            var totalRoomCount = _floors.Sum(floor => floor.GetRoomCount());
            PrintCalendarOverview();
            Console.WriteLine(@$"
                    MyBuilding - State: {State} - Floors: {_floors.Count} - Rooms: {totalRoomCount}");
                    
        }
        public void PrintFloorInfo(Floor floor)
        {
            floor.PrintFloorInfo();
        }
        public void PrintCalendarOverview()
        {
            Console.WriteLine(@$"
                    Day: {Calendar.GetCalendarInfo("CurrentDay")} - Time: {Calendar.GetCalendarInfo("currentTime")} - Working Hours: {Calendar.GetCalendarInfo("workingHours")}
                    Outdoortemp: {Calendar.GetCalendarInfo("temperature")}c - Outdoor Lux: {Calendar.GetCalendarInfo("lux")}");
        }
        public async Task StartWeekSimulation()
        {
            Calendar.StartWeek(_floors);
            // StartBuildingCycle();
            await SendUpdateOutdoorValues();
            
        }
        private async Task StartBuildingCycle()
        {
            while (true)
            {
                var currentHour = Calendar.GetCurrentDay().GetCurrentHour();
                var workDay = Calendar.GetCurrentDay().GetWorkTime("Start") > 0;
                var state = Status.Sleep;
                if (EmptyBuilding()) state = Status.Awake;
                else if (!EmptyBuilding() && workDay)
                {
                    if (currentHour.Value >= 6) state = Status.Wakeup;
                    if (currentHour.Value >= 16) state = Status.Sleep;
                }
                else if (!workDay) state = Status.Sleep;
                ChangeState(state);
                await Task.Delay(Timer.CheckDelay);
            }
        } //MÅ SJEKKE "BLOKK" OG OVERSTYRING I ROMCONTROLLER
        private bool EmptyBuilding()
        {
            var check = false;
            foreach (var floor in _floors)
            {
                foreach (var room in floor.GetRooms())
                {
                    if (room.GetController().CheckOccupied()) check = true;
                }
            }
            return check;
        }
        private async Task SendUpdateOutdoorValues()
        {
            while (true)
            {
                foreach (var floor in _floors)
                {
                    floor.SetUpdateOutdoorValues(GetOutdoorTemperature(), GetOutdoorLux());
                }
                await Task.Delay(Timer.HourDelay);
            }
        }
        public double GetOutdoorTemperature()
        {
            var OutdoorTemp = Calendar.GetCurrentDay().GetCurrentHour().GetTemp();
            return OutdoorTemp;
        }
        public int GetOutdoorLux()
        {
            var OutdoorLux = Calendar.GetCurrentDay().GetCurrentHour().GetLux();
            return OutdoorLux;
        }
        public Calendar GetCalendar()
        {
            return Calendar;
        }
    }
}
