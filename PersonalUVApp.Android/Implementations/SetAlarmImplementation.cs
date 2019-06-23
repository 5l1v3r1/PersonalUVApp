using System;
using Android.App;
using Android.Content;
using Android.Widget;
using PersonalUVApp.DepInj;
using PersonalUVApp.Droid.Implementations;

[assembly: Xamarin.Forms.Dependency(typeof(SetAlarmImplementation))]
namespace PersonalUVApp.Droid.Implementations
{
    public class SetAlarmImplementation : ISetAlarm
    {
        public void SetAlarm(int hour, int minute, string title, string message, int mode)
        {
            Intent myintent;
            if (mode == 1)
                myintent = new Intent(Android.App.Application.Context, typeof(NotificationAlarmReceiver));
            else 
                myintent = new Intent(Android.App.Application.Context, typeof(AlarmReceiver));

            myintent.PutExtra("message", message);
            myintent.PutExtra("title", title);
            int _id = DateTime.Now.Millisecond;

            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            long millis = (long)ts.TotalMilliseconds;
            int i = (int)millis;

            PendingIntent pendingintent = PendingIntent.GetBroadcast(Android.App.Application.Context, i, myintent, PendingIntentFlags.OneShot);

            Java.Util.Date date = new Java.Util.Date();
            Java.Util.Calendar cal = Java.Util.Calendar.Instance;
            cal.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();
            cal.Set(Java.Util.CalendarField.HourOfDay, hour);
            cal.Set(Java.Util.CalendarField.Minute, minute);
            cal.Set(Java.Util.CalendarField.Second, 0);

            AlarmManager alarmManager = Android.App.Application.Context.GetSystemService(Android.Content.Context.AlarmService) as AlarmManager;
            alarmManager.Set(AlarmType.RtcWakeup, cal.TimeInMillis, pendingintent);

            Toast.MakeText(Android.App.Application.Context, "Alarm Created!", ToastLength.Long).Show();
        }
    }
}
