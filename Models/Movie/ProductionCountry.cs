namespace LumeServer.Models.Movie
{
    public class ProductionCountry
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MovieProductionCountry> MovieProductionCountries { get; set; }
    }

}
