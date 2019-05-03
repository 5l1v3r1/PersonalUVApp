using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;
using Java.Util;

namespace PersonalUVApp.Droid
{
    [Service(Exported = true, Name = "com.deuceng.PersonalUVApp.AppStickyService")]
    public class AppStickyService : IntentService
    {
        static readonly string CHANNEL_ID = "location_notification";


        public override void OnCreate()
        {
            base.OnCreate();
            Toast.MakeText(this, "service started ", ToastLength.Long).Show();

            System.Diagnostics.Debug.WriteLine("Sticky Service - Created");
        }

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            CreateNotificationChannel();
            WireAlarm(intent);

            return StartCommandResult.Sticky;
        }

        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            System.Diagnostics.Debug.WriteLine("Sticky Service - Binded");
            Toast.MakeText(this, "Service started", ToastLength.Long).Show();

            return null;
        }

        public override void OnDestroy()
        {
            try
            {
                base.OnDestroy();

            }
            catch (Java.Lang.IllegalStateException ex)
            {
                //Log.Debug("MainActivity.OnDestroy", ex, "The activity was destroyed twice");
                System.Diagnostics.Debug.WriteLine("Sticky Service error " + ex);
            }
        }


        private void Cancelnotification(int id)
        {
            NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Cancel(id);
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var name = Resources.GetString(Resource.String.channel_name);
            var description = GetString(Resource.String.channel_description);
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.Default)
            {
                Description = description
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
        public void WireAlarm(Intent intent)
        {
            var title = intent.GetStringExtra("title");
            var message = intent.GetStringExtra("message");

            Intent mainActivityIntent = new Intent(this, typeof(MainActivity));

            const int pendingIntentId = 10;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(this, pendingIntentId, mainActivityIntent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, CHANNEL_ID)
                .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.notifyicon24)
                .SetStyle(new NotificationCompat.BigTextStyle()
                    .BigText(message));

            Notification notification = builder.Build();

            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);

        }

        protected override void OnHandleIntent(Intent intent)
        {
            throw new NotImplementedException();
        }
    }
}
