using Microsoft.AspNetCore.Identity;

namespace Infraestructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Puedes extender con más propiedades si quieres
        public string FullName { get; set; } = string.Empty;
    }
}
