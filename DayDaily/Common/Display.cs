using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DayDaily.Common.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayDevice
    {
        public int cb;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;
        public int StateFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceKey;

        public DisplayDevice(int flags)
        {
            cb = 0;
            StateFlags = flags;
            DeviceName = new string((char)32, 32);
            DeviceString = new string((char)32, 128);
            DeviceID = new string((char)32, 128);
            DeviceKey = new string((char)32, 128);
            cb = Marshal.SizeOf(this);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINTL
    {
        [MarshalAs(UnmanagedType.I4)]
        public int x;
        [MarshalAs(UnmanagedType.I4)]
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DEVMODE
    {
        // You can define the following constant
        // but OUTSIDE the structure because you know
        // that size and layout of the structure
        // is very important
        // CCHDEVICENAME = 32 = 0x50
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;
        // In addition you can define the last character array
        // as following:
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        //public Char[] dmDeviceName;

        // After the 32-bytes array
        [MarshalAs(UnmanagedType.U2)]
        public UInt16 dmSpecVersion;

        [MarshalAs(UnmanagedType.U2)]
        public UInt16 dmDriverVersion;

        [MarshalAs(UnmanagedType.U2)]
        public UInt16 dmSize;

        [MarshalAs(UnmanagedType.U2)]
        public UInt16 dmDriverExtra;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmFields;

        public POINTL dmPosition;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmDisplayOrientation;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmDisplayFixedOutput;

        [MarshalAs(UnmanagedType.I2)]
        public Int16 dmColor;

        [MarshalAs(UnmanagedType.I2)]
        public Int16 dmDuplex;

        [MarshalAs(UnmanagedType.I2)]
        public Int16 dmYResolution;

        [MarshalAs(UnmanagedType.I2)]
        public Int16 dmTTOption;

        [MarshalAs(UnmanagedType.I2)]
        public Int16 dmCollate;

        // CCHDEVICENAME = 32 = 0x50
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;
        // Also can be defined as
        //[MarshalAs(UnmanagedType.ByValArray,
        //    SizeConst = 32, ArraySubType = UnmanagedType.U1)]
        //public Byte[] dmFormName;

        [MarshalAs(UnmanagedType.U2)]
        public UInt16 dmLogPixels;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmBitsPerPel;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmPelsWidth;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmPelsHeight;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmDisplayFlags;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmDisplayFrequency;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmICMMethod;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmICMIntent;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmMediaType;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmDitherType;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmReserved1;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmReserved2;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmPanningWidth;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dmPanningHeight;
    }

    internal static class NativeMethods
    {
        [DllImport("User32.dll")]
        public static extern bool EnumDisplayDevices(IntPtr lpDevice, int iDevNum, ref DisplayDevice lpDisplayDevice, int dwFlags);
        [DllImport("User32.dll", CharSet = CharSet.Ansi)]
        public static extern bool EnumDisplaySettings(string devName, int modeNum, ref DEVMODE devMode);
        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);
    }

    internal static class DeviceStatus
    {
        public const int DISPLAY_DEVICE_ACTIVE = 0x1;
        public const int DISPLAY_DEVICE_PRIMARY_DEVICE = 0x4;
        public const int DISPLAY_DEVICE_MIRRORING_DRIVER = 0x8;
        public const int DISPLAY_DEVICE_VGA_COMPATIBLE = 0x10;
        public const int DISPLAY_DEVICE_REMOVABLE = 0x20;
        public const int DISPLAY_DEVICE_MODESPRUNED = 0x8000000;
    }

    public static class DisplayHelper
    {
        public static void EnumModes(int devNum)
        {
            string devName = GetDeviceName(devNum);
            DEVMODE devMode = new DEVMODE();
            int modeNum = 0;
            bool result = true;
            do
            {
                result = NativeMethods.EnumDisplaySettings(devName, modeNum, ref devMode);
                if (result)
                {
                    string item = DevmodeToString(devMode);
                }
                modeNum++;
            } while (result);
        }

        // populates DEVMODE for the specified device and mode
        internal static DEVMODE GetDevmode(int devNum, int modeNum)
        {
            DEVMODE devMode = new DEVMODE();
            string devName = GetDeviceName(devNum);
            NativeMethods.EnumDisplaySettings(devName, modeNum, ref devMode);
            return devMode;
        }

        internal static string DevmodeToString(DEVMODE devMode)
        {
            return devMode.dmPelsWidth.ToString() +
                " x " + devMode.dmPelsHeight.ToString() +
                ", " + devMode.dmBitsPerPel.ToString() +
                " bits, " +
                devMode.dmDisplayFrequency.ToString() + " Hz";
        }

        //populates Display Devices list
        public static void EnumDevices()
        {
            DisplayDevice d = new DisplayDevice(0);
            int devNum = 0;
            bool result;
            do
            {
                result = NativeMethods.EnumDisplayDevices(IntPtr.Zero, devNum, ref d, 0);
                if (result)
                {
                    string item = devNum.ToString() + ". " + d.DeviceString.Trim();
                    if ((d.StateFlags & DeviceStatus.DISPLAY_DEVICE_PRIMARY_DEVICE) != 0)
                    {
                        item += " - main";
                    }
                    Debug.WriteLine(item);
                }
                devNum++;
            } while (result);
        }

        public static string GetDeviceName(int devNum)
        {
            DisplayDevice d = new DisplayDevice(0);
            bool result = NativeMethods.EnumDisplayDevices(IntPtr.Zero, devNum, ref d, 0);
            return (result ? d.DeviceName.Trim() : "#error#");
        }

        //whether the specified device is the main device
        public static bool IsMainDevice(int devNum)
        {
            DisplayDevice d = new DisplayDevice(0);
            if (NativeMethods.EnumDisplayDevices(IntPtr.Zero, devNum, ref d, 0))
            {
                return ((d.StateFlags & DeviceStatus.DISPLAY_DEVICE_PRIMARY_DEVICE) != 0);
            }
            return false;
        }
    }
}