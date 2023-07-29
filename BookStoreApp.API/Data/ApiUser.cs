using Microsoft.AspNetCore.Identity;

namespace BookStoreApp.API.Data;

public class ApiUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
