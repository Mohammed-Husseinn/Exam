namespace ExaminationSystem;

public sealed class Answer : ICloneable, IComparable<Answer>, IComparable
{
    public Answer() : this(0, string.Empty)
    {
    }

    public Answer(int answerId, string answerText)
    {
        ArgumentNullException.ThrowIfNull(answerText);

        AnswerId = answerId;
        AnswerText = answerText;
    }

    public int AnswerId { get; }

    public string AnswerText { get; }

    public object Clone() => new Answer(AnswerId, AnswerText);

    public int CompareTo(Answer? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        var idComparison = AnswerId.CompareTo(other.AnswerId);
        return idComparison != 0
            ? idComparison
            : string.Compare(AnswerText, other.AnswerText, StringComparison.Ordinal);
    }

    int IComparable.CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is not Answer answer)
        {
            throw new ArgumentException("Object must be an Answer.", nameof(obj));
        }

        return CompareTo(answer);
    }

    public override string ToString() => $"{AnswerId}. {AnswerText}";
}
