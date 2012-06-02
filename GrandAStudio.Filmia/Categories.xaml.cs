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
using System.Windows.Data;
using System.Windows.Media.Imaging;



namespace GrandAStudio.Sportia
{
    public partial class Categories : PhoneApplicationPage
    {
        Moviadb1DataContext db = App.CurrentApp.DB;
        public Categories()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            checkButtonState(btnCAt00, txtCAt00, 2, btnCAt00_Tap);

            checkButtonState(btnCAt90, txtCAt90, 4, btnCAt90_Tap);

            checkButtonState(btnCAt80, txtCAt80, 9, btnCAt80_Tap);

            checkButtonState(btnCAtQT, txtCAtQT, 7, btnCAtQT_Tap);

            checkButtonState(btnCAtStars, txtCAtStars, 6, btnCAtStars_Tap);

            checkButtonState(btnCAtRan, txtCAtRan, 8, btnCAtRan_Tap);

            checkButtonState(btnCAtRoles, txtCAtRoles, 1, btnCAtRoles_Tap);

            checkButtonState(btnCAtOscars, txtCAtOscars, 3, btnCAtOscars_Tap);

            checkButtonState(btnCAtDirectors, txtCAtDirectors, 11, btnCAtDirectors_Tap);

            checkButtonState(btnCAtAC, txtCAtAC, 12, btnCAtAC_Tap);

            checkButtonState(btnCAtHS, txtCAtHS, 10, btnCAtHS_Tap);

            checkButtonState(btnCAtAdvanced, txtCAtAdvanced, 5, btnCAtAdvanced_Tap);

            checkButtonState(btnCAtOldies, txtCAtOldies, 13, btnCAtOldies_Tap);

        }

        private void checkButtonState(Image btn, TextBlock tb, int p, EventHandler<GestureEventArgs> evt)
        {
            Categories cat = db.Categories.Where(c => c.IID == (long)p).FirstOrDefault();
            SolidColorBrush sb = new SolidColorBrush();
            if (cat.IsUnlocked == 1)
            {
                btn.Tap +=new EventHandler<GestureEventArgs>(evt);
               tb.Tap += new EventHandler<GestureEventArgs>(evt);
                ((BitmapImage)btn.Source).UriSource = new Uri(@"Images/category blue.png", UriKind.Relative);
                sb.Color = Color.FromArgb(100, 00, 0, 255);
               
                if (cat.IsCompleted == 1)
                {
                    ((BitmapImage)btn.Source).UriSource = new Uri(@"Images/category green.png", UriKind.Relative);
                    sb.Color = Color.FromArgb(100, 00, 255, 00);
                }
                tb.Foreground = sb;
            }
        }

        private void btnCAt00_Tap(object sender, GestureEventArgs e)
        {
            StartGame(2);      

        }

        private void StartGame(int p)
        {
            App.ViewModel.CurrentCat = p;
            NavigationService.Navigate(new Uri("/TipsPage.xaml", UriKind.Relative));
        }

        private void btnCAt90_Tap(object sender, GestureEventArgs e)
        {
            StartGame(4);  
        }

        private void btnCAt80_Tap(object sender, GestureEventArgs e)
        {
            StartGame(9);  
        }

        private void btnCAtQT_Tap(object sender, GestureEventArgs e)
        {
            StartGame(7);  
        }

        private void btnCAtStars_Tap(object sender, GestureEventArgs e)
        {
            StartGame(6);  
        }

        private void btnCAtRan_Tap(object sender, GestureEventArgs e)
        {
            StartGame(8);  
        }

        private void btnCAtRoles_Tap(object sender, GestureEventArgs e)
        {
            StartGame(1);  
        }

        private void btnCAtOscars_Tap(object sender, GestureEventArgs e)
        {
            StartGame(3);  
        }

        private void btnCAtDirectors_Tap(object sender, GestureEventArgs e)
        {
            StartGame(11);  
        }

        private void btnCAtAC_Tap(object sender, GestureEventArgs e)
        {
            StartGame(12);  
        }

        private void btnCAtHS_Tap(object sender, GestureEventArgs e)
        {
            StartGame(10);  
        }

        private void btnCAtAdvanced_Tap(object sender, GestureEventArgs e)
        {
            StartGame(5);  
        }

       

        private void btnCAtOldies_Tap(object sender, GestureEventArgs e)
        {
            StartGame(13);  
        }

       
    }
}