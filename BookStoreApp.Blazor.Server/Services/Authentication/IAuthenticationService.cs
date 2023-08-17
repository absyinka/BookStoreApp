using BookStoreApp.Blazor.Server.Base;

namespace BookStoreApp.Blazor.Server.Services.Authentication;

public interface IAuthenticationService
{
    Task<bool> AuthenticateAsync(LoginUserDto loginModel);
    Task Logout();
}
