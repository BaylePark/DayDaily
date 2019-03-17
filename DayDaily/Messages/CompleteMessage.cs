using GalaSoft.MvvmLight;

namespace DayDaily.Messages
{
    public class CompleteMessage
    {
        public ViewModelBase From { get; set; }

        public CompleteMessage(ViewModelBase from)
        {
            From = from;
        }
    }
}
