using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HidDisplay.PluginDefinition;
using HidDisplay.SkinModel.InputSourceDescription;
using HidDisplay.SkinModel.Core;
using HidDisplay.SkinModel.Core.Display;
using WindowsHardwareWatch.HardwareWatch;
using WindowsHardwareWatch.HardwareWatch.Enums;
using WindowsHardwareWatch.Windows;

namespace HidDisplayDnc
{
    /// <summary>
    /// Helper class for main window.
    /// </summary>
    internal static class EventWpfUtility
    {
        /// <summary>
        /// Creates new action to set item visibility to visible.
        /// </summary>
        /// <param name="img">Item to set visibility.</param>
        /// <returns>New action.</returns>
        public static Action MakeShowAction(Image img)
        {
            return () => img.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Creates new action to set item visibility to hidden.
        /// </summary>
        /// <param name="img">Item to set visibility.</param>
        /// <returns>New action.</returns>
        public static Action MakeHideAction(Image img)
        {
            return () => img.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Parses input args (as double) for the radial vector display type and for the
        /// input description, and generates wpf transform to be applied.
        /// </summary>
        /// <param name="x">X param.</param>
        /// <param name="y">Y value.</param>
        /// <param name="item">Single item definition.</param>
        /// <returns>Wpf transform based on values and item definition.</returns>
        public static TransformGroup RadialVector2Transform(double x, double y, InputHandlerItem item)
        {
            var hwDescription = (RangeableInput2Description)item.Hw;
            var uiDescription = (RadialVector)item.Ui;

            var transformGroup = new TransformGroup();

            var inputCeiling = hwDescription.InputCeiling;
            var radialFactor = uiDescription.RadialFactor;
            var scaleMin = uiDescription.ScaleMin;
            var scaleMax = uiDescription.ScaleMax;
            var scaleNorm = uiDescription.ScaleNorm;
            var slideFactor = uiDescription.SlideFactor;
            var slideMax = uiDescription.SlideMax;

            if (radialFactor < 1)
            {
                radialFactor = 1;
            }

            double scaleFactor = 1.0;
            double norm = 0.0;
            double h;

            double xaverage = x;
            double yaverage = y;

            if (hwDescription.Invert1)
            {
                xaverage *= -1;
            }

            if (hwDescription.Invert2)
            {
                yaverage *= -1;
            }

            var rad = Math.Atan2(yaverage, xaverage);
            var deg = rad * 180.0 / Math.PI;

            deg -= 90; // rotate from cartesian origin (towards right) to wpf origin (towards top of screen)
            deg *= -1; // counter clockwise from origin point is negative, clockwise from origin is positive

            var rotateTransform = new RotateTransform(deg);
            transformGroup.Children.Add(rotateTransform);

            norm = Math.Sqrt(xaverage * xaverage + yaverage * yaverage);

            // set a max cutoff value;
            if (norm > inputCeiling)
            {
                norm = inputCeiling;
            }

            if (!uiDescription.UseScale)
            {
                scaleFactor = 1.0;
            }
            else
            {
                scaleFactor = (norm * scaleNorm / scaleMax) + scaleMin;

                var scaleTransform = new ScaleTransform(scaleFactor, scaleFactor);
                transformGroup.Children.Add(scaleTransform);
            }

            if (!uiDescription.UseSlide)
            {
                h = scaleFactor * radialFactor;
            }
            else
            {
                h = Math.Min(slideMax, slideFactor * norm * scaleFactor * radialFactor);
            }

            var translateX = h * Math.Cos(rad);
            var translateY = h * Math.Sin(rad) * -1; // translate from cartesian back to wpf coordinates
            
            var translateTransform = new TranslateTransform(translateX, translateY);
            transformGroup.Children.Add(translateTransform);

            //System.Diagnostics.Debug.WriteLine($"{x},{y} ({norm}) -> {deg}");

            return transformGroup;
        }
    }
}
