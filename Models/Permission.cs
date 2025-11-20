namespace TiendaEcomerce.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string? Key { get; set; }  // "Course.ReadOwn"
        public string? Description { get; set; }
    }
}
