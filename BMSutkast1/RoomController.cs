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
            if (State == Status.Awake) Vent.On = true;
            if (State == Status.Standby) ; //timer 10min så ventOff
            if(State == Status.Sleep) Vent.On = false;
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
                    Light.On = true;
                    Light.Value += 5;
                    Lux.ActualLux += 50;
                }
                Light.On = Lux.ActualLux != 0;
               await Task.Delay(1000);
            }
        }

        internal async Task AdjustTemperature()
        {
            while (Math.Abs(Temp.GetTemp() - _setTemperature) > 0.1)
            {
                if (Temp.GetTemp() > _setTemperature) //
                {
                    SwitchCooling();
                }
                if (Temp.GetTemp() < _setTemperature)
                {
                    SwitchHeating();
                }
                if (Math.Abs(Temp.GetTemp() - _setTemperature) < 0.9) //"Delay" i ovn/kjøler 
                {
                    _heat.On = false;
                    _heat.Interlock = false;
                    _cool.On = false;
                    _cool.Interlock = false;
                }
                await Task.Delay(2000); //tid regnes ut og hentes fra klokke/kalender.
              //Thread.Sleep(200);
            }
        }

        private void SwitchHeating()
        {
            _heat.On = true;
            _heat.Interlock = false;
            _cool.On = false;
            _cool.Interlock = true;
            Temp.AdjustTemperature(_heat.Interlock, _cool.Interlock);
            //Temp.ActualTemperature += 0.3; //lage dette til en funksjon/variabel med forskjellige faktorer?
        }

        private void SwitchCooling()
        {
            //må legges inn en "stopper" så ikke cooling går på om utendørstemp < innendørs/ønsket
            _heat.On = false;
            _heat.Interlock = true;
            _cool.On = true;
            _cool.Interlock = false;
            Temp.AdjustTemperature(_heat.Interlock, _cool.Interlock);
            //Temp.ActualTemperature -= 0.3; //lage dette til en funksjon/variabel med forskjellige faktorer?
        }

        public async Task PrintRoomInfo()
        {
          //  Console.Clear();
          Console.WriteLine(@$"
                    Type:{RoomType} State:{State} 
                    LYS:            Actual: {Lux.ActualLux} -> Wanted: {_setLux} :  Value%: {Light.Value}
                    Varme:          Actual: {Temp.GetTemp()} -> Wanted: {_setTemperature} :  ON/OFF: H-{_heat.On}/C-{_cool.On}
                    Ventilasjon:    ON/OFF: {Vent.On}");
        }
        
        internal object PrintControllerInfo()
        {
            var occupied = Motion.MotionOpen() ? "YES" : "NO";
            string info =
                $"State: {State} - Temperature: {Temp.GetTemp()}c /{_setTemperature}c - Occupied: {occupied} - Power consumption: ??? Kw / h";
            return info;
        }

        public void ChangeSetValue(string type, int value)
        {
            if (type == "Lux")
            {
                _setLux = value < 1000 ? value : _setLux;
                AdjustLights();
            }

            if (type == "Temp")
            {
                _setTemperature = value is < 28 and > 15 ? value : _setTemperature;
                AdjustTemperature();
            }
        }

    }
}
