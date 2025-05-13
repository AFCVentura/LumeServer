namespace LumeServer.Models.User
{
    public class UserDailyProfile
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public float MinVoteAverage { get; set; }
        public float MaxVoteAverage { get; set; }
        public int MinVoteCount { get; set; }
        public int MaxVoteCount { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsTheLatest { get; set; }
        public ICollection<UserDailyProfileCluster> UserDailyProfileClusters { get; set; }
    }
}
