namespace TiendaEcomerce.Models
{
    public class RegisterViewModel
    {
        public required string Email { get; set; }

        public required string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Password { get; set; }
    }
}
