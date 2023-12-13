using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using TestASP.Model;
using TestASP.Core.IService;
using TestASP.Data;

namespace TestASP.API.Services
{
    internal class JwtSerivceManager : IJwtSerivceManager
    {
        IConfiguration _configuration;
        //UserManager<User> _userManager;
        //SignInManager<User> _signInManager;
        /*UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;*/

        public JwtSerivceManager(IConfiguration configuration
            //UserManager<User> userManager,
            //SignInManager<User> signInManager,
            /*UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager*/)
        {
            _configuration = configuration;
            /*_userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;*/
        }

        public bool IsEnabled => _configuration.GetValue<bool>("JWT:IsEnabled");
        public string CreateToken(UserDto user)
        {
            if(user != null)
            {
                List<Claim> authClaims = CreateClaims(user.Username, user.Role);

                JwtSecurityToken token = CreateSecurityToken(authClaims);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return null;
        }

        public string CreateToken(Data.User user)
        {
            return CreateToken(new UserDto(user));
        }

        public ClaimsPrincipal CreateClaimsPrincipal(UserDto user)
        {
            var claimsIdentity = new ClaimsIdentity(
                CreateClaims(user.Username, user.Role),
                CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(claimsIdentity);
        }

        public ClaimsPrincipal CreateClaimsPrincipal(Data.User user)
        {
            return CreateClaimsPrincipal(new UserDto(user));
        }

        #region signin
        //public async Task<string> CreateToken(SignInUserRequestDto signInUser)
        //{
        //    ApplicationUser user = await _userManager.FindByNameAsync(signInUser.Username);
        //    if (user != null && await _userManager.CheckPasswordAsync(user, signInUser.Password))
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);

        //        List<Claim> authClaims = CreateClaims(user.UserName);

        //        //if user has many roles
        //        foreach (var userRole in userRoles)
        //        {
        //            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //        }

        //        JwtSecurityToken token = CreateSecurityToken(authClaims);

        //        //SignInResult result = await _signInManager.PasswordSignInAsync(user, signInUser.Password, false, false);
        //        //if (!result.Succeeded)
        //        //{
        //        //    throw new Exception(result.ToString());
        //        //}

        //        return new JwtSecurityTokenHandler().WriteToken(token);
        //    }
        //    return null;
        //}

        //public async Task<bool> SaveIndentity(SignUpUserRequestDto signUpUser, bool isAdmin = false)
        //{
        //    ApplicationUser userExists = await _userManager.FindByNameAsync(signUpUser.Username);
        //    if (userExists != null)
        //    {
        //        return false;
        //    }
        //    ApplicationUser user = new ApplicationUser()
        //    {
        //        Email = signUpUser.Email,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = signUpUser.Username,
        //    };
        //    IdentityResult result = await _userManager.CreateAsync(user, signUpUser.Password);
        //    if (!result.Succeeded)
        //    {
        //        throw new Exception(result.ToString());
        //    }

        //    if (isAdmin)
        //    {
        //        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
        //            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        //        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
        //            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        //        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        //        {
        //            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        //        }
        //    }
        //    //else
        //    //{
        //    //    if (!await _roleManager.RoleExistsAsync(UserRoles.User))
        //    //        await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
        //    //    await _userManager.AddToRoleAsync(user, UserRoles.User);
        //    //}
        //    return result.Succeeded;
        //}
        #endregion

        #region V2
        //public async Task<ApplicationUser> Authenticate(SignInUserRequestDto signInUser)
        //{
        //    if (signInUser != null)
        //    {

        //        ApplicationUser user = await _userManager.FindByNameAsync(signInUser.Username);
        //        if (user != null && await _userManager.CheckPasswordAsync(user, signInUser.Password))
        //        {
        //            return user;
        //        }
        //    }
        //    return null;
        //}

        public string GenerateToken(ApplicationUser user)
        {
            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            SigningCredentials signingCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
            List<Claim> authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                };

            JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        List<Claim> CreateClaims(string username, string? role = null)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,username),
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role?.ToLower() ?? "user"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }

        JwtSecurityToken CreateSecurityToken(List<Claim> authClaims)
        {
            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            return new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
        }
        #endregion
    }
}
