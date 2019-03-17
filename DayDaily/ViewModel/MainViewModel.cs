﻿using DayDaily.Messages;
using DayDaily.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System.Windows;

namespace DayDaily.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly ISettingService _settingService;

        #region Binding Properties
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel { get => _currentViewModel; set => Set(ref _currentViewModel, value); }

        private Rect _windowRect = new Rect(0, 0, 1920, 1080);
        public Rect WindowRect { get => _windowRect; set => Set(ref _windowRect, value); }

        public Size _workingRect = new Size(1920, 1080);
        public Size WorkingRect { get => _workingRect; set => Set(ref _workingRect, value); }
        #endregion

        public MainViewModel(IDataService dataService, ISettingService settingService)
        {
            _dataService = dataService;
            _settingService = settingService;

            var selectedScreenIdx = _settingService.SelectedScreenIndex;
            WindowRect = _settingService.GetWindowRectFromIndex(selectedScreenIdx);
            WorkingRect = CalculateWorkingSize(WindowRect);
#if DEBUG
            CurrentViewModel = SimpleIoc.Default.GetInstance<LoadingViewModel>();
#else
            CurrentViewModel = SimpleIoc.Default.GetInstance<LoadingViewModel>();
#endif

            MessengerInstance.Register<ChangeWindowRectMessage>(this, (msg) =>
            {
                WindowRect = msg.ChangingRect;
                WorkingRect = CalculateWorkingSize(WindowRect);
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
                    CurrentViewModel = new JiraViewModel();
                }
            });
        }

        private Size CalculateWorkingSize(Rect windowRect)
        {
            Size size = new Size();
            size.Width = windowRect.Width * 9 / 10;
            size.Height = windowRect.Height * 9 / 10;
            return size;
        }
    }
}