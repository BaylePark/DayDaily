using DayDaily.Messages;
using DayDaily.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DayDaily.ViewModel
{
    public class LoadingViewModel : ViewModelBase
    {
        private IDataService _dataService;

        #region Commands 
        public RelayCommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get => _loadedCommand ?? (_loadedCommand = new RelayCommand(async () =>
            {
                await Task.Run(async () =>
                {
                    await _dataService.LoadAsync();
                    Thread.Sleep(1000);
                });

                MessengerInstance.Send(new CompleteMessage(this));
            }));
        }
        #endregion

        public LoadingViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }
    }
}
