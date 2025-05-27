namespace LumeServer.Models.Question
{
    public class ThemeAnswer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ThemeQuestion ThemeQuestion { get; set; }
        public int ThemeQuestionId { get; set; }
        public ICollection<ThemeAnswerGenre> ThemeAnswerGenres { get; set; }
        public ICollection<ThemeAnswerKeyword> ThemeAnswerKeywords { get; set; }

        public ThemeAnswer()
        {
            ThemeAnswerGenres = new List<ThemeAnswerGenre>();
            ThemeAnswerKeywords = new List<ThemeAnswerKeyword>();
        }
    }
}
