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

            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitIt);


            Messenger.Default.Register<NotificationMessage<Trip>>(this, a =>
            {
                if (a.Notification == "AddNewTripToCollection")
                    TripList.Add(a.Content);
            });
        }

        private void OnHitIt(NotificationMessage<User> usr)
        {
            if (usr.Notification == "SendCurrentUser")
            {
                CurrentUserId = usr.Content.Id;
                TripList = new ObservableCollection<Trip>(db.Trips.Where(tr => tr.UserId == CurrentUserId));
            }
        }

        private RelayCommand addNewTrip;
        public RelayCommand AddNewTrip
        {
            get => addNewTrip ?? (addNewTrip = new RelayCommand(
                () =>
                {
                    var NewTrip = new Trip();
                    
                    NewTrip.UserId = CurrentUserId;
                    NewTrip.User = db.Users.Single(u => u.Id == CurrentUserId);
                    Messenger.Default.Send(new NotificationMessage<Trip>(NewTrip, "NewTrip"));
                    navigationService.Navigate<NewTripPageView>();
                }
            ));
        }

        private RelayCommand<Trip> deleteTripCommand;
        public RelayCommand<Trip> DeleteTripCommand
        {
            get => deleteTripCommand ?? (deleteTripCommand = new RelayCommand<Trip>(
                param =>
                {
                   // TripList.Remove(param);
                    db.Trips.Remove(param);
                   
                    db.SaveChanges();
                }
            ));
        }

        private RelayCommand<Trip> showEditTripCommand;
        public RelayCommand<Trip> ShowEditTripCommand
        {
            get => showEditTripCommand ?? (showEditTripCommand = new RelayCommand<Trip>(
                param =>
                {
                    Messenger.Default.Send(new NotificationMessage<Trip>(param, "EditTrip"));
                    navigationService.Navigate<NewTripPageView>();
                }
            ));
        }

    }
}
