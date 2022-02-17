using System;
using System.Threading.Tasks;
using BMSutkast1;
using BMSutkast1.Output;
using BMSutkast1.Sensor;
using NUnit.Framework;

namespace TestProject
{
    public class Tests
    {
        [Test]
        public void NewLuxSensor()
        {
            var guid = Guid.NewGuid();
            var controller = new RoomController("Test", guid, 10);
            var input = new LuxSensor(controller.Id);
            Assert.AreEqual(controller.Id, input.RoomControllerId, "GUID ID Check");
            Assert.AreEqual(0, input.ActualLux, "StartLux when creating");
            Assert.AreEqual(0, input.SetValue, "SetValue on creation");
            Assert.Pass();
        }
        [Test]
        public void NewMotionSensor()
        {
            var guid = Guid.NewGuid();
            var controller = new RoomController("Test", guid, 10);
            var input = new MotionSensor(controller.Id);
            Assert.AreEqual(controller.Id, input.RoomControllerId, "GUID ID Check");
            Assert.AreEqual(false, input.Open, "StartBool");
            input.Timer = 20;
            Assert.AreEqual(20, input.Timer, "Adjusted timer");
            Assert.Pass();
        }
        [Test]
        public void NewLight()
        {
            var guid = Guid.NewGuid();
            var controller = new RoomController("Test", guid, 10);
            var light = new Light(controller.Id);
            Assert.AreEqual(0,light.Value, "StartValue");
            Assert.AreEqual(false, light.On, "StartBool");
            Assert.AreEqual(light.Type, "Light", "StartBool");
            Assert.Pass();
        }

        [Test]
        public void NewController()
        {
            var guid = Guid.NewGuid();
            var controller = new RoomController("Test", guid, 10);
            Assert.AreEqual(Status.Sleep ,controller.State, "On creation");
            Assert.AreEqual(0 ,controller.Temp.SetValue, "Set temp");
            Assert.Pass();
        }

        [Test]
        public async Task ControllerStateAsync()
        {
            var guid = Guid.NewGuid();
            var controller = new RoomController("Test", guid, 10);
            Assert.AreEqual(Status.Sleep, controller.State, "On creation");
            controller.State = Status.Awake;
            await controller.ChangeState();
            Assert.AreEqual(Status.Awake, controller.State, "State change");
            Assert.AreEqual(22.0, controller.SetTemperature, "SetTemp Awake");
            Assert.AreEqual(22.0, controller.Temp.ActualTemperature, "ActualTemp");
            Assert.AreEqual(500, controller.SetLux, "SetLux Awake");
            Assert.AreEqual(500, controller.Lux.ActualLux, "Actual lux Awake");
            Assert.Pass();
        }
    }
}