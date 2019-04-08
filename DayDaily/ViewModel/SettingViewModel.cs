using DayDaily.Messages;
using DayDaily.Model;
using DayDaily.Model.VO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DayDaily.ViewModel
{
    public class ScreenControlViewModel : ViewModelBase
    {
        DisplayDeviceInfo _info;
        public DisplayDeviceInfo Info { get => _info; set => Set(ref _info, value); }

        bool _isSelected = false;
        public bool IsSelected { get => _isSelected; set => Set(ref _isSelected, value); }

        public ScreenControlViewModel(DisplayDeviceInfo info)
        {
            Info = info;
        }
    }

    public enum MoveFocusType
    {
        Left,
        Right
    }

    public class SettingViewModel : ViewModelBase
    {
        IList<ScreenControlViewModel> _screenList = new ObservableCollection<ScreenControlViewModel>();
        public IList<ScreenControlViewModel> ScreenList
        {
            get => _screenList;
            set => Set(ref _screenList, value);
        }

        private int _selectedScreenIndex = 0;
        public int SelectedScreenIndex
        {
            get => _selectedScreenIndex;
            set
            {
                if (value < 0 || value >= ScreenList.Count) return;
                if (ScreenList.Count > _selectedScreenIndex)
                    ScreenList[_selectedScreenIndex].IsSelected = false;
                _selectedScreenIndex = value;
                ScreenList[_selectedScreenIndex].IsSelected = true;
                MessengerInstance.Send(new ChangeWindowRectMessage(ScreenList[_selectedScreenIndex].Info.Region.Location));
            }
        }

        #region Services
        IDisplayService _displayService;
        ISettingService _settingService;
        #endregion

        #region Commands 
        public RelayCommand<MoveFocusType> _moveFocusCommand;
        public ICommand MoveFocusCommand
        {
            get => _moveFocusCommand ?? (_moveFocusCommand = new RelayCommand<MoveFocusType>((param) =>
            {
                if (param == MoveFocusType.Left) SelectedScreenIndex--;
                else SelectedScreenIndex++;
            }));
        }

        public RelayCommand _completeCommand;
        public ICommand CompleteCommand
        {
            get => _completeCommand ?? (_completeCommand = new RelayCommand(() =>
            {
                MessengerInstance.Send(new CompleteMessage(this));

                Task.Run(() =>
                {
                    _settingService.LastScreenInfo = ScreenList[_selectedScreenIndex].Info;
                    _settingService.SaveSettings();
                });
            }));
        }
        #endregion

        private void InitScreenInfo()
        {
            var lastScreenInfo = _settingService.LastScreenInfo;
            var selectedScreenIndex = 0;
            foreach (var device in _displayService.DisplayDevices)
            {
                ScreenList.Add(new ScreenControlViewModel(device));
                if (lastScreenInfo?.DeviceKey == device.DeviceKey)
                {
                    selectedScreenIndex = ScreenList.Count - 1;
                }
            }
            SelectedScreenIndex = selectedScreenIndex;
        }

        public SettingViewModel(IDisplayService displayService, ISettingService settingService)
        {
            _displayService = displayService;
            _settingService = settingService;
            InitScreenInfo();
            _displayService.DisplayChanged += (s, e) =>
            {
                ScreenList.Clear();
                InitScreenInfo();
            };
        }
    }
}
