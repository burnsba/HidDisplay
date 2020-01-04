using System;
using System.Collections.Generic;
using BurnsBac.WindowsHardware.SerialPort;
using HidDisplay.Controller.ControllerState.Nintendo64;

namespace HidDisplay.Controller.Readers
{
    /// <summary>
    /// Listens to serial port for NintendoSpy protocol messages and forwards state change events.
    /// </summary>
    /// <remarks>
    /// https://github.com/jaburns/NintendoSpy .
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1629:", Justification = "Documentation text ends in period.")]
    public class NintendoSpy64 : SerialTranslatorBase
    {
        private List<byte> _readBuffer = new List<byte>();

        /// <summary>
        /// Event delegate to accept state change events.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="state">Controller state data.</param>
        public delegate void Nintendo64ControllerStateHandler(object sender, Nintendo64ControllerState state);

        /// <summary>
        /// Event to accept state change.
        /// </summary>
        public event Nintendo64ControllerStateHandler Nintendo64ControllerStateChange;

        /// <inheritdoc />
        protected override void Handler(object sender, byte[] data)
        {
            if (Nintendo64ControllerStateChange == null)
            {
                return;
            }

            _readBuffer.AddRange(data);

            int endPacketIndex = -1;
            int startPacketIndex = -1;

            bool a;
            bool b;
            bool z;
            bool start;
            bool c_up;
            bool c_down;
            bool c_left;
            bool c_right;
            bool d_up;
            bool d_down;
            bool d_left;
            bool d_right;
            bool l_shoulder;
            bool r_shoulder;
            short analogx;
            short analogy;

            do
            {
                // Try and find 2 splitting characters in our buffer.
                endPacketIndex = _readBuffer.LastIndexOf(0x0A);
                if (endPacketIndex <= 1)
                {
                    return;
                }

                startPacketIndex = _readBuffer.LastIndexOf(0x0A, endPacketIndex - 1);
                if (startPacketIndex == -1)
                {
                    return;
                }

                // Grab the latest packet out of the buffer and fire it off to the receive event listeners.
                var dataLen = endPacketIndex - startPacketIndex;

                byte[] dataBytes = null;

                try
                {
                    dataBytes = _readBuffer.GetRange(startPacketIndex, dataLen).ToArray();
                    _readBuffer.RemoveRange(0, endPacketIndex + 1);
                }
                catch (IndexOutOfRangeException)
                {
                    // someone modified the list between the above statements.
                    // probably safe to ignore.
                    ///////

                    return;
                }

                var bits = new byte[32];
                a = dataBytes[1] > 0;
                b = dataBytes[2] > 0;
                z = dataBytes[3] > 0;
                start = dataBytes[4] > 0;
                d_up = dataBytes[5] > 0;
                d_down = dataBytes[6] > 0;
                d_left = dataBytes[7] > 0;
                d_right = dataBytes[8] > 0;

                //// unused // bits[9] = (dataBytes[9] > 0) ? (byte)100 : (byte)0;
                //// unused // bits[10] = (dataBytes[10] > 0) ? (byte)100 : (byte)0;
                l_shoulder = dataBytes[11] > 0;
                r_shoulder = dataBytes[12] > 0;
                c_up = dataBytes[13] > 0;
                c_down = dataBytes[14] > 0;
                c_left = dataBytes[15] > 0;
                c_right = dataBytes[16] > 0;

                analogx = (short)Utility.MakeSignedReverseByte(dataBytes, 17);
                analogy = (short)Utility.MakeSignedReverseByte(dataBytes, 25);

                Nintendo64ControllerStateChange(this, new Nintendo64ControllerState()
                {
                    ControllerPort = 1,
                    A = a,
                    B = b,
                    Z = z,
                    Start = start,
                    L_Shoulder = l_shoulder,
                    R_Shoulder = r_shoulder,
                    C_Up = c_up,
                    C_Down = c_down,
                    C_Left = c_left,
                    C_Right = c_right,
                    D_Up = d_up,
                    D_Down = d_down,
                    D_Left = d_left,
                    D_Right = d_right,
                    AnalogX = analogx,
                    AnalogY = analogy,
                });
            }
            while (true);
        }
    }
}
