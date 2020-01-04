using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.Controller.ControllerState.Nintendo64
{
    /// <summary>
    /// SNES controller.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Button constants.")]
    public struct SnesControllerState
    {
        /// <summary>
        /// Gets or sets controller port.
        /// </summary>
        public int ControllerPort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether button A is pressed.
        /// </summary>
        public bool A { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether button B is pressed.
        /// </summary>
        public bool B { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether button X is pressed.
        /// </summary>
        public bool X { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether button Y is pressed.
        /// </summary>
        public bool Y { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether select button is pressed.
        /// </summary>
        public bool Select { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether start button is pressed.
        /// </summary>
        public bool Start { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether D pad up is pressed.
        /// </summary>
        public bool D_Up { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether D pad down is pressed.
        /// </summary>
        public bool D_Down { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether D pad left is pressed.
        /// </summary>
        public bool D_Left { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether D pad right is pressed.
        /// </summary>
        public bool D_Right { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shoulder L is pressed.
        /// </summary>
        public bool L_Shoulder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shoulder R is pressed.
        /// </summary>
        public bool R_Shoulder { get; set; }
    }
}
