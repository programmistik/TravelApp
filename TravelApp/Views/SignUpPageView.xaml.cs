﻿using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelApp.Services;
using TravelApp.ViewModels;

namespace TravelApp.Views
{
    /// <summary>
    /// Interaction logic for SignUpPageView.xaml
    /// </summary>
    public partial class SignUpPageView : UserControl, IPasswordSupplier
    {
        public SignUpPageView()
        {
            InitializeComponent();
        }

        public SecureString GetPassword { get => pBox.SecurePassword; }

        public bool ConfirmPassword()
        {
            return pBox.Password == pBox2.Password;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var data = DataContext as SignUpPageViewModel;
            data.VideoDevices = new ObservableCollection<FilterInfo>();
            data.GetVideoDevices();
            if (data.CurrentDevice != null)
            {
                data.videoSource = new VideoCaptureDevice(data.CurrentDevice.MonikerString);
                data.videoSource.NewFrame += data.video_NewFrame;
                data.videoSource.Start();
            }
        }
       
    }
}
