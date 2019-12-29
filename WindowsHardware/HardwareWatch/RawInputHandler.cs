using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WinApi.Error;
using WinApi.Hid;
using WinApi.User32;

namespace WindowsHardware.HardwareWatch
{
    /// <summary>
    /// This class is designed to accept a WndProc message for raw input and extract HID information.
    /// </summary>
    /// <remarks>
    /// Raw input stuff happens in this class, all the HID device data should be over in <see cref="HidDeviceInfo"/>.
    /// </remarks>
    public class RawInputHandler : IDisposable
    {
        /// <summary>
        /// Devices seens by this handler. Key is the raw input device handle, not file handle.
        /// </summary>
        private Dictionary<IntPtr, HidDeviceInfo> _devices = new Dictionary<IntPtr, HidDeviceInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RawInputHandler"/> class.
        /// </summary>
        public RawInputHandler()
        {

        }

        /// <summary>
        /// Nothing to dispose in this class. Calls dispose on children <see cref="HidDeviceInfo"/>.
        /// </summary>
        public void Dispose()
        {
            foreach (var kvp in _devices)
            {
                kvp.Value.Dispose();
            }
        }

        public RawInput WndProcToRawInput(IntPtr lParam)
        {
            var ri = WinApi.User32.Managed.GetRawInputData(lParam);
            return ri;
        }

        /// <summary>
        /// Extracts HID information from RawInput message.
        /// </summary>
        /// <param name="ri">RawInput.</param>
        /// <returns>Associated device information and current button/range status are returned.</returns>
        public HidResult RawInputHidDetail(RawInput ri)
        {
            IntPtr preparsedData = IntPtr.Zero;
            var preparsedDataAllocated = false;
            var result = new HidResult();

            try
            {
                preparsedData = GetPreparsedData(ri);
                preparsedDataAllocated = true;

                // Lookup capabilities in cache.
                HidDeviceInfo hidDevice;
                if (!_devices.TryGetValue(ri.Header.hDevice, out hidDevice))
                {
                    hidDevice = new HidDeviceInfo(ri.Header.hDevice);
                    _devices.Add(ri.Header.hDevice, hidDevice);
                }

                // Load up hardware description stuff
                hidDevice.GetDeviceNameFull();
                hidDevice.GetManufacturer();
                hidDevice.GetPhysicalDescriptor();
                hidDevice.GetSerialNumber();
                hidDevice.GetProduct();
                hidDevice.GetRegistryDescription();

                result.HidDeviceInfo = hidDevice;

                if (ri.Header.dwType == RawInputDeviceType.Hid)
                {
                    // Ensure the following information is loaded
                    hidDevice.GetCapabilities(preparsedData);
                    hidDevice.GetButtonCapabilities(preparsedData);
                    hidDevice.GetValueCapabilities(preparsedData);

                    var availableReports = GetAllReports(hidDevice, ri);

                    // Get button status.
                    if (hidDevice.HidpCapabilities.NumberInputButtonCaps > 0)
                    {
                        // It's not exactly clear to me, but I think you only need to do this once, just
                        // on the first buttonCaps collection.
                        //
                        // It might be the case that you're supposed to check for usages or multiple reports
                        // but only one report is used in practice? Not really sure.
                        //
                        // https://www.codeproject.com/Articles/185522/Using-the-Raw-Input-API-to-Process-Joystick-Input

                        var buttonStatus = GetButtonUsageList(hidDevice, preparsedData, availableReports);

                        result.ButtonIndexActive = buttonStatus.Item2;

                        var numberOfButtons = (hidDevice.ButtonCaps[0].Range.UsageMax - hidDevice.ButtonCaps[0].Range.UsageMin + 1);
                        var bButtonStates = new byte[numberOfButtons];

                        for (int i = 0; i < buttonStatus.Item1; i++)
                            bButtonStates[result.ButtonIndexActive[i] - hidDevice.ButtonCaps[0].Range.UsageMin] = 1;

                        result.ButtonStates = bButtonStates.Select(x => x > 0 ? true : false).ToArray();
                    }

                    // Now get other value status.
                    if (hidDevice.HidpCapabilities.NumberInputValueCaps > 0)
                    {
                        result.UsageValues = GetUsageValues(hidDevice, preparsedData, availableReports);
                    }
                }

                return result;
            }
            finally
            {
                if (preparsedDataAllocated)
                {
                    Marshal.FreeHGlobal(preparsedData);
                }
            }
        }

        /// <summary>
        /// I didn't want to stick this in the <see cref="WinApi.Hid.Managed"/> class as this allocates memory
        /// and returns, then you use that object (pPreparsedData) to make other calls.
        /// </summary>
        /// <param name="ri">RawInput object, just need the device handle in the header.</param>
        /// <returns>Allocated memory for the preparsedData object.</returns>
        private IntPtr GetPreparsedData(RawInput ri)
        {
            uint dwSize = 0;
            int callResult;
            int win32error;

            callResult = WinApi.User32.Api.GetRawInputDeviceInfo(ri.Header.hDevice, GetRawInputDeviceInfoCommand.RIDI_PREPARSEDDATA, IntPtr.Zero, ref dwSize);
            if (callResult < 0)
            {
                throw new BadResultException("GetRawInputDeviceInfo(RIDI_PREPARSEDDATA), pData == null") { CallResult = callResult };
            }

            IntPtr pPreparsedData = Marshal.AllocHGlobal((int)dwSize);

            callResult = WinApi.User32.Api.GetRawInputDeviceInfo(ri.Header.hDevice, GetRawInputDeviceInfoCommand.RIDI_PREPARSEDDATA, pPreparsedData, ref dwSize);
            
            win32error = Marshal.GetLastWin32Error();

            if (win32error > 0)
            {
                throw new Win32ErrorCode($"GetLastWin32Error: {win32error}") { ErrorCode = win32error };
            }

            if (callResult < 0)
            {
                throw new BadResultException($"GetRawInputDeviceInfo (RIDI_PREPARSEDDATA), pData allocated") { CallResult = callResult };
            }

            return pPreparsedData;
        }

