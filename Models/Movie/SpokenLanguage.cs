using LumeServer.Models.Question;
using LumeServer.Models.User;

namespace LumeServer.Models.Movie
{
    public class SpokenLanguage
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ExtraAnswerSpokenLanguage> ExtraAnswerSpokenLanguages { get; set; }
        public ICollection<MovieSpokenLanguage> MovieSpokenLanguages { get; set; }
        public ICollection<UserDailyProfileSpokenLanguage> UserDailyProfileSpokenLanguages { get; set; }
        public ICollection<UserGeneralProfileSpokenLanguage> UserGeneralProfileSpokenLanguages { get; set; }

    }
}
