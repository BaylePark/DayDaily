using DayDaily.Model;
using DayDaily.Model.VO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Windows.Input;

namespace DayDaily.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        public IList<Record> AllRecords { get; private set; }

        private IStatisticsService _statisticsService;

        #region Binding Properties
        Record _bestUser;
        public Record BestUser { get => _bestUser; set => Set(ref _bestUser, value); }

        int _wholeTime;
        public int WholeTime { get => _wholeTime; set => Set(ref _wholeTime, value); }
        #endregion

        #region Commands
        public RelayCommand _completeCommand;
        public ICommand CompleteCommand
        {
            get => _completeCommand ?? (_completeCommand = new RelayCommand(() =>
            {
                System.Windows.Application.Current.Shutdown();
            }));
        }
        #endregion

        public StatisticsViewModel(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;

            BestUser = _statisticsService.BestUser;
            AllRecords = _statisticsService.AllRecords;
            WholeTime = _statisticsService.WholeTime;
        }
    }
}
