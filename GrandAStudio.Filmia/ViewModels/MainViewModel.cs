using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data.Linq.Mapping;
//using System.Data.Linq;
using Microsoft.Phone.Data.Linq;
using System.Linq;



namespace GrandAStudio.Sportia
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Moviadb1DataContext dbcontext = App.CurrentApp.DB;
        public MainViewModel()
        {
            this.Items = new ObservableCollection<Questions>();
            
        }
       
        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<Questions> Items { get; private set; }

        private string _CodeEntered = "";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string CodeEntered
        {
            get
            {
                return _CodeEntered;
            }
            set
            {
                if (value != _CodeEntered)
                {
                    _CodeEntered = value;
                    NotifyPropertyChanged("CodeEntered");
                }
            }
        }
        public bool _IsSoundsOn =true;
        public bool IsSoundsOn { get 
        {
            _IsSoundsOn = Convert.ToBoolean( GScore.IsSoundOn);
            return _IsSoundsOn;
        } set {

            _IsSoundsOn = value;
            GScore.IsSoundOn = Convert.ToInt32( value);
            Save();
            NotifyPropertyChanged("IsSoundsOn");
        } }


        private Score score;

        public Score GScore
        {
            get
            {
                if (score == null)
                {
                    score = dbcontext.Score.First();
                }
                return score;
            }
            set
            {
                score = value;
                NotifyPropertyChanged("score");
            }
        }

        private Questions currentQuestion ;

        public Questions CurrentQuestion
        {
            get
            {
               

                return currentQuestion;
            }
            set
            {
                currentQuestion = value;
                NotifyPropertyChanged("CurrentQuestion");
            }
        }
        public int questionCount = 0;

       

        private void resetAnsweredQuestionsForCategory()
        {
            throw new NotImplementedException();
        }

        private void resetAskedQuestionsForCategory()
        {
            throw new NotImplementedException();
        }
        private int timer = 10;

        public int Timer
        {
            get
            {
                return timer;
            }
            set {
                timer = value;
                NotifyPropertyChanged("Timer");
            }
        }

        private int currentScore = 0;

        public int CurrentScore
        {
            get
            {
                return currentScore;
            }
            set
            {
                currentScore = value;
                NotifyPropertyChanged("CurrentScore");
            }
        }

        private int gameRightAnswer = 0;

        public int GameRightAnswer
        {
            get
            {
                return gameRightAnswer;
            }
            set
            {
                gameRightAnswer = value;
                NotifyPropertyChanged("gameRightAnswer");
            }
        }

        private int gameWrongAnswer = 0;

        public int GameWrongAnswer
        {
            get
            {
                return gameWrongAnswer;
            }
            set
            {
                gameWrongAnswer = value;
                NotifyPropertyChanged("gameWrongAnswer");
            }
        }

        private int gamePasses = 0;

        public int GamePasses
        {
            get
            {
                return gamePasses;
            }
            set
            {
                gamePasses = value;
                NotifyPropertyChanged("gamePasses");
            }
        }
        private int currentCat = 0;

        public int CurrentCat
        {
            get
            {
                return currentCat;
            }
            set
            {
                
                currentCat = value;
                NotifyPropertyChanged("CurrentCat");
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData(long catID)
        {
            // Sample data; replace with real data
          //  DataLoadOptions options = new DataLoadOptions();
            CurrentCat = (int)catID;

            List<Questions> questions = ResetQuestionsByCat(catID);    
              //IsSoundsOn = dbcontext.Score.First().
            
            this.IsDataLoaded = true;
        }
        public Tips GetRandomTips()
        {
           

            Random r = new Random();
            int randomIndex = 0;

            randomIndex = r.Next(1, dbcontext.Tips.Count());

            Tips tips =  dbcontext.Tips.Where(t => t.IID == randomIndex).FirstOrDefault();
            string tp = tips.Text;
            Tips tipNew = new Tips();
            tipNew.Text = @tp.Replace("\\r\\n", Environment.NewLine);
            return tipNew;
        }
        public Questions GetRandomQuestion()
        {
            if (Items.Count < 1)
            {
                ResetQuestionsByCat(CurrentCat);
            }

            Random r = new Random();
            int randomIndex = 0;

            randomIndex = r.Next(0, Items.Count); //Choose a random object in the list

            Questions q = Items[randomIndex];
            q.IsAsked = 1;
            Items.RemoveAt(randomIndex);
            CurrentQuestion = q;
            questionCount++;
            App.ViewModel.Save();
            return q;


        }
        private List<Questions> ResetQuestionsByCat(long catID)
        {
            bool resetAnswered = false;

            
            int tcount = 0;
            int aCount = 0;
            tcount = dbcontext.Questions.Where(qq => qq.CategoryID == catID

                        && qq.IsDeleted == 0).Count();
            aCount = dbcontext.Questions.Where(qq => qq.CategoryID == catID

                     && qq.IsDeleted == 0 && qq.IsAnswered == 1).Count();
            if (tcount == aCount)
            {
                dbcontext.Categories.Where(c => c.IID == catID).FirstOrDefault().IsCompleted = 1;
                resetAnswered = true;
            }

            List<Questions> questions = new List<Questions>();
            questions = getQuestionsByCategory(catID);
            if (questions.Count < 15)
            {
                foreach (Questions q in (dbcontext.Questions.Where(qq => qq.CategoryID == catID

                            && qq.IsDeleted == 0)))
                {
                    //if(resetAnswered)
                    //q.IsAnswered = 0;
                    
                    q.IsAsked = 0;

                }


               
                App.ViewModel.Save();
                questions = getQuestionsByCategory(catID).ToList();
            }
            Items = new ObservableCollection<Questions>(questions);
            return questions;
        }
        private List<Questions> getQuestionsByCategory(long catID)
        {
            byte isComplete =  dbcontext.Categories.Where(c => c.IID == catID).Select(c=> c.IsCompleted).FirstOrDefault().Value ;
          
            List<Questions> questions = new List<Questions>();
            if (isComplete == 1)
            {
                questions = dbcontext.Questions.Where(q => q.CategoryID == catID

                              && q.IsAsked == 0
                              && q.IsDeleted == 0).ToList();
                return questions;
            }
            else
            {
                questions = dbcontext.Questions.Where(q => q.CategoryID == catID
                               && q.IsAnswered == 0
                               && q.IsAsked == 0
                               && q.IsDeleted == 0).ToList();
            }
            return questions;
        }
       

        public void ResetGame()
        {
            Timer = 0;
            //CurrentCat = 0;
            CurrentScore = 0;
            GameRightAnswer = 0;
            GameWrongAnswer = 0;
            GamePasses = 0;
            questionCount = 0;
            IsDataLoaded = false;
        }

        /// <summary>
        /// Saves data of the updated question
        /// </summary>
        /// 
        public void Save()
        {
            dbcontext.SubmitChanges();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}