        /// <summary>
        /// Retrieves all current HID input reports for the device. Requires the file
        /// handle be available (this will attempt to create if it doesn't exist).
        /// </summary>
        /// <param name="hidDevice">Hid device.</param>
        /// <returns>All available input reports.</returns>
        private Dictionary<int, byte[]> GetAllReports(HidDeviceInfo hidDevice, RawInput ri)
        {
            bool bres;
            Dictionary<int, byte[]> availableReports = new Dictionary<int, byte[]>();

            int reportBufferSize = hidDevice.HidpCapabilities.InputReportByteLength;
            var reportIds = hidDevice
                .ButtonCaps.Select(x => x.ReportID)
                .Union(hidDevice.ValueCaps.Select(x => x.ReportID))
                .Distinct();

            if (reportIds.Count() > 1)
            {
                var fileHandle = hidDevice.GetFileHandle();
                foreach (var reportId in reportIds)
                {
                    byte[] reportBuffer = new byte[reportBufferSize];
                    reportBuffer[0] = reportId;
                    bres = WinApi.Hid.Api.HidD_GetInputReport(fileHandle, reportBuffer, (uint)reportBufferSize);
                    if (!bres)
                    {
                        var err = Marshal.GetLastWin32Error();
                        throw new BadResultException($"HidD_GetInputReport, win32error={err}") { CallResult = bres };
                    }

                    availableReports.Add(reportId, reportBuffer);
                }
            }
            else
            {
                // If there's only one report, copy from the rawinput buffer.
                // This avoids the dll call, but also avoids a bug in HidD_GetInputReport 
                // when the only reportid is zero.
                byte[] reportBuffer = (byte[])ri.Data.Hid.bRawData.Clone();

                availableReports.Add(reportIds.First(), reportBuffer);
            }

            return availableReports;
        }

        /// <summary>
        /// Gets current button status.
        /// </summary>
        /// <param name="hidDevice">Hid device.</param>
        /// <param name="preparsedData">Preparsed data.</param>
        /// <param name="availableReports">All input reports.</param>
        /// <returns>Number of buttons set, and the status of each button.</returns>
        private (uint, ushort[]) GetButtonUsageList(HidDeviceInfo hidDevice, IntPtr preparsedData, Dictionary<int, byte[]> availableReports)
        {
            HidpStatus hidpstatus;
            var buttonCaps0 = hidDevice.ButtonCaps[0];

            uint altUsageLength = (uint)(buttonCaps0.Range.UsageMax - buttonCaps0.Range.UsageMin + 1);
            ushort[] usageList = new ushort[altUsageLength];

            var reportBytes = availableReports[buttonCaps0.ReportID];

            hidpstatus = (HidpStatus)WinApi.Hid.Api.HidP_GetUsages(
                HidpReportType.HidP_Input,
                buttonCaps0.UsagePage,
                0,
                usageList,
                ref altUsageLength,
                preparsedData,
                reportBytes,
                (uint)reportBytes.Length);

            if (hidpstatus != HidpStatus.HIDP_STATUS_SUCCESS)
            {
                throw new HidpStatusException($"HidP_GetUsages: {hidpstatus.ToString()}") { StatusCode = hidpstatus };
            }

            return (altUsageLength, usageList);
        }

        /// <summary>
        /// Gets current HID status for things with ranges (controller sticks).
        /// </summary>
        /// <param name="hidDevice">Hid device.</param>
        /// <param name="preparsedData">Preparsed data.</param>
        /// <param name="availableReports">All input reports.</param>
        /// <returns>Range data.</returns>
        private List<HidResult.UsagePageUsageValue> GetUsageValues(HidDeviceInfo hidDevice, IntPtr preparsedData, Dictionary<int, byte[]> availableReports)
        {
            HidpStatus hidpstatus;
            uint usageValue;

            var results = new List<HidResult.UsagePageUsageValue>();

            for (int i = 0; i < hidDevice.HidpCapabilities.NumberInputValueCaps; i++)
            {
                var valueCapabilitiy = hidDevice.ValueCaps[i];
                var report = availableReports[valueCapabilitiy.ReportID];

                ushort usage = valueCapabilitiy.IsRange ? valueCapabilitiy.Range.UsageMin : valueCapabilitiy.NotRange.Usage;
                hidpstatus = (HidpStatus)WinApi.Hid.Api.HidP_GetUsageValue(
                    HidpReportType.HidP_Input,
                    valueCapabilitiy.UsagePage,
                    0,
                    usage,
                    out usageValue,
                    preparsedData,
                    report,
                    (uint)report.Length);

                if (hidpstatus != HidpStatus.HIDP_STATUS_SUCCESS)
                {
                    throw new HidpStatusException($"HidP_GetUsages: {hidpstatus.ToString()}") { StatusCode = hidpstatus };
                }
                else
                {
                    results.Add(new HidResult.UsagePageUsageValue() { Usage = usage, UsagePage = (WinApi.Hid.HidUsagePages)valueCapabilitiy.UsagePage, Value = usageValue });
                }
            }

            return results;
        }
    }
}
