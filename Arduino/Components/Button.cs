using System;
using System.Collections.Generic;

namespace Zelectro
{
    public enum ButtonState
    {
        Up = 0,
        Down = 1
    }

    public class Button : Component
    {
        protected ButtonState _state = ButtonState.Up;
        protected ButtonState _prev_state = ButtonState.Up;

        public ButtonState State
        {
            get { return _state; }
        }

        public List<System.Action> OnDown = new List<System.Action>();
        public List<System.Action> OnClick = new List<System.Action>();

        public Button(Pin pin) : base(pin)
        {
            _context.pinMode(pin, IOType.Input);
            _context.OnLoop.Add(Click);
        }

        protected virtual void Click()
        {
            var state = (ButtonState)_context.digitalRead(_pin);
            if (state != _state)
            {
                if (state == ButtonState.Up)
                    foreach (var e in OnClick)
                        e.Invoke();

                if (state == ButtonState.Down)
                    foreach (var e in OnDown)
                        e.Invoke();
                _state = state;
            }
        }
    }
}

