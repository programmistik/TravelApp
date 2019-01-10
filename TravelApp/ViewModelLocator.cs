using Autofac;
using Autofac.Configuration;
using GalaSoft.MvvmLight;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelApp.Services;
using TravelApp.ViewModels;
using TravelApp.Views;

namespace TravelApp
{
    class ViewModelLocator
    {
        private AppViewModel appViewModel;
       
        private SignUpPageViewModel SignUpViewModel;
        private LogInPageViewModel LogInViewModel;
        private MainPageViewModel MainViewModel;
        private NewTripPageViewModel NewTripViewModel;
        private ShowPdfPageViewModel ShowPdfViewModel;
        private ShowMapPageViewModel ShowMapViewModel;
        private ShowGoogleMapPageViewModel ShowGoogleMapViewModel;

        private INavigationService navigationService;

        public static IContainer Container;

        public ViewModelLocator()
        {
            try
            {
                var config = new ConfigurationBuilder();
                config.AddJsonFile(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\autofac.json");
                var module = new ConfigurationModule(config.Build());
                var builder = new ContainerBuilder();
                builder.RegisterModule(module);
                Container = builder.Build();

                navigationService = Container.Resolve<INavigationService>();
                appViewModel = Container.Resolve<AppViewModel>();
           //     
                SignUpViewModel = Container.Resolve<SignUpPageViewModel>();
                LogInViewModel = Container.Resolve<LogInPageViewModel>();
                MainViewModel = Container.Resolve<MainPageViewModel>();
                NewTripViewModel = Container.Resolve<NewTripPageViewModel>();
                ShowPdfViewModel = Container.Resolve<ShowPdfPageViewModel>();
                ShowMapViewModel = Container.Resolve<ShowMapPageViewModel>();
                ShowGoogleMapViewModel = Container.Resolve<ShowGoogleMapPageViewModel>();

                navigationService.Register<LogInPageView>(LogInViewModel);
                navigationService.Register<SignUpPageView>(SignUpViewModel);
                navigationService.Register<MainPageView>(MainViewModel);
                navigationService.Register<NewTripPageView>(NewTripViewModel);
                navigationService.Register<ShowPdfPageView>(ShowPdfViewModel);
                navigationService.Register<ShowMapPageView>(ShowMapViewModel);
                navigationService.Register<ShowGoogleMapPageView>(ShowGoogleMapViewModel);

                navigationService.Navigate<LogInPageView>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ViewModelBase GetAppViewModel()
        {
            return appViewModel;
        }
    }
}