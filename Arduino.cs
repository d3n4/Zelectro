using System;
using System.Threading;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;

namespace Zelectro
{
    public class Arduino
    {
        public enum ConnectionError
        {
            InvalidSoftware,
            InvalidPlug,
            None
        }

        private static ArduinoProgram _program = null;
        protected string _name = "COM1";
        protected int _baud = 9600;
        protected SerialPort _serial;
        protected List<ArduinoProgram> _programs = new List<ArduinoProgram>();
        protected ConnectionError _error = ConnectionError.None;

        public ConnectionError Error { get { return _error; } }

        public static ArduinoProgram Context
        {
            get { return _program; }
        }

        public Arduino()
        {
        }

        public Arduino(string Port, int Baud = 9600)
        {
            _name = Port;
            _baud = Baud;
        }

        public T GetContext<T>() where T : ArduinoProgram
        {
            return (T)_program;
        }

        public void AddProgram<T>() where T : ArduinoProgram, new()
        {
            _programs.Add(_program = new T(){ arduino = this });
        }

        public bool Connect()
        {
            _error = ConnectionError.None;
            try
            {
                _serial = new SerialPort(_name, _baud);
                _serial.Open();
                while(!_serial.IsOpen);
                _serial.Close();
                _serial.Open();
                while(!_serial.IsOpen);
                if(pull<Status>() == Status.OK)
                {
                    Thread.Sleep(100);
                    foreach(var program in _programs)
                        program.setup();
                    send<Action>(Action.ChangeStatus);
                    if(pull<Status>() == Status.OK){
                        foreach(var program in _programs)
                            program.Work = true;
                        return true;
                    }
                }
                _error = ConnectionError.InvalidSoftware;
            }
            catch
            {
                _error = ConnectionError.InvalidPlug;
                if (_serial.IsOpen)
                    _serial.Close();
            }

            return false;
        }

        public byte pull()
        {
            try
            {
                return (byte)_serial.ReadByte();
            } catch {
            }
            return 0x0;
        }

        public T pull<T>() where T : struct, IComparable, IConvertible, IFormattable
        {
            return (T)Enum.Parse(typeof(T), Enum.GetName(typeof(T), pull()));
        }

        public void send(byte[] bytes, int offset = 0, int length = 1)
        {
            _serial.Write(bytes, offset, length);
        }

        public void send(byte bytes)
        {
            send(new[]{ bytes });
        }

        public void send<T>(T action, params byte[] args) where T : struct, IComparable, IConvertible, IFormattable 
        {
            send(action.ToByte(null));
            foreach (var arg in args)
                send(arg);
        }
    }
}

