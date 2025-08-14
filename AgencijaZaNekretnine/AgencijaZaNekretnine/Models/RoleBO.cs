using Microsoft.AspNetCore.Identity;

namespace AgencijaZaNekretnine.Models
{
    public class RoleBO:IdentityRole
    {
        public ICollection<UserRoleBO>? UserRoles { get; set; }
    }
}
