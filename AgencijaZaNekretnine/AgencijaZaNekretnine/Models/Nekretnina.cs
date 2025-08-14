using System;
using System.Collections.Generic;

namespace AgencijaZaNekretnine.Models
{
    public partial class Nekretnina
    {
        public Nekretnina()
        {
            SlikeNekretnines = new HashSet<SlikeNekretnine>();
            Ugovors = new HashSet<Ugovor>();
        }

        public int Id { get; set; }
        public int IdTip { get; set; }
        public int IdStatus { get; set; }
        public string? IdUser { get; set; }
        public int IdPreduzimac { get; set; }
        public string Adresa { get; set; } = null!;
        public decimal Cena { get; set; }
        public double Kvadratura { get; set; }
        public double BrojSoba { get; set; }
        public string? Opis { get; set; }
        public string? Slika { get; set; }

        public virtual Preduzimac IdPreduzimacNavigation { get; set; } = null!;
        public virtual Status IdStatusNavigation { get; set; } = null!;
        public virtual Tip IdTipNavigation { get; set; } = null!;
        public virtual AspNetUser? IdUserNavigation { get; set; }
        public virtual ICollection<SlikeNekretnine> SlikeNekretnines { get; set; }
        public virtual ICollection<Ugovor> Ugovors { get; set; }
    }
}
