using System.ComponentModel.DataAnnotations;

namespace AgencijaZaNekretnine.Models
{
    public class RezervacijaBO
    {
        public int Id { get; set; }
        public string Opis { get; set; }

       // [DataType(DataType.DateTime)]
        public DateTime DatumOd { get; set; }
       // [DataType(DataType.DateTime)]
        public DateTime DatumDo { get; set; }

        //[Required(ErrorMessage = "Morate izabrati agenta")]
        public AspNetUser? Agent { get; set; }

    }
}
