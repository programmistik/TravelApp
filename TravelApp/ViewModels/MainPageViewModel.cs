using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;
using TravelApp.Views;

namespace TravelApp.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        private int CurrentUserId { get; set; }

        private ObservableCollection<Trip> tripList = new ObservableCollection<Trip>();
        public ObservableCollection<Trip> TripList { get => tripList; set => Set(ref tripList, value); }

        private readonly INavigationService navigationService;
        private readonly AppDbContext db;
        private readonly IMessageService messageService;

        public MainPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.db = db;
            this.messageService = messageService;

            Messenger.Default.Register<SendCurrentUser>(this, usr => 
            {
                CurrentUserId = usr.UserId;
                TripList = new ObservableCollection<Trip>(db.Trips.Where(tr => tr.UserId == usr.UserId));
            });
            TripList = new ObservableCollection<Trip>(db.Trips.Where(tr => tr.UserId == CurrentUserId));

            Messenger.Default.Register<AddToCollection>(this, a =>
            {
                if (a.Add)
                    TripList = new ObservableCollection<Trip>(db.Trips.Where(tr => tr.UserId == CurrentUserId));
            });
        }

        

        private RelayCommand addNewTrip;
        public RelayCommand AddNewTrip
        {
            get => addNewTrip ?? (addNewTrip = new RelayCommand(
                () =>
                {
                    var NewTrip = new Trip();
                    NewTrip.DepartureDate = DateTime.Now;
                    NewTrip.ArrivalDate = DateTime.Now;
                    NewTrip.CityList = new ObservableCollection<CityList>();
                    NewTrip.Tickets = new ObservableCollection<Ticket>();
                    NewTrip.UserId = CurrentUserId;
                   // NewTrip.User = db.Users.Single(u => u.Id == CurrentUserId);
                    Messenger.Default.Send(NewTrip);
                    navigationService.Navigate<NewTripPageView>();
                }
            ));
        }


    }
}
