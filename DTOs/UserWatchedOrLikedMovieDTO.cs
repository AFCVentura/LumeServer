namespace LumeServer.DTOs
{
    public class UserWatchedOrLikedMovieDTO
    {
        public int MovieId { get; set; }
        public bool Liked { get; set; }
        public bool Watched { get; set; }
    }
}
