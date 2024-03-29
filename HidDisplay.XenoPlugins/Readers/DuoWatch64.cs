﻿using System;
using System.Collections.Generic;
using BurnsBac.WindowsHardware.SerialPort;
using HidDisplay.Controller.ControllerState.Nintendo64;

namespace HidDisplay.Controller.Readers
{
    /// <summary>
    /// Listens to serial port for DuoWatch64 protocol messages and forwards state change events.
    /// </summary>
    public class DuoWatch64 : SerialTranslatorBase
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

            int button_received_count;

            do
            {
                button_received_count = 0;

                endPacketIndex = _readBuffer.IndexOf((byte)'|');
                if (endPacketIndex < 0)
                {
                    return;
                }

                if (endPacketIndex < 7)
                {
                    try
                    {
                        _readBuffer.RemoveRange(0, endPacketIndex + 1);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        // someone modified the list between the above statements.
                        // probably safe to ignore.
                    }

                    continue;
                }

                startPacketIndex = _readBuffer.LastIndexOf((byte)'>', endPacketIndex - 1);
                if (startPacketIndex < 0)
                {
                    return;
                }

                var dataLen = endPacketIndex - startPacketIndex;

                if (startPacketIndex > endPacketIndex)
                {
                    try
                    {
                        _readBuffer.RemoveRange(0, startPacketIndex);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        // someone modified the list between the above statements.
                        // probably safe to ignore.
                    }

                    continue;
                }

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
                    return;
                }

                var bits = new byte[32];

                a = (dataBytes[3] & 0x01) > 0;
                button_received_count += a ? 1 : 0;

                b = (dataBytes[3] & 0x02) > 0;
                button_received_count += b ? 1 : 0;

                z = (dataBytes[3] & 0x04) > 0;
                button_received_count += z ? 1 : 0;

                start = (dataBytes[3] & 0x08) > 0;
                button_received_count += start ? 1 : 0;

                d_up = (dataBytes[3] & 0x10) > 0;
                button_received_count += d_up ? 1 : 0;

                d_down = (dataBytes[3] & 0x20) > 0;
                button_received_count += d_down ? 1 : 0;

                d_left = (dataBytes[3] & 0x40) > 0;
                button_received_count += d_left ? 1 : 0;

                d_right = (dataBytes[3] & 0x80) > 0;
                button_received_count += d_right ? 1 : 0;

                //// unused // bits[8] = ((dataBytes[4] & 0x01) > 0) ? (byte)100 : (byte)0;
                //// unused // bits[9] = ((dataBytes[4] & 0x02) > 0) ? (byte)100 : (byte)0;

                l_shoulder = (dataBytes[4] & 0x04) > 0;
                button_received_count += l_shoulder ? 1 : 0;

                r_shoulder = (dataBytes[4] & 0x08) > 0;
                button_received_count += r_shoulder ? 1 : 0;

                c_up = (dataBytes[4] & 0x10) > 0;
                button_received_count += c_up ? 1 : 0;

                c_down = (dataBytes[4] & 0x20) > 0;
                button_received_count += c_down ? 1 : 0;

                c_left = (dataBytes[4] & 0x40) > 0;
                button_received_count += c_left ? 1 : 0;

                c_right = (dataBytes[4] & 0x80) > 0;
                button_received_count += c_right ? 1 : 0;

                // analog inputs are each one signed integer byte
                analogx = (short)Utility.MakeSignedReverseByte(dataBytes[5]);
                analogy = (short)Utility.MakeSignedReverseByte(dataBytes[6]);

                int port;

                if (!int.TryParse((dataBytes[1] - '0').ToString(), out port))
                {
                    // invalid packet
                    continue;
                }

                if (dataBytes[3] == 0xff && (dataBytes[4] & 0xfc) == 0xfc)
                {
                    // invalid packet
                    continue;
                }

                // try to filter out bad packets
                if (button_received_count > 7)
                {
                    // invalid packet
                    continue;
                }

                Nintendo64ControllerStateChange(this, new Nintendo64ControllerState()
                {
                    ControllerPort = port,
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
