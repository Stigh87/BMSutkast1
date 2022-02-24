using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BMS.Output;
using BMS.Sensor;

namespace BMS
{
    public class RoomController
    {
        private Guid Id { get; }
        public Guid RoomId { get; set; }
        private readonly string _roomType;
        internal Status State;

        private bool occupied;

        //inputs:
        //IO-SENSOR
        private DoorSensor _door;
        private WindowSensor _window;
        private MotionSensor _motion;

        //VALUE-SENSOR
        private Co2Sensor _co2;
        private LuxSensor _lux;
        private TemperatureSensor _temp;

        private ButtonPanel _button;

        //FRA COMMAND/DISPLAY
        private double _setTemperature;
        private int _setLux;

        //Outputs:
        private readonly Cooling _cool;
        private readonly Heating _heat;
        private Ventilation _vent;
        private Light _light;

        public RoomController(string roomtype, Guid roomId)
        {
            RoomId = roomId;
            _roomType = roomtype;
            Id = Guid.NewGuid(); 
            State = Status.Sleep;
            _motion = new MotionSensor(Id);
            _temp = new TemperatureSensor(Id);
            _lux = new LuxSensor(Id);
            if (_roomType == "Meeting")
            {
                _button = new ButtonPanel(Id);
            }
            if (_roomType == "Office")
            {
                _window = new WindowSensor(Id);
            }
            if (_roomType != "Office")
            {
                _co2 = new Co2Sensor(Id);
            }
            if (_roomType != "Mingle" || _roomType != "Cubicle")
            {
                _door = new DoorSensor(Id);
            }
            _heat = new Heating(Id);
            _cool = new Cooling(Id);
            _vent = new Ventilation(Id);
            _light = new Light(Id);
            _setTemperature = 15;
            occupied = false;
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
            if (State == Status.Awake) _vent.On = true;
            if (State == Status.Standby) _vent.On = false; //timer 10min så ventOff
            if (State == Status.Sleep) _vent.On = false;
            if (State == Status.Wakeup) _vent.On = true; //timer 10min så ventOff, om ikke awake fra motion.

        }
        private async Task AdjustLights()
        {
            while (State == Status.Awake)
            {
                if (_lux.ActualLux > _setLux)
                {
                    _light.Value = _light.Value > 0 ? _light.Value -= 1 : _light.Value = 0;
                }
                if (_lux.ActualLux < _setLux)
                {
                    _light.On = true;
                    _light.Value = _light.Value < 100 ? _light.Value += 1 : _light.Value = 100;
                }
                _light.On = _lux.ActualLux != 0;
                await _lux.UpdateActualLux(_light);
                await Task.Delay(Timer.CheckDelay/4);
            }
            if (State != Status.Awake) {_light.Value = 0; _light.On = false; }
        }
        internal async Task AdjustTemperature()
        {
            while (true)    //Math.Abs(_temp.GetTemp() - _setTemperature) > 0.01
            {
                if (_temp.GetTemp() > _setTemperature) 
                {
                    SwitchCooling();
                }
                if (_temp.GetTemp() < _setTemperature)
                {
                    SwitchHeating();
                }
                _temp.AdjustTemperature(_heat.On, _cool.On, _vent.On);
                await Task.Delay(Timer.CheckDelay);
            }
        }
        private void SwitchHeating()
        {
                _cool.On = false;
                var outdoorTemp = _temp.GetOutdoorTemp();
                if (outdoorTemp < _setTemperature) _heat.On = true;
        }
        private void SwitchCooling()
        {
                _heat.On = false;
                if(_temp.GetOutdoorTemp() > _setTemperature) _cool.On = true;
        }
        public async Task PrintRoomInfo()
        {
            var heatCool = _heat.On ? "Heater on" : _cool.On ? "Cooling on" : "Idle";

          Console.WriteLine(@$"
                    Type: {_roomType} State: {State} 
                    LYS:            Actual: {_lux.GetLuxString().PadLeft(6, ' ')} -> Wanted: {_setLux.ToString().PadLeft(6, ' ')}  :  Value%: {_light.Value}
                    Varme:          Actual: {_temp.GetTemp().ToString().PadLeft(5, ' ')}c -> Wanted: {_setTemperature.ToString().PadLeft(5, ' ')}c  :  {heatCool}
                    Ventilasjon:    ON/OFF: {_vent.On}");
        }
        internal object PrintControllerInfo()
        {
            var occupiedString = occupied ? "YES" : "NO";
            string info =
                $"State: {State.ToString().PadRight(7, ' ')} - Temperature: {_temp.GetTemp().ToString().PadLeft(5, ' ')}c /{_setTemperature}c - Occupied: {occupiedString}";
            return info;
        }
        public void ChangeSetValue(string type, int value)
        {
            if (type == "Lux")
            {
                _setLux = value <= 1000 && value >= 0 ? value : _setLux;
                AdjustLights();
            }

            if (type == "Temp")
            {
                _setTemperature = value is <= 28 and >= 15 ? value : _setTemperature;
                AdjustTemperature();
            }
        }
        public async Task SetUpdateOutdoorValues(double currentOutdoorTemperature, int currentOutdoorLux)
        {
           await _temp.SetUpdateOutdoorValue(currentOutdoorTemperature);
           await _lux.SetUpdateOutdoorValue(currentOutdoorLux, _light);
        }
        public bool CheckOccupied()
        {
            return occupied;
        }
        public void Populate()
        {
            _motion.DetectMotion();
            occupied = true;
            State = Status.Awake;
            ChangeState();
        }
        public void GoHome()
        {
            _motion.TimeOut();
            occupied = false;
            State = Status.Standby;
            ChangeState();
        }
    }
}
