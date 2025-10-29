namespace TiendaEcomerce.Models
{
    public class LoginViewModel
    {
        public string? ReturnUrl { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; } = null;

        public bool RememberMe { get; set; }
    }
}
