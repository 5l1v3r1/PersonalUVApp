using SQLite;

namespace PersonalUVApp.Helper
{
    public interface ISQLiteConnection
    {
        SQLiteConnection CreateConnection();
    }
}
