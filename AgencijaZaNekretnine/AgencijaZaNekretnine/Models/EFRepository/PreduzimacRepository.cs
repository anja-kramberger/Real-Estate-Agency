using AgencijaZaNekretnine.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgencijaZaNekretnine.Models.EFRepository
{
    public class PreduzimacRepository : IPreduzimacRepository
    {
        private AgencijaNekretninaContext agencijaContext = new AgencijaNekretninaContext();
        public void Add(PreduzimacBO preduzimacBO)
        {
            Preduzimac preduzimac = new Preduzimac();
            preduzimac.Id = preduzimacBO.IDPreduzimaca;
            preduzimac.Naziv = preduzimacBO.Naziv;
            preduzimac.Opis = preduzimacBO.Opis;
            preduzimac.BrojNekretnina = preduzimacBO.BrojNekretnina;
            agencijaContext.Preduzimacs.Add(preduzimac);
            agencijaContext.SaveChanges();
        }

        public void Delete(PreduzimacBO preduzimacBO)
        {
            Preduzimac preduzimac = agencijaContext.Preduzimacs.FirstOrDefault(p => p.Id == preduzimacBO.IDPreduzimaca);
            agencijaContext.Preduzimacs.Remove(preduzimac);
            agencijaContext.SaveChanges();

        }

        public IQueryable<PreduzimacBO> GetAll()
        {
            List<PreduzimacBO> preduzimaci = new List<PreduzimacBO>();
            foreach (Preduzimac preduzimac in agencijaContext.Preduzimacs.Include(a => a.Nekretninas))
            {
                PreduzimacBO preduzimacBO = new PreduzimacBO();
                preduzimacBO.IDPreduzimaca = preduzimac.Id;
                preduzimacBO.Naziv = preduzimac.Naziv;
                preduzimacBO.BrojNekretnina = preduzimac.BrojNekretnina;
                preduzimacBO.Opis = preduzimac.Opis;
                //var nekretnina = agencijaContext.Nekretninas.Where(n => n.IdPreduzimac == preduzimac.Id).ToList();

                //preduzimacBO.Count = preduzimac.Nekretninas.Where(n => n.IdPreduzimac == preduzimacBO.IDPreduzimaca).Count();
                preduzimacBO.Count = preduzimac.Nekretninas.Select(n => n.IdPreduzimac == preduzimacBO.IDPreduzimaca).Count();
                preduzimaci.Add(preduzimacBO);
            }
            return preduzimaci.AsQueryable();
            //return (IQueryable<PreduzimacBO>)preduzimaci;
        }
        /*public int Count(int Id)
        {
            int count = 0;
            count = agencijaContext.Nekretninas.Where(x => x.IdPreduzimac == Id).Count();
            foreach(var item in agencijaContext.Nekretninas.Where(x => x.IdPreduzimac == Id))
            {
                count++;
            }
           return count;
        }*/

        public IEnumerable<NekretninaBO> GetAllNekretnine(int ID) 
        {
            List<NekretninaBO> nekretnine = new List<NekretninaBO>();
            foreach (Nekretnina nekretnina in agencijaContext.Nekretninas.Where(n => n.IdPreduzimac == ID))
            {
                NekretninaBO nekretninaBO = new NekretninaBO();
                nekretninaBO.Adresa = nekretnina.Adresa;
                nekretninaBO.Cena = (float)nekretnina.Cena;
                Status status = agencijaContext.Statuses.SingleOrDefault(s => s.Id == nekretnina.IdStatus);
                nekretninaBO.Status = new StatusBO()
                {
                    Id = status.Id,
                    Naziv = status.NazivStatusa
                };
                nekretnine.Add(nekretninaBO);
            }
            return nekretnine;
            //return preduzimaci.AsQueryable();
        }

        public PreduzimacBO GetById(int id)
        {
            Preduzimac preduzimac = agencijaContext.Preduzimacs.SingleOrDefault(p => p.Id == id);
            PreduzimacBO preduzimacBO = new PreduzimacBO();
            preduzimacBO.IDPreduzimaca = preduzimac.Id;
            preduzimacBO.Naziv = preduzimac.Naziv;
            preduzimacBO.BrojNekretnina = preduzimac.BrojNekretnina;
            preduzimacBO.Opis = preduzimac.Opis;
            return preduzimacBO;
        }

        public void Update(PreduzimacBO preduzimacBO)
        {
            Preduzimac preduzimac = agencijaContext.Preduzimacs.FirstOrDefault(p => p.Id == preduzimacBO.IDPreduzimaca);
            if (preduzimac == null) return;
            //preduzimac.Idpreduzimaca = preduzimacBO.IDPreduzimaca;
            preduzimac.Naziv = preduzimacBO.Naziv;
            preduzimac.Opis = preduzimacBO.Opis;
            preduzimac.BrojNekretnina = preduzimacBO.BrojNekretnina;
            agencijaContext.SaveChanges();
        }
    }
}
