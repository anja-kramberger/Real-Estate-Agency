using System;
using System.Collections.Generic;

namespace AgencijaZaNekretnine.Models
{
    public partial class Status
    {
        public Status()
        {
            Nekretninas = new HashSet<Nekretnina>();
        }

        public int Id { get; set; }
        public string NazivStatusa { get; set; } = null!;

        public virtual ICollection<Nekretnina> Nekretninas { get; set; }
    }
}
