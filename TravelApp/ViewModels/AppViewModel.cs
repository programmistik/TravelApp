using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        private ViewModelBase currentPage;
        public ViewModelBase CurrentPage { get => currentPage; set => Set(ref currentPage, value); }

        private string applicationTitle;
        public string ApplicationTitle { get => applicationTitle; set => Set(ref applicationTitle, value); }


        private readonly INavigationService navigation;

        public AppViewModel(INavigationService navigation)
        {
            this.navigation = navigation;

            Messenger.Default.Register<ViewModelBase>(this, viewModel => CurrentPage = viewModel);
            Messenger.Default.Register<ViewModelBase>(this, viewModel => 
            {
                var str = viewModel.ToString();
                if (viewModel is LogInPageViewModel)
                    ApplicationTitle = "Log in";
                else if (viewModel is SignUpPageViewModel)
                    ApplicationTitle = "Sign Up";
                else if (viewModel is MainPageViewModel)
                    ApplicationTitle = "Main page";
                else if (viewModel is NewTripPageViewModel)
                    ApplicationTitle = "Trip";
                else if (viewModel is ShowMapPageViewModel)
                {
                    var vm = viewModel as ShowMapPageViewModel;
                    ApplicationTitle = vm.PageTitle;
                }
            }
            );
        }

        private RelayCommand<Type> navigateCommand;
        public RelayCommand<Type> NavigateCommand
        {
            get => navigateCommand ?? (navigateCommand = new RelayCommand<Type>(
                param =>
                {
                    navigation.Navigate(param);
                }
            ));
        }

        
    }
}

