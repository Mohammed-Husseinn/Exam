namespace ExaminationSystem;

public sealed class TrueFalseQuestion : Question
{
    public TrueFalseQuestion(
        string header,
        string body,
        double mark,
        bool rightAnswer)
        : base(header, body, mark, CreateAnswers(), rightAnswer ? 1 : 2)
    {
    }

    public override Question Clone()
    {
        var clone = new TrueFalseQuestion(Header, Body, Mark, RightAnswer.AnswerId == 1);
        CopySelectionTo(clone);
        return clone;
    }

    private static Answer[] CreateAnswers() =>
    [
        new Answer(1, "True"),
        new Answer(2, "False")
    ];
}
