using TestASP.Data;

namespace TestASP.API.Models
{
    public class UserDto : PublicProfile
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }

        public UserDto(){}
        public UserDto(Data.User user, string rootUrl = "") : base(user,rootUrl)
        {
            Username = user.Username;
            Email = user.Email;
        }

        public Data.User ToData()
        {
            return new Data.User()
            {
                FirstName = FirstName,
                LastName = LastName,
                Username = Username,
                Password = "",
                Email = Email,
            };
        }
    }
}
