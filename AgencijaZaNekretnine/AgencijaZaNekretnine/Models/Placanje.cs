using System;
using System.Collections.Generic;

namespace AgencijaZaNekretnine.Models
{
    public partial class Placanje
    {
        public Placanje()
        {
            Ugovors = new HashSet<Ugovor>();
        }

        public int Id { get; set; }
        public string NacinPlacanja { get; set; } = null!;
        public string? Opis { get; set; }

        public virtual ICollection<Ugovor> Ugovors { get; set; }
    }
}
