
namespace LumeServer.Models.User
{
    public class WishList
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int MovieId { get; set; }
        public Movie.Movie Movie { get; set; } // Classe movie na pasta movie
    }
}
