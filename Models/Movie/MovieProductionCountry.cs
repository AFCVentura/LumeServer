namespace LumeServer.Models.Movie
{
    public class MovieProductionCountry
    {
        public string MovieId { get; set; }
        public string ProductionCountryId { get; set; }

        public Movie Movie { get; set; }
        public ProductionCountry ProductionCountry { get; set; }
    }
}
