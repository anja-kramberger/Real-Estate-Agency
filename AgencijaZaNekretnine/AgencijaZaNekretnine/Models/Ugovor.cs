using System;
using System.Collections.Generic;

namespace AgencijaZaNekretnine.Models
{
    public partial class Ugovor
    {
        public int Id { get; set; }
        public int IdPlacanja { get; set; }
        public int IdNekretnine { get; set; }
        public string PunoIme { get; set; } = null!;
        public string BrojTelefona { get; set; } = null!;
        public DateTime Datum { get; set; }
        public decimal Iznos { get; set; }
        public long Jmbg { get; set; }
        public string? PdfUrl { get; set; }

        public virtual Nekretnina IdNekretnineNavigation { get; set; } = null!;
        public virtual Placanje IdPlacanjaNavigation { get; set; } = null!;
    }
}
