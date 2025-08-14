using System.ComponentModel.DataAnnotations;

namespace AgencijaZaNekretnine.Models
{
    
    public class TipBO
    {
        //[Required(ErrorMessage = "Morate uneti tip")]
        public int Id { get; set; }
        public string? Naziv { get; set; }
    }
}
