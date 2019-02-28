using PersonalUVApp.Models;
using SQLite;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PersonalUVApp.Helper
{
    public class SQLiteManager
    {
        private readonly SQLiteConnection _connection;

        public SQLiteManager()
        {
            _connection = DependencyService.Get<ISQLiteConnection>().CreateConnection();
            _connection.CreateTable<User>();
        }

        #region CRUD
        public int Insert(User user)
        {
            return _connection.Insert(user);
        }

        public int Update(User user)
        {
            return _connection.Update(user);
        }

        public int Delete(int id)
        {
            return _connection.Delete<User>(id);
        }

        public IEnumerable<User> GetAll()
        {
            //return (from i in connection.Table<Student>() select i);
            return _connection.Table<User>();
        }

        public User Get(int id)
        {
            return _connection.Table<User>().FirstOrDefault(x => x.Id == id);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
        #endregion
    }
}
