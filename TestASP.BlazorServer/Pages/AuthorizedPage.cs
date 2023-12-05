using System.Diagnostics.CodeAnalysis;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TestASP.BlazorServer.IServices;
using TestASP.Model;

namespace TestASP.BlazorServer.Pages;

public class AuthorizedPage : ComponentBase
{
    //[Inject, NotNull] public IUserService UserService { get; set; } = default;
    [Inject, NotNull] public IUserService UserService { get; set; } = default;
    [Inject, NotNull] public AuthenticationStateProvider authStateProvider { get; set; } = default;

    public UserDto? LoggedInUser { get; private set; }
    public AuthenticationState? AuthState { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        AuthState = await authStateProvider.GetAuthenticationStateAsync();
        await base.OnInitializedAsync();
    }

    public bool IsLoggedIn => AuthState?.User.Identity?.IsAuthenticated ?? false;
    

    public async Task<UserDto?> GetLoggedInUser(ToastService? toastService = null)
    {
        if(IsLoggedIn && LoggedInUser == null)
        {
            var result = await UserService.GetAsync(AuthState?.User.Identity?.Name ?? "");
            if (result != null)
            {
                if (!result.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(result.Error) && toastService != null)
                    {
                        await toastService!.Error("Retreive Error", result.Error);
                    }
                }
                LoggedInUser = result.Data;
            }
        }

        return LoggedInUser;
    }

}