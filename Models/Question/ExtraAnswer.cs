namespace LumeServer.Models.Question
{
    public class ExtraAnswer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public float MinVoteAverage { get; set; }
        public float MaxVoteAverage { get; set; }
        public int MinVoteCount { get; set; }
        public int MaxVoteCount { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public ExtraQuestion Question { get; set; }
        public int QuestionId { get; set; }
        public ICollection<ExtraAnswerProductionCountry> ExtraAnswerProductionCountries { get; set; }
        public ICollection<ExtraAnswerSpokenLanguage> ExtraAnswerSpokenLanguages { get; set; }

        public ExtraAnswer()
        {
            ExtraAnswerProductionCountries = new List<ExtraAnswerProductionCountry>();
            ExtraAnswerSpokenLanguages = new List<ExtraAnswerSpokenLanguage>();
        }
    }
}
