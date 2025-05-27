using LumeServer.Models.Movie;

namespace LumeServer.Models.Question
{
    public class ThemeAnswerKeyword
    {
        public ThemeAnswer ThemeAnswer { get; set; }
        public int ThemeAnswerId { get; set; }
        public Keyword Keyword { get; set; }
        public int KeywordId { get; set; }
    }
}
