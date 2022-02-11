using System;

namespace BMSutkast1
{
    public static class Display
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
        internal Building Building;

        public Display(Building building)
        {
            Building = building;
        }

        public void DisplayPrint()
        {
            
        }

        public void MainMenu()
        {

            Console.WriteLine($"");
        }
    }
}