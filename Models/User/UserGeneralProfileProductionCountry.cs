using LumeServer.Models.Movie;

namespace LumeServer.Models.User
{
    public class UserGeneralProfileProductionCountry
    {
        public string UserId { get; set; }
        public int ProductionCountryId { get; set; }

        public User User { get; set; }
        public ProductionCountry ProductionCountry { get; set; }
    }
}
