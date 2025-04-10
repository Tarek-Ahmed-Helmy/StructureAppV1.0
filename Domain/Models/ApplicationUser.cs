using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class ApplicationUser : IdentityUser
{
    [Required, MaxLength(100)]
    public string? FirstName { get; set; }
    
    [Required, MaxLength(100)]
    public string? LastName { get; set; }

    [Required, MaxLength(100)]
    public string? Address { get; set; }
}
