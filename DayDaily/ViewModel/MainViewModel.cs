using DayDaily.Common;
using DayDaily.Messages;
using DayDaily.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace DayDaily.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDisplayService _displayService;
        private readonly IDataService _dataService;
        private readonly ISettingService _settingService;
        private readonly IStatisticsService _statisticsService;

        #region Binding Properties
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel { get => _currentViewModel; set => Set(ref _currentViewModel, value); }

        private Point _location = new Point(0, 0);
        public Point Location { get => _location; set => Set(ref _location, value); }
        #endregion

        #region Commands
        RelayCommand displayChangedCommand_;
        public ICommand DisplayChangedCommand => displayChangedCommand_ ?? (displayChangedCommand_ = new RelayCommand(() =>
        {
            _displayService.ChangeAllResolution(new Size(1920, 1080));
            _displayService.NotifyDisplayChanged();
        }));
        #endregion

        int _userIndex = -1;
        public void IncreaseUser(UserPageControlMessage msg)
        {
            if (msg.Type == UserPageControlType.Next)
            {
                _userIndex++;
                _dataService.SetUserByOrder(_userIndex);
            }
            else
            {
                _userIndex--;
                _dataService.SetUserByOrder(_userIndex);
            }
            CurrentViewModel = new JiraViewModel()
            {
                IsFirstUser = _userIndex == 0,
                IsLastUser = _userIndex == _dataService.GetAllUserInfos().Count - 1,
                IsAgainUser = msg.Type == UserPageControlType.Prev,
            };
        }

        public MainViewModel(IDisplayService displayService, IDataService dataService, ISettingService settingService, IStatisticsService statisticsService)
        {
            _displayService = displayService;
            _dataService = dataService;
            _settingService = settingService;
            _statisticsService = statisticsService;

            _displayService.ChangeAllResolution(new Size(1920, 1080));
            CurrentViewModel = SimpleIoc.Default.GetInstance<LoadingViewModel>();

            MessengerInstance.Register<ChangeWindowRectMessage>(this, (msg) =>
            {
                Location = msg.Location;
                Debug.WriteLine(string.Format("Location has been changed ({0}, {1})", Location.X, Location.Y));
            });

            MessengerInstance.Register<CompleteMessage>(this, (msg) =>
            {
                if (msg.From is LoadingViewModel)
                {
                    CurrentViewModel = SimpleIoc.Default.GetInstance<SettingViewModel>();
                }
                else if (msg.From is SettingViewModel)
                {
                    CurrentViewModel = SimpleIoc.Default.GetInstance<UserViewModel>();
                }
                else if (msg.From is UserViewModel)
                {
                    _statisticsService.StartDailyMeeting();
                    IncreaseUser(new UserPageControlMessage(UserPageControlType.Next));
                }
                else if (msg.From is JiraViewModel)
                {
                    _statisticsService.EndDailyMeeting();
                    CurrentViewModel = SimpleIoc.Default.GetInstance<StatisticsViewModel>();
                }
            });

            MessengerInstance.Register<UserPageControlMessage>(this, (msg) =>
            {
                IncreaseUser(msg);
            });
        }
    }
}