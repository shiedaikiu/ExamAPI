namespace TestAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public int? GroupId { get; set; }
        public Group? Group { get; set; }
        public ICollection<StudentExam> StudentExams { get; set; } = new List<StudentExam>();
    }
}
