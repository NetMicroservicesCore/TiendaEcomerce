namespace TiendaEcomerce.Models
{
    public class RoleViewModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string Id { get; set; }= string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
