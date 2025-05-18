using LumeServer.Models.Movie;

namespace LumeServer.Models.Question
{
    public class ExtraAnswerProductionCountry
    {
        public ExtraAnswer ExtraAnswer { get; set; }
        public int ExtraAnswerId { get; set; }
        public ProductionCountry ProductionCountry { get; set; }
        public int ProductionCountryId { get; set; }
    }
}
