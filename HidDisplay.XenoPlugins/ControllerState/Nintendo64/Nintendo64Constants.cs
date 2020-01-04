using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.Controller.ControllerState.Nintendo64
{
    /// <summary>
    /// Constants for Nintendo64 controller.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Button constants.")]
    public static class Nintendo64Constants
    {
        /// <summary>
        /// HidDisplay button A.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 0.
        ///
        /// I don't want to assign this id 0, so I'm using
        /// the first non-bit in the console communication protocol.
        /// Id: 32.
        /// </remarks>
        public const int Button_A_Id = 32;

        /// <summary>
        /// HidDisplay button B.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 1.
        /// Id: 1.
        /// </remarks>
        public const int Button_B_Id = 1;

        /// <summary>
        /// HidDisplay button Z.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 2.
        /// Id: 2.
        /// </remarks>
        public const int Button_Z_Id = 2;

        /// <summary>
        /// HidDisplay start button.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 3.
        /// Id: 3.
        /// </remarks>
        public const int Button_Start_Id = 3;

        /// <summary>
        /// HidDisplay C up button.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 4.
        /// Id: 4.
        /// </remarks>
        public const int Button_C_Up_Id = 4;

        /// <summary>
        /// HidDisplay C down button.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 5.
        /// Id: 5.
        /// </remarks>
        public const int Button_C_Down_Id = 5;

        /// <summary>
        /// HidDisplay C left button.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 6.
        /// Id: 6.
        /// </remarks>
        public const int Button_C_Left_Id = 6;

        /// <summary>
        /// HidDisplay C right button.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 7.
        /// Id: 7.
        /// </remarks>
        public const int Button_C_Right_Id = 7;

        /// <summary>
        /// HidDisplay shoulder L.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 10.
        /// Id: 10.
        /// </remarks>
        public const int Button_L_Shoulder_Id = 10;

        /// <summary>
        /// HidDisplay shoulder R.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 11.
        /// Id: 11.
        /// </remarks>
        public const int Button_R_Shoulder_Id = 11;

        /// <summary>
        /// HidDisplay D pad up.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 12.
        /// Id: 12.
        /// </remarks>
        public const int Button_D_Up_Id = 12;

        /// <summary>
        /// HidDisplay D pad down.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 13.
        /// Id: 13.
        /// </remarks>
        public const int Button_D_Down_Id = 13;

        /// <summary>
        /// HidDisplay D pad left.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 14.
        /// Id: 14.
        /// </remarks>
        public const int Button_D_Left_Id = 14;

        /// <summary>
        /// HidDisplay D pad right.
        /// </summary>
        /// <remarks>
        /// Source protocol bit: 15.
        /// Id: 15.
        /// </remarks>
        public const int Button_D_Right_Id = 15;
    }
}
