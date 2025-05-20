using LumeServer.Models.Question;
using LumeServer.Models.User;

namespace LumeServer.Models.Movie
{
    public class ProductionCountry
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MovieProductionCountry> MovieProductionCountries { get; set; }
        public ICollection<ExtraAnswerProductionCountry> ExtraAnswerProductionCountries { get; set; }
        public ICollection<UserDailyProfileProductionCountry> UserDailyProfileProductionCountries { get; set; }
        public ICollection<UserGeneralProfileProductionCountry> UserGeneralProfileProductionCountries { get; set; }

    }

}
