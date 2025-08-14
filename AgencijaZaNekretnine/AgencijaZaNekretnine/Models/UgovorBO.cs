using System.ComponentModel.DataAnnotations;

namespace AgencijaZaNekretnine.Models
{
    public class UgovorBO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Morate uneti ime i prezime")]
        public string? PunoIme { get; set; }
        
        [Required(ErrorMessage = "Morate uneti JMBG")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Jmbg mora sadržati samo 11 cifri.")]
        public long Jmbg { get; set; }
        
        [Required(ErrorMessage = "Morate uneti broj telefona")]
        [RegularExpression(@"^\d{10,}$", ErrorMessage = "Nije dobro unesen broj telefona")]
        public string? BrojTelefona { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Morate uneti datum izdavanja ugovora")]
        public DateTime Datum { get; set; }
        
        [Required(ErrorMessage = "Iznos ne sme biti prazan")]
        public float Iznos { get; set; }
        
        [Required(ErrorMessage = "Morate uneti nekretninu")]
        public NekretninaBO? Nekretnina { get; set; }
        
        [Required(ErrorMessage = "Morate uneti placanje")]
        public PlacanjeBO? Placanje { get; set; }
        //[Required(ErrorMessage = "Morate uneti pdf")]
        public IFormFile? Pdf { get; set; }
        public string? PdfUrl { get; set; }

    }
}
