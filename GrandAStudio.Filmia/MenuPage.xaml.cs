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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GrandAStudio.Sportia
{
    public partial class MenuPage : PhoneApplicationPage
    {
        DispatcherTimer dt = new DispatcherTimer();
        public MenuPage()
        {
            InitializeComponent();
           
            dt.Interval = TimeSpan.FromMilliseconds(33);
            dt.Tick += delegate { try { FrameworkDispatcher.Update(); } catch { } };
            dt.Start();

        }

        

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Categories.xaml", UriKind.Relative));
        }

        private void btnOption_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Options.xaml", UriKind.Relative));
        }

        private void btnStats_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/StatsPage.xaml", UriKind.Relative));
        }

        private void btnFacebook_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnTwitter_Click(object sender, RoutedEventArgs e)
        {

        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // set son on time of navigation to a sound start page not while page loads
            NavigationService.RemoveBackEntry();
            string source ="";
            
         
            base.OnNavigatedTo(e);
        }
        
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            App.CurrentApp.StopbackgroundSound();
            if (App.CurrentApp.phoneMediaState == Microsoft.Xna.Framework.Media.MediaState.Playing)
            {
                MediaPlayer.Resume();
            }
            //App.CurrentApp.CloseBGPlayer();
            base.OnBackKeyPress(e);
        }
    }
}