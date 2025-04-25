namespace LumeServer.Models.Movie
{
    public class MovieKeyword
    {
        public string MovieId { get; set; }
        public int KeywordId { get; set; }

        public Movie Movie { get; set; }
        public Keyword Keyword { get; set; }
    }
}
