namespace TestAPI.Models
{
    public class StudentExam
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int ExamId { get; set; }
        public Exam? Exam { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsSubmitted { get; set; } = false;
        public DateTime? SubmittedAt { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
