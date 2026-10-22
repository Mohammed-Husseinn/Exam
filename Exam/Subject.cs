namespace ExaminationSystem;

public sealed class Subject : ICloneable, IComparable<Subject>, IComparable
{
    public Subject() : this(0, string.Empty)
    {
    }

    public Subject(int subjectId, string subjectName)
    {
        ArgumentNullException.ThrowIfNull(subjectName);

        SubjectId = subjectId;
        SubjectName = subjectName;
    }

    public int SubjectId { get; }

    public string SubjectName { get; }

    public Exam? Exam { get; private set; }

    public Exam CreateExam(
        ExamType examType,
        TimeSpan time,
        params Question[] questions)
    {
        Exam = examType switch
        {
            ExamType.Final => new FinalExam(this, time, questions),
            ExamType.Practical => new PracticalExam(this, time, questions),
            _ => throw new ArgumentOutOfRangeException(nameof(examType), examType, "Unsupported exam type.")
        };

        return Exam;
    }

    public Exam CreateExam(
        ExamType examType,
        int timeInMinutes,
        params Question[] questions) =>
        CreateExam(examType, TimeSpan.FromMinutes(timeInMinutes), questions);

    public object Clone()
    {
        var clone = new Subject(SubjectId, SubjectName);

        if (Exam is not null)
        {
            clone.CreateExam(
                Exam.ExamType,
                Exam.Time,
                Exam.Questions.Select(question => question.Clone()).ToArray());

            if (Exam.IsFinished)
            {
                clone.Exam!.Finish();
            }
        }

        return clone;
    }

    public int CompareTo(Subject? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        var idComparison = SubjectId.CompareTo(other.SubjectId);
        return idComparison != 0
            ? idComparison
            : string.Compare(SubjectName, other.SubjectName, StringComparison.Ordinal);
    }

    int IComparable.CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is not Subject subject)
        {
            throw new ArgumentException("Object must be a Subject.", nameof(obj));
        }

        return CompareTo(subject);
    }

    public override string ToString() =>
        $"Subject {SubjectId}: {SubjectName} (Exam: {Exam?.ExamType.ToString() ?? "None"})";
}
