using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WinApi.Error;
using WinApi.Hid;

namespace WinApi.User32
{
    /// <summary>
    /// Managed wrapper for user32 calls. Handles pointers, memory allocation, and return results.
    /// </summary>
    public static class Managed
    {
        /// <summary>
        /// A convenient function for getting all raw input devices.
        /// This method will get all devices, including virtual devices
        /// For remote desktop and any other device driver that's registered
        /// as such a device.
        /// </summary>
        /// <returns></returns>
        public static RawInputDeviceList[] GetAllRawDevices()
        {
            uint deviceCount = 0;
            uint dwSize = (uint)Marshal.SizeOf(typeof(RawInputDeviceList));

            // First call the system routine with a null pointer
            // for the array to get the size needed for the list
            uint retValue = WinApi.User32.Api.GetRawInputDeviceList(null, ref deviceCount, dwSize);

            // If anything but zero is returned, the call failed, so return a null list
            if (0 != retValue)
                return null;

            // Now allocate an array of the specified number of entries
            RawInputDeviceList[] deviceList = new RawInputDeviceList[deviceCount];

            // Now make the call again, using the array
            retValue = WinApi.User32.Api.GetRawInputDeviceList(deviceList, ref deviceCount, dwSize);

            if (retValue != dwSize)
            {
                var err = Marshal.GetLastWin32Error();
                throw new BadResultException($"GetRawInputDeviceList, win32error={err}") { CallResult = dwSize };
            }

            // Finally, return the filled in list
            return deviceList;
        }

        /// <summary>
        /// Calls User32.GetRawInputData. Accepts lParam from WndProc and retrieves the RawInput information.
        /// </summary>
        /// <param name="lParam">Pointer.</param>
        /// <returns>RawInput.</returns>
        public static RawInput GetRawInputData(IntPtr lParam)
        {
            if (lParam == IntPtr.Zero)
            {
                throw new ArgumentException($"{nameof(lParam)} pointer is not set.");
            }

            uint dwSize = 0;
            int callResult;
            int win32error;

            IntPtr rawInputBuffer = IntPtr.Zero;
            bool rawInputBufferAllocated = false;

            try
            {
                callResult = WinApi.User32.Api.GetRawInputData(lParam, RawInputCommand.Input, IntPtr.Zero, ref dwSize, Marshal.SizeOf(typeof(RawInputHeader)));

                if (callResult != 0)
                {
                    win32error = Marshal.GetLastWin32Error();
                    if (win32error > 0)
                    {
                        throw new Win32ErrorCode($"GetLastWin32Error: {win32error}");
                    }
                    else
                    {
                        throw new BadResultException($"GetRawInputData returned {callResult} (pData == null)");
                    }
                }

                rawInputBuffer = Marshal.AllocHGlobal((int)dwSize);
                rawInputBufferAllocated = true;
                RawInput ri;

                callResult = WinApi.User32.Api.GetRawInputData(lParam, RawInputCommand.Input, rawInputBuffer, ref dwSize, Marshal.SizeOf(typeof(RawInputHeader)));

                if (callResult != dwSize)
                {
                    win32error = Marshal.GetLastWin32Error();
                    if (win32error > 0)
                    {
                        throw new Win32ErrorCode($"GetLastWin32Error: {win32error}");
                    }
                    else
                    {
                        throw new BadResultException($"GetRawInputData returned {callResult} (retrieving rawInputBuffer, dwSize={dwSize})");
                    }
                }

                var ribytes = new byte[dwSize];
                Marshal.Copy(rawInputBuffer, ribytes, 0, (int)dwSize);
                ri = RawInput.FromBytes(ribytes, 0, out int t);

                return ri;
            }
            finally
            {
                if (rawInputBufferAllocated)
                {
                    Marshal.FreeHGlobal(rawInputBuffer);
                }
            }
        }

        /// <summary>
        /// Calls User32.GetRawInputDeviceInfo.
        /// </summary>
        /// <param name="inputDevice">Pointer from RawInput header to device.</param>
        /// <returns>Windows device name.</returns>
        public static string GetRawInputDeviceName(IntPtr inputDevice)
        {
            if (inputDevice == IntPtr.Zero)
            {
                throw new ArgumentException($"{nameof(inputDevice)} pointer is not set.");
            }

            uint dwsize = 0;
            int callResult;
            int win32error;
            IntPtr pdata = IntPtr.Zero;
            var pdataAllocated = false;

            try
            {
                callResult = WinApi.User32.Api.GetRawInputDeviceInfo(inputDevice, GetRawInputDeviceInfoCommand.RIDI_DEVICENAME, IntPtr.Zero, ref dwsize);
                if (callResult < 0)
                {
                    throw new BadResultException("GetRawInputDeviceInfo(RIDI_DEVICENAME), pData == null") { CallResult = callResult };
                }

                pdata = Marshal.AllocHGlobal((int)dwsize);
                callResult = WinApi.User32.Api.GetRawInputDeviceInfo(inputDevice, GetRawInputDeviceInfoCommand.RIDI_DEVICENAME, pdata, ref dwsize);

                win32error = Marshal.GetLastWin32Error();

                if (win32error > 0)
                {
                    throw new Win32ErrorCode($"GetLastWin32Error: {win32error}") { ErrorCode = win32error };
                }

                if (callResult <= 0)
                {
                    throw new BadResultException($"GetRawInputDeviceInfo (RIDI_DEVICENAME), pData allocated") { CallResult = callResult };
                }

                var deviceName = Marshal.PtrToStringAnsi(pdata);

                return deviceName;
            }
            finally
            {
                if (pdataAllocated)
                {
                    Marshal.FreeHGlobal(pdata);
                }
            }
        }

