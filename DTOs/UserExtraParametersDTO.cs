using LumeServer.Models.Movie;

namespace LumeServer.DTOs
{
    public class UserExtraParametersDTO
    {
        public float MinVoteAverage { get; set; }
        public float MaxVoteAverage { get; set; }
        public int MinVoteCount { get; set; }
        public int MaxVoteCount { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public int MinDuration { get; set; }
        public int MaxDuration { get; set; }
        public ICollection<int> ProductionCountryIds { get; set; }
        public ICollection<int> SpokenLanguageIds { get; set; }
    }
}
