using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsHardware.Bluetooth.Characteristics
{
    /// <summary>
    /// Constants and helper functions for characteristics.
    /// </summary>
    /// <remarks>
    /// https://www.bluetooth.com/specifications/gatt/characteristics/
    /// </remarks>
    public static class CharacteristicUuids
    {
        /// <summary>
        /// List of assigned numbers.
        /// </summary>
        public enum AssignedNumbers : ushort
        {
            Aerobic_Heart_Rate_Lower_Limit = 0x2A7E,
            Aerobic_Heart_Rate_Upper_Limit = 0x2A84,
            Aerobic_Threshold = 0x2A7F,
            Age = 0x2A80,
            Aggregate = 0x2A5A,
            Alert_Category_ID = 0x2A43,
            Alert_Category_ID_Bit_Mask = 0x2A42,
            Alert_Level = 0x2A06,
            Alert_Notification_Control_Point = 0x2A44,
            Alert_Status = 0x2A3F,
            Altitude = 0x2AB3,
            Anaerobic_Heart_Rate_Lower_Limit = 0x2A81,
            Anaerobic_Heart_Rate_Upper_Limit = 0x2A82,
            Anaerobic_Threshold = 0x2A83,
            Analog = 0x2A58,
            Analog_Output = 0x2A59,
            Apparent_Wind_Direction = 0x2A73,
            Apparent_Wind_Speed = 0x2A72,
            Appearance = 0x2A01,
            Barometric_Pressure_Trend = 0x2AA3,
            Battery_Level = 0x2A19,
            Battery_Level_State = 0x2A1B,
            Battery_Power_State = 0x2A1A,
            Blood_Pressure_Feature = 0x2A49,
            Blood_Pressure_Measurement = 0x2A35,
            Body_Composition_Feature = 0x2A9B,
            Body_Composition_Measurement = 0x2A9C,
            Body_Sensor_Location = 0x2A38,
            Bond_Management_Control_Point = 0x2AA4,
            Bond_Management_Features = 0x2AA5,
            Boot_Keyboard_Input_Report = 0x2A22,
            Boot_Keyboard_Output_Report = 0x2A32,
            Boot_Mouse_Input_Report = 0x2A33,
            BSS_Control_Point = 0x2B2B,
            BSS_Response = 0x2B2C,
            CGM_Feature = 0x2AA8,
            CGM_Measurement = 0x2AA7,
            CGM_Session_Run_Time = 0x2AAB,
            CGM_Session_Start_Time = 0x2AAA,
            CGM_Specific_Ops_Control_Point = 0x2AAC,
            CGM_Status = 0x2AA9,
            Cross_Trainer_Data = 0x2ACE,
            CSC_Feature = 0x2A5C,
            CSC_Measurement = 0x2A5B,
            Current_Time = 0x2A2B,
            Cycling_Power_Control_Point = 0x2A66,
            Cycling_Power_Feature = 0x2A65,
            Cycling_Power_Measurement = 0x2A63,
            Cycling_Power_Vector = 0x2A64,
            Database_Change_Increment = 0x2A99,
            Database_Hash = 0x2B2A,
            Date_of_Birth = 0x2A85,
            Date_of_Threshold_Assessment = 0x2A86,
            Date_Time = 0x2A08,
            Date_UTC = 0x2AED,
            Day_Date_Time = 0x2A0A,
            Day_of_Week = 0x2A09,
            Descriptor_Value_Changed = 0x2A7D,
            Dew_Point = 0x2A7B,
            Digital = 0x2A56,
            Digital_Output = 0x2A57,
            DST_Offset = 0x2A0D,
            Elevation = 0x2A6C,
            Email_Address = 0x2A87,
            Emergency_ID = 0x2B2D,
            Emergency_Text = 0x2B2E,
            Exact_Time_100 = 0x2A0B,
            Exact_Time_256 = 0x2A0C,
            Fat_Burn_Heart_Rate_Lower_Limit = 0x2A88,
            Fat_Burn_Heart_Rate_Upper_Limit = 0x2A89,
            Firmware_Revision_String = 0x2A26,
            First_Name = 0x2A8A,
            Fitness_Machine_Control_Point = 0x2AD9,
            Fitness_Machine_Feature = 0x2ACC,
            Fitness_Machine_Status = 0x2ADA,
            Five_Zone_Heart_Rate_Limits = 0x2A8B,
            Floor_Number = 0x2AB2,
            Central_Address_Resolution = 0x2AA6,
            Device_Name = 0x2A00,
            Peripheral_Preferred_Connection_Parameters = 0x2A04,
            Peripheral_Privacy_Flag = 0x2A02,
            Reconnection_Address = 0x2A03,
            Service_Changed = 0x2A05,
            Gender = 0x2A8C,
            Glucose_Feature = 0x2A51,
            Glucose_Measurement = 0x2A18,
            Glucose_Measurement_Context = 0x2A34,
            Gust_Factor = 0x2A74,
            Hardware_Revision_String = 0x2A27,
            Heart_Rate_Control_Point = 0x2A39,
            Heart_Rate_Max = 0x2A8D,
            Heart_Rate_Measurement = 0x2A37,
            Heat_Index = 0x2A7A,
            Height = 0x2A8E,
            HID_Control_Point = 0x2A4C,
            HID_Information = 0x2A4A,
            Hip_Circumference = 0x2A8F,
            HTTP_Control_Point = 0x2ABA,
            HTTP_Entity_Body = 0x2AB9,
            HTTP_Headers = 0x2AB7,
            HTTP_Status_Code = 0x2AB8,
            HTTPS_Security = 0x2ABB,
            Humidity = 0x2A6F,
            IDD_Annunciation_Status = 0x2B22,
            IDD_Command_Control_Point = 0x2B25,
            IDD_Command_Data = 0x2B26,
            IDD_Features = 0x2B23,
            IDD_History_Data = 0x2B28,
            IDD_Record_Access_Control_Point = 0x2B27,
            IDD_Status = 0x2B21,
            IDD_Status_Changed = 0x2B20,
            IDD_Status_Reader_Control_Point = 0x2B24,
            IEEE_11073_20601_Regulatory_Certification_Data_List = 0x2A2A,
            Indoor_Bike_Data = 0x2AD2,
            Indoor_Positioning_Configuration = 0x2AAD,
            Intermediate_Cuff_Pressure = 0x2A36,
            Intermediate_Temperature = 0x2A1E,
            Irradiance = 0x2A77,
            Language = 0x2AA2,
            Last_Name = 0x2A90,
            Latitude = 0x2AAE,
            LN_Control_Point = 0x2A6B,
            LN_Feature = 0x2A6A,
            Local_East_Coordinate = 0x2AB1,
            Local_North_Coordinate = 0x2AB0,
            Local_Time_Information = 0x2A0F,
            Location_and_Speed_Characteristic = 0x2A67,
            Location_Name = 0x2AB5,
            Longitude = 0x2AAF,
            Magnetic_Declination = 0x2A2C,
            Magnetic_Flux_Density_2D = 0x2AA0,
            Magnetic_Flux_Density_3D = 0x2AA1,
            Manufacturer_Name_String = 0x2A29,
            Maximum_Recommended_Heart_Rate = 0x2A91,
            Measurement_Interval = 0x2A21,
            Model_Number_String = 0x2A24,
            Navigation = 0x2A68,
            Network_Availability = 0x2A3E,
            New_Alert = 0x2A46,
            Object_Action_Control_Point = 0x2AC5,
            Object_Changed = 0x2AC8,
            Object_FirstCreated = 0x2AC1,
            Object_ID = 0x2AC3,
            Object_LastModified = 0x2AC2,
            Object_List_Control_Point = 0x2AC6,
            Object_List_Filter = 0x2AC7,
            Object_Name = 0x2ABE,
            Object_Properties = 0x2AC4,
            Object_Size = 0x2AC0,
            Object_Type = 0x2ABF,
            OTS_Feature = 0x2ABD,
            PLX_Continuous_Measurement_Characteristic = 0x2A5F,
            PLX_Features = 0x2A60,
            PLX_SpotCheck_Measurement = 0x2A5E,
            PnP_ID = 0x2A50,
            Pollen_Concentration = 0x2A75,
            Position_2D = 0x2A2F,
            Position_3D = 0x2A30,
            Position_Quality = 0x2A69,
            Pressure = 0x2A6D,
            Protocol_Mode = 0x2A4E,
            Pulse_Oximetry_Control_Point = 0x2A62,
            Rainfall = 0x2A78,
            RC_Feature = 0x2B1D,
            RC_Settings = 0x2B1E,
            Reconnection_Configuration_Control_Point = 0x2B1F,
            Record_Access_Control_Point = 0x2A52,
            Reference_Time_Information = 0x2A14,
            Registered_User_Characteristic = 0x2B37,
            Removable = 0x2A3A,
            Report = 0x2A4D,
            Report_Map = 0x2A4B,
            Resolvable_Private_Address_Only = 0x2AC9,
            Resting_Heart_Rate = 0x2A92,
            Ringer_Control_point = 0x2A40,
            Ringer_Setting = 0x2A41,
            Rower_Data = 0x2AD1,
            RSC_Feature = 0x2A54,
            RSC_Measurement = 0x2A53,
            SC_Control_Point = 0x2A55,
            Scan_Interval_Window = 0x2A4F,
            Scan_Refresh = 0x2A31,
            Scientific_Temperature_Celsius = 0x2A3C,
            Secondary_Time_Zone = 0x2A10,
            Sensor_Location = 0x2A5D,
            Serial_Number_String = 0x2A25,
            Service_Required = 0x2A3B,
            Software_Revision_String = 0x2A28,
            Sport_Type_for_Aerobic_and_Anaerobic_Thresholds = 0x2A93,
            Stair_Climber_Data = 0x2AD0,
            Step_Climber_Data = 0x2ACF,
            String = 0x2A3D,
            Supported_Heart_Rate_Range = 0x2AD7,
            Supported_Inclination_Range = 0x2AD5,
            Supported_New_Alert_Category = 0x2A47,
            Supported_Power_Range = 0x2AD8,
            Supported_Resistance_Level_Range = 0x2AD6,
            Supported_Speed_Range = 0x2AD4,
            Supported_Unread_Alert_Category = 0x2A48,
            System_ID = 0x2A23,
            TDS_Control_Point = 0x2ABC,
            Temperature = 0x2A6E,
            Temperature_Celsius = 0x2A1F,
            Temperature_Fahrenheit = 0x2A20,
            Temperature_Measurement = 0x2A1C,
            Temperature_Type = 0x2A1D,
            Three_Zone_Heart_Rate_Limits = 0x2A94,
            Time_Accuracy = 0x2A12,
            Time_Broadcast = 0x2A15,
            Time_Source = 0x2A13,
            Time_Update_Control_Point = 0x2A16,
            Time_Update_State = 0x2A17,
            Time_with_DST = 0x2A11,
            Time_Zone = 0x2A0E,
            Training_Status = 0x2AD3,
            Treadmill_Data = 0x2ACD,
            True_Wind_Direction = 0x2A71,
            True_Wind_Speed = 0x2A70,
            Two_Zone_Heart_Rate_Limit = 0x2A95,
            Tx_Power_Level = 0x2A07,
            Uncertainty = 0x2AB4,
            Unread_Alert_Status = 0x2A45,
            URI = 0x2AB6,
            User_Control_Point = 0x2A9F,
            User_Index = 0x2A9A,
            UV_Index = 0x2A76,
            VO2_Max = 0x2A96,
            Waist_Circumference = 0x2A97,
            Weight = 0x2A98,
            Weight_Measurement = 0x2A9D,
            Weight_Scale_Feature = 0x2A9E,
            Wind_Chill = 0x2A79,
        }

        /// <summary>
        /// Gets the display name of the characteristic.
        /// </summary>
        /// <param name="characteristicUuid">UUID to find name of.</param>
        /// <returns>Name of assigned number, or string.empty.</returns>
        public static string CharacteristicToString(Guid characteristicUuid)
        {
            var assigned = Utility.UuidToAssignedNumber(characteristicUuid);

            switch (assigned)
            {
                case 0x2A7E: return "Aerobic Heart Rate Lower Limit";
                case 0x2A84: return "Aerobic Heart Rate Upper Limit";
                case 0x2A7F: return "Aerobic Threshold";
                case 0x2A80: return "Age";
                case 0x2A5A: return "Aggregate";
                case 0x2A43: return "Alert Category ID";
                case 0x2A42: return "Alert Category ID Bit Mask";
                case 0x2A06: return "Alert Level";
                case 0x2A44: return "Alert Notification Control Point";
                case 0x2A3F: return "Alert Status";
                case 0x2AB3: return "Altitude";
                case 0x2A81: return "Anaerobic Heart Rate Lower Limit";
                case 0x2A82: return "Anaerobic Heart Rate Upper Limit";
                case 0x2A83: return "Anaerobic Threshold";
                case 0x2A58: return "Analog";
                case 0x2A59: return "Analog Output";
                case 0x2A73: return "Apparent Wind Direction";
                case 0x2A72: return "Apparent Wind Speed";
                case 0x2A01: return "Appearance";
                case 0x2AA3: return "Barometric Pressure Trend";
                case 0x2A19: return "Battery Level";
                case 0x2A1B: return "Battery Level State";
                case 0x2A1A: return "Battery Power State";
                case 0x2A49: return "Blood Pressure Feature";
                case 0x2A35: return "Blood Pressure Measurement";
                case 0x2A9B: return "Body Composition Feature";
                case 0x2A9C: return "Body Composition Measurement";
                case 0x2A38: return "Body Sensor Location";
                case 0x2AA4: return "Bond Management Control Point";
                case 0x2AA5: return "Bond Management Features";
                case 0x2A22: return "Boot Keyboard Input Report";
                case 0x2A32: return "Boot Keyboard Output Report";
                case 0x2A33: return "Boot Mouse Input Report";
                case 0x2B2B: return "BSS Control Point";
                case 0x2B2C: return "BSS Response";
                case 0x2AA8: return "CGM Feature";
                case 0x2AA7: return "CGM Measurement";
                case 0x2AAB: return "CGM Session Run Time";
                case 0x2AAA: return "CGM Session Start Time";
                case 0x2AAC: return "CGM Specific Ops Control Point";
                case 0x2AA9: return "CGM Status";
                case 0x2ACE: return "Cross Trainer Data";
                case 0x2A5C: return "CSC Feature";
                case 0x2A5B: return "CSC Measurement";
                case 0x2A2B: return "Current Time";
                case 0x2A66: return "Cycling Power Control Point";
                case 0x2A65: return "Cycling Power Feature";
                case 0x2A63: return "Cycling Power Measurement";
                case 0x2A64: return "Cycling Power Vector";
                case 0x2A99: return "Database Change Increment";
                case 0x2B2A: return "Database Hash";
                case 0x2A85: return "Date of Birth";
                case 0x2A86: return "Date of Threshold Assessment";
                case 0x2A08: return "Date Time";
                case 0x2AED: return "Date UTC";
                case 0x2A0A: return "Day Date Time";
                case 0x2A09: return "Day of Week";
                case 0x2A7D: return "Descriptor Value Changed";
                case 0x2A7B: return "Dew Point";
                case 0x2A56: return "Digital";
                case 0x2A57: return "Digital Output";
                case 0x2A0D: return "DST Offset";
                case 0x2A6C: return "Elevation";
                case 0x2A87: return "Email Address";
                case 0x2B2D: return "Emergency ID";
                case 0x2B2E: return "Emergency Text";
                case 0x2A0B: return "Exact Time 100";
                case 0x2A0C: return "Exact Time 256";
                case 0x2A88: return "Fat Burn Heart Rate Lower Limit";
                case 0x2A89: return "Fat Burn Heart Rate Upper Limit";
                case 0x2A26: return "Firmware Revision String";
                case 0x2A8A: return "First Name";
                case 0x2AD9: return "Fitness Machine Control Point";
                case 0x2ACC: return "Fitness Machine Feature";
                case 0x2ADA: return "Fitness Machine Status";
                case 0x2A8B: return "Five Zone Heart Rate Limits";
                case 0x2AB2: return "Floor Number";
                case 0x2AA6: return "Central Address Resolution";
                case 0x2A00: return "Device Name";
                case 0x2A04: return "Peripheral Preferred Connection Parameters";
                case 0x2A02: return "Peripheral Privacy Flag";
                case 0x2A03: return "Reconnection Address";
                case 0x2A05: return "Service Changed";
                case 0x2A8C: return "Gender";
                case 0x2A51: return "Glucose Feature";
                case 0x2A18: return "Glucose Measurement";
                case 0x2A34: return "Glucose Measurement Context";
                case 0x2A74: return "Gust Factor";
                case 0x2A27: return "Hardware Revision String";
                case 0x2A39: return "Heart Rate Control Point";
                case 0x2A8D: return "Heart Rate Max";
                case 0x2A37: return "Heart Rate Measurement";
                case 0x2A7A: return "Heat Index";
                case 0x2A8E: return "Height";
                case 0x2A4C: return "HID Control Point";
                case 0x2A4A: return "HID Information";
                case 0x2A8F: return "Hip Circumference";
                case 0x2ABA: return "HTTP Control Point";
                case 0x2AB9: return "HTTP Entity Body";
                case 0x2AB7: return "HTTP Headers";
                case 0x2AB8: return "HTTP Status Code";
                case 0x2ABB: return "HTTPS Security";
                case 0x2A6F: return "Humidity";
                case 0x2B22: return "IDD Annunciation Status";
                case 0x2B25: return "IDD Command Control Point";
                case 0x2B26: return "IDD Command Data";
                case 0x2B23: return "IDD Features";
                case 0x2B28: return "IDD History Data";
                case 0x2B27: return "IDD Record Access Control Point";
                case 0x2B21: return "IDD Status";
                case 0x2B20: return "IDD Status Changed";
                case 0x2B24: return "IDD Status Reader Control Point";
                case 0x2A2A: return "IEEE 11073-20601 Regulatory Certification Data List";
                case 0x2AD2: return "Indoor Bike Data";
                case 0x2AAD: return "Indoor Positioning Configuration";
                case 0x2A36: return "Intermediate Cuff Pressure";
                case 0x2A1E: return "Intermediate Temperature";
                case 0x2A77: return "Irradiance";
                case 0x2AA2: return "Language";
                case 0x2A90: return "Last Name";
                case 0x2AAE: return "Latitude";
                case 0x2A6B: return "LN Control Point";
                case 0x2A6A: return "LN Feature";
                case 0x2AB1: return "Local East Coordinate";
                case 0x2AB0: return "Local North Coordinate";
                case 0x2A0F: return "Local Time Information";
                case 0x2A67: return "Location and Speed Characteristic";
                case 0x2AB5: return "Location Name";
                case 0x2AAF: return "Longitude";
                case 0x2A2C: return "Magnetic Declination";
                case 0x2AA0: return "Magnetic Flux Density – 2D";
                case 0x2AA1: return "Magnetic Flux Density – 3D";
                case 0x2A29: return "Manufacturer Name String";
                case 0x2A91: return "Maximum Recommended Heart Rate";
                case 0x2A21: return "Measurement Interval";
                case 0x2A24: return "Model Number String";
                case 0x2A68: return "Navigation";
                case 0x2A3E: return "Network Availability";
                case 0x2A46: return "New Alert";
                case 0x2AC5: return "Object Action Control Point";
                case 0x2AC8: return "Object Changed";
                case 0x2AC1: return "Object First-Created";
                case 0x2AC3: return "Object ID";
                case 0x2AC2: return "Object Last-Modified";
                case 0x2AC6: return "Object List Control Point";
                case 0x2AC7: return "Object List Filter";
                case 0x2ABE: return "Object Name";
                case 0x2AC4: return "Object Properties";
                case 0x2AC0: return "Object Size";
                case 0x2ABF: return "Object Type";
                case 0x2ABD: return "OTS Feature";
                case 0x2A5F: return "PLX Continuous Measurement Characteristic";
                case 0x2A60: return "PLX Features";
                case 0x2A5E: return "PLX Spot-Check Measurement";
                case 0x2A50: return "PnP ID";
                case 0x2A75: return "Pollen Concentration";
                case 0x2A2F: return "Position 2D";
                case 0x2A30: return "Position 3D";
                case 0x2A69: return "Position Quality";
                case 0x2A6D: return "Pressure";
                case 0x2A4E: return "Protocol Mode";
                case 0x2A62: return "Pulse Oximetry Control Point";
                case 0x2A78: return "Rainfall";
                case 0x2B1D: return "RC Feature";
                case 0x2B1E: return "RC Settings";
                case 0x2B1F: return "Reconnection Configuration Control Point";
                case 0x2A52: return "Record Access Control Point";
                case 0x2A14: return "Reference Time Information";
                case 0x2B37: return "Registered User Characteristic";
                case 0x2A3A: return "Removable";
                case 0x2A4D: return "Report";
                case 0x2A4B: return "Report Map";
                case 0x2AC9: return "Resolvable Private Address Only";
                case 0x2A92: return "Resting Heart Rate";
                case 0x2A40: return "Ringer Control point";
                case 0x2A41: return "Ringer Setting";
                case 0x2AD1: return "Rower Data";
                case 0x2A54: return "RSC Feature";
                case 0x2A53: return "RSC Measurement";
                case 0x2A55: return "SC Control Point";
                case 0x2A4F: return "Scan Interval Window";
                case 0x2A31: return "Scan Refresh";
                case 0x2A3C: return "Scientific Temperature Celsius";
                case 0x2A10: return "Secondary Time Zone";
                case 0x2A5D: return "Sensor Location";
                case 0x2A25: return "Serial Number String";
                case 0x2A3B: return "Service Required";
                case 0x2A28: return "Software Revision String";
                case 0x2A93: return "Sport Type for Aerobic and Anaerobic Thresholds";
                case 0x2AD0: return "Stair Climber Data";
                case 0x2ACF: return "Step Climber Data";
                case 0x2A3D: return "String";
                case 0x2AD7: return "Supported Heart Rate Range";
                case 0x2AD5: return "Supported Inclination Range";
                case 0x2A47: return "Supported New Alert Category";
                case 0x2AD8: return "Supported Power Range";
                case 0x2AD6: return "Supported Resistance Level Range";
                case 0x2AD4: return "Supported Speed Range";
                case 0x2A48: return "Supported Unread Alert Category";
                case 0x2A23: return "System ID";
                case 0x2ABC: return "TDS Control Point";
                case 0x2A6E: return "Temperature";
                case 0x2A1F: return "Temperature Celsius";
                case 0x2A20: return "Temperature Fahrenheit";
                case 0x2A1C: return "Temperature Measurement";
                case 0x2A1D: return "Temperature Type";
                case 0x2A94: return "Three Zone Heart Rate Limits";
                case 0x2A12: return "Time Accuracy";
                case 0x2A15: return "Time Broadcast";
                case 0x2A13: return "Time Source";
                case 0x2A16: return "Time Update Control Point";
                case 0x2A17: return "Time Update State";
                case 0x2A11: return "Time with DST";
                case 0x2A0E: return "Time Zone";
                case 0x2AD3: return "Training Status";
                case 0x2ACD: return "Treadmill Data";
                case 0x2A71: return "True Wind Direction";
                case 0x2A70: return "True Wind Speed";
                case 0x2A95: return "Two Zone Heart Rate Limit";
                case 0x2A07: return "Tx Power Level";
                case 0x2AB4: return "Uncertainty";
                case 0x2A45: return "Unread Alert Status";
                case 0x2AB6: return "URI";
                case 0x2A9F: return "User Control Point";
                case 0x2A9A: return "User Index";
                case 0x2A76: return "UV Index";
                case 0x2A96: return "VO2 Max";
                case 0x2A97: return "Waist Circumference";
                case 0x2A98: return "Weight";
                case 0x2A9D: return "Weight Measurement";
                case 0x2A9E: return "Weight Scale Feature";
                case 0x2A79: return "Wind Chill";
                default:
                    return string.Empty;
            }
        }
    }
}
