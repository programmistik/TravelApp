using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;
using TravelApp.Views;

namespace TravelApp.ViewModels
{
    class NewTripPageViewModel : ViewModelBase
    {
        private string cityToSearch;
        public string CityToSearch { get => cityToSearch; set => Set(ref cityToSearch, value); }

        private string tripName;
        public string TripName { get => tripName; set => Set(ref tripName, value); }

        private DateTime departureDate;
        public DateTime DepartureDate { get => departureDate; set => Set(ref departureDate, value); }

        private DateTime arrivalDate;
        public DateTime ArrivalDate { get => arrivalDate; set => Set(ref arrivalDate, value); }

        private ObservableCollection<CityList> cityCollection;
        public ObservableCollection<CityList> CityCollection { get => cityCollection; set => Set(ref cityCollection, value); }

        private ObservableCollection<Ticket> ticketList;
        public ObservableCollection<Ticket> TicketList { get => ticketList; set => Set(ref ticketList, value); }

        private string ticketName;
        public string TicketName { get => ticketName; set => Set(ref ticketName, value); }

        private Trip currentTrip;
        public Trip CurrentTrip { get => currentTrip; set => Set(ref currentTrip, value); }

        
        private readonly IAPIService apiService;
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly IDialogService dialogService;
        private readonly AppDbContext db;

        public NewTripPageViewModel(INavigationService navigationService, 
                                    IMessageService messageService, 
                                    IDialogService dialogService, 
                                    IAPIService apiService,
                                    AppDbContext db)
        {
            //CityList = new ObservableCollection<CityList>();
            //TicketList = new ObservableCollection<Ticket>();
            this.navigationService = navigationService;
            this.apiService = apiService;
            this.messageService = messageService;
            this.dialogService = dialogService;
            this.db = db;
            Messenger.Default.Register<Trip>(this, tr =>
            {
                CurrentTrip = tr;
                TripName = tr.TripName;
                DepartureDate = tr.DepartureDate;
                ArrivalDate = tr.ArrivalDate;
                CityCollection = new ObservableCollection<CityList>(tr.CityList);
                TicketList = new ObservableCollection<Ticket>(tr.Tickets);
            });
        }

        private RelayCommand addCityCommand;
        public RelayCommand AddCityCommand
        {
            get => addCityCommand ?? (addCityCommand = new RelayCommand(
                () =>
                {
                    var cName = apiService.GetCityName(CityToSearch);
                    if (cName != "")
                    {
                        // first search in db
                        var cityInDb = db.Cities.Where(n => n.CityName == cName);
                        if(cityInDb.Any())
                        {
                            var record = new CityList();
                            record.CityId = cityInDb.Single().Id;
                            record.City = cityInDb.Single();
                            CityToSearch = "";
                            CityCollection.Add(record);
                            CurrentTrip.CityList.Add(record);
                        }
                        else
                        {
                            var city = apiService.GetCity(CityToSearch);
                            if (String.IsNullOrEmpty(city.CityName) != true)
                            {
                                db.Cities.Add(city);
                                db.SaveChanges();
                                var record = new CityList();
                                record.CityId = city.Id;
                                record.City = city;
                                CityToSearch = "";
                                CityCollection.Add(record);
                                CurrentTrip.CityList.Add(record);
                            }
                        }
                    }
                    
                }
            ));
        }

        private RelayCommand addNewTicketCommand;
        public RelayCommand AddNewTicketCommand
        {
            get => addNewTicketCommand ?? (addNewTicketCommand = new RelayCommand(
                () =>
                {
                    if (string.IsNullOrEmpty(TicketName))
                        messageService.ShowInfo("Enter ticket name first");
                    else
                    {
                        var link = dialogService.OpenFileDialog("PDF files (*.pdf)|*.pdf");
                        if (link != "")
                        {
                            var tckt = new Ticket();
                            tckt.TicketName = TicketName;
                            tckt.TicketUri = link;
                            TicketList.Add(tckt);

                            TicketName = "";
                        }
                    }
                }
            ));
        }

        private RelayCommand<Ticket> deleteTicketCommand;
        public RelayCommand<Ticket> DeleteTicketCommand
        {
            get => deleteTicketCommand ?? (deleteTicketCommand = new RelayCommand<Ticket>(
                param =>
                {
                    if (messageService.ShowYesNo("Are you sure?"))
                    {
                        //db.Contacts.Remove(param);
                        //db.SaveChanges();
                        TicketList.Remove(param);
                    }
                }
            ));
        }

        private RelayCommand<Ticket> showTicketCommand;
        public RelayCommand<Ticket> ShowTicketCommand
        {
            get => showTicketCommand ?? (showTicketCommand = new RelayCommand<Ticket>(
                param =>
                {
                    Messenger.Default.Send(param.TicketUri);
                    navigationService.Navigate<ShowPdfPageView>();
                }
            ));
        }

        private RelayCommand<CityList> deleteCommand;
        public RelayCommand<CityList> DeleteCommand
        {
            get => deleteCommand ?? (deleteCommand = new RelayCommand<CityList>(
                param =>
                {
                    if (messageService.ShowYesNo("Are you sure?"))
                    {
                        //db.Contacts.Remove(param);
                        //db.SaveChanges();
                        CityCollection.Remove(param);
                    }
                }
            ));
        }
        private RelayCommand<CityList> showCommand;
        public RelayCommand<CityList> ShowCommand
        {
            get => showCommand ?? (showCommand = new RelayCommand<CityList>(
                param =>
                {
                    var city = param.City;
                }
            ));
        }
        private RelayCommand<CityList> showMapCommand;
        public RelayCommand<CityList> ShowMapCommand
        {
            get => showMapCommand ?? (showMapCommand = new RelayCommand<CityList>(
                param =>
                {
                    var city = param.City;
                    Messenger.Default.Send(city);
                    navigationService.Navigate<ShowMapPageView>();
                }
            ));
        }

        private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand(
                () =>
                {
                    // save all in db
                    
                    CurrentTrip.ArrivalDate = ArrivalDate;
                    CurrentTrip.DepartureDate = DepartureDate;
                    CurrentTrip.CityList = new ObservableCollection<CityList>(CityCollection);
                    CurrentTrip.Tickets = new ObservableCollection<Ticket>(TicketList);
                    if (string.IsNullOrEmpty(CurrentTrip.TripName))
                    {
                        CurrentTrip.TripName = TripName;
                        db.Trips.Add(CurrentTrip);
                        //db.SaveChanges();
                        
                    }
                    else
                        CurrentTrip.TripName = TripName;

                    db.SaveChanges();
                    Messenger.Default.Send(new AddToCollection(true, CurrentTrip));
                    navigationService.Navigate<MainPageView>();
                }
            ));
        }

    }
}
