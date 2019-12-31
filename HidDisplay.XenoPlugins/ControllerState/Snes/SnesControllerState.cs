using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.Controller.ControllerState.Nintendo64
{
    /// <summary>
    /// SNES controller.
    /// </summary>
    public struct SnesControllerState
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
        /// Gets or sets button X.
        /// </summary>
        public bool X { get; set; }

        /// <summary>
        /// Gets or sets button Y.
        /// </summary>
        public bool Y { get; set; }

        /// <summary>
        /// Gets or sets select button.
        /// </summary>
        public bool Select { get; set; }

        /// <summary>
        /// Gets or sets start button.
        /// </summary>
        public bool Start { get; set; }

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
    }
}
