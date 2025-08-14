namespace AgencijaZaNekretnine.Models.Interfaces
{
    public interface IUgovorRepository
    {
        IEnumerable<UgovorBO> GetAll();
        IEnumerable<NekretninaBO> GetAllNekretnine(string id);
        IEnumerable<PlacanjeBO> GetAllPlacanja();
        IEnumerable<UgovorBO> GetByNekretninaID(int nekretninaId);
        IEnumerable<UgovorBO> GetAllByAgent(string id);
        void UpdateNekretnina(NekretninaBO nekretnina);
        UgovorBO GetByID(int id);
        void Add(UgovorBO ugovor);
        void Update(UgovorBO ugovorBO);
        string getPlacanjeById(int id);
        NekretninaBO getNekretninaById(int id);
    }
}
