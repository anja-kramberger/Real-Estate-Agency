using AgencijaZaNekretnine.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Debugger.Contracts.HotReload;

namespace AgencijaZaNekretnine.Models.EFRepository
{
    public class UgovorRepository:IUgovorRepository
    {
        private AgencijaNekretninaContext agencijaContext = new AgencijaNekretninaContext();
        public void Add(UgovorBO ugovor)
        {
            Ugovor ugovorModel = new Ugovor();
            ugovorModel.Id = ugovor.Id;
            ugovorModel.PunoIme = ugovor.PunoIme;
            ugovorModel.BrojTelefona = ugovor.BrojTelefona;
            ugovorModel.Jmbg = ugovor.Jmbg;
            ugovorModel.Datum = ugovor.Datum;
            ugovorModel.Iznos = (decimal)ugovor.Iznos;
            ugovorModel.IdNekretnine = ugovor.Nekretnina.Id;
            ugovorModel.IdPlacanja = ugovor.Placanje.Id;
            ugovorModel.PdfUrl = ugovor.PdfUrl;
            agencijaContext.Ugovors.Add(ugovorModel);
            agencijaContext.SaveChanges();
        }

        public IEnumerable<UgovorBO> GetAllByAgent(string id)
        {
            List<UgovorBO> ugovori = new List<UgovorBO>();
            foreach (Ugovor ugovor in agencijaContext.Ugovors.ToList())  //ToList()
            {
                UgovorBO ugovorBo = new UgovorBO();
                ugovorBo.Id = ugovor.Id;
                ugovorBo.PunoIme = ugovor.PunoIme;
                ugovorBo.BrojTelefona = ugovor.BrojTelefona;
                ugovorBo.Jmbg = ugovor.Jmbg;
                ugovorBo.Datum = ugovor.Datum;
                ugovorBo.Iznos = (float)ugovor.Iznos;
                ugovorBo.PdfUrl = ugovor.PdfUrl;
                Placanje p = agencijaContext.Placanjes.SingleOrDefault(a => a.Id == ugovor.IdPlacanja);
                ugovorBo.Placanje = new PlacanjeBO()
                {
                    Id = p.Id,
                    NacinPlacanja = p.NacinPlacanja
                };
                AspNetUser a = agencijaContext.AspNetUsers.SingleOrDefault(a => a.Id == id);
                /*ugovorBo.Nekretnina.Agent = new AspNetUser()
                {
                    Id = a.Id,
                    LastName = a.LastName,
                    FirstName = a.FirstName,
                    UserName = a.UserName
                };*/
                //Nekretnina n = agencijaContext.Nekretninas.SingleOrDefault(b => b.Id == ugovor.IdNekretnine);
                Nekretnina n = agencijaContext.Nekretninas.SingleOrDefault(b => b.Id == ugovor.IdNekretnine);
                ugovorBo.Nekretnina = new NekretninaBO()
                {
                    Id = n.Id,
                    Adresa = n.Adresa,
                    Agent = a,
                    Kvadratura = (float)n.Kvadratura
                };
                Tip t = agencijaContext.Tips.SingleOrDefault(a => a.Id == n.IdTip);
                ugovorBo.Nekretnina.Tip = new TipBO()
                {
                    Id = t.Id,
                    Naziv = t.NazivTipa
                };
                if(n.IdUser == id)
                {
                    ugovori.Add(ugovorBo);
                }
                
            }
            return ugovori;
        }

