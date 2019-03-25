using DayDaily.Model.VO;
using System.Collections.Generic;

namespace DayDaily.Model
{
    public interface IStatisticsService
    {
        Record BestUser { get; }
        IList<Record> AllRecords { get; }
        int WholeTime { get; }

        void StartDailyMeeting();
        void EndDailyMeeting();
        void StartUser(UserInfo user);
        void EndUser(UserInfo user);
    }
}
