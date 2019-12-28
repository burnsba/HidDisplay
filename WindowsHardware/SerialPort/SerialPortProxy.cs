using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Timers;

namespace WindowsHardware.SerialPort
{
    using SP = System.IO.Ports.SerialPort;

    /// <summary>
    /// Serial port wrapper class. Periodically polls port for data.
    /// </summary>
    public class SerialPortProxy : IDisposable
    {
        private Timer _pollTimer;
        private SP _serialPort;

        /// <summary>
        /// Event delegate to accept raw data from serial port.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="data">Serial port data.</param>
        public delegate void DataReceivedHandler(object sender, byte[] data);

        /// <summary>
        /// Event to accept raw data from serial port.
        /// </summary>
        public event DataReceivedHandler DataReceivedEvent;

        /// <summary>
        /// Gets or sets connection baudrate.
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// Gets or sets rate to poll serial port for new data.
        /// </summary>
        public int PollRateMs { get; set; }

        /// <summary>
        /// Gets or sets connection port.
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// Gets or sets how to handle read errors.
        /// </summary>
        public ReadErrorHandling ReadErrorAction { get; set; } = ReadErrorHandling.IgnoreRetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialPortProxy"/> class.
        /// </summary>
        /// <param name="portName">Port to open.</param>
        /// <param name="baudRate">Connection baudrate.</param>
        /// <param name="pollRateMs">Data poll interval time.</param>
        public SerialPortProxy(string portName, int baudRate, int pollRateMs)
        {
            PortName = portName;
            BaudRate = baudRate;
            PollRateMs = pollRateMs;

            _pollTimer = new System.Timers.Timer();
            _pollTimer.AutoReset = true;
            _pollTimer.Interval = pollRateMs;
            _pollTimer.Elapsed += PortPoll;
        }

        /// <summary>
        /// Stops polling port for data. Serial port is closed and discarded.
        /// </summary>
        public void Stop()
        {
            if (!_pollTimer.Enabled)
            {
                return;
            }

            _pollTimer.Stop();

            try
            {
                _serialPort.Close();
            }
            catch (System.IO.IOException)
            { }
            _serialPort.Dispose();
            _serialPort = null;
        }

        /// <summary>
        /// Opens a new connection for the serial port and starts polling for data.
        /// </summary>
        public void Start()
        {
            if (_pollTimer.Enabled)
            {
                return;
            }

            _pollTimer.Start();

            _serialPort = new SP(PortName, BaudRate);

            _serialPort.Open();
            _serialPort.DiscardInBuffer();
        }

        /// <summary>
        /// Port poll interval event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Args.</param>
        private void PortPoll(object sender, EventArgs e)
        {
            if (object.ReferenceEquals(null, _serialPort))
            {
                return;
            }

            if (!_serialPort.IsOpen)
            {
                return;
            }

            if (DataReceivedEvent == null)
            {
                return;
            }

            var bytesAvailable = _serialPort.BytesToRead;
            if (bytesAvailable < 1)
            {
                return;
            }

            var temp = new byte[bytesAvailable];

            try
            {
                _serialPort.Read(temp, 0, bytesAvailable);
                _serialPort.DiscardInBuffer();
            }
            catch (System.OperationCanceledException)
            {
                if (ReadErrorAction == ReadErrorHandling.IgnoreRetry)
                {
                    return;
                }
                else if (ReadErrorAction == ReadErrorHandling.Stop)
                {
                    Stop();
                    return;
                }
                else if (ReadErrorAction == ReadErrorHandling.Throw)
                {
                    throw;
                }
            }
            catch (System.IO.IOException)
            {
                if (ReadErrorAction == ReadErrorHandling.IgnoreRetry)
                {
                    return;
                }
                else if (ReadErrorAction == ReadErrorHandling.Stop)
                {
                    Stop();
                    return;
                }
                else if (ReadErrorAction == ReadErrorHandling.Throw)
                {
                    throw;
                }
            }

            if (DataReceivedEvent != null)
            {
                DataReceivedEvent(this, temp);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!object.ReferenceEquals(null, _pollTimer))
            {
                _pollTimer.Stop();
            }

            if (!object.ReferenceEquals(null, _serialPort))
            {
                if (_serialPort.IsOpen)
                {
                    try
                    {
                        _serialPort.Close();
                    }
                    catch (System.IO.IOException)
                    { }
                }

                _serialPort.Dispose();
                _serialPort = null;
            }
        }
    }
}
