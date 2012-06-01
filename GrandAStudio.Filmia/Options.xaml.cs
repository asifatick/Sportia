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
using System.Windows.Media.Imaging;

namespace GrandAStudio.Sportia
{
    public partial class Options : PhoneApplicationPage
    {
        public Options()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void btnSounds_Click(object sender, RoutedEventArgs e)
        {
           

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.ViewModel.IsSoundsOn == true)
            {
               // ((BitmapImage)btnSounds.Source).UriSource = new Uri(@"Images/mainscreensoundon.png", UriKind.Relative);
                rbSoundOn.IsChecked = true;

            }
            else
            {
               // ((BitmapImage)btnSounds.Source).UriSource = new Uri(@"Images/mainscreensoundoff.png", UriKind.Relative);
                rbSoundOff.IsChecked = true;
            }
            
        }

        private void btnSounds_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void btnSounds_Tap(object sender, GestureEventArgs e)
        {
            if (App.ViewModel.IsSoundsOn == true)
            {
                App.ViewModel.IsSoundsOn = false;
                App.CurrentApp.StopbackgroundSound();
                ((BitmapImage)btnSounds.Source).UriSource = new Uri(@"Images/mainscreensoundoff.png", UriKind.Relative);

            }
            else
            {
                App.ViewModel.IsSoundsOn = true;
                App.CurrentApp.SetCurrentBackGroundSound("menu.mp3","1",true);
                ((BitmapImage)btnSounds.Source).UriSource = new Uri(@"Images/mainscreensoundon.png", UriKind.Relative);
            }
        }

        private void btnUnlock_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Codes.xaml", UriKind.Relative));
        }

        private void rbSoundOn_Checked(object sender, RoutedEventArgs e)
        {
            App.ViewModel.IsSoundsOn = true;
            if(!App.CurrentApp.IsBgSoundPlaying )
                App.CurrentApp.SetCurrentBackGroundSound("menu.mp3", "1",true);
        }

        private void rbSoundOff_Checked(object sender, RoutedEventArgs e)
        {
           

            App.ViewModel.IsSoundsOn = false;
            App.CurrentApp.StopbackgroundSound();
        }

      
    }
}