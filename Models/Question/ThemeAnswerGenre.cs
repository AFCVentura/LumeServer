using LumeServer.Models.Movie;

namespace LumeServer.Models.Question
{
    public class ThemeAnswerGenre
    {
        public ThemeAnswer ThemeAnswer { get; set; }
        public int ThemeAnswerId { get; set; }
        public Genre Genre { get; set; }
        public int GenreId { get; set; }
    }
}
