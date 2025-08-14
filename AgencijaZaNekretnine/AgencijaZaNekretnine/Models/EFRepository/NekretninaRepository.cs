using AgencijaZaNekretnine.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgencijaZaNekretnine.Models.EFRepository
{
    public class NekretninaRepository : INekretninaRepository
    {
        private AgencijaNekretninaContext agencijaContext = new AgencijaNekretninaContext();
        public void Add(NekretninaBO nekretnina)
        {
            Nekretnina nekretninaModel = new Nekretnina();
            nekretninaModel.Id = nekretnina.Id;
            nekretninaModel.Adresa = nekretnina.Adresa;
            nekretninaModel.Cena = (decimal)nekretnina.Cena;
            nekretninaModel.Kvadratura = nekretnina.Kvadratura;
            nekretninaModel.BrojSoba = nekretnina.BrojSoba;
            nekretninaModel.IdUser = nekretnina.Agent.Id;
            nekretninaModel.IdTip = nekretnina.Tip.Id;
            nekretninaModel.IdPreduzimac = nekretnina.Preduzimac.IDPreduzimaca;
            nekretninaModel.IdStatus = 1;
            nekretninaModel.Opis = nekretnina.Opis;
            nekretninaModel.Slika = nekretnina.Slika;
            agencijaContext.Nekretninas.Add(nekretninaModel);
            agencijaContext.SaveChanges();
        }

        public void AddSlike(SlikeNekretnineBO slikaBO)
        {
            SlikeNekretnine slika = new SlikeNekretnine();
            slika.Id = slikaBO.Id;
            slika.IdNekretnine = slikaBO.Nekretnina.Id;
            slika.UrlSlike = slikaBO.UrlSlike;
            agencijaContext.SlikeNekretnines.Add(slika);
            agencijaContext.SaveChanges();
        }

        public void Delete(NekretninaBO nekretnina)
        {
            Nekretnina nekretninaModel = agencijaContext.Nekretninas.FirstOrDefault(t => t.Id == nekretnina.Id);
            agencijaContext.Nekretninas.Remove(nekretninaModel);
            agencijaContext.SaveChanges();
        }

        public void DeleteImage(int id)
        {
            SlikeNekretnine slika = agencijaContext.SlikeNekretnines.FirstOrDefault(t => t.Id == id);
            agencijaContext.SlikeNekretnines.Remove(slika);
            agencijaContext.SaveChanges();
        }
        public string VratiSliku(NekretninaBO nekretnina)
        {
            Nekretnina nekretninaModel = agencijaContext.Nekretninas.FirstOrDefault(t => t.Id == nekretnina.Id);
            string slika = nekretninaModel.Slika;
            return slika;
        }

        public IEnumerable<NekretninaBO> GetAll()
        {
            List<NekretninaBO> nekretnine = new List<NekretninaBO>();
            foreach (Nekretnina nekretnina in agencijaContext.Nekretninas.ToList())
            {
                NekretninaBO nekretninaBo = new NekretninaBO();
                nekretninaBo.Id = nekretnina.Id;
                nekretninaBo.Cena = (float)nekretnina.Cena;
                nekretninaBo.Adresa = nekretnina.Adresa;
                nekretninaBo.Kvadratura = (float)nekretnina.Kvadratura;
                nekretninaBo.BrojSoba = (float)nekretnina.BrojSoba;
                nekretninaBo.Opis = nekretnina.Opis;
                nekretninaBo.Slika = nekretnina.Slika;
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
                AspNetUser agent = agencijaContext.AspNetUsers.SingleOrDefault(t => t.Id == nekretnina.IdUser);
                nekretninaBo.Agent = new AspNetUser()
                {
                    Id = agent.Id,
                    FirstName = agent.FirstName,
                    LastName = agent.LastName,
                    UserName = agent.UserName,
                    Discriminator = agent.Discriminator
                };
                Preduzimac preduzimac = agencijaContext.Preduzimacs.SingleOrDefault(t => t.Id == nekretnina.IdPreduzimac);
                nekretninaBo.Preduzimac = new PreduzimacBO()
                {
                    IDPreduzimaca = preduzimac.Id,
                    Naziv = preduzimac.Naziv
                };
                nekretnine.Add(nekretninaBo);
            }
            return nekretnine;
        }

        public IEnumerable<StatusBO> GetAllStatusi()
        {
            List<StatusBO> statusi = new List<StatusBO>();
            foreach (Status status in agencijaContext.Statuses)
            {
                StatusBO statusBo = new StatusBO();
                statusBo.Id = status.Id;
                statusBo.Naziv = status.NazivStatusa;
                statusi.Add(statusBo);
            }
            return statusi;
        }

        public IEnumerable<SlikeNekretnineBO> GetAllSlike(int IdNekr)  // (int idnekr)
        {
            List<SlikeNekretnineBO> slike = new List<SlikeNekretnineBO>();
            foreach (SlikeNekretnine slika in agencijaContext.SlikeNekretnines.Where(s => s.IdNekretnine == IdNekr).ToList())
            //foreach (SlikeNekretnine slika in agencijaContext.SlikeNekretnines)
            {
                SlikeNekretnineBO slikeBo = new SlikeNekretnineBO();
                slikeBo.Id = slika.Id;
                Nekretnina n = agencijaContext.Nekretninas.SingleOrDefault(b => b.Id == slika.IdNekretnine);
                slikeBo.Nekretnina = new NekretninaBO()
                {
                    Id = n.Id,
                    Adresa = n.Adresa,

                };
                slikeBo.UrlSlike = slika.UrlSlike;
                slike.Add(slikeBo);
            }
            return slike;
        }

        public IEnumerable<TipBO> GetAllTipovi()
        {
            List<TipBO> tipovi = new List<TipBO>();
            foreach (Tip tip in agencijaContext.Tips)
            {
                TipBO tipBo = new TipBO();
                tipBo.Id = tip.Id;
                tipBo.Naziv = tip.NazivTipa;
                tipovi.Add(tipBo);
            }
            return tipovi;
        }
        
        public IEnumerable<PreduzimacBO> GetAllPreduzimaci()
        {
            List<PreduzimacBO> preduzimaci = new List<PreduzimacBO>();
            foreach (Preduzimac preduzimac in agencijaContext.Preduzimacs.Include(p => p.Nekretninas))
            {
                PreduzimacBO preduzimacBO = new PreduzimacBO();
                preduzimacBO.IDPreduzimaca = preduzimac.Id;
                preduzimacBO.Naziv = preduzimac.Naziv;
                preduzimacBO.BrojNekretnina = preduzimac.BrojNekretnina;
                preduzimacBO.Count = preduzimac.Nekretninas.Select(n => n.IdPreduzimac == preduzimacBO.IDPreduzimaca).Count();
                if(preduzimacBO.Count < preduzimacBO.BrojNekretnina)
                {
                    preduzimaci.Add(preduzimacBO);
                }
                
            }
            return preduzimaci;
        }

        public NekretninaBO GetById(int idNekretnine)
        {
            Nekretnina nekretnina = agencijaContext.Nekretninas.SingleOrDefault(n => n.Id == idNekretnine);
            NekretninaBO nekretninaBo = new NekretninaBO();
            nekretninaBo.Id = nekretnina.Id;
            nekretninaBo.Cena = (float)nekretnina.Cena;
            nekretninaBo.Adresa = nekretnina.Adresa;
            nekretninaBo.Opis = nekretnina.Opis;
            nekretninaBo.Slika = nekretnina.Slika;
            nekretninaBo.Kvadratura = (float)nekretnina.Kvadratura;
            nekretninaBo.BrojSoba = (float)nekretnina.BrojSoba;
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
            AspNetUser agent = agencijaContext.AspNetUsers.SingleOrDefault(t => t.Id == nekretnina.IdUser);
            nekretninaBo.Agent = new AspNetUser()
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
            };
            Preduzimac preduzimac = agencijaContext.Preduzimacs.SingleOrDefault(t => t.Id == nekretnina.IdPreduzimac);
            nekretninaBo.Preduzimac = new PreduzimacBO()
            {
                IDPreduzimaca = preduzimac.Id,
                Naziv = preduzimac.Naziv
            };
            
            return nekretninaBo;
        }

        public int GetIdByAddress(string adresa)
        {
            Nekretnina nekretnina = agencijaContext.Nekretninas.FirstOrDefault(t => t.Adresa == adresa);
            nekretnina.Adresa = adresa;
            int id = nekretnina.Id;
            
            return id;
        }

        

        public void Update(NekretninaBO nekretninaBO)
        {
            Nekretnina nekretninaModel = agencijaContext.Nekretninas.FirstOrDefault(t => t.Id == nekretninaBO.Id);
            if (nekretninaModel == null) return;//
            nekretninaModel.Cena = (decimal)nekretninaBO.Cena;
            nekretninaModel.Adresa = nekretninaBO.Adresa;
            nekretninaModel.Kvadratura = (float)nekretninaBO.Kvadratura;
            nekretninaModel.BrojSoba = (float)nekretninaBO.BrojSoba;
            nekretninaModel.IdTip = nekretninaBO.Tip.Id;
            //nekretninaModel.Idstatus = nekretninaBO.Status.Id;
            nekretninaModel.Opis = nekretninaBO.Opis;
            nekretninaModel.Slika = nekretninaBO.Slika;
            nekretninaModel.IdPreduzimac = nekretninaBO.Preduzimac.IDPreduzimaca;
            nekretninaModel.IdUser = nekretninaBO.Agent.Id;
            agencijaContext.SaveChanges();
        }
    }
}
