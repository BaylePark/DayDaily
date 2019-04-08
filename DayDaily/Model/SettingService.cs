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

        public DisplayDeviceInfo LastScreenInfo { get => _settings.LastDisplayDevice; set => _settings.LastDisplayDevice = value; }

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
        
        public void SaveSettings()
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(_settings));
        }
    }
}
