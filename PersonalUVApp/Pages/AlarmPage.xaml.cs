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

            tpAlarm.Time = DateTime.Now.TimeOfDay;
            tpNotificationAlarm.Time = DateTime.Now.TimeOfDay;

            this.uvIndexMeans = uvIndexMeans;
            this.recommendText = recommendText;
        }

        void CreateAlarm(object sender, System.EventArgs e)
        {
            DependencyService.Get<ISetAlarm>().SetAlarm(DateTime.Now.Hour, DateTime.Now.Minute, uvIndexMeans, recommendText, 0);
        }

        void CreateNotificationAlarm(object sender, System.EventArgs e)
        {
            DependencyService.Get<ISetAlarm>().SetAlarm(DateTime.Now.Hour, DateTime.Now.Minute, uvIndexMeans, recommendText, 1);
        }
    }
}
