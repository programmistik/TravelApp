using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;
using TravelApp.Services;
using TravelApp.Views;

namespace TravelApp.ViewModels
{

    public class ShowMapPageViewModel : ViewModelBase
    {
        private Location latLon;
        public Location LatLon { get => latLon; set => Set(ref latLon, value); }

        private string pageTitle;
        public string PageTitle { get => pageTitle; set => Set(ref pageTitle, value); }


        private readonly INavigationService navigation;

        

        public ShowMapPageViewModel(INavigationService navigation)
        {
            this.navigation = navigation;
            Messenger.Default.Register<City>(this, city => LatLon = new Location(Convert.ToDouble(city.Latitude, CultureInfo.InvariantCulture), Convert.ToDouble(city.Longitude, CultureInfo.InvariantCulture)));
            Messenger.Default.Register<City>(this, city => PageTitle = city.CityName);
        }

        private RelayCommand backCommand;
        public RelayCommand BackCommand
        {
            get => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<NewTripPageView>();
                }
            ));
        }
    }
}
