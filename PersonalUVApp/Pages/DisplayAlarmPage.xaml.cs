using System;
using System.Collections.Generic;
using Android.Media;
using Xamarin.Forms;

namespace PersonalUVApp.Pages
{
    public partial class DisplayAlarmPage : ContentPage
    {
        Ringtone r;

        public DisplayAlarmPage(string title, string msg, Ringtone r)
        {
            InitializeComponent();

            //lblTitle.Text = title;
            //lblMsg.Text = msg;
            //this.r = r;
        }

        void StopAlarm(object sender, System.EventArgs e)
        {
            r.Stop();
            btnCloseAlarm.Opacity = .5;
        }

        void ReturnMainPage(object sender, System.EventArgs e)
        {
            App.UvApp.PopPage();
        }
    }
}
