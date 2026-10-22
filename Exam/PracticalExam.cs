namespace ExaminationSystem;

public sealed class PracticalExam : Exam
{
    public PracticalExam(Subject subject, TimeSpan time, params Question[] questions)
        : base(subject, time, ValidateQuestions(questions))
    {
    }

    public PracticalExam(Subject subject, int timeInMinutes, params Question[] questions)
        : base(subject, timeInMinutes, ValidateQuestions(questions))
    {
    }

    public override ExamType ExamType => global::ExaminationSystem.ExamType.Practical;

    public override void ShowExam()
    {
        ShowResults();
    }

    public override Exam Clone()
    {
        var clone = new PracticalExam(Subject, Time, Questions.Select(question => question.Clone()).ToArray());
        if (IsFinished)
        {
            clone.Finish();
        }

        return clone;
    }

    private static Question[] ValidateQuestions(Question[] questions)
    {
        ArgumentNullException.ThrowIfNull(questions);

        if (questions.Any(question => question is not MCQQuestion))
        {
            throw new ArgumentException("A practical exam can contain only MCQ questions.", nameof(questions));
        }

        return questions;
    }
}
