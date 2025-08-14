using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Org.BouncyCastle.Asn1.Cmp;

namespace AgencijaZaNekretnine.Models
{
    public class NekretninaBO
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Morate uneti cenu polja")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Cena mora biti izražena u brojevima.")]
        public float Cena { get; set; }


        [Required(ErrorMessage = "Morate uneti adrest nekretnine")]
        public string? Adresa { get; set; }

        [Required(ErrorMessage = "Morate uneti kvadraturu")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Kvadratura mora biti izražena u brojevima.")]

        public float Kvadratura { get; set; }


        [Required(ErrorMessage = "Morate uneti broj soba")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Broj soba mora biti izražena u brojevima.")]

        public float BrojSoba { get; set; }

        
        public string? Opis { get; set; }
        public string? Slika { get; set; } //string?
        [NotMapped]
        public IFormFile? File { get; set; } 
        public ICollection<SlikeNekretnineBO>? ImgUrls { get; set; }

        //[Required(ErrorMessage = "Morate izabrati status nekretnine")]
        public StatusBO? Status { get; set; }


        //[BindRequired]
        //[Required(ErrorMessage = "Morate izabrati tip nekretnine")]
        //[Range(1, Int32.MaxValue, ErrorMessage = "Morate izabrati tip nekretninee")]
        //[GenericRequired(ErrorMessage = "Morate izabrati Tip nekretnine")]
        [Required(ErrorMessage = "Morate izabrati tip nekretnine")]
        public TipBO? Tip { get; set; }


        [Required(ErrorMessage = "Morate izabrati agenta")]
        public AspNetUser? Agent { get; set; }

        //[BindRequired]
        //[Required(ErrorMessage = "Morate izabrati preduzimaca")]
        //[GenericRequired(ErrorMessage ="Morate izabrati Preduzimaca")]
        [Required(ErrorMessage = "Morate izabrati Preduzimaca")]
        public PreduzimacBO? Preduzimac { get; set; }
        [NotMapped]
        public IEnumerable<IFormFile>? Images { get; set; }
        [NotMapped]
        public IFormFile? Floorplan { get; set; } 
        
    }
}
