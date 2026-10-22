namespace ExaminationSystem;

public sealed class FinalExam : Exam
{
    public FinalExam(Subject subject, TimeSpan time, params Question[] questions)
        : base(subject, time, questions)
    {
    }

    public FinalExam(Subject subject, int timeInMinutes, params Question[] questions)
        : base(subject, timeInMinutes, questions)
    {
    }

    public override ExamType ExamType => global::ExaminationSystem.ExamType.Final;

    public override void ShowExam()
    {
        ShowResults();
    }

    public override Exam Clone()
    {
        var clone = new FinalExam(Subject, Time, Questions.Select(question => question.Clone()).ToArray());
        if (IsFinished)
        {
            clone.Finish();
        }

        return clone;
    }
}
