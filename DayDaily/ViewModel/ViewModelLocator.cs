﻿/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:DayDaily.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using CommonServiceLocator;
using DayDaily.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace DayDaily.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
                SimpleIoc.Default.Register<ISettingService, Design.DesignSettingService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, SecDataService>();
                SimpleIoc.Default.Register<ISettingService, SettingService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoadingViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();
            SimpleIoc.Default.Register<UserViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main { get => ServiceLocator.Current.GetInstance<MainViewModel>(); }

        public LoadingViewModel Loading { get => ServiceLocator.Current.GetInstance<LoadingViewModel>(); }

        public SettingViewModel Setting { get => ServiceLocator.Current.GetInstance<SettingViewModel>(); }

        public UserViewModel User { get => ServiceLocator.Current.GetInstance<UserViewModel>(); }

        public static void Cleanup()
        {
        }
    }
}