using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DayDaily.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
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
    }
}
