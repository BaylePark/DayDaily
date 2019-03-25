using DayDaily.Model.VO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DayDaily.Model
{
    public class StatisticsService : IStatisticsService
    {
        public Record BestUser
        {
            get
            {
                return (from r in (from rec in AllRecords
                                   group rec by rec.User.Name into grouppedRec
                                   select new Record()
                                   {
                                       User = grouppedRec.ElementAt(0).User,
                                       Duration = grouppedRec.Sum(s => s.Duration)
                                   })
                        where r.Duration <= 180
                        select r).Max();
            }
        }

        public IList<Record> AllRecords { get; private set; } = new List<Record>();

        public int WholeTime { get; private set; }

        DateTime _baseTime;

        private int GetSeconds()
        {
            var span = DateTime.Now - _baseTime;
            return (int)span.TotalSeconds;
        }

        public void StartDailyMeeting()
        {
            _baseTime = DateTime.Now;
        }

        public void EndDailyMeeting()
        {
            WholeTime = GetSeconds();
        }

        public void StartUser(UserInfo user)
        {
            AllRecords.Add(new Record()
            {
                User = user,
                StartTime = GetSeconds()
            });
        }

        public void EndUser(UserInfo user)
        {
            var lastRecord = (from rec in AllRecords
                              where rec.User.Name == user.Name
                              select rec).LastOrDefault();
            lastRecord.EndTime = GetSeconds();
        }
    }
}
