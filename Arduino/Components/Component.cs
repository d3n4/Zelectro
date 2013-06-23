using System;

namespace Zelectro
{
    public abstract class Component
    {
        protected ArduinoProgram _context = Arduino.Context;
        protected Pin _pin;

        public ArduinoProgram context
        {
            set
            {
                _context = value;
            }

            get
            {
                return _context;
            }
        }

        public Component(Pin pin)
        {
            _pin = pin;
        }

        public Component(Pin pin, ArduinoProgram context)
        {
            _pin = pin;
            _context = context;
        }

        public Component(ArduinoProgram context)
        {
            _context = context;
        }
    }
}

