using LumeServer.Models.Movie;

namespace LumeServer.Models.User
{
    public class UserDailyProfileProductionCountry
    {
        public int UserDailyProfileId { get; set; }
        public int ProductionCountryId { get; set; }
        public UserDailyProfile UserDailyProfile { get; set; }
        public ProductionCountry ProductionCountry { get; set; }
    }
}
