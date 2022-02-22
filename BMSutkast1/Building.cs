using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSutkast1
{
    public class Building
    {
        private readonly List<Floor> _floors;
        private readonly Calendar Calendar;
        public Status State;

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
                //legge inn : fra kalender -> workhours? eller ikke.
                if(State == Status.Awake) await floor.ChangeState(Status.Wakeup, Calendar); //varme opp etasje for etasje? 1-count
                else await floor.ChangeState(State, Calendar);
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
            var DayDelay = 15000;   //trekkes ut i egen klasse for styring av delays i simulator?
            var HourDelay = 15000;   //trekkes ut i egen klasse for styring av delays i simulator?
            Calendar.StartWeek(DayDelay, HourDelay);
            await SendUpdateOutdoorValues(HourDelay);
        }
        private async Task SendUpdateOutdoorValues(int hourDelay)
        {
            
            while (true)
            {
                foreach (var floor in _floors)
                {
                    floor.SetUpdateOutdoorValues(GetOutdoorTemperature(), GetOutdoorLux(), hourDelay);
                }
                await Task.Delay(hourDelay);
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
