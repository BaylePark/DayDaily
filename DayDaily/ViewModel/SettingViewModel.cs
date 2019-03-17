using DayDaily.Messages;
using DayDaily.Model;
using DayDaily.Model.VO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DayDaily.ViewModel
{
    public class ScreenControlViewModel : ViewModelBase
    {
        ScreenInfo _info;
        public ScreenInfo Info { get => _info; set => Set(ref _info, value); }

        bool _isSelected = false;
        public bool IsSelected { get => _isSelected; set => Set(ref _isSelected, value); }

        public ScreenControlViewModel(ScreenInfo info)
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
        public IList<ScreenControlViewModel> ScreenList { get; private set; } = new List<ScreenControlViewModel>();

        private int _selectedScreenIndex = 0;
        public int SelectedScreenIndex
        {
            get => _selectedScreenIndex;
            set
            {
                if (value < 0 || value >= ScreenList.Count) return;
                ScreenList[_selectedScreenIndex].IsSelected = false;
                _selectedScreenIndex = value;
                ScreenList[_selectedScreenIndex].IsSelected = true;
                MessengerInstance.Send(new ChangeWindowRectMessage() { ChangingRect = _settingService.GetWindowRectFromIndex(_selectedScreenIndex) });
                _settingService.SelectedScreenIndex = _selectedScreenIndex;
            }
        }

        #region Services
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
                Task.Run(() => _settingService.SaveSettings());
            }));
        }
        #endregion

        public SettingViewModel(ISettingService settingService)
        {
            _settingService = settingService;

            var selectedScreenIndex = _settingService.SelectedScreenIndex;
            foreach (var screen in _settingService.GetAllScreens())
            {
                var scvm = new ScreenControlViewModel(screen);
                ScreenList.Add(scvm);
            }
            SelectedScreenIndex = selectedScreenIndex;
        }
    }
}
