using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BMSutkast1.Output;
using BMSutkast1.Sensor;

namespace BMSutkast1
{
    public class RoomController
    {
        public Guid _id { get; }
        public string RoomType;
        public Guid RoomId;
        public Status State;

        //inputs:
        //IO-SENSOR
        public DoorSensor Door;
        public WindowSensor Window;
        public MotionSensor Motion;

        //VALUE-SENSOR
        public Co2Sensor Co2;
        public LuxSensor Lux;
        public TemperaturSensor Temp;

        public ButtonPanel Button;

        //FRA COMMAND/DISPLAY
        public double SetTemperature; //max 22 min 16 (T>22 ? 22 : T<16 ? 16 : T)
        public int SetLux;

        //Outputs:
        public Cooling Cool;
        public Heating Heat;
        public Ventilation Vent;
        public Light Light;

        public RoomController(string roomtype, Guid roomId, int area)
        {
            RoomId = roomId;
            RoomType = roomtype;
            _id = Guid.NewGuid();
            State = Status.Sleep;
            Motion = new MotionSensor(_id);
            Temp = new TemperaturSensor(_id);
            Lux = new LuxSensor(_id);
            if (RoomType == "Meeting")
            {
                Button = new ButtonPanel(_id);
            }
            if (RoomType == "Office")
            {
                Window = new WindowSensor(_id);
            }
            if (RoomType != "Office")
            {
                Co2 = new Co2Sensor(_id);
            }
            if (RoomType != "Mingle" || RoomType != "Cubicle")
            {
                Door = new DoorSensor(_id);
            }

            Heat = new Heating(_id);
            Cool = new Cooling(_id);
            Vent = new Ventilation(_id);
            Light = new Light(_id);
            SetTemperature = 15;
        }
        public async Task ChangeState()
        {
            SetTemperature = State switch
            {
                Status.Sleep => 16,
                Status.Wakeup => 18,
                Status.Standby => 20,
                Status.Awake => 22,
                _ => SetTemperature
            };
            SetLux = State == Status.Awake ? 500 : 0;
            await AdjustTemperature();
            await AdjustLights();
            await AdjustVentilation();
        }

        private async Task AdjustVentilation()
        {
            if (State == Status.Awake) Vent.OnOff = true;
            if (State == Status.Standby) ; //timer 10min så ventOff
            if(State == Status.Sleep) Vent.OnOff = false;
            if (State == Status.Wakeup) ; //timer 10min så ventOff, om ikke awake fra motion.

        }

        private async Task AdjustLights()
        {
           while (Lux.ActualLux != SetLux)
            {
                if (Lux.ActualLux > SetLux)
                {
                    Light.Value -= 5;
                    Lux.ActualLux -= 50;
                }
                if (Lux.ActualLux < SetLux)
                {
                    Light.OnOff = true;
                    Light.Value += 5;
                    Lux.ActualLux += 50;
                }
                Light.OnOff = Lux.ActualLux != 0;
                //Task.Delay(1300);
            }
        }

        internal async Task AdjustTemperature()
        {
            while (Math.Abs(Temp.ActualTemperature - SetTemperature) > 0.1)
            {
                if (Temp.ActualTemperature> SetTemperature)
                {
                    Heat.OnOff = false;
                    Heat.Interlock = true;
                    Cool.OnOff = true;
                    Cool.Interlock = false;
                    Temp.ActualTemperature -= 0.5; //lage dette til en funksjon/variabel med forskjellige faktorer?
                }
                if (Temp.ActualTemperature < SetTemperature)
                {
                    Heat.OnOff = true;
                    Heat.Interlock = false;
                    Cool.OnOff = false;
                    Cool.Interlock = true;
                    Temp.ActualTemperature += 0.5; //lage dette til en funksjon/variabel med forskjellige faktorer?
                }
                if (Math.Abs(Temp.ActualTemperature - SetTemperature) < 0.9)
                {
                    Heat.OnOff = false;
                    Heat.Interlock = false;
                    Cool.OnOff = false;
                    Cool.Interlock = false;
                }
               await Task.Delay(1300);
              //Thread.Sleep(200);
            }
        }
        
        public async Task PrintRoomInfo()
        {
            //send herfra til diplay -> med relevant info/alt?
            Console.Clear();
            Console.WriteLine(@$"
                    Type:{RoomType} State:{State} 
                    LYS:            Actual: {Lux.ActualLux} -> Wanted: {SetLux} :  Value%: {Light.Value}
                    Varme:          Actual: {Temp.ActualTemperature} -> Wanted: {SetTemperature} :  ON/OFF: H-{Heat.OnOff}/C-{Cool.OnOff}
                    Ventilasjon:    ON/OFF: {Vent.OnOff}");
        }
        public async Task Printer()
        {
            PrintRoomInfo();
               // Task.Delay(1300);
        }





    }
}
