namespace LumeServer.Models.Movie
{
    public class ProductionCountry
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<MovieProductionCountry> MovieProductionCountries { get; set; }
    }

}
