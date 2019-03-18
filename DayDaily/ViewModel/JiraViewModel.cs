using DayDaily.Messages;
using DayDaily.Model;
using DayDaily.Model.VO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Windows.Input;

namespace DayDaily.ViewModel
{
    public class JiraViewModel : ViewModelBase
    {
        public IList<JiraItem> JiraItems { get; private set; }

        UserInfo _userInfo;
        public UserInfo UserInfo { get => _userInfo; set => Set(ref _userInfo, value); }

        bool _splashViewVisibility;
        public bool SplashViewVisibility { get => _splashViewVisibility; set => Set(ref _splashViewVisibility, value); }

        IDataService _dataService;

        #region Commands
        public RelayCommand _skipUserCommand;
        public ICommand SkipUserCommand
        {
            get => _skipUserCommand ?? (_skipUserCommand = new RelayCommand(() =>
            {
                MessengerInstance.Send(new SkipUserMessage());
            }));
        }

        public RelayCommand _startDailyMeetingCommand;
        public ICommand StartDailyMeetingCommand
        {
            get => _startDailyMeetingCommand ?? (_startDailyMeetingCommand = new RelayCommand(() =>
            {
                SplashViewVisibility = false;
            }));
        }
        #endregion

        public JiraViewModel()
        {
            _dataService = GalaSoft.MvvmLight.Ioc.SimpleIoc.Default.GetInstance<IDataService>();

            UserInfo = _dataService.CurrentUser;
            JiraItems = _dataService.GetJiraItemsByUserName(UserInfo.Name);
            SplashViewVisibility = true;
        }
    }
}
