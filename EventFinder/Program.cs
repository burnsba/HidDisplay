using System;
using System.Threading;
using BurnsBac.WindowsHardware.HardwareWatch;
using BurnsBac.WindowsHardware.HardwareWatch.Enums;
using BurnsBac.WindowsHardware.Windows;

namespace EventFinder
{
    /// <summary>
    /// Helper program, hooks mouse and keyboard events (for the current exe) and displays the event codes.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            bool shutdown = false;

            var keyboardWatcher = new KeyboardWatcher();
            keyboardWatcher.Setup(WindowTitleMatch.Contains, "EventFinder.exe");

            keyboardWatcher.KeyboardChangeEvent += (s, e) =>
            {
                Console.WriteLine($"{e.Direction.ToString()} {(e.Alt ? "ALT " : "")}{e.Key.ToString()} = {(int)e.Key}");
            };

            var mouseWatcher = new MouseWatcher();
            mouseWatcher.Setup(WindowTitleMatch.Contains, "EventFinder.exe");

            mouseWatcher.MouseChangeEvent += (s, e) =>
            {
                if (e.EventType == MouseEventType.Button)
                {
                    Console.WriteLine($"{e.EventType.ToString()} {e.Button.ToString()} (={(int)e.Button}) {e.ButtonDirection.ToString()}");
                }
                else if (e.EventType == MouseEventType.Scroll)
                {
                    Console.WriteLine($"{e.EventType.ToString()} {e.Scroll.ToString()} (={(int)e.Scroll}) {e.ScrollDirection.ToString()}");
                }
                else if (e.EventType == MouseEventType.Move)
                {
                    Console.WriteLine($"{e.EventType.ToString()} delta: {e.DeltaPosition.X}, {e.DeltaPosition.Y} -- new: {e.NewPosition.X}, {e.NewPosition.Y}");
                }
            };

            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;

                keyboardWatcher.Dispose();
                mouseWatcher.Dispose();

                shutdown = true;
            };

            // Unlike the wpf app, the console needs to pump windows messages to receive events.
            var m = MessagePump.Instance.GetEnumerator();
            while (shutdown == false && m.MoveNext())
            {
                Thread.Sleep(25);
            }
        }
    }
}
