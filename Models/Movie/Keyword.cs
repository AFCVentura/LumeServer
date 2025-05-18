using LumeServer.Models.Question;

namespace LumeServer.Models.Movie
{
    public class Keyword
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MovieKeyword> MovieKeywords { get; set; }
        public ICollection<ThemeAnswerKeyword> ThemeAnswerKeywords { get; set; }
    }
}
