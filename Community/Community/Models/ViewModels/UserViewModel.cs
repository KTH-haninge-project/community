
namespace Community.ViewModels
{
    public class UserViewModel
    {
        public string id { get; set; }
        public string email { get; set; }

        public UserViewModel(string id, string email)
        {
            this.id = id;
            this.email = email;
        }
    }
}