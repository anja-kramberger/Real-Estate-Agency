using System;
using System.Collections.Generic;

namespace AgencijaZaNekretnine.Models
{
    public partial class SlikeNekretnine
    {
        public int Id { get; set; }
        public int IdNekretnine { get; set; }
        public string UrlSlike { get; set; } = null!;

        public virtual Nekretnina IdNekretnineNavigation { get; set; } = null!;
    }
}
