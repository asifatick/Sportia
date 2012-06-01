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
    public partial class TipsPage : PhoneApplicationPage
    {
        DispatcherTimer dtMain = new DispatcherTimer();
        public TipsPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel.GetRandomTips();
           

            dtMain.Interval = new TimeSpan(0, 0, 0, 0, 4000); // 4 Seconds
            dtMain.Tick += new EventHandler(dt_Tick);

            this.Loaded += new RoutedEventHandler(TipsPage_Loaded);
        }
        private void TipsPage_Loaded(object sender, RoutedEventArgs e)
        {
            dtMain.Start();
        }
        void dt_Tick(object sender, EventArgs e)
        {
            dtMain.Stop();
            App.CurrentApp.SetCurrentBackGroundSound("gameplay.mp3","1");
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dtMain.Stop();
        }
    }
}