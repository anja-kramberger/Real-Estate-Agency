using System;
using System.Collections.Generic;

namespace AgencijaZaNekretnine.Models
{
    public partial class Tip
    {
        public Tip()
        {
            Nekretninas = new HashSet<Nekretnina>();
        }

        public int Id { get; set; }
        public string NazivTipa { get; set; } = null!;

        public virtual ICollection<Nekretnina> Nekretninas { get; set; }
    }
}
