namespace ExaminationSystem;

public sealed class MCQQuestion : Question
{
    public MCQQuestion(
        string header,
        string body,
        double mark,
        Answer[] answerList,
        int rightAnswerId)
        : base(header, body, mark, answerList, rightAnswerId)
    {
    }

    public MCQQuestion(
        string header,
        string body,
        double mark,
        int rightAnswerId,
        params Answer[] answerList)
        : this(header, body, mark, answerList, rightAnswerId)
    {
    }

    public override Question Clone()
    {
        var clone = new MCQQuestion(Header, Body, Mark, AnswerList, RightAnswer.AnswerId);
        CopySelectionTo(clone);
        return clone;
    }
}
