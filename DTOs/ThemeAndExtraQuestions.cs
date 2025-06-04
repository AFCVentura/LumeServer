using LumeServer.Models.Question;

namespace LumeServer.DTOs
{
    public class ThemeAndExtraQuestions
    {
        public List<ThemeQuestion> ThemeQuestions { get; set; }
        public List<ExtraQuestion> ExtraQuestions { get; set; }
    }
}
