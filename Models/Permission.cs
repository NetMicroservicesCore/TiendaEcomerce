namespace TiendaEcomerce.Models
{
    public class Permission
    {

        public int PermissionId { get; set; }   // Debe coincidir con la clave
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
