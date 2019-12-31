using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsHardware.Bluetooth.Characteristics
{
    /// <summary>
    /// Heart rate measurement characteristic.
    /// </summary>
    /// <remarks>
    /// https://www.bluetooth.com/wp-content/uploads/Sitecore-Media-Library/Gatt/Xml/Characteristics/org.bluetooth.characteristic.heart_rate_measurement.xml
    /// </remarks>
    public class HeartRateMeasurement
    {
        /// <summary>
        /// Sensor contact status.
        /// </summary>
        public enum SensorContactStatus
        {
            NotSupported = 0,
            NotSupported2 = 1,
            ContactNotDetected = 2,
            ContectDetected = 3,
        }

        /// <summary>
        /// Energy expended present flag.
        /// </summary>
        public enum EnergyExpendedEnum
        {
            NotPresent = 0,
            Present = 1,
        }

        /// <summary>
        /// Energy expended present flag.
        /// </summary>
        public enum RrIntervalEnum
        {
            NotPresent = 0,
            Present = 1,
        }

        /// <summary>
        /// Gets or sets sensor contact status.
        /// </summary>
        public SensorContactStatus ContactStatus { get; set; }

        /// <summary>
        /// Gets or sets whether or not energy expended data is present.
        /// </summary>
        public EnergyExpendedEnum EnergyExpendedPresent { get; set; }

        /// <summary>
        /// Gets or sets whether or not RR-Interval data is present.
        /// </summary>
        public RrIntervalEnum RrIntervalPresent { get; set; }

        /// <summary>
        /// Gets or sets heart rate. Units: beats per minute (bpm).
        /// </summary>
        public ushort HeartRate { get; set; }

        /// <summary>
        /// Gets or sets energy expended. Units: kilo Joules.
        /// </summary>
        public ushort EnergyExpended { get; set; }

        /// <summary>
        /// Gets or sets RR-Intervals. The RR-Interval value represents the time between two R-Wave detections. Resolution of 1/1024 second.
        /// </summary>
        public List<ushort> RrIntervals { get; set; } = new List<ushort>();

        /// <summary>
        /// Converts <see cref="SensorContactStatus"/> to specification text.
        /// </summary>
        /// <param name="e">Object to convert.</param>
        /// <returns>Standard text.</returns>
        public static string SensorContactStatusToString(SensorContactStatus status)
        {
            switch (status)
            {
                case (SensorContactStatus.ContactNotDetected):
                    return "Sensor Contact feature is supported, but contact is not detected";
                case (SensorContactStatus.ContectDetected):
                    return "Sensor Contact feature is supported and contact is detected";
                default:
                    return "Sensor Contact feature is not supported in the current connection";
            }
        }

        /// <summary>
        /// Converts <see cref="EnergyExpendedEnum"/> to specification text.
        /// </summary>
        /// <param name="e">Object to convert.</param>
        /// <returns>Standard text.</returns>
        public static string EneryExpendedToString(EnergyExpendedEnum e)
        {
            switch (e)
            {
                case (EnergyExpendedEnum.Present):
                    return "Energy Expended field is present. Units: kilo Joules";
                default:
                    return "Energy Expended field is not present";
            }
        }

        /// <summary>
        /// Converts <see cref="RrIntervalEnum"/> to specification text.
        /// </summary>
        /// <param name="e">Object to convert.</param>
        /// <returns>Standard text.</returns>
        public static string RrIntervalToString(RrIntervalEnum e)
        {
            switch (e)
            {
                case (RrIntervalEnum.Present):
                    return "One or more RR-Interval values are present.";
                default:
                    return "RR-Interval values are not present.";
            }
        }

        /// <summary>
        /// Converts a raw characteristic data reading into a <see cref="HeartRateMeasurement"/>. This reads to the end of the byte array.
        /// </summary>
        /// <param name="bytes">Bytes to read from.</param>
        /// <param name="offset">Offset to start reading from.</param>
        /// <returns>Parsed data result.</returns>
        public static HeartRateMeasurement FromBytes(byte[] bytes, int offset)
        {
            var flags = bytes[offset];
            bool is16bit;
            int readBytes = 0;
            var result = new HeartRateMeasurement();

            if ((flags & 0x01) > 0)
            {
                is16bit = true;
            }
            else
            {
                is16bit = false;
            }

            result.ContactStatus = (SensorContactStatus)((flags >> 1) & 0x03);
            result.EnergyExpendedPresent = (EnergyExpendedEnum)((flags >> 3) & 0x01);
            result.RrIntervalPresent = (RrIntervalEnum)((flags >> 4) & 0x01);

            readBytes++; // flags

            if (is16bit)
            {
                result.HeartRate = (ushort)((ushort)bytes[offset + 2] << 8 | (ushort)bytes[offset + 1]);
                readBytes += 2;
            }
            else
            {
                result.HeartRate = (ushort)bytes[1];
                readBytes++;
            }

            if (result.EnergyExpendedPresent == EnergyExpendedEnum.Present)
            {
                result.EnergyExpended = (ushort)((ushort)bytes[offset + readBytes + 1] << 8 | (ushort)bytes[offset + readBytes]);
                readBytes += 2;
            }

            if (result.RrIntervalPresent == RrIntervalEnum.Present)
            {
                // probably need this as a parameter, but not much is happening with this byte array. Should
                // be alright to just read to the end.
                while (offset + readBytes < bytes.Length)
                {
                    result.RrIntervals.Add((ushort)((ushort)bytes[offset + readBytes + 1] << 8 | (ushort)bytes[offset + readBytes]));
                    readBytes += 2;
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"Heartrate: {HeartRate}");

            if (EnergyExpendedPresent == EnergyExpendedEnum.Present)
            {
                sb.Append($", EnergyExpended: {EnergyExpended} (kilo Joules)");
            }

            if (RrIntervals.Any())
            {
                sb.Append($", RrIntervals: {string.Join(",", RrIntervals.Select(x => x))}");
            }

            return sb.ToString();
        }
    }
}
