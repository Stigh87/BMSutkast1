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
        public Status State;

        public int PowerConsumption;

        public Building()
        {
            State = Status.Sleep; //skal arves (reversert) fra floor
            _floors = new List<Floor>
            {
                new(1),
                new(2),
                new(3)
            };
        }
        public void PrintFloorsOverview()
        {
            foreach (var floor in _floors)
            {
                floor.PrintFloorInfo();
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
                if(State == Status.Awake) await floor.ChangeState(Status.Wakeup);
                else await floor.ChangeState(State);
            }
        }
    }
}
