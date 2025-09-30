namespace TestAPI.DTOs
{
        public record OptionDto(int Id, string Text);
        public record QuestionDto(int Id, string Text, List<OptionDto> Options);


        public record ExamViewDto(int ExamId, string Title, DateTime StartDateTime, DateTime EndDateTime, List<QuestionDto> Questions);
    
}