        public IEnumerable<UgovorBO> GetAll()
        {
            List<UgovorBO> ugovori = new List<UgovorBO>();
            foreach (Ugovor ugovor in agencijaContext.Ugovors.ToList())//ToList()
            {
                UgovorBO ugovorBo = new UgovorBO();
                ugovorBo.Id = ugovor.Id;
                ugovorBo.PunoIme = ugovor.PunoIme;
                ugovorBo.BrojTelefona = ugovor.BrojTelefona;
                ugovorBo.Jmbg = ugovor.Jmbg;
                ugovorBo.Datum = ugovor.Datum;
                ugovorBo.Iznos = (float)ugovor.Iznos;
                Placanje p = agencijaContext.Placanjes.SingleOrDefault(a => a.Id == ugovor.IdPlacanja);
                ugovorBo.Placanje = new PlacanjeBO()
                {
                    Id = p.Id,
                    NacinPlacanja = p.NacinPlacanja
                };
                //Nekretnina n = agencijaContext.Nekretninas.SingleOrDefault(b => b.Id == ugovor.IdNekretnine);
                Nekretnina n = agencijaContext.Nekretninas.SingleOrDefault(b => b.Id == ugovor.IdNekretnine);
                ugovorBo.Nekretnina = new NekretninaBO()
                {
                    Id = n.Id,
                    Adresa = n.Adresa,
                };
                Tip t = agencijaContext.Tips.SingleOrDefault(a => a.Id == n.IdTip);
                ugovorBo.Nekretnina.Tip = new TipBO()
                {
                    Id = t.Id,
                    Naziv = t.NazivTipa,
                };
                AspNetUser a = agencijaContext.AspNetUsers.SingleOrDefault(a => a.Id == n.IdUser);
                ugovorBo.Nekretnina.Agent = new AspNetUser()
                {
                    Id = a.Id,
                    LastName = a.LastName,
                    FirstName = a.FirstName,
                    UserName = a.UserName
                };
                ugovori.Add(ugovorBo);
            }
            return ugovori;
        }

        

         public IEnumerable<NekretninaBO> GetAllNekretnine(string id)
         {
            List<NekretninaBO> nekretnine = new List<NekretninaBO>();
            foreach (Nekretnina nekretnina in agencijaContext.Nekretninas.Where(s => s.IdStatus == 1 && s.IdUser == id).ToList())
            {
                NekretninaBO nekretninaBo = new NekretninaBO();
                nekretninaBo.Id = nekretnina.Id;
                nekretninaBo.Adresa = nekretnina.Adresa;
                nekretninaBo.Cena = (float)nekretnina.Cena;
                Tip t = agencijaContext.Tips.SingleOrDefault(a => a.Id == nekretnina.IdTip);
                nekretninaBo.Tip = new TipBO()
                {
                    Id = t.Id,
                    Naziv = t.NazivTipa
                };
                Status s = agencijaContext.Statuses.SingleOrDefault(a => a.Id == nekretnina.IdStatus);
                nekretninaBo.Status = new StatusBO()
                {
                    Id = s.Id,
                    Naziv = s.NazivStatusa
                };
                AspNetUser a = agencijaContext.AspNetUsers.SingleOrDefault(a => a.Id == id);
                nekretninaBo.Agent = new AspNetUser()
                {
                    Id = a.Id,
                    LastName = a.LastName,
                    FirstName = a.FirstName,
                    UserName = a.UserName
                };
                nekretnine.Add(nekretninaBo);
            }
            return nekretnine;
         }

        public IEnumerable<PlacanjeBO> GetAllPlacanja()
        {
            List<PlacanjeBO> placanja = new List<PlacanjeBO>();
            foreach (Placanje placanje in agencijaContext.Placanjes)
            {
                PlacanjeBO placanjeBo = new PlacanjeBO();
                placanjeBo.Id = placanje.Id;
                placanjeBo.NacinPlacanja = placanje.NacinPlacanja;
                placanja.Add(placanjeBo);
            }
            return placanja;
        }

