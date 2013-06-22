using System;
using System.Threading;

namespace Zelectro
{
    public class LED : Component
    {

        public LED(Pin pin) : base(pin)
        {
            _context.pinMode(pin, IOType.Output);
        }

        public void On()
        {
            _context.digitalWrite(_pin, Voltage.HIGH);
        }

        public void Off()
        {
            _context.digitalWrite(_pin, Voltage.LOW);
        }

        public void Blink(int delay)
        {
            On();
            Thread.Sleep(delay);
            Off();
            Thread.Sleep(delay);
        }
    }
}

