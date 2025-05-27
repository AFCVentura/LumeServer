using LumeServer.Models.Question;

namespace LumeServer.Models.Movie
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
        public ICollection<ThemeAnswerGenre> ThemeAnswerGenres { get; set; }

    }
}
