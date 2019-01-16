using AForge.Video;
using AForge.Video.DirectShow;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TravelApp.Models;
using TravelApp.Services;
using TravelApp.Views;

namespace TravelApp.ViewModels
{
    class SignUpPageViewModel : ViewModelBase, IDisposable
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly IDialogService dialogService;
        private readonly IPasswordSupplier passwordService;
        private readonly AppDbContext db;
        private string ImageLink { get; set; }
        private BitmapImage image;
        public BitmapImage Image { get => image; set => Set(ref image, value); }
        public ObservableCollection<FilterInfo> VideoDevices { get; set; }
        private FilterInfo currentDevice;
        public FilterInfo CurrentDevice { get => currentDevice; set => Set(ref currentDevice, value); }
        public IVideoSource videoSource;

        private User user = new User();
        public User User { get => user; set => Set(ref user, value); }

        private string msg;
        public string Msg { get => msg; set => Set(ref msg, value); }

        private string msgColor;
        public string MsgColor { get => msgColor; set => Set(ref msgColor, value); }

        private string passCheckError;
        public string PassCheckError { get => passCheckError; set => Set(ref passCheckError, value); }

        private bool passwordConfirmation;
        public bool PasswordConfirmation { get => passwordConfirmation; set => Set(ref passwordConfirmation, value); }


        public SignUpPageViewModel(INavigationService navigationService, 
                                   IMessageService messageService, 
                                   IDialogService dialogService,
                                   IPasswordSupplier passwordService,
                                   AppDbContext db)
        {
            this.navigationService = navigationService;
            this.db = db;
            this.messageService = messageService;
            this.dialogService = dialogService;
            this.passwordService = passwordService;

            VideoDevices = new ObservableCollection<FilterInfo>();
            GetVideoDevices();

        }
        public void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {

                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    var bi = new BitmapImage();
                    bi.BeginInit();
                    var ms = new MemoryStream();
                    bitmap.Save(ms, ImageFormat.Bmp);
                    ms.Seek(0, SeekOrigin.Begin);
                    bi.StreamSource = ms;
                    bi.EndInit();

                    bi.Freeze();
                    Dispatcher.CurrentDispatcher.Invoke(() => Image = bi);
                }
            }
            catch (Exception)
            {
                StopCamera();
            }
        }
        private void StopCamera()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.NewFrame -= video_NewFrame;
            }
            Image = null;
        }

        public void GetVideoDevices()
        {
            var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo device in devices)
                VideoDevices.Add(device);

            if (VideoDevices.Any())
                CurrentDevice = VideoDevices[0];
        }
        public void Dispose()
        {
            if (videoSource != null && videoSource.IsRunning)
                videoSource.SignalToStop();


        }

        private RelayCommand snapCommand;
        public RelayCommand SnapCommand
        {
            get => snapCommand ?? (snapCommand = new RelayCommand(
                () =>
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(Image));
                    ImageLink = Environment.CurrentDirectory+"\\snap.png";
                    using (var fileStream = new FileStream(ImageLink, FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }
                    StopCamera();
                    Image = new BitmapImage(new Uri(ImageLink));
                }
            ));
        }
        private RelayCommand uploadFileCommand;
        public RelayCommand UploadFileCommand
        {
            get => uploadFileCommand ?? (uploadFileCommand = new RelayCommand(
                () =>
                {
                    ImageLink = dialogService.OpenFileDialog();
                    if (ImageLink != "")
                    {
                        StopCamera();
                        Image = new BitmapImage(new Uri(ImageLink));
                    }
                }
            ));
        }
        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get => cancelCommand ?? (cancelCommand = new RelayCommand(
                () =>
                {
                    // do nothing
                    MsgColor = Msg = PassCheckError = "";
                    User = new User();
                    StopCamera();
                    navigationService.Navigate<LogInPageView>();
                }
            ));
        }

       
        private RelayCommand<object> okCommand;
        public RelayCommand<object> OkCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand<object>(
                param =>
                {
                    // check login unique
                    if (string.IsNullOrEmpty(User.Login))
                        messageService.ShowError("Enter username!", "Sing up failed");
                    else
                    {
                        if (MsgColor == "Red")
                        {
                            messageService.ShowError("Please, choose another username!\nThis one is already taken.","Sing up failed");
                        }
                        else
                        {
                            if (!PasswordConfirmation)
                            {
                                messageService.ShowError("Please, confirm your password!", "Sing up failed");
                            }
                            else // OK!
                            {
                                var passwordContainer = param as IPasswordSupplier;
                                if (passwordContainer != null)
                                {
                                    var sPass = passwordContainer.GetPassword;
                                    RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
                                    byte[] salt = new byte[32];
                                    csprng.GetBytes(salt);
                                    var saltValue = Convert.ToBase64String(salt);

                                    byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + new NetworkCredential(string.Empty, sPass).Password);
                                    SHA256Managed hashstring = new SHA256Managed();
                                    byte[] hash = hashstring.ComputeHash(saltedPassword);
                                    saltValue = Convert.ToBase64String(salt);
                                    var hashValue = Convert.ToBase64String(hash);
                                    User.SaltValue = saltValue;
                                    User.HashValue = hashValue;
                                    User.Photo = ImageLink;
                                    db.Users.Add(User);
                                    db.SaveChanges();
                                }
                                MsgColor = Msg = PassCheckError = "";
                                User = new User();
                                StopCamera();
                                navigationService.Navigate<LogInPageView>();
                            }
                        }
                    }

                    
                }
            ));
        }
        private RelayCommand<string> lostFocusCommand_tbUN;
        public RelayCommand<string> LostFocusCommand_tbUN
        {
            get => lostFocusCommand_tbUN ?? (lostFocusCommand_tbUN = new RelayCommand<string>(
                param =>
                {
                    if (!string.IsNullOrEmpty(param))
                    {
                        var qwr = db.Users.Where(u => u.Login == param);
                        if (qwr.Any() == true)
                        {
                            MsgColor = "Red";
                            Msg = "This username is already taken";
                        }
                        else
                        {
                            MsgColor = "Green";
                            Msg = "You can use this username";
                        }
                    }
                }
            ));
        }

        private RelayCommand<object> lostFocusCommand_pBox;
        public RelayCommand<object> LostFocusCommand_pBox
        {
            get => lostFocusCommand_pBox ?? (lostFocusCommand_pBox = new RelayCommand<object>(
                param =>
                {
                    var passwordContainer = param as IPasswordSupplier;
                    if (passwordContainer != null)
                    {
                        var chk = passwordContainer.ConfirmPassword();
                        if (chk)
                        {
                            PassCheckError = "";
                            PasswordConfirmation = true;
                        }
                        else
                        {
                            PassCheckError = "Passwords don't match";
                            PasswordConfirmation = false;
                        }
                    }
                }
            ));
        }



    }
   

}
