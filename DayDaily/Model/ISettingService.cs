﻿using DayDaily.Model.VO;
using System.Collections.Generic;
using System.Windows;

namespace DayDaily.Model
{
    public interface ISettingService
    {
        int SelectedScreenIndex { get; set; }
        int ScreenCount { get; }

        Rect GetWindowRectFromIndex(int index);
        IEnumerable<ScreenInfo> GetAllScreens();

        void SaveSettings();
    }
}
