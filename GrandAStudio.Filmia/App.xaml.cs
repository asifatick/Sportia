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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Windows.Resources;
using Microsoft.Xna.Framework.Media;

namespace GrandAStudio.Sportia
{
    public partial class App : Application
    {
        private static MainViewModel viewModel = null;

        public MediaState phoneMediaState;

        private static App app;
        public bool IsBgSoundPlaying = false;
        public static SoundEffect _GameSound;
        public  SoundEffect _BackSound;
       // public static SoundEffectInstance _currentSound;

        public static App CurrentApp

        {

            get { return app; }

        }
        public void CloseBGPlayer()
        {
            IsBgSoundPlaying = false;
        }
        public void StopbackgroundSound()
        {
            IsBgSoundPlaying = false;
            MediaElement player = null; // get the media element from App resources
            if (App.Current.Resources.Contains("bgPlayer"))
            {
                player = App.Current.Resources["bgPlayer"] as MediaElement;
            }
            if (player != null)
            {
                player.Stop();

            }
            
        }
        public void pausebackgroundSound()
        {
            IsBgSoundPlaying = false;
            MediaElement player = null; // get the media element from App resources
            if (App.Current.Resources.Contains("bgPlayer"))
            {
                player = App.Current.Resources["bgPlayer"] as MediaElement;
            }
            if (player != null)
            {
                player.Pause();

            }

        }
        public void resumebackgroundSound()
        {

            SetCurrentBackGroundSound(_CurrentBGSound, "1");

        }
        public SoundState CurrentSoundState
        {
            get
            {
                if (_currentSound != null)
                    return _currentSound.State;

                return SoundState.Stopped;
            }
        }
        
        private SoundEffectInstance _currentSound = null;
        public bool PlaySound(string source)
        {
            if (App.ViewModel.IsSoundsOn)
            {
                SoundEffect se;

                if (_currentSound != null)
                {
                    _currentSound.Stop();
                    _currentSound = null;
                }
                var isoStore = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
                if (isoStore.FileExists(source))
                {
                    byte[] fileContents;
                    using (var fileStream = isoStore.OpenFile(source, FileMode.Open))
                    {
                        se = SoundEffect.FromStream(fileStream);
                        fileStream.Close();//not really needed, but it makes me feel better. 
                    }

                    _currentSound = se.CreateInstance();
                    _currentSound.Play();

                }
                return true;
            }
            return false;
        }
        string _CurrentBGSound = "";

        public void SetCurrentBackGroundSound(string _fileName, string title)
        { 
        SetCurrentBackGroundSound(  _fileName,  title, false);
        }

        public  void SetCurrentBackGroundSound( string _fileName, string title, bool confirmed )
        {
            _CurrentBGSound = _fileName;

            MediaElement player = null; // get the media element from App resources
            if (App.Current.Resources.Contains("bgPlayer"))
            {
                player = App.Current.Resources["bgPlayer"] as MediaElement;
            }
            phoneMediaState = MediaPlayer.State;
            if ((player != null && App.ViewModel.IsSoundsOn))
                            {
            switch (MediaPlayer.State)
            {
                case    MediaState.Playing:
                    {
                        if (!confirmed)
                        {
                            if (MessageBox.Show("Do you want to play Game Sounds?", "Sportia", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                            {
                                StopbackgroundSound();
                                App.ViewModel.IsSoundsOn = false;
                            }
                            else
                            {
                                MediaPlayer.Pause();

                                player.Source = (new Uri("/sounds/" + _CurrentBGSound, UriKind.Relative));
                                IsBgSoundPlaying = true;
                                App.ViewModel.IsSoundsOn = true;

                            }
                        }
                        else
                        {
                            MediaPlayer.Pause();

                            player.Source = (new Uri("/sounds/" + _CurrentBGSound, UriKind.Relative));
                            IsBgSoundPlaying = true;
                                               
                        }
                        break;
                    }
                

                default:
                    {
                        player.Source = (new Uri("/sounds/" + _CurrentBGSound, UriKind.Relative));
                        IsBgSoundPlaying = true;
                        break;
                    }
            }
          
            }
          
         
        }
        

        /*
         "Button_Pressed.mp3",
                                                "categories.mp3", 
                                                "cheer.mp3",
                "gameplay.mp3","menu.mp3","score_screen.mp3","unlock.mp3","wrong_answer8.mp3",
         */
        private void CopyToIsolatedStorage()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string[] files = new string[] { 
                "ButtonPressed.wav",
                "unlock.wav",
                "wronganswer.wav"};

                foreach (var _fileName in files)
                {
                    if (!storage.FileExists(_fileName))
                    {
                        string _filePath = "sounds/" + _fileName;
                        StreamResourceInfo resource = Application.GetResourceStream(new Uri(_filePath, UriKind.Relative));

                        using (IsolatedStorageFileStream file = storage.CreateFile(_fileName))
                        {
                            int chunkSize = 4096;
                            byte[] bytes = new byte[chunkSize];
                            int byteCount;

                            while ((byteCount = resource.Stream.Read(bytes, 0, chunkSize)) > 0)
                            {
                                file.Write(bytes, 0, byteCount);
                            }
                        }
                    }
                }
            }
        }
 

        public Moviadb1DataContext DB { get; private set; }

 

        /// <summary>

        /// Constructor for the Application object.

        /// </summary>


 

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }


       
        
        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            this.ApplicationLifetimeObjects.Add(new
                 XNAAsyncDispatcher(TimeSpan.FromMilliseconds(50)));
            app = this;
            DB = new Moviadb1DataContext();
            if (!App.CurrentApp.DB.DatabaseExists())
            {                
                App.CurrentApp.DB.CreateDatabase();               
            }
            CopyToIsolatedStorage();
           
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
               PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // Ensure that application state is restored appropriately
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData(8);
            }
            resumebackgroundSound();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            // Ensure that required application state is persisted here.
            pausebackgroundSound();

        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
              
                StopbackgroundSound();
                CloseBGPlayer();
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

        private void bgPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            MediaElement player = null; // get the media element from App resources
            if (App.Current.Resources.Contains("bgPlayer"))
            {
                player = App.Current.Resources["bgPlayer"] as MediaElement;
            }
            if (player != null)
            {
                //player.Source = (new Uri("/sounds/" + _CurrentBGSound, UriKind.Relative));
                player.Play();
            }
        }

        private void bgPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement player = null; // get the media element from App resources
            if (App.Current.Resources.Contains("bgPlayer"))
            {
                player = App.Current.Resources["bgPlayer"] as MediaElement;
            }
            if (player != null)
            {
                player.Source = (new Uri("/sounds/" + _CurrentBGSound, UriKind.Relative));
               // player.Play();
            }
        }
    }
}