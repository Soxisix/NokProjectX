namespace NokProjectX.Wpf.Entities
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsSelected { get; set; }
    }
}