using Microsoft.AspNetCore.Identity;

namespace server.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public ICollection<ApplicationUser> Users { get; set; } = [];
}