        public UgovorBO GetByID(int id)
        {
            Ugovor ugovor = agencijaContext.Ugovors.SingleOrDefault(n => n.Id == id);
            UgovorBO ugovorBo = new UgovorBO();
            ugovorBo.Id = ugovor.Id;
            ugovorBo.PunoIme = ugovor.PunoIme;
            ugovorBo.BrojTelefona = ugovor.BrojTelefona;
            ugovorBo.Jmbg = ugovor.Jmbg;
            ugovorBo.Datum = ugovor.Datum;
            ugovorBo.Iznos = (float)ugovor.Iznos;
            Placanje p = agencijaContext.Placanjes.SingleOrDefault(a => a.Id == ugovor.IdPlacanja);
            ugovorBo.Placanje = new PlacanjeBO()
            {
                Id = p.Id,
                NacinPlacanja = p.NacinPlacanja
            };
            Nekretnina n = agencijaContext.Nekretninas.SingleOrDefault(b => b.Id == ugovor.IdNekretnine);
            ugovorBo.Nekretnina = new NekretninaBO()
            {
                Id = n.Id,
                Adresa = n.Adresa
            };
            
            return ugovorBo;
        }
        public IEnumerable<UgovorBO> GetByNekretninaID(int nekretninaId)
        {
            List<UgovorBO> ugovori = new List<UgovorBO>();
            foreach (Ugovor ugovor in agencijaContext.Ugovors.Where(s => s.IdNekretnine == nekretninaId).ToList())
            {
                UgovorBO ugovorBo = new UgovorBO();
                ugovorBo.Id = ugovor.Id;
                ugovorBo.PunoIme = ugovor.PunoIme;
                ugovorBo.BrojTelefona = ugovor.BrojTelefona;
                ugovorBo.Jmbg = ugovor.Jmbg;
                ugovorBo.Datum = ugovor.Datum;
                ugovorBo.Iznos = (float)ugovor.Iznos;
                Placanje p = agencijaContext.Placanjes.SingleOrDefault(a => a.Id == ugovor.IdPlacanja);
                ugovorBo.Placanje = new PlacanjeBO()
                {
                    Id = p.Id,
                    NacinPlacanja = p.NacinPlacanja
                };
                Nekretnina n = agencijaContext.Nekretninas.SingleOrDefault(b => b.Id == ugovor.IdNekretnine);
                ugovorBo.Nekretnina = new NekretninaBO()
                {
                    Id = n.Id,
                    Adresa = n.Adresa
                };
                ugovori.Add(ugovorBo);
            }
            return ugovori;
        }
        public void Update(UgovorBO ugovorBO)
        {
            Ugovor ugovor = agencijaContext.Ugovors.FirstOrDefault(t => t.Id == ugovorBO.Id);
            if (ugovor == null) return;
            ugovor.PdfUrl = ugovorBO.PdfUrl;
            agencijaContext.SaveChanges();
        }
        public void UpdateNekretnina(NekretninaBO nekretnina)
        {
            Nekretnina nekretninaModel = agencijaContext.Nekretninas.FirstOrDefault(t => t.Id == nekretnina.Id);
            if (nekretninaModel == null) return;
            nekretninaModel.IdStatus = 2;
            agencijaContext.SaveChanges();
        }

        public string getPlacanjeById(int id)
        {
            Placanje placanje = agencijaContext.Placanjes.SingleOrDefault(n => n.Id == id);
            PlacanjeBO placanjeBo = new PlacanjeBO();
            placanjeBo.Id = placanje.Id;
            placanjeBo.NacinPlacanja = placanje.NacinPlacanja;
            return placanjeBo.NacinPlacanja;
        }
        public NekretninaBO getNekretninaById(int id)
        {
            Nekretnina nekretnina = agencijaContext.Nekretninas.SingleOrDefault(n => n.Id == id);
            NekretninaBO nekretninaBo = new NekretninaBO();
            nekretninaBo.Id = nekretnina.Id;
            nekretninaBo.Cena = (float)nekretnina.Cena;
            nekretninaBo.Kvadratura = (float)nekretnina.Kvadratura;
            nekretninaBo.Adresa = nekretnina.Adresa;
            Tip tip = agencijaContext.Tips.SingleOrDefault(t => t.Id == nekretnina.IdTip);
            nekretninaBo.Tip = new TipBO()
            {
                Id = tip.Id,
                Naziv = tip.NazivTipa
            };
            Status status = agencijaContext.Statuses.SingleOrDefault(s => s.Id == nekretnina.IdStatus);
            nekretninaBo.Status = new StatusBO()
            {
                Id = status.Id,
                Naziv = status.NazivStatusa
            };
            return nekretninaBo;
        }
    }
}
