using System;

namespace DayDaily.Model.VO
{
    public class Record : IComparable<Record>
    {
        public UserInfo User { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        private int _duration = -1;
        public int Duration
        {
            get
            {
                if (_duration == -1)
                    return EndTime - StartTime;
                else return _duration;
            }
            set
            {
                _duration = value;
            }
        }

        public int CompareTo(Record other)
        {
            if (Duration > other.Duration) return 1;
            else if (Duration == other.Duration) return 0;
            else return -1;
        }
    }
}