        /// <summary>
        /// Calls Hid.HidP_GetCaps. Gets HID capabilities.
        /// </summary>
        /// <param name="preparsedData">Pointer to preparsed data structure.</param>
        /// <returns>HID capabilities.</returns>
        public static HidpCaps HidpGetCapabilities(IntPtr preparsedData)
        {
            if (preparsedData == IntPtr.Zero)
            {
                throw new ArgumentException($"{nameof(preparsedData)} pointer is not set.");
            }

            HidpStatus hidpstatus;
            HidpCaps hidpcaps;

            hidpstatus = (HidpStatus)WinApi.Hid.Api.HidP_GetCaps(preparsedData, out hidpcaps);
            if (hidpstatus != HidpStatus.HIDP_STATUS_SUCCESS)
            {
                throw new HidpStatusException($"HidP_GetCaps: {hidpstatus.ToString()}") { StatusCode = hidpstatus };
            }

            return hidpcaps;
        }

        /// <summary>
        /// Calls Hid.HidP_GetButtonCaps. Gets HID button capabilities.
        /// </summary>
        /// <param name="hidpcaps">HID capabilities.</param>
        /// <param name="preparsedData">Pointer to preparsed data structure.</param>
        /// <returns>HID button capabilities.</returns>
        public static HidpButtonCaps[] HidpGetButtonCapabilities(HidpCaps hidpcaps, IntPtr preparsedData)
        {
            if (preparsedData == IntPtr.Zero)
            {
                throw new ArgumentException($"{nameof(preparsedData)} pointer is not set.");
            }

            uint dwSize;
            HidpStatus hidpstatus;
            IntPtr pbuttoncaps = IntPtr.Zero;
            bool pbuttoncapsAllocated = false;

            try
            {
                dwSize = hidpcaps.NumberInputButtonCaps;
                var bytelength = Marshal.SizeOf(typeof(HidpButtonCaps)) * (int)dwSize;
                HidpButtonCaps[] buttonCaps = new HidpButtonCaps[(int)dwSize];

                pbuttoncaps = Marshal.AllocHGlobal(bytelength);
                pbuttoncapsAllocated = true;

                hidpstatus = (HidpStatus)WinApi.Hid.Api.HidP_GetButtonCaps(HidpReportType.HidP_Input, pbuttoncaps, ref dwSize, preparsedData);
                if (hidpstatus != HidpStatus.HIDP_STATUS_SUCCESS)
                {
                    throw new HidpStatusException($"HidP_GetButtonCaps: {hidpstatus.ToString()}") { StatusCode = hidpstatus };
                }

                var buttonCapBytes = new byte[bytelength];
                Marshal.Copy(pbuttoncaps, buttonCapBytes, 0, bytelength);
                for (int i = 0; i < (int)hidpcaps.NumberInputButtonCaps; i++)
                {
                    buttonCaps[i] = HidpButtonCaps.FromBytes(buttonCapBytes, i * Marshal.SizeOf(typeof(HidpButtonCaps)), out int t);
                }

                return buttonCaps;
            }
            finally
            {
                if (pbuttoncapsAllocated)
                {
                    Marshal.FreeHGlobal(pbuttoncaps); 
                }
            }
        }

        /// <summary>
        /// Calls Hid.HidP_GetValueCaps. Gets HID button capabilities.
        /// </summary>
        /// <param name="hidpcaps">HID capabilities.</param>
        /// <param name="preparsedData">Pointer to preparsed data structure.</param>
        /// <returns>HID button capabilities.</returns>
        public static HidpValueCaps[] HidpGetValueCapabilities(HidpCaps hidpcaps, IntPtr preparsedData)
        {
            if (preparsedData == IntPtr.Zero)
            {
                throw new ArgumentException($"{nameof(preparsedData)} pointer is not set.");
            }

            uint dwSize;
            HidpStatus hidpstatus;
            IntPtr pvaluecaps = IntPtr.Zero;
            bool pvaluecapsAllocated = false;

            try
            {
                dwSize = hidpcaps.NumberInputValueCaps;
                var bytelength = Marshal.SizeOf(typeof(HidpValueCaps)) * (int)dwSize;
                HidpValueCaps[] valueCaps = new HidpValueCaps[(int)dwSize];

                pvaluecaps = Marshal.AllocHGlobal(bytelength);
                pvaluecapsAllocated = true;

                hidpstatus = (HidpStatus)WinApi.Hid.Api.HidP_GetValueCaps(HidpReportType.HidP_Input, pvaluecaps, ref dwSize, preparsedData);
                if (hidpstatus != HidpStatus.HIDP_STATUS_SUCCESS)
                {
                    throw new HidpStatusException($"HidP_GetValueCaps: {hidpstatus.ToString()}") { StatusCode = hidpstatus };
                }

                var valueCapBytes = new byte[bytelength];
                Marshal.Copy(pvaluecaps, valueCapBytes, 0, bytelength);
                for (int i = 0; i < (int)hidpcaps.NumberInputValueCaps; i++)
                {
                    valueCaps[i] = HidpValueCaps.FromBytes(valueCapBytes, i * Marshal.SizeOf(typeof(HidpValueCaps)), out int t);
                }

                return valueCaps;
            }
            finally
            {
                if (pvaluecapsAllocated)
                {
                    Marshal.FreeHGlobal(pvaluecaps);
                }
            }
        }
    }
}
