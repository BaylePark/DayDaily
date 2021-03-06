﻿using DayDaily.Messages;
using DayDaily.Model;
using DayDaily.Model.VO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace DayDaily.ViewModel
{
    public class JiraViewModel : ViewModelBase
    {
        public IList<JiraItem> JiraItems { get; private set; }

        UserInfo _userInfo;
        public UserInfo UserInfo { get => _userInfo; set => Set(ref _userInfo, value); }

        bool _splashViewVisibility;
        public bool SplashViewVisibility { get => _splashViewVisibility; set => Set(ref _splashViewVisibility, value); }

        bool _isFirstUser;
        public bool IsFirstUser { get => _isFirstUser; set => Set(ref _isFirstUser, value); }

        bool _isLastUser;
        public bool IsLastUser { get => _isLastUser; set => Set(ref _isLastUser, value); }

        bool _isAgainUser;
        public bool IsAgainUser { get => _isAgainUser; set => Set(ref _isAgainUser, value); }

        bool _isShaking;
        public bool IsShaking { get => _isShaking; set => Set(ref _isShaking, value); }

        int _currentClock;
        public int CurrentClock { get => _currentClock; set => Set(ref _currentClock, value); }

        double _verticalScrollOffset;
        public double VerticalScrollOffset { get => _verticalScrollOffset; set => Set(ref _verticalScrollOffset, value); }

        double _shakingFactor = 0.03;
        public double ShakingFactor { get => _shakingFactor; set => Set(ref _shakingFactor, value); }

        public IList<JiraItem>[] JiraItemsByStatus { get; private set; } = new IList<JiraItem>[5];

        DispatcherTimer _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        private readonly IDataService _dataService;
        private readonly IStatisticsService _statisticsService;

        #region Commands
        public RelayCommand _nextUserCommand;
        public ICommand NextUserCommand
        {
            get => _nextUserCommand ?? (_nextUserCommand = new RelayCommand(
                () =>
                {
                    CompleteUser();
                    if (SplashViewVisibility == false)
                    {
                        _statisticsService.EndUser(UserInfo);
                    }
                    if (IsLastUser)
                    {
                        MessengerInstance.Send(new CompleteMessage(this));
                        return;
                    }
                    MessengerInstance.Send(new UserPageControlMessage(UserPageControlType.Next));
                }));
        }

        public RelayCommand _prevUserCommand;
        public ICommand PrevUserCommand
        {
            get => _prevUserCommand ?? (_prevUserCommand = new RelayCommand(
                () =>
                {
                    CompleteUser();
                    if (SplashViewVisibility == false)
                    {
                        _statisticsService.EndUser(UserInfo);
                    }
                    MessengerInstance.Send(new UserPageControlMessage(UserPageControlType.Prev));
                },
                () => !IsFirstUser));
        }

        public RelayCommand _startDailyMeetingCommand;
        public ICommand StartDailyMeetingCommand
        {
            get => _startDailyMeetingCommand ?? (_startDailyMeetingCommand = new RelayCommand(() =>
            {
                if (SplashViewVisibility == false)
                {
#if DEBUG
                    IsShaking = true;
#endif
                    return;
                }
                SplashViewVisibility = false;
                _timer.Start();
                _statisticsService.StartUser(UserInfo);
            }));
        }

        public RelayCommand _upKeyCommand;
        public ICommand UpKeyCommand
        {
            get => _upKeyCommand ?? (_upKeyCommand = new RelayCommand(
                () =>
                {
                    VerticalScrollOffset--;
                }));
        }
        #endregion

        void CompleteUser()
        {
            _timer.Stop();
        }

        public JiraViewModel()
        {
            _dataService = GalaSoft.MvvmLight.Ioc.SimpleIoc.Default.GetInstance<IDataService>();
            _statisticsService = GalaSoft.MvvmLight.Ioc.SimpleIoc.Default.GetInstance<IStatisticsService>();

            UserInfo = _dataService.CurrentUser;
            JiraItems = _dataService.GetJiraItemsByUserName(UserInfo.Name);
            SplashViewVisibility = true;

            for (int i = 0; i < 5; i++)
            {
                JiraItemsByStatus[i] = new List<JiraItem>(from jiraitem in JiraItems
                                                          where jiraitem.Status == (JiraItemStatus)i && jiraitem.Type != JiraItemType.Epic
                                                          select jiraitem);
            }

            _timer.Tick += (s, e) =>
            {
                CurrentClock++;
                if (CurrentClock >= 120)
                {
                    if (!IsShaking) IsShaking = true;
                    if (ShakingFactor < 3.0)
                    {
                        ShakingFactor += 0.03;
                    }
                }
            };
        }
    }
}
