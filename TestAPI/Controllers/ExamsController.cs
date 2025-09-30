using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTOs;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ExamsController(AppDbContext db) => _db = db;
        [HttpGet("{examId}/student/{studentId}")]
        public async Task<IActionResult> GetExamForStudent(int examId, int studentId)
        {
            var exam = await _db.Exams
                .Include(e => e.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(e => e.Id == examId);
            if (exam == null) return NotFound();

            var alreadySubmitted = await _db.StudentExams.AnyAsync(se => se.ExamId == examId && se.StudentId == studentId && se.IsSubmitted);
            if (alreadySubmitted) return BadRequest("آزمون قبلاً ثبت شده است.");

            var qdto = exam.Questions.Select(q => new QuestionDto(q.Id, q.Text, q.Options.Select(o => new OptionDto(o.Id, o.Text)).ToList())).ToList();
            var dto = new ExamViewDto(exam.Id, exam.Title, exam.StartDateTime, exam.EndDateTime, qdto);
            return Ok(dto);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitExamDto model)
        {
            var exam = await _db.Exams.FirstOrDefaultAsync(e => e.Id == model.ExamId);
            if (exam == null) return NotFound();

            var existing = await _db.StudentExams.FirstOrDefaultAsync(se => se.ExamId == model.ExamId && se.StudentId == model.StudentId);
            if (existing != null && existing.IsSubmitted) return BadRequest("قبلاً ثبت شده است.");

            if (existing == null)
            {
                existing = new StudentExam { ExamId = model.ExamId, StudentId = model.StudentId };
                _db.StudentExams.Add(existing);
                await _db.SaveChangesAsync();
            }

            // ثبت پاسخ‌ها با زمان سرور
            foreach (var a in model.Answers)
            {
                var sa = new StudentAnswer
                {
                    StudentExamId = existing.Id,
                    QuestionId = a.QuestionId,
                    SelectedOptionId = a.SelectedOptionId,
                    AnsweredAt = DateTime.UtcNow
                };
                _db.StudentAnswers.Add(sa);
            }

            existing.IsSubmitted = true;
            existing.SubmittedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok(new { Message = "ثبت موفقیت‌آمیز" });
        }


    }
}
