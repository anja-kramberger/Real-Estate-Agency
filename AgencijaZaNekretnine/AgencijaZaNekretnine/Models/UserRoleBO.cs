using Microsoft.AspNetCore.Identity;

namespace AgencijaZaNekretnine.Models
{
    public class UserRoleBO:IdentityUserRole<string>
    {
        public virtual UserBO? User { get; set; }
        public virtual RoleBO? Role { get; set;}
    }
}
