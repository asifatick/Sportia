using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Threading;

namespace GrandAStudio.Sportia
{
    public partial class MainPage : PhoneApplicationPage
    {
        DispatcherTimer dtMain = new DispatcherTimer();
        public MainPage()
        {
            InitializeComponent();
           

            // Set the data context of the listbox control to the sample data

            DataContext = App.ViewModel;

            dtMain.Interval = new TimeSpan(0, 0, 0, 0, 2000); // 1 Seconds
            dtMain.Tick += new EventHandler(dt_Tick);

            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            dtMain.Start();
        }
        void dt_Tick(object sender, EventArgs e)
        {
            dtMain.Stop();
            App.CurrentApp.SetCurrentBackGroundSound("menu.mp3","1");
            NavigationService.Navigate(new Uri("/MenuPage.xaml", UriKind.Relative));
        }
    }
}