using DayDaily.Model;
using DayDaily.Model.VO;
using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace DayDaily.ViewModel
{
    public class JiraViewModel : ViewModelBase
    {
        public IList<JiraItem> JiraItems { get; private set; }

        UserInfo _userInfo;
        public UserInfo UserInfo { get => _userInfo; set => Set(ref _userInfo, value); }

        string _welcomeMessage;
        public string WelcomeMessage { get => _welcomeMessage; set => Set(ref _welcomeMessage, value); }

        bool _splashViewVisibility = true;
        public bool SplashViewVisibility { get => _splashViewVisibility; set => Set(ref _splashViewVisibility, value); }

        IDataService _dataService;

        public JiraViewModel()
        {
            _dataService = GalaSoft.MvvmLight.Ioc.SimpleIoc.Default.GetInstance<IDataService>();

            var developer = _dataService.GetCurrentDeveloperInfo();
            UserInfo = developer.UserInfo;
            JiraItems = developer.JiraItems;

            WelcomeMessage = _dataService.GetWelcomeMessage();
        }
    }
}
