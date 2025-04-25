namespace LumeServer.Models.Movie
{
    public class SpokenLanguage
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<MovieSpokenLanguage> MovieSpokenLanguages { get; set; }
    }
}
