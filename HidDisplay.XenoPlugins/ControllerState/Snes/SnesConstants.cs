using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.Controller.ControllerState.Nintendo64
{
    /// <summary>
    /// Constants for Nintendo64 controller.
    /// </summary>
    /// <remarks>
    /// Based on https://gamefaqs.gamespot.com/snes/916396-super-nintendo/faqs/5395 .
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Button constants.")]
    public static class SnesConstants
    {
        /// <summary>
        /// HidDisplay button B.
        /// </summary>
        /// <remarks>
        /// Source clock 1.
        /// </remarks>
        public const int Button_B_Id = 1;

        /// <summary>
        /// HidDisplay button Y.
        /// </summary>
        /// <remarks>
        /// Source clock 2.
        /// </remarks>
        public const int Button_Y_Id = 2;

        /// <summary>
        /// HidDisplay button Select.
        /// </summary>
        /// <remarks>
        /// Source clock 3.
        /// </remarks>
        public const int Button_Select_Id = 3;

        /// <summary>
        /// HidDisplay start Start.
        /// </summary>
        /// <remarks>
        /// Source clock 4.
        /// </remarks>
        public const int Button_Start_Id = 4;

        /// <summary>
        /// HidDisplay D pad up.
        /// </summary>
        /// <remarks>
        /// Source clock 5.
        /// </remarks>
        public const int Button_D_Up_Id = 5;

        /// <summary>
        /// HidDisplay D pad down.
        /// </summary>
        /// <remarks>
        /// Source clock 6 .
        /// </remarks>
        public const int Button_D_Down_Id = 6;

        /// <summary>
        /// HidDisplay D pad left.
        /// </summary>
        /// <remarks>
        /// Source clock 7.
        /// </remarks>
        public const int Button_D_Left_Id = 7;

        /// <summary>
        /// HidDisplay D pad right.
        /// </summary>
        /// <remarks>
        /// Source clock 8.
        /// </remarks>
        public const int Button_D_Right_Id = 8;

        /// <summary>
        /// HidDisplay A button.
        /// </summary>
        /// <remarks>
        /// Source clock 9.
        /// </remarks>
        public const int Button_A_Id = 9;

        /// <summary>
        /// HidDisplay X button.
        /// </summary>
        /// <remarks>
        /// Source clock 10.
        /// </remarks>
        public const int Button_X_Id = 10;

        /// <summary>
        /// HidDisplay shoulder L.
        /// </summary>
        /// <remarks>
        /// Source clock 11.
        /// </remarks>
        public const int Button_L_Shoulder_Id = 11;

        /// <summary>
        /// HidDisplay shoulder R.
        /// </summary>
        /// <remarks>
        /// Source clock 12.
        /// </remarks>
        public const int Button_R_Shoulder_Id = 12;
    }
}
