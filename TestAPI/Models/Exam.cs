namespace TestAPI.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int GroupId { get; set; }
        public Group? Group { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
