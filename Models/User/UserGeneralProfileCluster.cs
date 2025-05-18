using LumeServer.Models.Movie;

namespace LumeServer.Models.User
{
    public class UserGeneralProfileCluster
    {
        public string UserId { get; set; }
        public int ClusterId { get; set; }
        public User User { get; set; }
        public Cluster Cluster { get; set; }
    }
}
