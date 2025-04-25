namespace LumeServer.Models.Movie
{
    public class MovieSpokenLanguage
    {
        public string MovieId { get; set; }
        public string SpokenLanguageId { get; set; }

        public Movie Movie { get; set; }
        public SpokenLanguage SpokenLanguage { get; set; }
    }
}
