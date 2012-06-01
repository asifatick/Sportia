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
namespace GrandAStudio.Sportia
{
    public static  class Util
    {
        public enum CategoryEnum
        {
            C00 =1,
            C80,
            C90,
            QUOTES,
            ROLES,
            ODIES,
            OSCAR,
            RANDOM,
            STAR,
            Director,
            Animation_Comedy,
            Horror_Scifi,
            Advanced
        }
        public static  bool UpdateUnlockCategories()
        {
            bool hasUnlocked = false;
            Moviadb1DataContext dbc = App.CurrentApp.DB;
            List<Categories> lockedCats = dbc.Categories.Where(c => c.IsUnlocked == 0).ToList();

            foreach (Categories cat in lockedCats)
            {
                Score s = dbc.Score.First();
                string[] con = cat.UnlockCode.Split('_');
                if (con[1] == "P")
                {
                    int  rpoint  ;
                    int.TryParse(con[0], out rpoint);
                    if (rpoint <= s.Total)
                    {
                        cat.IsUnlocked = 1;
                        hasUnlocked = true;
                    }
                }
                else if (con[1] == "G")
                {
                    int rgames;
                    int.TryParse(con[0], out rgames);
                    if (rgames <= s.TotalGames)
                    {
                        cat.IsUnlocked = 1;
                        hasUnlocked = true;
                    }
                }
            }
            dbc.SubmitChanges();
            return hasUnlocked;
        }
    }
}
