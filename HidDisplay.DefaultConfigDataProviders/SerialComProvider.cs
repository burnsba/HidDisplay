﻿using System;
using System.Collections.Generic;
using System.Linq;
using BurnsBac.HotConfig.DataSource;
using Microsoft.Win32;

namespace HidDisplay.DefaultConfigDataProviders
{
    /// <summary>
    /// Lists available serial ports.
    /// </summary>
    public class SerialComProvider : IConfigDataProviderOnce
    {
        /// <inheritdoc />
        public Dictionary<string, string> FetchData()
        {
            var portList = GetPortNames();

            return portList.ToDictionary(x => x);
        }

        /// <summary>
        /// Gets list of serial com ports.
        /// </summary>
        /// <returns>List of serial com ports.</returns>
        /// <remarks>
        /// Had assembly loading problems just trying to call SerialPort.GetPortNames, so copied implementation here.
        /// https://github.com/dotnet/runtime/blob/master/src/libraries/System.IO.Ports/src/System/IO/Ports/SerialPort.Win32.cs .
        /// </remarks>
        private String[] GetPortNames()
        {
            // Hitting the registry for this isn't the only way to get the ports.
            //
            // WMI: https://msdn.microsoft.com/en-us/library/aa394413.aspx
            // QueryDosDevice: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365461.aspx
            //
            // QueryDosDevice involves finding any ports that map to \Device\Serialx (call with null to get all, then iterate to get the actual device name)
            /////////

            using (RegistryKey serialKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM"))
            {
                if (serialKey != null)
                {
                    string[] result = serialKey.GetValueNames();
                    for (int i = 0; i < result.Length; i++)
                    {
                        // Replace the name in the array with its value.
                        result[i] = (string)serialKey.GetValue(result[i]);
                    }

                    return result;
                }
            }

            return Array.Empty<string>();
        }
    }
}
