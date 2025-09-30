namespace TestAPI.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
