using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PersonalUVApp.Helper
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static bool IsUserModelSet => AppSettings.Contains(UserModelJson);
        public static bool IsRememberMeOnSet => AppSettings.Contains(UserModelJson);

        private const string UserModelJson = "UserModelJson";
        private const string IsRememberMeOn = "IsRememberMeOn";


        public static string UserJson
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserModelJson, "");
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserModelJson, value);
            }
        }

        public static bool IsRememberMe
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsRememberMeOn, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsRememberMeOn, value);
            }
        }



    }
}
