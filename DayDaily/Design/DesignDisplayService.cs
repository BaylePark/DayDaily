using DayDaily.Model;
using DayDaily.Model.VO;
using System.Collections.Generic;
using System.Windows;

namespace DayDaily.Design
{
    public class DesignDisplayService : IDisplayService
    {
        public IList<DisplayDeviceInfo> DisplayDevices => new List<DisplayDeviceInfo>(new DisplayDeviceInfo[]
        {
            new DisplayDeviceInfo()
            {
                DeviceKey = "HASH#1",
                DeviceName = "A",
                DeviceString = "Display #1",
                IsPrimary = true,
                Region = new Rect(0, 0, 1920, 1080)
            },
            new DisplayDeviceInfo()
            {
                DeviceKey = "HASH#2",
                DeviceName = "B",
                DeviceString = "Display #2",
                IsPrimary = false,
                Region = new Rect(1920, 0, 1920, 1080)
            }
        });

        public void ChangeAllResolution(Size resolution)
        {   
        }

        public void RevertResolution()
        {
        }
    }
}
