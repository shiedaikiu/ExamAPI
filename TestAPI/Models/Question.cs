namespace TestAPI.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public Exam? Exam { get; set; }
        public string Text { get; set; } = null!;
        public ICollection<Option> Options { get; set; } = new List<Option>();
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
