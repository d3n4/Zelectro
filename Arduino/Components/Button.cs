using System;
using System.Collections.Generic;

namespace Zelectro
{
    public enum ButtonState
    {
        UP = 0,
        DOWN = 1
    }

    public class Button : Component
    {
        public delegate void ButtonHandler(object sender);
        public event ButtonHandler Click;
        public event ButtonHandler Release;
        public event ButtonHandler Down;

        protected ButtonState _state = ButtonState.UP;
        protected ButtonState _prev_state = ButtonState.UP;

        public int Ticks { get; protected set; }

        public ButtonState State
        {
            get { return _state; }
        }

        public Button(Pin pin) : base(pin)
        {
            _context.pinMode(pin, IOType.Input);
            _context.Loop += loop;
        }

        protected void loop (object sender)
        {
            var state = (ButtonState)_context.digitalRead(_pin);
            if (state != _state)
            {
                if (state == ButtonState.UP)
                {
                    if (Click != null)
                        Click(this);
                    if (Release != null)
                        Release(this);
                }

                if (state == ButtonState.DOWN)
                if (Down != null)
                    Down(this);
                _state = state;
                Ticks = 0;
            }
            else
                Ticks++;
        }
    }
}

