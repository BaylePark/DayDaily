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

    [Flags]
    public enum DispChangeField : uint
    {
        DM_ORIENTATION = 0x00000001,
        DM_PAPERSIZE = 0x00000002,
        DM_PAPERLENGTH = 0x00000004,
        DM_PAPERWIDTH = 0x00000008,
        DM_SCALE = 0x00000010,
        DM_POSITION = 0x00000020,
        DM_NUP = 0x00000040,
        DM_DISPLAYORIENTATION = 0x00000080,
        DM_COPIES = 0x00000100,
        DM_DEFAULTSOURCE = 0x00000200,
        DM_PRINTQUALITY = 0x00000400,
        DM_COLOR = 0x00000800,
        DM_DUPLEX = 0x00001000,
        DM_YRESOLUTION = 0x00002000,
        DM_TTOPTION = 0x00004000,
        DM_COLLATE = 0x00008000,
        DM_FORMNAME = 0x00010000,
        DM_LOGPIXELS = 0x00020000,
        DM_BITSPERPEL = 0x00040000,
        DM_PELSWIDTH = 0x00080000,
        DM_PELSHEIGHT = 0x00100000,
        DM_DISPLAYFLAGS = 0x00200000,
        DM_DISPLAYFREQUENCY = 0x00400000,
        DM_ICMMETHOD = 0x00800000,
        DM_ICMINTENT = 0x01000000,
        DM_MEDIATYPE = 0x02000000,
        DM_DITHERTYPE = 0x04000000,
        DM_PANNINGWIDTH = 0x08000000,
        DM_PANNINGHEIGHT = 0x10000000,
        DM_DISPLAYFIXEDOUTPUT = 0x20000000
    };

    [Flags()]
    public enum ChangeDisplaySettingsFlags : uint
    {
        CDS_NONE = 0,
        CDS_UPDATEREGISTRY = 0x00000001,
        CDS_TEST = 0x00000002,
        CDS_FULLSCREEN = 0x00000004,
        CDS_GLOBAL = 0x00000008,
        CDS_SET_PRIMARY = 0x00000010,
        CDS_VIDEOPARAMETERS = 0x00000020,
        CDS_ENABLE_UNSAFE_MODES = 0x00000100,
        CDS_DISABLE_UNSAFE_MODES = 0x00000200,
        CDS_RESET = 0x40000000,
        CDS_RESET_EX = 0x20000000,
        CDS_NORESET = 0x10000000
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINTL
    {
        [MarshalAs(UnmanagedType.I4)]
        public int x;
        [MarshalAs(UnmanagedType.I4)]
        public int y;
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    public struct DEVMODE
    {
        public const int CCHDEVICENAME = 32;
        public const int CCHFORMNAME = 32;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
        [System.Runtime.InteropServices.FieldOffset(0)]
        public string dmDeviceName;
        [System.Runtime.InteropServices.FieldOffset(32)]
        public Int16 dmSpecVersion;
        [System.Runtime.InteropServices.FieldOffset(34)]
        public Int16 dmDriverVersion;
        [System.Runtime.InteropServices.FieldOffset(36)]
        public Int16 dmSize;
        [System.Runtime.InteropServices.FieldOffset(38)]
        public Int16 dmDriverExtra;
        [System.Runtime.InteropServices.FieldOffset(40)]
        public DispChangeField dmFields;

        [System.Runtime.InteropServices.FieldOffset(44)]
        public Int16 dmOrientation;
        [System.Runtime.InteropServices.FieldOffset(46)]
        public Int16 dmPaperSize;
        [System.Runtime.InteropServices.FieldOffset(48)]
        public Int16 dmPaperLength;
        [System.Runtime.InteropServices.FieldOffset(50)]
        public Int16 dmPaperWidth;
        [System.Runtime.InteropServices.FieldOffset(52)]
        public Int16 dmScale;
        [System.Runtime.InteropServices.FieldOffset(54)]
        public Int16 dmCopies;
        [System.Runtime.InteropServices.FieldOffset(56)]
        public Int16 dmDefaultSource;
        [System.Runtime.InteropServices.FieldOffset(58)]
        public Int16 dmPrintQuality;

        [System.Runtime.InteropServices.FieldOffset(44)]
        public POINTL dmPosition;
        [System.Runtime.InteropServices.FieldOffset(52)]
        public Int32 dmDisplayOrientation;
        [System.Runtime.InteropServices.FieldOffset(56)]
        public Int32 dmDisplayFixedOutput;

        [System.Runtime.InteropServices.FieldOffset(60)]
        public short dmColor; // See note below!
        [System.Runtime.InteropServices.FieldOffset(62)]
        public short dmDuplex; // See note below!
        [System.Runtime.InteropServices.FieldOffset(64)]
        public short dmYResolution;
        [System.Runtime.InteropServices.FieldOffset(66)]
        public short dmTTOption;
        [System.Runtime.InteropServices.FieldOffset(68)]
        public short dmCollate; // See note below!
        /*
        [System.Runtime.InteropServices.FieldOffset(70)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
        public string dmFormName;
        */
        [System.Runtime.InteropServices.FieldOffset(102)]
        public Int16 dmLogPixels;
        [System.Runtime.InteropServices.FieldOffset(104)]
        public Int32 dmBitsPerPel;
        [System.Runtime.InteropServices.FieldOffset(108)]
        public Int32 dmPelsWidth;
        [System.Runtime.InteropServices.FieldOffset(112)]
        public Int32 dmPelsHeight;
        [System.Runtime.InteropServices.FieldOffset(116)]
        public Int32 dmDisplayFlags;
        [System.Runtime.InteropServices.FieldOffset(116)]
        public Int32 dmNup;
        [System.Runtime.InteropServices.FieldOffset(120)]
        public Int32 dmDisplayFrequency;
        [System.Runtime.InteropServices.FieldOffset(124)]
        public Int32 dmICMMethod;
        [System.Runtime.InteropServices.FieldOffset(128)]
        public Int32 dmICMIntent;
        [System.Runtime.InteropServices.FieldOffset(132)]
        public Int32 dmMediaType;
        [System.Runtime.InteropServices.FieldOffset(136)]
        public Int32 dmDitherType;
        [System.Runtime.InteropServices.FieldOffset(140)]
        public Int32 dmReserved1;
        [System.Runtime.InteropServices.FieldOffset(144)]
        public Int32 dmReserved2;
        [System.Runtime.InteropServices.FieldOffset(148)]
        public Int32 dmPanningWidth;
        [System.Runtime.InteropServices.FieldOffset(152)]
        public Int32 dmPanningHeight;
    }

    internal static class NativeMethods
    {
        [DllImport("User32.dll")]
        public static extern bool EnumDisplayDevices(IntPtr lpDevice, int iDevNum, ref DisplayDevice lpDisplayDevice, int dwFlags);
        [DllImport("User32.dll", CharSet = CharSet.Ansi)]
        public static extern bool EnumDisplaySettings(string devName, int modeNum, ref DEVMODE devMode);
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern bool EnumDisplaySettingsEx(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode, int dwFlags);
        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);
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