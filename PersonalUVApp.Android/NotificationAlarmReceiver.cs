using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using Android.Widget;

namespace PersonalUVApp.Droid 
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { "android.intent.action.BOOT_COMPLETED" }, Priority = (int)IntentFilterPriority.LowPriority)]
    public class NotificationAlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //Toast.MakeText(Android.App.Application.Context, "NotificationAlarmReceiver  OnReceive", ToastLength.Long).Show();

            var message = intent.GetStringExtra("message");
            var title = intent.GetStringExtra("title");
            Intent i = new Intent(context, typeof(AppStickyService));
            i.PutExtra("message", message);
            i.PutExtra("title", title);
            context.StartService(i);
        }
    }

}
