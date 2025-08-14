namespace AgencijaZaNekretnine.Models.Interfaces
{
    public interface INekretninaRepository
    {
        IEnumerable<NekretninaBO> GetAll();
        string VratiSliku(NekretninaBO nekretnina);
        NekretninaBO GetById(int idNekretnine);
        int GetIdByAddress(string adresa);
        void AddSlike(SlikeNekretnineBO slikaBO);
        IEnumerable<TipBO> GetAllTipovi();
        IEnumerable<StatusBO> GetAllStatusi();
        IEnumerable<PreduzimacBO> GetAllPreduzimaci();
        IEnumerable<SlikeNekretnineBO> GetAllSlike(int IdNekr);
        void Add(NekretninaBO nekretnina);
        void DeleteImage(int id);
        void Delete(NekretninaBO nekretnina);
        void Update(NekretninaBO nekretnina);
    }
}
