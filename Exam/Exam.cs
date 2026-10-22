using System.Diagnostics;

namespace ExaminationSystem;

public enum ExamType
{
    Final,
    Practical
}

public abstract class Exam : ICloneable, IComparable<Exam>, IComparable
{
    private readonly List<Question> _questions;
    private readonly Stopwatch _stopwatch = new();

    protected Exam(Subject subject, TimeSpan time, IEnumerable<Question> questions)
    {
        ArgumentNullException.ThrowIfNull(subject);
        ArgumentNullException.ThrowIfNull(questions);

        if (time <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(time), "Exam time must be greater than zero.");
        }

        var questionArray = questions.ToArray();
        if (questionArray.Length == 0)
        {
            throw new ArgumentException("An exam must have at least one question.", nameof(questions));
        }

        if (questionArray.Any(question => question is null))
        {
            throw new ArgumentException("The question list cannot contain null questions.", nameof(questions));
        }

        Subject = subject;
        Time = time;
        _questions = questionArray.Select(question => question.Clone()).ToList();
    }

    protected Exam(Subject subject, int timeInMinutes, params Question[] questions)
        : this(subject, TimeSpan.FromMinutes(timeInMinutes), questions)
    {
    }

    public Subject Subject { get; }

    public TimeSpan Time { get; }

    public TimeSpan TimeOfExam => Time;

    public int NumberOfQuestions => _questions.Count;

    public IReadOnlyList<Question> Questions => _questions.AsReadOnly();

    public bool IsFinished { get; private set; }

    public TimeSpan ElapsedTime => _stopwatch.Elapsed;

    public double Grade => _questions
        .Where(question => question.IsCorrect)
        .Sum(question => question.Mark);

    public double TotalMark => _questions.Sum(question => question.Mark);

    public abstract ExamType ExamType { get; }

    public void Start()
    {
        if (IsFinished)
        {
            throw new InvalidOperationException("A finished exam cannot be started again.");
        }

        if (!_stopwatch.IsRunning)
        {
            _stopwatch.Start();
        }
    }

    public void Finish()
    {
        if (IsFinished)
        {
            return;
        }

        if (!_stopwatch.IsRunning)
        {
            Start();
        }

        _stopwatch.Stop();
        IsFinished = true;
    }

    public abstract void ShowExam();

    public abstract Exam Clone();

    object ICloneable.Clone() => Clone();

    public int CompareTo(Exam? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        var timeComparison = Time.CompareTo(other.Time);
        return timeComparison != 0
            ? timeComparison
            : NumberOfQuestions.CompareTo(other.NumberOfQuestions);
    }

    int IComparable.CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is not Exam exam)
        {
            throw new ArgumentException("Object must be an Exam.", nameof(obj));
        }

        return CompareTo(exam);
    }

    public override string ToString() =>
        $"{ExamType} Exam - Subject: {Subject.SubjectName}, Time: {Time}, Questions: {NumberOfQuestions}";

    protected void ShowResults()
    {
        Console.WriteLine($"{ExamType} Exam Results:");

        for (var index = 0; index < _questions.Count; index++)
        {
            var question = _questions[index];
            var selectedAnswer = question.SelectedAnswer?.AnswerText ?? "Not answered";

            Console.WriteLine($"Question {index + 1}: {question.Body}");
            Console.WriteLine($"Your Answer => {selectedAnswer}");
            Console.WriteLine($"Correct Answer => {question.RightAnswer.AnswerText}");
            Console.WriteLine();
        }

        Console.WriteLine($"Your Grade is {Grade:0.##} from {TotalMark:0.##}");
        Console.WriteLine($"Time = {ElapsedTime:c}");
        Console.WriteLine("Thank you");
    }
}
