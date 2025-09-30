using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Models;

namespace TestAPI.Pages
{
    public class ExamModel : PageModel
    {
        private readonly AppDbContext _db;
        public ExamModel(AppDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public int ExamId { get; set; }
        [BindProperty]
        public int StudentId { get; set; }
        [BindProperty]
        public List<AnswerInput> Answers { get; set; } = new();
public Exam? Exam { get; set; }
        public List<Question> Questions { get; set; } = new();
        public async Task OnGetAsync(int examId, int studentId)
        {
            ExamId = examId;
            StudentId = studentId;
            Exam = await _db.Exams.Include(e => e.Questions)
            .FirstOrDefaultAsync(e => e.Id == examId);
            if (Exam != null)
            {
                Questions = (List<Question>)Exam.Questions;
            }
        }
        public async Task<IActionResult> OnPostAsync(int examId, int studentId)
        {
            var studentTest = new StudentExam
            {
                StudentId = studentId,
                ExamId = examId,
                SubmittedAt = DateTime.UtcNow,
                StudentAnswers = Answers.Select(a => new StudentAnswer
                {
                    QuestionId = a.QuestionId,
                    SelectedOption = a.SelectedOption
                }).ToList()
            };
            _db.StudentExams.Add(studentTest);
            await _db.SaveChangesAsync();
            return RedirectToPage("/ExamResult", new { examId, studentId });
        }
        public class AnswerInput
        {
            public int QuestionId { get; set; }
            public Option? SelectedOption { get; set; }
        }
    }
}
