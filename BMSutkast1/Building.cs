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

        public List<Floor> Floors;
        public Display Display;
        public Status State;

        public int PowerConsumption;

        public Building()
        {

            State = Status.Sleep; //skal arves (reversert) fra floor
            Floors = new List<Floor>
            {
                new Floor(1),
                new Floor(2),
                new Floor(3)
            };
        }

        public void WakeBuilding()
        {
            State = Status.Wakeup;
            foreach (var floor in Floors)
            {
                floor.State = Status.Wakeup;
            }
        }
    }
    

    
}
