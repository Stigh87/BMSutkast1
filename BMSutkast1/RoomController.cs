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
        private Guid Id { get; }
        public Guid RoomId { get; set; }
        private readonly string RoomType;
        internal Status State;

        //inputs:
        //IO-SENSOR
        private DoorSensor Door;
        private WindowSensor Window;
        private MotionSensor Motion;

        //VALUE-SENSOR
        private Co2Sensor Co2;
        private LuxSensor Lux;
        private TemperaturSensor Temp;

        private ButtonPanel Button;

        //FRA COMMAND/DISPLAY
        private double _setTemperature; //max 22 min 16 (T>22 ? 22 : T<16 ? 16 : T)
        private int _setLux;

        //Outputs:
        private readonly Cooling _cool;
        private readonly Heating _heat;
        private Ventilation Vent;
        private Light Light;

        public RoomController(string roomtype, Guid roomId, int area)
        {
            RoomId = roomId;
            RoomType = roomtype;
            Id = Guid.NewGuid();
            State = Status.Sleep;
            Motion = new MotionSensor(Id);
            Temp = new TemperaturSensor(Id);
            Lux = new LuxSensor(Id);
            if (RoomType == "Meeting")
            {
                Button = new ButtonPanel(Id);
            }
            if (RoomType == "Office")
            {
                Window = new WindowSensor(Id);
            }
            if (RoomType != "Office")
            {
                Co2 = new Co2Sensor(Id);
            }
            if (RoomType != "Mingle" || RoomType != "Cubicle")
            {
                Door = new DoorSensor(Id);
            }
            _heat = new Heating(Id);
            _cool = new Cooling(Id);
            Vent = new Ventilation(Id);
            Light = new Light(Id);
            _setTemperature = 15;
        }

        public async Task ChangeState()
        {
            _setTemperature = State switch
            {
                Status.Sleep => 16,
                Status.Wakeup => 18,
                Status.Standby => 20,
                Status.Awake => 22,
                _ => _setTemperature
            };
            _setLux = State == Status.Awake ? 500 : 0;
             AdjustTemperature();
             AdjustLights();
             AdjustVentilation();
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
           while (Lux.ActualLux != _setLux)
            {
                if (Lux.ActualLux > _setLux)
                {
                    Light.Value -= 5;
                    Lux.ActualLux -= 50;
                }
                if (Lux.ActualLux < _setLux)
                {
                    Light.OnOff = true;
                    Light.Value += 5;
                    Lux.ActualLux += 50;
                }
                Light.OnOff = Lux.ActualLux != 0;
               await Task.Delay(1000);
            }
        }

        internal async Task AdjustTemperature()
        {
            while (Math.Abs(Temp.ActualTemperature - _setTemperature) > 0.1)
            {
                if (Temp.ActualTemperature> _setTemperature)
                {
                    _heat.OnOff = false;
                    _heat.Interlock = true;
                    _cool.OnOff = true;
                    _cool.Interlock = false;
                    Temp.ActualTemperature -= 0.5; //lage dette til en funksjon/variabel med forskjellige faktorer?
                }
                if (Temp.ActualTemperature < _setTemperature)
                {
                    _heat.OnOff = true;
                    _heat.Interlock = false;
                    _cool.OnOff = false;
                    _cool.Interlock = true;
                    Temp.ActualTemperature += 0.5; //lage dette til en funksjon/variabel med forskjellige faktorer?
                }
                if (Math.Abs(Temp.ActualTemperature - _setTemperature) < 0.9)
                {
                    _heat.OnOff = false;
                    _heat.Interlock = false;
                    _cool.OnOff = false;
                    _cool.Interlock = false;
                }
                await Task.Delay(2000);
              //Thread.Sleep(200);
            }
        }
        
        public async Task PrintRoomInfo()
        {
          //  Console.Clear();
            Console.WriteLine(@$"
                    Type:{RoomType} State:{State} 
                    LYS:            Actual: {Lux.ActualLux} -> Wanted: {_setLux} :  Value%: {Light.Value}
                    Varme:          Actual: {Temp.ActualTemperature} -> Wanted: {_setTemperature} :  ON/OFF: H-{_heat.OnOff}/C-{_cool.OnOff}
                    Ventilasjon:    ON/OFF: {Vent.OnOff}");
        }
        
        internal object PrintControllerInfo()
        {
            var occupied = Motion.MotionOpen() ? "YES" : "NO";
            string Info =
                $"State: {State} - Temperature: {Temp.GetTemp()}c /{_setTemperature}c - Occupied: {occupied} - Power consumption: ??? Kw / h";
            return Info;
        }

        
    }
}
