namespace TiendaEcomerce.Models
{
    public class PermissionSelection
    {
        public int PermissionId { get; set; }
        public string? Key { get; set; }
        public string? Description { get; set; }
        public bool Assigned { get; set; }
    }
}
