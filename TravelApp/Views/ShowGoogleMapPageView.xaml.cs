using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace TravelApp.Views
{
    /// <summary>
    /// Interaction logic for GoogleMapPageView.xaml
    /// </summary>
    public partial class ShowGoogleMapPageView : UserControl
    {
        public ShowGoogleMapPageView()
        {
            InitializeComponent();
        }

        //private void AllpyMapType(object sender, RoutedEventArgs e)
        //{
        //    var MapUri = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\map1.html";
        //    var NewMapUri = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\map2.html";
        //    if (File.Exists(MapUri))
        //    {
        //        StreamReader objReader = new StreamReader(MapUri);
        //        string line = "";
        //        line = objReader.ReadToEnd();
        //        objReader.Close();
        //        var Itm = (ComboBoxItem)MapType.SelectedItem;
        //        line = line.Replace("roadmap", Itm.Content.ToString());
        //        StreamWriter page = File.CreateText(NewMapUri);
        //        page.Write(line);
        //        page.Close();
        //        myWeb.Navigate(NewMapUri);
        //    }
        //}
    }
}
