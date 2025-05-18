using LumeServer.Models.Movie;

namespace LumeServer.Models.User
{
    public class UserDailyProfileCluster
    {
        public int UserDailyProfileId { get; set; }
        public int ClusterId { get; set; }
        public UserDailyProfile UserDailyProfile { get; set; }
        public Cluster Cluster { get; set; }
    }
}
