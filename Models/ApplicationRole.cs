using Microsoft.AspNetCore.Identity;

namespace TiendaEcomerce.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
