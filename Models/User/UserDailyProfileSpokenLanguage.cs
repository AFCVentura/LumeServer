using LumeServer.Models.Movie;

namespace LumeServer.Models.User
{
    public class UserDailyProfileSpokenLanguage
    {
        public int UserDailyProfileId { get; set; }
        public int SpokenLanguageId { get; set; }

        public UserDailyProfile UserDailyProfile { get; set; }
        public SpokenLanguage SpokenLanguage { get; set; }
    }
}
