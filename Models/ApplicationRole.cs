using Microsoft.AspNetCore.Identity;

namespace TiendaEcomerce.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }

        public List<RolePermission>? RolePermissions { get; set; }
    }
}
