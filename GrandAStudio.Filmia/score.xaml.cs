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


namespace GrandAStudio.Sportia
{
    public partial class score : PhoneApplicationPage
    {
        public score()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            App.ViewModel.GScore.TotalGames += 1;
            App.ViewModel.GScore.TotalQuestions += 15;
            App.ViewModel.GScore.TotalPasses += App.ViewModel.GamePasses;
            App.ViewModel.GScore.TotalRightAnswers += App.ViewModel.GameRightAnswer;
            App.ViewModel.GScore.TotlaWrongAnswers += App.ViewModel.GameWrongAnswer;
            textBlock2.Visibility = System.Windows.Visibility.Collapsed;
            if (App.ViewModel.CurrentScore <= 0)
            {
                image2.Visibility = System.Windows.Visibility.Collapsed;

            }
            else
            {
                image2.Visibility = System.Windows.Visibility.Visible;
                App.ViewModel.GScore.Total += App.ViewModel.CurrentScore;
                App.ViewModel.Save();
            }
            if (Util.UpdateUnlockCategories())
            {
                textBlock2.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.CurrentApp.SetCurrentBackGroundSound("menu.mp3", "1");
            //NavigationService.
            
            NavigationService.Navigate(new Uri(@"/MenuPage.xaml", UriKind.Relative));
        }
    }
}