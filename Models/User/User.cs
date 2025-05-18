using Microsoft.AspNetCore.Identity;

namespace LumeServer.Models.User
{
    // Esse é o Model do usuário, ele apenas extende a classe IdentityUser, que já tem todos os atributos necessários para um usuário.
    public class User : IdentityUser
    {
        public string? DisplayName { get; set; }
        public float MinVoteAverage { get; set; }
        public float MaxVoteAverage { get; set; }
        public int MinVoteCount { get; set; }
        public int MaxVoteCount { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }

        public ICollection<UserDailyProfile> DailyProfiles { get; set; }
        public ICollection<UserGeneralProfileCluster> GeneralProfileClusters { get; set; }
        public ICollection<WatchedList> WatchedList { get; set; }
        public ICollection<WishList> WishList { get; set; }
    }
}
