namespace SMS.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;

        public List<QuestionOption> Options { get; set; } = new();
    }

    public class QuestionOption
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}