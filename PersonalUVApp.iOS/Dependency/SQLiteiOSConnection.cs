using Foundation;
using PersonalUVApp.Helper;
using PersonalUVApp.iOS.Dependency;
using SQLite;
using System;
using System.IO;
[assembly: Xamarin.Forms.Dependency(typeof(SQLiteiOSConnection))]
namespace PersonalUVApp.iOS.Dependency
{
    public class SQLiteiOSConnection : ISQLiteConnection
    {
        public SQLiteConnection CreateConnection()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }
            string path = Path.Combine(libFolder, App.DbName + ".db");

            // This is where we copy in the pre-created database
            if (!File.Exists(path))
            {
                string existingDb = NSBundle.MainBundle.PathForResource(App.DbName, "db");
                File.Copy(existingDb, path);
            }

            SQLiteConnection connection = new SQLiteConnection(path, false);

            // Return the database connection 
            return connection;
        }
    }
}