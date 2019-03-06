using PersonalUVApp.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PersonalUVApp.Helper
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;
        //static bool IsUserSet => AppSettings.Contains(nameof(User.Username));

        #region Setting Constants

        private const string UserNameKey = "UserName";
        private static readonly string UserNameKeyDefault = string.Empty;
        private const string PasswordKey = "Password";
        private static readonly string PasswordKeyDefault = string.Empty;
        public static bool IsUserSet => AppSettings.Contains(UserNameKey);
        public static bool IsPasswordSet => AppSettings.Contains(PasswordKey);

        #endregion
        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(UserNameKey, UserNameKeyDefault);

            set => AppSettings.AddOrUpdateValue(UserNameKey, value);

        }
        public static string Password
        {
            get => AppSettings.GetValueOrDefault(PasswordKey, PasswordKeyDefault);

            set => AppSettings.AddOrUpdateValue(PasswordKey, value);

        }

    }
}
