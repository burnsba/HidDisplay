using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.PluginDefinition
{
    /// <summary>
    /// Input event args.
    /// </summary>
    public class GenericInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericInputEventArgs"/> class.
        /// </summary>
        public GenericInputEventArgs()
        {
            OriginTime = DateTime.Now;

            Button2s = new List<Button2>();
            Button3s = new List<Button3>();
            RangeableInputs = new List<IRangeableInput>();
            RangeableInput2s = new List<IRangeableInput2>();
            RangeableInput3s = new List<IRangeableInput3>();
        }

        /// <summary>
        /// Gets or sets list of 2d button events.
        /// </summary>
        /// <remarks>
        /// E.g. keyboard key, mouse button.
        /// </remarks>
        public List<Button2> Button2s { get; set; }

        /// <summary>
        /// Gets or sets list of 3d button events.
        /// </summary>
        /// <remarks>
        /// E.g. mouse scroll.
        /// </remarks>
        public List<Button3> Button3s { get; set; }

        /// <summary>
        /// Gets or sets the time of the input event.
        /// </summary>
        public DateTime OriginTime { get; set; }

        /// <summary>
        /// Gets or sets list of 2d rangeable events.
        /// </summary>
        /// <remarks>
        /// E.g. mouse movement.
        /// </remarks>
        public List<IRangeableInput2> RangeableInput2s { get; set; }

        /// <summary>
        /// Gets or sets list of 3d rangeable events.
        /// </summary>
        /// <remarks>
        /// E.g. 3d accelerometer.
        /// </remarks>
        public List<IRangeableInput3> RangeableInput3s { get; set; }

        /// <summary>
        /// Gets or sets list of 1d rangeable events.
        /// </summary>
        /// <remarks>
        /// E.g. heart rate.
        /// </remarks>
        public List<IRangeableInput> RangeableInputs { get; set; }
    }
}
