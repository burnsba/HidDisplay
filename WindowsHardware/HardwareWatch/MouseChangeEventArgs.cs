using System;
using System.Collections.Generic;
using System.Text;
using WinApi.Windows;
using WindowsHardwareWatch.HardwareWatch.Enums;
using WindowsHardwareWatch.Windows;

namespace WindowsHardwareWatch.HardwareWatch
{
    /// <summary>
    /// Arguments for mouse change event.
    /// </summary>
    public class MouseChangeEventArgs
    {
        private const int Multiplier2 = 100;
        private const int Multiplier3 = 10000;

        /// <summary>
        /// What triggered the event.
        /// </summary>
        public MouseEventType EventType { get; internal set; }

        /// <summary>
        /// Which button triggered the event.
        /// </summary>
        public ButtonSource Button { get; internal set; }

        /// <summary>
        /// Which scroll wheel triggered the event.
        /// </summary>
        public ScrollSource Scroll { get; internal set; }

        /// <summary>
        /// How the button changed to cause the event.
        /// </summary>
        public ButtonChangeDirection ButtonDirection { get; internal set; }

        /// <summary>
        /// How the scroll wheel changed to cause the event.
        /// </summary>
        public ScrollChangeDirection ScrollDirection { get; internal set; }

        /// <summary>
        /// New absolute position of mouse cursor.
        /// </summary>
        public WindowsPoint NewPosition { get; internal set; }

        /// <summary>
        /// Offset between new and previous position of mouse cursor.
        /// </summary>
        public WindowsPoint DeltaPosition { get; internal set; }

        /// <summary>
        /// Gets id of event source.
        /// </summary>
        public int EventSourceId
        {
            get
            {
                return GetEventSourceId(EventType, Scroll, ScrollDirection, Button, ButtonDirection);
            }
        }

        public static int GetEventSourceId(MouseEventType eventType, ScrollSource scroll, ScrollChangeDirection scrollDirection, ButtonSource button, ButtonChangeDirection buttonDirection)
        {
            switch (eventType)
            {
                case MouseEventType.Move:
                    return (int)eventType;

                case MouseEventType.Scroll:
                    return (int)eventType + ((int)scroll * Multiplier2) + ((int)scrollDirection * Multiplier3);

                case MouseEventType.Button:
                    return (int)eventType + ((int)button * Multiplier2) + ((int)buttonDirection * Multiplier3);

                default:
                    return 0;
            }
        }
    }
}
