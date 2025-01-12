
using Microsoft.AspNetCore.Identity;

namespace server.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? ProfileImage { get; set; }
        public string? Bio { get; set; }

        public Guid RoleId { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
