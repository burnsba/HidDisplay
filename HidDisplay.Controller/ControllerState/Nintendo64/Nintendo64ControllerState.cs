using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.Controller.ControllerState.Nintendo64
{
    /// <summary>
    /// Nintendo 64 controller.
    /// </summary>
    public struct Nintendo64ControllerState
    {
        /// <summary>
        /// Gets or sets controller port.
        /// </summary>
        public int ControllerPort { get; set; }

        /// <summary>
        /// Gets or sets button A.
        /// </summary>
        public bool A { get; set; }

        /// <summary>
        /// Gets or sets button B.
        /// </summary>
        public bool B { get; set; }

        /// <summary>
        /// Gets or sets button Z.
        /// </summary>
        public bool Z { get; set; }

        /// <summary>
        /// Gets or sets start button.
        /// </summary>
        public bool Start { get; set; }

        /// <summary>
        /// Gets or sets C up button.
        /// </summary>
        public bool C_Up { get; set; }

        /// <summary>
        /// Gets or sets C down button.
        /// </summary>
        public bool C_Down { get; set; }

        /// <summary>
        /// Gets or sets C left button.
        /// </summary>
        public bool C_Left { get; set; }

        /// <summary>
        /// Gets or sets C right button.
        /// </summary>
        public bool C_Right { get; set; }

        /// <summary>
        /// Gets or sets D pad up.
        /// </summary>
        public bool D_Up { get; set; }

        /// <summary>
        /// Gets or sets D pad down.
        /// </summary>
        public bool D_Down { get; set; }

        /// <summary>
        /// Gets or sets D pad left.
        /// </summary>
        public bool D_Left { get; set; }

        /// <summary>
        /// Gets or sets D pad right.
        /// </summary>
        public bool D_Right { get; set; }

        /// <summary>
        /// Gets or sets shoulder L.
        /// </summary>
        public bool L_Shoulder { get; set; }

        /// <summary>
        /// Gets or sets shoulder R.
        /// </summary>
        public bool R_Shoulder { get; set; }

        /// <summary>
        /// Gets or sets analog stick X value (actual one signed byte).
        /// </summary>
        public short AnalogX { get; set; }

        /// <summary>
        /// Gets or sets analog stick Y value (actual one signed byte).
        /// </summary>
        public short AnalogY { get; set; }
    }
}
