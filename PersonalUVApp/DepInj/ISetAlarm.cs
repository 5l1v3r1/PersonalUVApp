using System;
namespace PersonalUVApp.DepInj
{
    public interface ISetAlarm
    {
        void SetAlarm(int hour, int minute, string title, string message, int mode);
    }
}
