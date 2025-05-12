namespace LumeServer.Models.User
{
    public class UserDailyProfile
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsTheLatest { get; set; }
        public ICollection<UserDailyProfileCluster> UserDailyProfileClusters { get; set; }
    }
}
