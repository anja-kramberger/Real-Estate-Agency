using System;
using System.Collections.Generic;

namespace AgencijaZaNekretnine.Models
{
    public partial class Preduzimac
    {
        public Preduzimac()
        {
            Nekretninas = new HashSet<Nekretnina>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; } = null!;
        public int BrojNekretnina { get; set; }
        public string? Opis { get; set; }

        public virtual ICollection<Nekretnina> Nekretninas { get; set; }
    }
}
