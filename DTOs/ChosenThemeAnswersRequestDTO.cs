namespace LumeServer.DTOs
{
    public class ChosenThemeAnswersRequestDTO
    {
        public List<int> ThemeAnswerIds { get; set; }
        public List<int> ChosenMovieIds { get; set; }
        public string Id { get; set; }
    }
}
