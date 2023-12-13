using TestASP.Data;

namespace TestASP.Model
{
    public class UserDto : PublicProfile
    {
        public string Nickname { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string? AddressStr { get; set; }
        public string? BirthPlaceStr { get; set; }
        public DateOnly? Birthdate { get; set; }
        public string? Role { get; set; }

        public UserDto(){}
        public UserDto(User user, string rootUrl = "") : base(user,rootUrl)
        {
            Username = user.Username;
            Email = user.Email;
            Nickname = user.Nickname;
            //Token = user.Token;
            AddressStr = user.AddressStr;
            BirthPlaceStr = user.BirthPlaceStr;
            Birthdate = user.Birthdate;
            Role = user.Role;
        }

        public User ToData()
        {
            return new User()
            {
                FirstName = FirstName,
                LastName = LastName,
                Username = Username,
                Password = "",
                Email = Email,
                Role = Role,
                AddressStr = AddressStr,
                BirthPlaceStr = BirthPlaceStr,
                Birthdate = Birthdate ?? default,

            };
        }
    }
}
