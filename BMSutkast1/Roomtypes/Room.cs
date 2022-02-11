using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using BMSutkast1.Sensor;

namespace BMSutkast1
{
    public class Room
    {
        private readonly int Area; //areal sendes til romkontroller? avgjøre oppvarming og evt strømforbruk
        public Guid RoomId;
        public int RoomNr;
        public string RoomType;
        public int FloorNr;

        //  ROOMCONTROLLER:
        public RoomController Controller;

        public Room(int area, int roomIndex, int floorNr)
        {
            RoomId = new Guid();
            FloorNr = floorNr;
            Area = area;
            RoomNr = CreateRoomNr(roomIndex);
            Controller = new RoomController(RoomType, RoomId, Area);
        }

        private int CreateRoomNr(int roomIndex)
        {
            var floorNr = FloorNr.ToString();
            var id = roomIndex.ToString();
            var roomNr = Convert.ToInt32(floorNr + "0" + id);
            return roomNr;
        }

        //sycklus() få inn:
        //                  outdoortemp +/- inndoortemp i en while loop ->heat() cool()
        //                  outdoor lux? +/- dimming - (N, S, Ø, V ?)
        //      ELLER SELVE ROOM.SYCKLUS I EN WHILE?

        // noen "steg" i temp - det blir fortere kaldt / varmt med høye/lave temp

        /*
               public async Task ChangeState(Status state)
               {
                   //funsksjoner skal ut til romcontroll - 
                   State = state;
                   Controller.SetTemperature = State switch
                   {
                       Status.Sleep => 16,
                       Status.Wakeup => 18,
                       Status.Standby => 20,
                       Status.Awake => 22,
                       _ => SetTemperature
                   };
                   AdjustVentilation();
                   Controller.AdjustTemperature();
                   AdjustLights();
                   Printer();
               }

               private void AdjustVentilation()
               {
                   Ventilation = State == Status.Awake;
               }

               private async Task AdjustLights()
               {
                  SetLux = State == Status.Awake ? 500 : 0;

                  while (ActualLux != SetLux)
                  {
                      if (ActualLux > SetLux)
                      {
                          ActualLux -= 50;
                      }
                      if (ActualLux < SetLux)
                      {
                          Light = true;
                          ActualLux += 50;
                      }
                      Light = ActualLux != 0;
                      await Task.Delay(300);
                  } 
               }

               internal async Task AdjustTemperature()
               {
                   SetTemperature = State switch
                   {
                       Status.Sleep => 16,
                       Status.Wakeup => 18,
                       Status.Standby => 20,
                       Status.Awake => 22,
                       _ => SetTemperature
                   };
                   while (Math.Abs(ActualTemperature - SetTemperature) > 1)
                   {
                       if (ActualTemperature > SetTemperature)
                       {
                           Heater = false;
                           Cooler = true;
                           ActualTemperature -=0.5;
                       }
                       if (ActualTemperature < SetTemperature)
                       {
                           Heater = true;
                           Cooler = false;
                           ActualTemperature +=0.5;
                       }
                       await Task.Delay(100);

                   }
               }

                       public async Task Printer()
                       {
                           while (true)
                           {
                               PrintRoomInfo();
                               await Task.Delay(300);
                           }

                       }
               
        private void DetectMotion()
                {
                    Motion = true;
                    ChangeState(Status.Awake);
                    //wake floor -> building?
                    BuyTime();
                    StartTimer();
                }
                private void BuyTime()
                {
                    Timer = 15;
                }
                private void StartTimer()
                {
                    //kjøres async/parallelt?
                    for (int i = Timer; i > 0; i--)
                    {
                        Timer--;
                        if (Timer == 0) ChangeState(Status.Standby);
                    }
                }
        */
        /*
        public void PrintRoomInfo()
        {   Console.Clear();
            Console.WriteLine(@$"
                    Type:{RoomType} State:{State} 
                    LYS:            Actual: {ActualLux} -> Wanted: {SetLux} :  ON/OFF: {Light}
                    Varme:          Actual: {ActualTemperature} -> Wanted: {SetTemperature} :  ON/OFF: {Heater}/{Cooler}
                    Ventilasjon:    ON/OFF: {Ventilation}");
        }
        */

    }

}