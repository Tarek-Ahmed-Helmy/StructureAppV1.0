using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }
}
