using LumeServer.Models.Movie;

namespace LumeServer.Models.Question
{
    public class ExtraAnswerSpokenLanguage
    {
        public ExtraAnswer ExtraAnswer { get; set; }
        public int ExtraAnswerId { get; set; }
        public SpokenLanguage SpokenLanguage { get; set; }
        public int SpokenLanguageId { get; set; }
    }
}
