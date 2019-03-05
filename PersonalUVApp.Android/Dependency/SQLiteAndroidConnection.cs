using PersonalUVApp.Droid.Dependency;
using PersonalUVApp.Helper;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteAndroidConnection))]
namespace PersonalUVApp.Droid.Dependency
{
    public class SQLiteAndroidConnection : ISQLiteConnection
    {
        public SQLiteConnection CreateConnection()
        {
            string documentsDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentsDirectoryPath, App.DbName + ".db3");


            if (!File.Exists(path))
            {

                using (BinaryReader binaryReader = new BinaryReader(Android.App.Application.Context.Assets.Open(App.DbName + ".db3")))
                {
                    using (BinaryWriter binaryWriter = new BinaryWriter(new FileStream(path, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int length;
                        while ((length = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            binaryWriter.Write(buffer, 0, length);
                        }
                    }
                }
            }
            SQLiteConnection conn = new SQLiteConnection(path, false);

            return conn;
        }


    }
}
/*try
            {
                string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string path = System.IO.Path.Combine(documentPath, App.DbName);
                SQLitePlatformAndroid platform = new SQLitePlatformAndroid();
                SQLiteConnection connection = new SQLiteConnection(platform, path);
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }*/
