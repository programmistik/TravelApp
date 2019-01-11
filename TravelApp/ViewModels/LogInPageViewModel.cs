using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TravelApp.Models;
using TravelApp.Services;
using TravelApp.Views;

namespace TravelApp.ViewModels
{
    class LogInPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        private BitmapImage image;
        public BitmapImage Image { get => image; set => Set(ref image, value); }

        private string loginCheck;
        public string LoginCheck { get => loginCheck; set => Set(ref loginCheck, value); }

        private string checkColor;
        public string CheckColor { get => checkColor; set => Set(ref checkColor, value); }

        private string loginUserName;
        public string LoginUserName { get => loginUserName; set => Set(ref loginUserName, value); }

        private SecureString pass;
        public SecureString Pass { get => pass; set => Set(ref pass, value); }

        public LogInPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
        }

        private RelayCommand signUpCommand;
        public RelayCommand SignUpCommand
        {
            get => signUpCommand ?? (signUpCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<SignUpPageView>();
                }
            ));
        }


        private RelayCommand<object> logInCommand;
        public RelayCommand<object> LogInCommand
        {
            get => logInCommand ?? (logInCommand = new RelayCommand<object>(
                param =>
                {
                    if (CheckColor == "Red")
                    {
                        messageService.ShowInfo("There is no user with such Username.\nPlease, Sign up first");
                    }
                    else if (CheckColor == "Green")
                    {
                        var qwr = db.Users.Where(u => u.Login == LoginUserName);
                        if (qwr.Any() == true)
                        {
                            var passwordContainer = param as IPasswordSupplier;
                            if (passwordContainer != null)
                            {
                                var sPass = passwordContainer.GetPassword;

                                string saltValueFromDB = qwr.Single().SaltValue;
                                string hashValueFromDB = qwr.Single().HashValue;

                                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValueFromDB + new NetworkCredential(string.Empty, sPass).Password);
                                SHA256Managed hashstring = new SHA256Managed();
                                byte[] hash = hashstring.ComputeHash(saltedPassword);
                                string hashToCompare = Convert.ToBase64String(hash);
                                if (hashValueFromDB.Equals(hashToCompare))
                                {
                                    var Usr = qwr.Single();
                                    // messageService.ShowInfo("OK");
                                    //var Usr = new SendCurrentUser();
                                    //Usr.UserId = qwr.Single().Id;
                                    Messenger.Default.Send(new NotificationMessage<User>(Usr,"SendCurrentUser"));
                                    navigationService.Navigate<MainPageView>();
                                }
                                else
                                    messageService.ShowError("Login credentials incorrect. User not validated.");
                            }  
                        }
                    }
                    else
                    {
                        messageService.ShowInfo("Enter Username and password, please.");
                    }
                }
            ));
        }

       

        private RelayCommand forgotPasswordCommand;
        public RelayCommand ForgotPasswordCommand
        {
            get => forgotPasswordCommand ?? (forgotPasswordCommand = new RelayCommand(
                () =>
                {
                    messageService.ShowInfo("Contact administrator");
                    
                }
            ));
        }

        private RelayCommand<string> lostFocusCommand;
        public RelayCommand<string> LostFocusCommand
        {
            get => lostFocusCommand ?? (lostFocusCommand = new RelayCommand<string>(
                param =>
                {
                    if (!string.IsNullOrEmpty(param))
                    {
                        var qwr = db.Users.Where(u => u.Login == param);
                        if (qwr.Any() == true)
                        {
                            CheckColor = "Green";
                            LoginCheck = "✔";
                            var imgLink = qwr.Single().Photo;
                            if(imgLink != null)
                                Image = new BitmapImage(new Uri(imgLink));
                        }
                        else
                        {
                            CheckColor = "Red";
                            LoginCheck = "❌";
                            Image = null;
                        }
                    }
                }
            ));
        }
    }
}
