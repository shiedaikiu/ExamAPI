using TestAPI.Models;

namespace TestAPI.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Groups.Any())
                return; // Already seeded

            // Groups
            var group1 = new Group { Name = "Group A" };
            var group2 = new Group { Name = "Group B" };
            context.Groups.AddRange(group1, group2);
            context.SaveChanges();

            // Students
            var student1 = new Student { FullName = "Alice", GroupId = group1.Id };
            var student2 = new Student { FullName = "Bob", GroupId = group1.Id };
            var student3 = new Student { FullName = "Charlie", GroupId = group2.Id };
            var student4 = new Student { FullName = "Diana", GroupId = group2.Id };
            context.Students.AddRange(student1, student2, student3, student4);
            context.SaveChanges();

            // Exams
            var exam1 = new Exam
            {
                Title = "Math Test",
                GroupId = group1.Id,
                StartDateTime = DateTime.UtcNow.AddMinutes(-30),
                EndDateTime = DateTime.UtcNow.AddHours(1)
            };

            var exam2 = new Exam
            {
                Title = "Science Test",
                GroupId = group2.Id,
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow.AddHours(2)
            };

            context.Exams.AddRange(exam1, exam2);
            context.SaveChanges();

            // Questions & Options for exam1
            var q1 = new Question { ExamId = exam1.Id, Text = "2 + 2 = ?" };
            var q2 = new Question { ExamId = exam1.Id, Text = "5 * 3 = ?" };
            var q3 = new Question { ExamId = exam2.Id, Text = "What is the capital of France?" };
            var q4 = new Question { ExamId = exam2.Id, Text = "Which planet is known as the Red Planet?" };
            context.Questions.AddRange(q1, q2, q3,q4);
            context.SaveChanges();

            var optionsQ1 = new List<Option>
        {
            new Option { QuestionId = q1.Id, Text = "3", IsCorrect = false },
            new Option { QuestionId = q1.Id, Text = "4", IsCorrect = true },
            new Option { QuestionId = q1.Id, Text = "5", IsCorrect = false },
            new Option { QuestionId = q1.Id, Text = "6", IsCorrect = false },
        };

            var optionsQ2 = new List<Option>
        {
            new Option { QuestionId = q2.Id, Text = "8", IsCorrect = false },
            new Option { QuestionId = q2.Id, Text = "15", IsCorrect = true },
            new Option { QuestionId = q2.Id, Text = "10", IsCorrect = false },
            new Option { QuestionId = q2.Id, Text = "12", IsCorrect = false },
        };
            var optionsQ3 = new List<Option>
        {
            new Option { QuestionId = q3.Id, Text = "Berlin", IsCorrect = false },
            new Option { QuestionId = q3.Id, Text = "Madrid", IsCorrect = false },
            new Option { QuestionId = q3.Id, Text = "Paris", IsCorrect = true },
            new Option { QuestionId = q3.Id, Text = "Rome", IsCorrect = false },
        };
            var optionsQ4 = new List<Option>
        {
            new Option { QuestionId = q4.Id, Text = "Venus", IsCorrect = false },
            new Option { QuestionId = q4.Id, Text = "Mars", IsCorrect = true },
            new Option { QuestionId = q4.Id, Text = "Jupiter", IsCorrect = false },
            new Option { QuestionId = q4.Id, Text = "Saturn", IsCorrect = false },
        };
            context.Options.AddRange(optionsQ1);
            context.Options.AddRange(optionsQ2);
            context.Options.AddRange(optionsQ3);
            context.Options.AddRange(optionsQ4);
            context.SaveChanges();

            // --- Completed exam for Alice ---
            var studentExam1 = new StudentExam
            {
                StudentId = student1.Id,
                ExamId = exam1.Id,
                CreatedAt = DateTime.UtcNow,
                IsSubmitted = true,
                SubmittedAt = DateTime.UtcNow
            };
            context.StudentExams.Add(studentExam1);
            context.SaveChanges();

            context.StudentAnswers.AddRange(new List<StudentAnswer>
        {
            new StudentAnswer { StudentExamId = studentExam1.Id, QuestionId = q1.Id, SelectedOptionId = optionsQ1[1].Id },
            new StudentAnswer { StudentExamId = studentExam1.Id, QuestionId = q2.Id, SelectedOptionId = optionsQ2[1].Id }
        });
            context.SaveChanges();

            // --- Incomplete exam for Bob (one question inside time, one question late) ---
            var studentExam2 = new StudentExam
            {
                StudentId = student2.Id,
                ExamId = exam1.Id,
                CreatedAt = DateTime.UtcNow,
                IsSubmitted = true,
                SubmittedAt = DateTime.UtcNow
            };
            context.StudentExams.Add(studentExam2);
            context.SaveChanges();

            context.StudentAnswers.AddRange(new List<StudentAnswer>
        {
            new StudentAnswer { StudentExamId = studentExam2.Id, QuestionId = q1.Id, SelectedOptionId = optionsQ1[0].Id, AnsweredAt = DateTime.UtcNow.AddMinutes(-10) }, // inside time
            new StudentAnswer { StudentExamId = studentExam2.Id, QuestionId = q2.Id, SelectedOptionId = optionsQ2[2].Id, AnsweredAt = DateTime.UtcNow.AddHours(2) } // outside time
        });
            context.SaveChanges();

            // --- Exam not taken for Charlie and Diana ---
            // Do nothing for student3 and student4 (they haven't taken exam2 yet)
        }
    }

}
