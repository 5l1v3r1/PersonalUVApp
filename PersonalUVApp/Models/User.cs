namespace PersonalUVApp.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string SkinType { get; set; }
        public string Location { get; set; }
        public bool IsRemember{ get; set; }
    }
}
