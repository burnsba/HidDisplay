using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WinApi.Error;
using WinApi.Hid;

namespace WinApi.Hid
{
    /// <summary>
    /// Managed wrapper for hid calls. Handles pointers, memory allocation, and return results.
    /// </summary>
    public static class Managed
    {

        /// <summary>
        /// Calls hid.HidD_GetManufacturerString.
        /// </summary>
        /// <param name="handle">Open file handle for device.</param>
        /// <returns>Manufacturer.</returns>
        public static string HidD_GetManufacturerString(SafeHandle handle)
        {
            bool bresult;
            int win32error;
            IntPtr pdata = IntPtr.Zero;
            var pdataAllocated = false;

            try
            {
                pdata = Marshal.AllocHGlobal((int)2048);
                pdataAllocated = true;

                bresult = WinApi.Hid.Api.HidD_GetManufacturerString(handle, pdata, 2048);

                if (!bresult)
                {
                    win32error = Marshal.GetLastWin32Error();
                    throw new BadResultException($"HidD_GetManufacturerString, win32error={win32error}") { CallResult = bresult };
                }

                var manufacturer = Marshal.PtrToStringUni(pdata, 2048);

                int index = manufacturer.IndexOf('\0');
                if (index > -1)
                {
                    manufacturer = manufacturer.Substring(0, index);
                }

                return manufacturer;
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
        /// Calls hid.HidD_GetPhysicalDescriptor.
        /// </summary>
        /// <param name="handle">Open file handle for device.</param>
        /// <returns>Physical descriptor.</returns>
        public static string HidD_GetPhysicalDescriptor(SafeHandle handle)
        {
            bool bresult;
            int win32error;
            IntPtr pdata = IntPtr.Zero;
            var pdataAllocated = false;

            try
            {
                pdata = Marshal.AllocHGlobal((int)2048);
                pdataAllocated = true;

                bresult = WinApi.Hid.Api.HidD_GetPhysicalDescriptor(handle, pdata, 2048);

                if (!bresult)
                {
                    win32error = Marshal.GetLastWin32Error();
                    throw new BadResultException($"HidD_GetPhysicalDescriptor, win32error={win32error}") { CallResult = bresult };
                }

                var physicalDescriptor = Marshal.PtrToStringUni(pdata, 2048);

                int index = physicalDescriptor.IndexOf('\0');
                if (index > -1)
                {
                    physicalDescriptor = physicalDescriptor.Substring(0, index);
                }

                return physicalDescriptor;
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
        /// Calls hid.HidD_GetProductString.
        /// </summary>
        /// <param name="handle">Open file handle for device.</param>
        /// <returns>Product string.</returns>
        public static string HidD_GetProductString(SafeHandle handle)
        {
            bool bresult;
            int win32error;
            IntPtr pdata = IntPtr.Zero;
            var pdataAllocated = false;

            try
            {
                pdata = Marshal.AllocHGlobal((int)2048);
                pdataAllocated = true;

                bresult = WinApi.Hid.Api.HidD_GetProductString(handle, pdata, 2048);

                if (!bresult)
                {
                    win32error = Marshal.GetLastWin32Error();
                    throw new BadResultException($"HidD_GetProductString, win32error={win32error}") { CallResult = bresult };
                }

                var productString = Marshal.PtrToStringUni(pdata, 2048);

                int index = productString.IndexOf('\0');
                if (index > -1)
                {
                    productString = productString.Substring(0, index);
                }

                return productString;
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
        /// Calls hid.HidD_GetSerialNumberString.
        /// </summary>
        /// <param name="handle">Open file handle for device.</param>
        /// <returns>Serial number.</returns>
        public static string HidD_GetSerialNumberString(SafeHandle handle)
        {
            bool bresult;
            int win32error;
            IntPtr pdata = IntPtr.Zero;
            var pdataAllocated = false;

            try
            {
                pdata = Marshal.AllocHGlobal((int)2048);
                pdataAllocated = true;

                bresult = WinApi.Hid.Api.HidD_GetSerialNumberString(handle, pdata, 2048);

                if (!bresult)
                {
                    win32error = Marshal.GetLastWin32Error();
                    throw new BadResultException($"HidD_GetSerialNumberString, win32error={win32error}") { CallResult = bresult };
                }

                var serialNumber = Marshal.PtrToStringUni(pdata, 2048);

                int index = serialNumber.IndexOf('\0');
                if (index > -1)
                {
                    serialNumber = serialNumber.Substring(0, index);
                }

                return serialNumber;
            }
            finally
            {
                if (pdataAllocated)
                {
                    Marshal.FreeHGlobal(pdata);
                }
            }
        }
    }
}
