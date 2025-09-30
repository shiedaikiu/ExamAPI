using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TestAPI.Models;
using Group = TestAPI.Models.Group;

namespace TestAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Exam> Exams => Set<Exam>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Option> Options => Set<Option>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<StudentExam> StudentExams => Set<StudentExam>();
        public DbSet<StudentAnswer> StudentAnswers => Set<StudentAnswer>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentExam>()
            .HasIndex(se => new { se.StudentId, se.ExamId })
            .IsUnique();


            // Group → Exam
            modelBuilder.Entity<Group>()
            .HasMany(g => g.Exams)
            .WithOne(e => e.Group!)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Cascade);


            // Exam → Question
            modelBuilder.Entity<Exam>()
            .HasMany(e => e.Questions)
            .WithOne(q => q.Exam!)
            .HasForeignKey(q => q.ExamId)
            .OnDelete(DeleteBehavior.Cascade);


            // Question → Option
            modelBuilder.Entity<Question>()
            .HasMany(q => q.Options)
            .WithOne(o => o.Question!)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);


            // StudentExam → StudentAnswer (Cascade)
            modelBuilder.Entity<StudentExam>()
            .HasMany(se => se.StudentAnswers)
            .WithOne(sa => sa.StudentExam!)
            .HasForeignKey(sa => sa.StudentExamId)
            .OnDelete(DeleteBehavior.Cascade);


            // StudentAnswer → Question (Restrict to avoid multiple cascade paths)
            modelBuilder.Entity<StudentAnswer>()
            .HasOne(sa => sa.Question)
            .WithMany(q => q.StudentAnswers)
            .HasForeignKey(sa => sa.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);


            // StudentAnswer → Option (Restrict)
            modelBuilder.Entity<StudentAnswer>()
            .HasOne(sa => sa.SelectedOption)
            .WithMany()
            .HasForeignKey(sa => sa.SelectedOptionId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
