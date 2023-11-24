using System.Security.Claims;
using System.Threading.Tasks;
using TestASP.Data;

namespace TestASP.Core.IService
{
    public interface IJwtSerivceManager
    {
        string CreateToken(User user);
        //string CreateToken(UserDto user);
        //ClaimsPrincipal CreateClaimsPrincipal(UserDto user);
        ClaimsPrincipal CreateClaimsPrincipal(User user);
        //Task<string> CreateToken(SignInUserRequestDto user);
        //Task<bool> SaveIndentity(SignUpUserRequestDto signUpUser, bool isAdmin = false);
        bool IsEnabled { get; }
    }
}
