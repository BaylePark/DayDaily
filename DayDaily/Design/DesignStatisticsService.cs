using DayDaily.Model;
using DayDaily.Model.VO;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;

namespace DayDaily.Design
{
    public class DesignStatisticsService : IStatisticsService
    {
        public Record BestUser => new Record()
        {
            User = new UserInfo() { Name = "박병훈", SingleID = "bayle.park", Belong = "S/W Platform Lab" },
            StartTime = 5,
            EndTime = 175
        };

        public IList<Record> AllRecords { get; private set; } = new List<Record>();
        public int WholeTime { get; private set; }

        private Random _random = new Random();

        public DesignStatisticsService()
        {
            var dataService = SimpleIoc.Default.GetInstance<IDataService>();
            var users = dataService.GetAllUserInfos();
            int lastEndTime = 0;
            foreach(var user in users)
            {
                var startTime = lastEndTime + _random.Next(200);
                lastEndTime = startTime + _random.Next(120) + 60;
                AllRecords.Add(new Record() { User = user, StartTime = startTime, EndTime = lastEndTime });
            }
            WholeTime = lastEndTime + 40;
        }

        public void StartDailyMeeting()
        {
            throw new NotImplementedException();
        }

        public void EndDailyMeeting()
        {
            throw new NotImplementedException();
        }

        public void StartUser(UserInfo user)
        {
            throw new NotImplementedException();
        }

        public void EndUser(UserInfo user)
        {
            throw new NotImplementedException();
        }
    }
}
