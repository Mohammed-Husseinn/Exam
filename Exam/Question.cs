namespace ExaminationSystem;

public abstract class Question : ICloneable, IComparable<Question>, IComparable
{
    private readonly Answer[] _answerList;
    private Answer? _selectedAnswer;

    protected Question(
        string header,
        string body,
        double mark,
        IEnumerable<Answer> answerList,
        int rightAnswerId)
    {
        ArgumentNullException.ThrowIfNull(header);
        ArgumentNullException.ThrowIfNull(body);
        ArgumentNullException.ThrowIfNull(answerList);

        if (mark < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(mark), "Mark cannot be negative.");
        }

        var answers = answerList.ToArray();
        if (answers.Length == 0)
        {
            throw new ArgumentException("A question must have at least one answer.", nameof(answerList));
        }

        if (answers.Any(answer => answer is null))
        {
            throw new ArgumentException("The answer list cannot contain null answers.", nameof(answerList));
        }

        if (answers.Select(answer => answer.AnswerId).Distinct().Count() != answers.Length)
        {
            throw new ArgumentException("Answer IDs must be unique.", nameof(answerList));
        }

        Header = header;
        Body = body;
        Mark = mark;
        _answerList = answers.Select(answer => (Answer)answer.Clone()).ToArray();
        RightAnswer = _answerList.SingleOrDefault(answer => answer.AnswerId == rightAnswerId)
            ?? throw new ArgumentException("The right answer must be in the answer list.", nameof(rightAnswerId));
    }

    protected Question(
        string header,
        string body,
        double mark,
        Answer[] answerList,
        Answer? rightAnswer)
        : this(
            header,
            body,
            mark,
            answerList,
            rightAnswer?.AnswerId ?? throw new ArgumentNullException(nameof(rightAnswer)))
    {
    }

    public string Header { get; }

    public string Body { get; }

    public double Mark { get; }

    public Answer[] AnswerList => _answerList.Select(answer => (Answer)answer.Clone()).ToArray();

    public Answer[] Answers => AnswerList;

    public Answer RightAnswer { get; }

    public Answer CorrectAnswer => RightAnswer;

    public Answer? SelectedAnswer => _selectedAnswer is null
        ? null
        : (Answer)_selectedAnswer.Clone();

    public bool IsAnswered => _selectedAnswer is not null;

    public bool IsCorrect => _selectedAnswer?.AnswerId == RightAnswer.AnswerId;

    public void SelectAnswer(int answerId)
    {
        _selectedAnswer = _answerList.SingleOrDefault(answer => answer.AnswerId == answerId)
            ?? throw new ArgumentException($"Answer ID {answerId} does not exist.", nameof(answerId));
    }

    public void SelectAnswer(Answer answer)
    {
        ArgumentNullException.ThrowIfNull(answer);
        SelectAnswer(answer.AnswerId);
    }

    public void ClearAnswer() => _selectedAnswer = null;

    public abstract Question Clone();

    object ICloneable.Clone() => Clone();

    public int CompareTo(Question? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        var markComparison = Mark.CompareTo(other.Mark);
        return markComparison != 0
            ? markComparison
            : string.Compare(Header, other.Header, StringComparison.Ordinal);
    }

    int IComparable.CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is not Question question)
        {
            throw new ArgumentException("Object must be a Question.", nameof(obj));
        }

        return CompareTo(question);
    }

    public override string ToString() => $"{Header}: {Body} (Mark: {Mark})";

    protected void CopySelectionTo(Question clone)
    {
        ArgumentNullException.ThrowIfNull(clone);

        if (_selectedAnswer is not null)
        {
            clone.SelectAnswer(_selectedAnswer.AnswerId);
        }
    }
}
