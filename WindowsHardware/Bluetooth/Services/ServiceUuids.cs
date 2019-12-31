using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardware.Bluetooth.Services
{
    /// <summary>
    /// Constants and helper functions for services.
    /// </summary>
    /// <remarks>
    /// https://www.bluetooth.com/specifications/gatt/services/
    /// </remarks>
    public static class ServiceUuids
    {
        /// <summary>
        /// List of assigned numbers.
        /// </summary>
        public enum AssignedNumbers : ushort
        {
            Generic_Access = 0x1800,
            Alert_Notification_Service = 0x1811,
            Automation_IO = 0x1815,
            Battery_Service = 0x180F,
            Binary_Sensor = 0x183B,
            Blood_Pressure = 0x1810,
            Body_Composition = 0x181B,
            Bond_Management_Service = 0x181E,
            Continuous_Glucose_Monitoring = 0x181F,
            Current_Time_Service = 0x1805,
            Cycling_Power = 0x1818,
            Cycling_Speed_and_Cadence = 0x1816,
            Device_Information = 0x180A,
            Emergency_Configuration = 0x183C,
            Environmental_Sensing = 0x181A,
            Fitness_Machine = 0x1826,
            Generic_Attribute = 0x1801,
            Glucose = 0x1808,
            Health_Thermometer = 0x1809,
            Heart_Rate = 0x180D,
            HTTP_Proxy = 0x1823,
            Human_Interface_Device = 0x1812,
            Immediate_Alert = 0x1802,
            Indoor_Positioning = 0x1821,
            Insulin_Delivery = 0x183A,
            Internet_Protocol_Support_Service = 0x1820,
            Link_Loss = 0x1803,
            Location_and_Navigation = 0x1819,
            Mesh_Provisioning_Service = 0x1827,
            Mesh_Proxy_Service = 0x1828,
            Next_DST_Change_Service = 0x1807,
            Object_Transfer_Service = 0x1825,
            Phone_Alert_Status_Service = 0x180E,
            Pulse_Oximeter_Service = 0x1822,
            Reconnection_Configuration = 0x1829,
            Reference_Time_Update_Service = 0x1806,
            Running_Speed_and_Cadence = 0x1814,
            Scan_Parameters = 0x1813,
            Transport_Discovery = 0x1824,
            Tx_Power = 0x1804,
            User_Data = 0x181C,
            Weight_Scale = 0x181D,
        }

        /// <summary>
        /// Gets the display name of the service.
        /// </summary>
        /// <param name="serviceUuid">UUID to find name of.</param>
        /// <returns>Name of assigned number, or string.empty.</returns>
        public static string ServiceToString(Guid serviceUuid)
        {
            var assigned = Utility.UuidToAssignedNumber(serviceUuid);
            switch (assigned)
            {
                case 0x1800: return "Generic Access";
                case 0x1811: return "Alert Notification Service";
                case 0x1815: return "Automation IO";
                case 0x180F: return "Battery Service";
                case 0x183B: return "Binary Sensor";
                case 0x1810: return "Blood Pressure";
                case 0x181B: return "Body Composition";
                case 0x181E: return "Bond Management Service";
                case 0x181F: return "Continuous Glucose Monitoring";
                case 0x1805: return "Current Time Service";
                case 0x1818: return "Cycling Power";
                case 0x1816: return "Cycling Speed and Cadence";
                case 0x180A: return "Device Information";
                case 0x183C: return "Emergency Configuration";
                case 0x181A: return "Environmental Sensing";
                case 0x1826: return "Fitness Machine";
                case 0x1801: return "Generic Attribute";
                case 0x1808: return "Glucose";
                case 0x1809: return "Health Thermometer";
                case 0x180D: return "Heart Rate";
                case 0x1823: return "HTTP Proxy";
                case 0x1812: return "Human Interface Device";
                case 0x1802: return "Immediate Alert";
                case 0x1821: return "Indoor Positioning";
                case 0x183A: return "Insulin Delivery";
                case 0x1820: return "Internet Protocol Support Service";
                case 0x1803: return "Link Loss";
                case 0x1819: return "Location and Navigation";
                case 0x1827: return "Mesh Provisioning Service";
                case 0x1828: return "Mesh Proxy Service";
                case 0x1807: return "Next DST Change Service";
                case 0x1825: return "Object Transfer Service";
                case 0x180E: return "Phone Alert Status Service";
                case 0x1822: return "Pulse Oximeter Service";
                case 0x1829: return "Reconnection Configuration";
                case 0x1806: return "Reference Time Update Service";
                case 0x1814: return "Running Speed and Cadence";
                case 0x1813: return "Scan Parameters";
                case 0x1824: return "Transport Discovery";
                case 0x1804: return "Tx Power";
                case 0x181C: return "User Data";
                case 0x181D: return "Weight Scale";
                default:
                    return string.Empty;
            }
        }
    }
}
