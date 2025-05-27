namespace LumeServer.Models.User
{
    public class WatchedList
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int MovieId { get; set; }
        public Movie.Movie Movie { get; set; } // Classe movie na pasta movie
    }
}
