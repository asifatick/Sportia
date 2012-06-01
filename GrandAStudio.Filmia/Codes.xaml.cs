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
    public partial class Codes : PhoneApplicationPage
    {
        /// <summary>
        /// 
        /// </summary>
        Moviadb1DataContext dbc = App.CurrentApp.DB;
        public Codes()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            Moviadb1DataContext dbc = App.CurrentApp.DB;
        }

        private void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void txtCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //processCode(txtCode.Text);

                if (!App.ViewModel.GScore.AvailPasses.ToLower().Split(',').Contains(txtCode.Text.ToLower()))
                {
                    if (processCode(txtCode.Text))
                    {
                        imgMsgStrip.Visibility = System.Windows.Visibility.Visible;
                        lblMsg.Visibility = System.Windows.Visibility.Visible;
                        App.ViewModel.GScore.AvailPasses = App.ViewModel.GScore.AvailPasses + "," + txtCode.Text;
                        App.CurrentApp.PlaySound("unlock.wav");
                       // App.ViewModel.GScore.TotalPasses++;
                        App.ViewModel.Save();
                    }
                    else
                    {
                        imgMsgStrip.Visibility = System.Windows.Visibility.Visible;
                        lblMsg.Visibility = System.Windows.Visibility.Visible;
                        lblMsg.Text = "You have entered an invalid pass code.";
                    }
                }
                else
                {
                    imgMsgStrip.Visibility = System.Windows.Visibility.Visible;
                    lblMsg.Visibility = System.Windows.Visibility.Visible;
                    lblMsg.Text = "You have already used the pass code once.";
                }
               // lblMsg.Text = App.ViewModel.CodeEntered;
                txtCode.Text = string.Empty;
                this.Focus();
            }
        }

        private bool processCode(string code)
        {
            bool IsValidCode = true;
            switch (code.ToUpper())
            {
                case "GANDA": { unlockCategory(6); }
                     break;
                case "LOCKS": { unlockCategory(13); }
                     break;
                case "BRAVE": { increaseScorePoint(25); }
                     break;
                case "GOODF": { increaseScorePoint(25); }
                     break;
                case "ANGEL": { increaseScorePoint(25); }
                     break;
                case "ZZXXQ": { increaseScorePoint(25); }
                     break;
                case "BTTFI": { increaseScorePoint(25); }
                    break;
                case "TOMBS": { increaseScorePoint(25); }
                    break;
                case "WESTC": { increaseScorePoint(25); }
                    break;
                case "ORLAN": { increaseScorePoint(25); }
               
                    
                    break;
                case "SCARF": { increaseScorePoint(50); }
                     break;
                case "CASIN": { increaseScorePoint(50); }
                     break;
                default: { IsValidCode = false; }
                     break;

            }
            return IsValidCode;
        }

        private void increaseScorePoint(int p)
        {
            
            long point = dbc.Score.First().Total.Value + p;
            dbc.Score.First().Total = point;
            lblMsg.Text = "You have unlocked "+ p.ToString() + " points.";
            dbc.SubmitChanges();
            Util.UpdateUnlockCategories();
        }

        private void unlockCategory(int p)
        {
            dbc.Categories.Where(c => c.IID == (long)p).FirstOrDefault().IsUnlocked = 1;
            lblMsg.Text = "You have unlocked a secret caregory.";
           dbc.SubmitChanges();
        }

        private void ContentPanel_Loaded(object sender, RoutedEventArgs e)
        {
            txtCode.Text = string.Empty;
            // Turn on Tab Stops.  You can set this in XAML as well.  
            this.IsTabStop = true;

            // Set focus on the TextBox.
            txtCode.Focus();
        }
    }
}