using System.ComponentModel.DataAnnotations;

namespace AgencijaZaNekretnine.Models
{
    public class PreduzimacBO
    {
        public int IDPreduzimaca { get; set; }
        [Required(ErrorMessage = "Morate uneti naziv")]
        public string? Naziv { get; set; }
        [Required(ErrorMessage = "Morate uneti broj nekretnina")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Broj nekretnina mora biti izrežen u brojevima.")]

        public int BrojNekretnina { get; set; }
        
        public string? Opis { get; set; }

        public int Count { get; set; }

        
    }
}
