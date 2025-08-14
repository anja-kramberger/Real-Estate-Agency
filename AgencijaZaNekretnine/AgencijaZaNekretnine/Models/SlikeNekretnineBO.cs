using System.ComponentModel.DataAnnotations;

namespace AgencijaZaNekretnine.Models
{
    public class SlikeNekretnineBO
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Morate uneti ime i prezime")]
        public string? UrlSlike { get; set; }
        //[Required(ErrorMessage = "Morate uneti nekretninu")]
        public NekretninaBO Nekretnina { get; set; }
    }
}
