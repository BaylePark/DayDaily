using DayDaily.Messages;
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
        private readonly IStatisticsService _statisticsService;

        #region Binding Properties
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel { get => _currentViewModel; set => Set(ref _currentViewModel, value); }

        private Rect _windowRect = new Rect(0, 0, 1920, 1080);
        public Rect WindowRect { get => _windowRect; set => Set(ref _windowRect, value); }

        public Size _workingRect = new Size(1920, 1080);
        public Size WorkingRect { get => _workingRect; set => Set(ref _workingRect, value); }
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

        public MainViewModel(IDataService dataService, ISettingService settingService, IStatisticsService statisticsService)
        {
            _dataService = dataService;
            _settingService = settingService;
            _statisticsService = statisticsService;

            var selectedScreenIdx = _settingService.SelectedScreenIndex;
            WindowRect = _settingService.GetWindowRectFromIndex(selectedScreenIdx);
            WorkingRect = CalculateWorkingSize(WindowRect);

            CurrentViewModel = SimpleIoc.Default.GetInstance<LoadingViewModel>();

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

        private Size CalculateWorkingSize(Rect windowRect)
        {
            Size size = new Size
            {
                Width = windowRect.Width * 9 / 10,
                Height = windowRect.Height * 9 / 10
            };
            return size;
        }
    }
}