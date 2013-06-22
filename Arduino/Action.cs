using System;

namespace Zelectro
{
    public enum Action
    {
        ChangeStatus = 0x0,
        PinMode = 0x1,
        DigitalWrite = 0x2,
        DigitalRead = 0x3,
        AnalogRead = 0x4,
        AnalogWrite = 0x5
    }
}

