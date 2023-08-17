using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.Base;
using BookStoreApp.Blazor.Server.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Server.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IClient _client;
    private readonly ILocalStorageService localStorage;
    private readonly AuthenticationStateProvider authProvider;

    public AuthenticationService(
        IClient client, 
        ILocalStorageService localStorage,
        AuthenticationStateProvider authProvider)
    {
        _client = client;
        this.localStorage = localStorage;
        this.authProvider = authProvider;
    }

    public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
    {
        var response = await _client.LoginAsync(loginModel);

        //Store Token
        await localStorage.SetItemAsync("accessToken", response.Token);

        //Change auth state of app
        await ((ApiAuthenticationStateProvider)authProvider).LoggedIn();

        return true;
    }

    public async Task Logout()
    {
        await ((ApiAuthenticationStateProvider)authProvider).LoggedOut();
    }
}
