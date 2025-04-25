namespace LumeServer.Models.Movie
{
    public class MovieProductionCompany
    {
        public string MovieId { get; set; }
        public int ProductionCompanyId { get; set; }

        public Movie Movie { get; set; }
        public ProductionCompany ProductionCompany { get; set; }
    }
}
