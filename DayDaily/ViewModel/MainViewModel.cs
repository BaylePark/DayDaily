using DayDaily.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;

namespace DayDaily.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        #region Binding Properties
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { Set(ref _currentViewModel, value); }
        }

        private Rect _windowRect = new Rect(0, 0, 1920, 1080);
        public Rect WindowRect
        {
            get { return _windowRect; }
            set { Set(ref _windowRect, value); }
        }
        #endregion

        #region Commands 
        public RelayCommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                return _loadedCommand ?? (_loadedCommand = new RelayCommand(async () =>
                {
                    await _dataService.LoadAsync();
                }));
            }
        }
        #endregion

        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}