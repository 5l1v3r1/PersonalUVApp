
using System.ComponentModel;
using PropertyChanged;

namespace PersonalUVApp.Models
{
    public class UserModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string SkinType { get; set; }
        public string Location { get; set; }
        public bool IsRemember { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
