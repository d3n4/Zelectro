using System;
using System.Threading;
using System.Collections.Generic;

namespace Zelectro
{
    public abstract class ArduinoProgram
    {
        public ArduinoProgram()
        {
            _thread = new Thread(_loop);
            _thread.Start();
        }

        public const byte INPUT = 0x0,
                          OUTPUT = 0x1,
                          HIGH = 0x1,
                          LOW = 0x0;

        public bool Work = false;

        public List<System.Action> OnLoop = new List<System.Action>();

        private Thread _thread = null;

        private Arduino _arduino = null;

        public Arduino arduino 
        {
            get {
                return _arduino;
            }

            set {
                if(_arduino == null) _arduino = value;
            }
        }

        public virtual void setup(){}
        public virtual void loop(){}

        private void _loop()
        {
            while(true)
            {
                if (Work)
                {
                    loop();
                    foreach (var loop_action in OnLoop)
                        loop_action.Invoke();
                }
                Thread.Sleep(1);
            }
        }

        public void pinMode(Pin pin, IOType type)
        {
            pinMode((byte)pin, (byte)type);
        }

        public void pinMode(Pin pin, byte type)
        {
            pinMode((byte)pin, type);
        }

        public void pinMode(byte pin, byte type)
        {
            _arduino.send<Action>(Action.PinMode, pin, type);
        }

        public void digitalWrite(Pin pin, Voltage type)
        {
            digitalWrite((byte)pin, (byte)type);
        }

        public void digitalWrite(Pin pin, byte type)
        {
            digitalWrite((byte)pin, type);
        }

        public void digitalWrite(byte pin, byte type)
        {
            _arduino.send<Action>(Action.DigitalWrite, pin, type);
        }

        public byte digitalRead(Pin pin)
        {
            return digitalRead((byte)pin);
        }

        public byte digitalRead(byte pin)
        {
            _arduino.send<Action>(Action.DigitalRead, pin);
            return _arduino.pull();
        }

        public byte analogRead(Pin pin)
        {
            return analogRead((byte)pin);
        }

        public byte analogRead(byte pin)
        {
            _arduino.send<Action>(Action.AnalogRead, pin);
            return _arduino.pull();
        }

        public void analogWrite(Pin pin, int value)
        {
            analogWrite((byte)pin, value);
        }

        public void analogWrite(byte pin, int value)
        {
            _arduino.send<Action>(Action.AnalogWrite, pin, (byte)value);
        }
    }
}

