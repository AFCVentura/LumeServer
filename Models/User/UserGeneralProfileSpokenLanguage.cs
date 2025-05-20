using LumeServer.Models.Movie;

namespace LumeServer.Models.User
{
    public class UserGeneralProfileSpokenLanguage
    {
        public string UserId { get; set; }
        public int SpokenLanguageId { get; set; }
        public User User { get; set; }
        public SpokenLanguage SpokenLanguage { get; set; }
    }
}
