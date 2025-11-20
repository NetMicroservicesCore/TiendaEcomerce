namespace TiendaEcomerce.Models
{
    public class RolePermissionViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<PermissionSelection> Permissions { get; set; }
    }
}
