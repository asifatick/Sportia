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
using System.Windows.Threading;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.IO;


namespace GrandAStudio.Sportia
{
    public partial class GamePage : PhoneApplicationPage
    {
        // Constructor
        public GamePage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            
            DataContext = App.ViewModel;
           
            dtMain.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 1 Seconds
            dtMain.Tick += new EventHandler(dt_Tick);

            dtAnswerState.Interval = new TimeSpan(0, 0, 0, 0, 750); // .75 Seconds
            dtSound.Interval = new TimeSpan(0, 0, 0, 0, 150);
            dtAnswerState.Tick += new EventHandler(dtAnswerState_Tick);
            dtSound.Tick += new EventHandler(dtSound_Tick);

            this.Loaded += new RoutedEventHandler(GamePage_Loaded);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {

                App.ViewModel.LoadData((int)App.ViewModel.CurrentCat);

                //textBlock1.Text  = App.ViewModel.CurrentQuestion.Question;
                //lstAnswers.ItemsSource = App.ViewModel.CurrentQuestion.Answers;
            }
            App.ViewModel.GetRandomQuestion();
            DataContext = App.ViewModel;

            App.ViewModel.ResetGame();
            startCountdown();
            NavigationService.RemoveBackEntry();
            base.OnNavigatedTo(e);
        }
        bool isScreenLocked = false;
        private Questions q;
        DispatcherTimer dtMain = new DispatcherTimer();
        DispatcherTimer dtAnswerState = new DispatcherTimer();
        DispatcherTimer dtSound = new DispatcherTimer();
        private Questions GetRandomQuestion()
        { 
            Random r = new Random();
            int randomIndex = 0;

            randomIndex = r.Next(0, App.ViewModel.Items.Count); //Choose a random object in the list
            if (App.ViewModel.Items.Count > 0)
            {
                 q = App.ViewModel.Items[randomIndex];
                q.IsAsked = 1;
                App.ViewModel.Save();
                App.ViewModel.Items.RemoveAt(randomIndex);
               
                return q;
            }
            return null;


        }
        private void SoundPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            gamePlayer.Play();
           
        }
        // Load data for the ViewModel Items
        private void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!App.ViewModel.IsDataLoaded)
            //{

            //    App.ViewModel.LoadData((int)App.ViewModel.CurrentCat);
               
            //    //textBlock1.Text  = App.ViewModel.CurrentQuestion.Question;
            //  //lstAnswers.ItemsSource = App.ViewModel.CurrentQuestion.Answers;
            //}
            //App.ViewModel.GetRandomQuestion();
            //// DataContext = GetRandomQuestion();
           
            //DataContext = App.ViewModel;

            //App.ViewModel.ResetGame();
            //startCountdown();
        }
       
        public void startCountdown()
        {
            App.ViewModel.Timer = 10;
            dtMain.Start();

        }

            void dt_Tick(object sender, EventArgs e)
            {
                if (App.ViewModel.Timer == 1)
                {
                    ((BitmapImage)imgResultState.Source).UriSource = new Uri(@"Images/alarm-clock.png", UriKind.Relative);
                    App.ViewModel.CurrentScore-= 2;
                   // App.ViewModel.GScore.TotlaWrongAnswers++;
                    App.ViewModel.GameWrongAnswer++;
                    App.ViewModel.Save();
                   
                    App.CurrentApp.PlaySound("wronganswer.wav");
                    dtAnswerState.Start();
                    dtMain.Stop();
                   // NextQuestion();
                   // dtMain.Stop();
                }
                App.ViewModel.Timer--;
            }

            void dtAnswerState_Tick(object sender, EventArgs e)
            {
                ((BitmapImage)imgResultState.Source).UriSource = new Uri(@"", UriKind.Relative);
                dtAnswerState.Stop();
                isScreenLocked = false;
                NextQuestion();
            }
            void dtSound_Tick(object sender, EventArgs e)
            {
                dtSound.Stop();
                //App.CurrentApp.ContinuePlaying(t, tr);
            }
        
            private void NextQuestion()
            {
                dtMain.Start();
                App.ViewModel.Timer = 10;
             
                if (App.ViewModel.questionCount < 11 )
                {
                    App.ViewModel.GetRandomQuestion();
                    //App.ViewModel.GScore.TotalQuestions++;
                    App.ViewModel.Save();
                }
                else
                {
                    UpdateAndShowScore();
                    dtMain.Stop();
                    App.CurrentApp.SetCurrentBackGroundSound("score_screen.mp3", "1");
                    NavigationService.Navigate(new Uri(@"/score.xaml?target=2",UriKind.Relative));
                }
            }

            private void UpdateAndShowScore()
            {
               
            }
           
            
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isScreenLocked)
            {
                
                if (e.AddedItems.Count > 0 && App.ViewModel.Timer > 0)
                {
                    isScreenLocked = true;
                    Answer a = (Answer)e.AddedItems[0];
                    App.ViewModel.CurrentScore += a.IsCorrect;
                    a.isClicked = 1;
                    SearchVisualTreeForSelectedButton(this.lstAnswers, a.answer);

                    ((BitmapImage)imgResultState.Source).UriSource = new Uri(@"Images/sad.png", UriKind.Relative);
                    if (a.IsCorrect == 3)
                    {
                        App.ViewModel.CurrentQuestion.IsAnswered = 1;

                        ((BitmapImage)imgResultState.Source).UriSource = new Uri(@"Images/happy.png", UriKind.Relative);

                        App.ViewModel.GameRightAnswer++;

                        App.CurrentApp.PlaySound("ButtonPressed.wav");
                    }
                    else
                    {
                        App.ViewModel.GameWrongAnswer++;
                        App.CurrentApp.PlaySound("ButtonPressed.wav");
                        while (App.CurrentApp.CurrentSoundState == SoundState.Playing)
                        {

                        }
                        App.CurrentApp.PlaySound("wronganswer.wav");
                    }



                    dtAnswerState.Start();
                    dtMain.Stop();

                    App.ViewModel.Save();

                }
            }
            
        }
        
        private void ListBox_Tap(object sender, GestureEventArgs e)
        {
            
        }

        private void deleteTaskButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_Tap_1(object sender, GestureEventArgs e)
        {


            
        }
       


        private void SearchVisualTreeForSelectedButton(DependencyObject targetElement, string ItemText)
        {
            var count = VisualTreeHelper.GetChildrenCount(targetElement);
            if (count == 0)
                return;

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(targetElement, i);
                if (child is Button )
                {
                    Button targetItem = (Button)child;

                    if (targetItem.CommandParameter  ==ItemText)
                    {
                        (((System.Windows.Media.Imaging.BitmapImage)(((System.Windows.Controls.Image)(targetItem.Content)).Source))).UriSource = new Uri(@"Images/answerpressed.png",UriKind.Relative);
                        
                        
                        return;
                    }
                }
                else
                {
                    SearchVisualTreeForSelectedButton(child, ItemText);
                }
            }
        }
        private void answerButton_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void imgSkip_Tap(object sender, GestureEventArgs e)
        {
            ((BitmapImage)imgResultState.Source).UriSource = new Uri(@"Images/sad.png", UriKind.Relative);
            App.ViewModel.CurrentScore--;
            App.ViewModel.GamePasses++;
            App.ViewModel.GameWrongAnswer ++;
            App.ViewModel.Save();
            App.CurrentApp.PlaySound("ButtonPressed.wav");
            while (App.CurrentApp.CurrentSoundState == SoundState.Playing)
            {

            }
            App.CurrentApp.PlaySound("wronganswer.wav");
            dtAnswerState.Start();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dtMain.Stop();
            App.CurrentApp.SetCurrentBackGroundSound("menu.mp3", "1");
        }

        private void SoundPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            //App.CurrentApp.ContinuePlaying(t, tr);
        }

        private void gamePlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            gamePlayer.Play();
        }
    }

    public class IdToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int id = (int)value;
            string img;

            switch (id)
            {
                case 0: img = @"/Images/answer.png"; break;
                case 1: img = @"/Images/answerpressed.png"; break;
                default: img = @"Resources/Image/unknown.png"; break;
            }

            return img;
        }

        #region IValueConverter Members


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}