using DayDaily.Common;
using DayDaily.Common.Interop;
using DayDaily.Model.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DayDaily.Model
{
    public class DisplayService : IDisplayService
    {
        public IList<DisplayDeviceInfo> DisplayDevices => new List<DisplayDeviceInfo>(GetAllDisplayDevices());

        public event EventHandler DisplayChanged;

        private IEnumerable<DisplayDeviceInfo> GetAllDisplayDevices()
        {
            var allScreens = new List<Screen>(Screen.GetAllScreens());
            var displayDevice = new DisplayDevice(0);
            int devNum = 0;
            while (NativeMethods.EnumDisplayDevices(IntPtr.Zero, devNum, ref displayDevice, 0))
            {
                devNum++;
                if ((displayDevice.StateFlags & DeviceStatus.DISPLAY_DEVICE_ACTIVE) == 0) continue;
                yield return new DisplayDeviceInfo()
                {
                    DeviceAccessID = devNum - 1,
                    DeviceName = displayDevice.DeviceName,
                    DeviceString = displayDevice.DeviceString,
                    DeviceID = displayDevice.DeviceID,
                    DeviceKey = displayDevice.DeviceKey,
                    IsPrimary = (displayDevice.StateFlags & DeviceStatus.DISPLAY_DEVICE_PRIMARY_DEVICE) != 0 ? true : false,
                    Region = (from screen in allScreens where screen.DeviceName == displayDevice.DeviceName select screen.DeviceBounds).ElementAt(0)
                };
            }
        }

        public void ChangeAllResolution(Size resolution)
        {
            var displayDevice = new DisplayDevice(0);
            int devNum = 0;
            while (NativeMethods.EnumDisplayDevices(IntPtr.Zero, devNum, ref displayDevice, 0))
            {
                devNum++;
                if ((displayDevice.StateFlags & DeviceStatus.DISPLAY_DEVICE_ACTIVE) == 0) continue;
                DEVMODE devmode = new DEVMODE
                {
                    dmSize = (short)System.Runtime.InteropServices.Marshal.SizeOf<DEVMODE>()
                };
                var ret = NativeMethods.EnumDisplaySettings(displayDevice.DeviceName, -1, ref devmode);
                var newDevMode = devmode;
                newDevMode.dmPelsWidth = (int)resolution.Width;
                newDevMode.dmPelsHeight = (int)resolution.Height;
                newDevMode.dmFields = DispChangeField.DM_PELSWIDTH | DispChangeField.DM_PELSHEIGHT;
                var result = NativeMethods.ChangeDisplaySettingsEx(displayDevice.DeviceName, ref newDevMode, IntPtr.Zero, ChangeDisplaySettingsFlags.CDS_FULLSCREEN, IntPtr.Zero);
            }
        }

        public void NotifyDisplayChanged()
        {
            DisplayChanged?.Invoke(this, new EventArgs());
        }
    }
}
