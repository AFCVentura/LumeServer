namespace LumeServer.Models.Question
{
    public class ThemeQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsGeneralProfile { get; set; }
        public bool IsMultipleChoice { get; set; }
        public ICollection<ThemeAnswer> ThemeAnswers { get; set; }

        public ThemeQuestion()
        {
            ThemeAnswers = new List<ThemeAnswer>();
        }
    }
}
