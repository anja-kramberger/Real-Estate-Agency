namespace AgencijaZaNekretnine.Models.Interfaces
{
    public interface IPreduzimacRepository
    {
        public void Add(PreduzimacBO preduzimacBO);
        public void Delete(PreduzimacBO preduzimacBO);
        public void Update(PreduzimacBO preduzimacBO);
        //IEnumerable<PreduzimacBO> GetAll();
        IQueryable<PreduzimacBO> GetAll();
        public PreduzimacBO GetById(int id);
    }
}
