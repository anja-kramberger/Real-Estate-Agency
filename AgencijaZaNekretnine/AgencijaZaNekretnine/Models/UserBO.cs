using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AgencijaZaNekretnine.Models
{
    public class UserBO:IdentityUser
    {
        [Required(ErrorMessage = "Morate uneti Ime")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Morate uneti Prezime")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Morate uneti Broj Telefona")]
        
        public string Phone { get; set; }


        public Boolean isApproved { get; set; }
        public Boolean Employed { get; set; }

        //public string Role {  get; set; }

        //public ICollection<UserRoleBO>? UserRoles { get; set; }
    }
}
