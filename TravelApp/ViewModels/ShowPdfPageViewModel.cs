using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TravelApp.Services;
using TravelApp.Views;

namespace TravelApp.ViewModels
{
    public class ShowPdfPageViewModel : ViewModelBase
    {
        private string ticketUri;
        public string TicketUri { get => ticketUri; set => Set(ref ticketUri, value); }

        private string pageTitle;
        public string PageTitle { get => pageTitle; set => Set(ref pageTitle, value); }

        private readonly INavigationService navigation;

        public ShowPdfPageViewModel(INavigationService navigation)
        {
            this.navigation = navigation;
            Messenger.Default.Register<string>(this, tickUri => { TicketUri = tickUri; PageTitle = "Ticket"; });
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

    public static class WebBrowserUtility
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserUtility), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = o as WebBrowser;
            if (browser != null)
            {
                string uri = e.NewValue as string;
                browser.Source = !String.IsNullOrEmpty(uri) ? new Uri(uri) : null;
            }
        }

    }
}
