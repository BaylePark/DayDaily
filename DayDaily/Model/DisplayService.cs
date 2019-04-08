using DayDaily.Common;
using DayDaily.Common.Interop;
using DayDaily.Model.VO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace DayDaily.Model
{
    public class DisplayService : IDisplayService
    {
        public IList<DisplayDeviceInfo> DisplayDevices => new List<DisplayDeviceInfo>(GetAllDisplayDevices());

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

        List<DEVMODE> _allDevmodes = new List<DEVMODE>();
        public void ChangeAllResolution(Size resolution)
        {
            var displayDevice = new DisplayDevice(0);
            int devNum = 0;
            while (NativeMethods.EnumDisplayDevices(IntPtr.Zero, devNum, ref displayDevice, 0))
            {
                devNum++;
                if ((displayDevice.StateFlags & DeviceStatus.DISPLAY_DEVICE_ACTIVE) == 0) continue;
                DEVMODE devmode = new DEVMODE();
                NativeMethods.EnumDisplaySettings(displayDevice.DeviceName, -1, ref devmode);
                _allDevmodes.Add(devmode);
                var newDevMode = devmode;
                newDevMode.dmPelsWidth = (uint)resolution.Width;
                newDevMode.dmPelsHeight = (uint)resolution.Height;
                var result = NativeMethods.ChangeDisplaySettings(ref newDevMode, 1);
            }
        }

        public void RevertResolution()
        {
            foreach(var devmode in _allDevmodes)
            {
                var prevDevmode = devmode;
                var result = NativeMethods.ChangeDisplaySettings(ref prevDevmode, 1);
            }
        }
    }
}
