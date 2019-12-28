using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinApi.Error;
using WinApi.Hid;
using WinApi.User32;
using WindowsHardware.HardwareWatch;

namespace EventFinderHid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowsHardware.HardwareWatch.RawInputHandler _rih = new WindowsHardware.HardwareWatch.RawInputHandler();

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);

            var win = source.Handle;

            RawInputDevice[] rid = new RawInputDevice[2];

            rid[0].UsagePage = HidUsagePages.GenericDesktop;
            rid[0].Usage = (ushort)WinApi.Hid.Usage.GenericDesktop.GamePad;
            rid[0].Flags = RawInputDeviceFlags.InputSink;
            rid[0].WindowHandle = win;

            rid[1].UsagePage = HidUsagePages.GenericDesktop;
            rid[1].Usage = (ushort)WinApi.Hid.Usage.GenericDesktop.Joystick;
            rid[1].Flags = RawInputDeviceFlags.InputSink;
            rid[1].WindowHandle = win;

            if (WinApi.User32.Api.RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0])) == false)
            {
                var err = Marshal.GetLastWin32Error();
                if (err > 0)
                {
                    throw new Win32ErrorCode($"GetLastWin32Error: {err}");
                }
                else
                {
                    throw new Win32ErrorCode("RegisterRawInputDevices failed with error code 0. Parameter count mis-match?");
                }
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case (int)WinApi.Windows.WindowsMessages.INPUT:
                    {
                        System.Diagnostics.Debug.WriteLine("Received WndProc.WM_INPUT");
                        var hidResult = _rih.WindowsMessageExtract(lParam);

                        System.Diagnostics.Debug.WriteLine($"HID input manufacturer: {hidResult.HidDeviceInfo.GetManufacturer()}");
                        System.Diagnostics.Debug.WriteLine($"HID product description: {hidResult.HidDeviceInfo.GetProduct()}");

                        System.Diagnostics.Debug.WriteLine(String.Join(",", hidResult.ButtonStates.Select(x => x ? "1" : "0")));
                        var xval = hidResult.UsageValues.First(x => x.UsagePage == HidUsagePages.GenericDesktop && x.Usage == (uint)WinApi.Hid.Usage.GenericDesktop.X);
                        var yval = hidResult.UsageValues.First(x => x.UsagePage == HidUsagePages.GenericDesktop && x.Usage == (uint)WinApi.Hid.Usage.GenericDesktop.Y);
                        System.Diagnostics.Debug.WriteLine($"{xval.Value},{yval.Value}");
                    }

                    break;
            }

            return hwnd;
        }
    }
}
