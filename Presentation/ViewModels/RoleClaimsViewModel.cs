using Utilities;

namespace Presentation.ViewModels;

public class RoleClaimsViewModel
{
    public RoleClaimsViewModel()
    {
        Claims = new List<AppClaim>();
    }
    public string RoleId { get; set; }
    public List<AppClaim> Claims { get; set; }
}
