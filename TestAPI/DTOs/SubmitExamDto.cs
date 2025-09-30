namespace TestAPI.DTOs
{
    public record SubmitAnswerDto(int QuestionId, int SelectedOptionId);
    public record SubmitExamDto(int ExamId, int StudentId, List<SubmitAnswerDto> Answers);
}
