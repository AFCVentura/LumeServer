using LumeServer.Models.Question.Enum;

namespace LumeServer.Models.Question
{
    public class ExtraQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsGeneralProfile { get; set; }
        public bool IsMultipleChoice { get; set; }
        public ExtraQuestionTypes QuestionType { get; set; }
        public ICollection<ExtraAnswer> Answers { get; set; }

        public ExtraQuestion()
        {
            Answers = new List<ExtraAnswer>();
        }
    }
}
