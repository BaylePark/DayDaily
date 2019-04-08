using DayDaily.Model.VO;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DayDaily.Model
{

    public interface IDisplayService
    {
        IList<DisplayDeviceInfo> DisplayDevices { get; }
        void ChangeAllResolution(Size resolution);
        event EventHandler DisplayChanged;
        void NotifyDisplayChanged();
    }
}
