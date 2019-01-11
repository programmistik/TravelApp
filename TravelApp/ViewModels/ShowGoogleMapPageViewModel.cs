using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;
using TravelApp.Services;
using TravelApp.Views;

namespace TravelApp.ViewModels
{
    class ShowGoogleMapPageViewModel : ViewModelBase
    {
        private string mapUri;
        public string MapUri { get => mapUri; set => Set(ref mapUri, value); }

        private string pageTitle;
        public string PageTitle { get => pageTitle; set => Set(ref pageTitle, value); }

        private readonly INavigationService navigation;

        public ShowGoogleMapPageViewModel(INavigationService navigation)
        {
            this.navigation = navigation;
            Messenger.Default.Register<NotificationMessage<City>>(this, OnHitIt);
            
        }

        private void OnHitIt(NotificationMessage<City> cty)
        {
            if (cty.Notification == "SendCityToGoogle")
            {
                PageTitle = cty.Content.CityName;
                var MyMapUri = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\MyMap.html";
                var NewMapUri = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\map1.html";

                if (File.Exists(MyMapUri))
                {
                    StreamReader objReader = new StreamReader(MyMapUri);
                    string line = "";
                    line = objReader.ReadToEnd();
                    objReader.Close();
                    var cityLatLon = cty.Content.Latitude + ", " + cty.Content.Longitude;
                    line = line.Replace("[origin]", cityLatLon);
                    StreamWriter page = File.CreateText(NewMapUri);
                    page.Write(line);
                    page.Close();
                    MapUri = NewMapUri;
                }

            }
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
