using System;
using System.Collections.Generic;
using PersonalUVApp.DepInj;
using Plugin.LocalNotifications;
using Xamarin.Forms;

namespace PersonalUVApp.Pages
{
    public partial class AlarmPage : ContentPage
    {

        private string uvIndexMeans;
        private string recommendText;

        public AlarmPage(string uvIndexMeans, string recommendText)
        {
            InitializeComponent();
            this.uvIndexMeans = uvIndexMeans;
            this.recommendText = recommendText;
        }

        void CreateAlarm(object sender, System.EventArgs e)
        {
            Console.WriteLine(DateTime.Now.Hour + " " + DateTime.Now.Minute);

            //LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.notifyicon;
            //CrossLocalNotifications.Current.Show("Extreme Risk",
            //   "A UV Index reading of 11 or more means extreme risk of harm from unprotected sun exposure. Take all precautions because unprotected skin and eyes can burn in minutes.");

            //string title = "Extreme Risk";
            //string message = "A UV Index reading of 11 or more means extreme risk of harm from unprotected sun exposure. Take all precautions because unprotected skin and eyes can burn in minutes.";

            DependencyService.Get<ISetAlarm>().SetAlarm(DateTime.Now.Hour, DateTime.Now.Minute+1, uvIndexMeans, recommendText);
        }
    }
}
