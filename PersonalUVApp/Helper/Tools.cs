using Acr.UserDialogs;
namespace PersonalUVApp.Helper
{
    public class Tools
    {
        public static void ShowMessage(string msg, string title, string okText = "OK")
        {
            UserDialogs.Instance.AlertAsync(msg, title, okText);
        }
    }
}
