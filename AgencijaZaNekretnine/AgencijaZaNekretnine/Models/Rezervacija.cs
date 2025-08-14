using System;
using System.Collections.Generic;

namespace AgencijaZaNekretnine.Models
{
    public partial class Rezervacija
    {
        public int Id { get; set; }
        public string IdAgenta { get; set; } = null!;
        public string? Opis { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }

        public virtual AspNetUser IdAgentaNavigation { get; set; } = null!;
    }
}
