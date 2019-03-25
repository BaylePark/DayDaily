using DayDaily.Model.VO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace DayDaily.Model
{
    class SettingService : ISettingService
    {
        readonly private string _path = @Path.Combine(Environment.CurrentDirectory, "settings.json");
        private Settings _settings = new Settings();

        public SettingService()
        {
            LoadSettingFile();
        }

        private void LoadSettingFile()
        {
            if (File.Exists(_path))
            {
                _settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_path));
            }
        }

        public int SelectedScreenIndex
        {
            get
            {
                if (_settings.SelectedScreenIndex >= ScreenCount)
                    return 0;
                return _settings.SelectedScreenIndex;
            }
            set => _settings.SelectedScreenIndex = value;
        }

        public int ScreenCount => Common.Screen.GetScreenCount();

        public Rect GetWindowRectFromIndex(int index)
        {
            return Common.Screen.GetScreenFromIndex(index).DeviceBounds;
        }

        public IEnumerable<ScreenInfo> GetAllScreens()
        {
            int index = 0;
            foreach (var screen in Common.Screen.AllScreens())
            {
                yield return new ScreenInfo() { Index = index++, IsPrimary = screen.IsPrimary, Resolution = screen.DeviceBounds.Size, DeviceName = screen.DeviceName };
            }
        }

        public void SaveSettings()
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(_settings));
        }
    }
}
