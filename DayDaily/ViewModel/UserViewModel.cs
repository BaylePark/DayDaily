using DayDaily.Messages;
using DayDaily.Model;
using DayDaily.Model.VO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DayDaily.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        public IList<UserInfo> ShuffledUsers { get; private set; } = new List<UserInfo>();

        #region Services
        IDataService _dataService;
        #endregion

        #region Commands
        public RelayCommand _completeCommand;
        public ICommand CompleteCommand
        {
            get => _completeCommand ?? (_completeCommand = new RelayCommand(() =>
            {
                MessengerInstance.Send(new CompleteMessage(this));
            }));
        }
        #endregion

        void AddShuffledUser(IList<UserInfo> users)
        {
            var random = new Random();
            while (users.Count > 0)
            {
                var index = random.Next() % users.Count;
                ShuffledUsers.Add(users[index]);
                users.RemoveAt(index);
            }
        }

        public UserViewModel(IDataService dataService)
        {
            _dataService = dataService;

            AddShuffledUser(_dataService.GetAllUserInfos());
        }
    }
}
