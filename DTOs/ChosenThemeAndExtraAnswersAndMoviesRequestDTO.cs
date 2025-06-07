namespace LumeServer.DTOs
{
    public class ChosenThemeAndExtraAnswersAndMoviesRequestDTO
    {
        public List<int> ThemeAnswerIds { get; set; }
        public List<int> ChosenMovieIds { get; set; }
        public List<int> ExtraAnswerIds { get; set; }
    }
}
