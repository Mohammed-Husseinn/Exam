using ExaminationSystem;

const int subjectId = 1;
const string subjectName = "Object-Oriented Programming";
const int choiceCount = 4;

var subject = new Subject(subjectId, subjectName);

var examTypeValue = ReadInt(
    "Enter the type of exam (1 for Practical, 2 for Final):",
    minimum: 1,
    maximum: 2);
var examType = examTypeValue == 1
    ? ExamType.Practical
    : ExamType.Final;

var timeInMinutes = ReadInt(
    "Please enter the time for the exam (30 to 180 minutes):",
    minimum: 30,
    maximum: 180);
var numberOfQuestions = ReadInt(
    "Please enter the number of questions:",
    minimum: 1,
    maximum: 100);

var questions = new List<Question>(numberOfQuestions);
for (var questionNumber = 1; questionNumber <= numberOfQuestions; questionNumber++)
{
    questions.Add(ReadQuestion(questionNumber, examType));
}

var exam = subject.CreateExam(examType, timeInMinutes, questions.ToArray());

if (!ReadYesNo("Do You Want To Start Exam (Y | N)"))
{
    return;
}

Console.WriteLine();
exam.Start();
AnswerExam(exam);
exam.Finish();
exam.ShowExam();

Question ReadQuestion(int questionNumber, ExamType type)
{
    var header = $"Question {questionNumber}";
    var body = ReadText("Please enter the question body:");
    var mark = ReadDouble("Please enter the question mark:");

    if (type == ExamType.Practical)
    {
        return ReadMultipleChoiceQuestion(header, body, mark, choiceCount);
    }

    var questionType = ReadInt(
        "Enter the type of question (1 for True/False, 2 for MCQ):",
        minimum: 1,
        maximum: 2);

    return questionType == 1
        ? ReadTrueFalseQuestion(header, body, mark)
        : ReadMultipleChoiceQuestion(header, body, mark, choiceCount);
}

MCQQuestion ReadMultipleChoiceQuestion(
    string header,
    string body,
    double mark,
    int numberOfChoices)
{
    Console.WriteLine("Choices of Question:");
    var answers = new Answer[numberOfChoices];

    for (var answerId = 1; answerId <= numberOfChoices; answerId++)
    {
        var answerText = ReadText($"Please enter choice number {answerId}:");
        answers[answerId - 1] = new Answer(answerId, answerText);
    }

    var rightAnswerId = ReadInt(
        $"Please enter the ID of the correct answer (1 to {numberOfChoices}):",
        minimum: 1,
        maximum: numberOfChoices);

    return new MCQQuestion(header, body, mark, answers, rightAnswerId);
}

TrueFalseQuestion ReadTrueFalseQuestion(string header, string body, double mark)
{
    Console.WriteLine("Choices of Question:");
    Console.WriteLine("1. True");
    Console.WriteLine("2. False");

    var rightAnswerId = ReadInt(
        "Please enter the ID of the correct answer (1 for True, 2 for False):",
        minimum: 1,
        maximum: 2);

    return new TrueFalseQuestion(header, body, mark, rightAnswerId == 1);
}

void AnswerExam(Exam currentExam)
{
    Console.WriteLine("Exam started.");

    for (var index = 0; index < currentExam.Questions.Count; index++)
    {
        var question = currentExam.Questions[index];
        Console.WriteLine();
        Console.WriteLine($"Question {index + 1}: {question.Body}");

        foreach (var answer in question.AnswerList)
        {
            Console.WriteLine(answer);
        }

        var answerId = ReadInt(
            $"Please enter your answer (1 to {question.AnswerList.Length}):",
            minimum: 1,
            maximum: question.AnswerList.Length);
        question.SelectAnswer(answerId);
    }
}

string ReadText(string prompt)
{
    while (true)
    {
        Console.WriteLine(prompt);
        var value = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(value))
        {
            return value.Trim();
        }

        Console.WriteLine("Input cannot be empty.");
    }
}

int ReadInt(string prompt, int minimum, int maximum)
{
    while (true)
    {
        Console.WriteLine(prompt);
        var input = Console.ReadLine();

        if (int.TryParse(input, out var value) && value >= minimum && value <= maximum)
        {
            return value;
        }

        Console.WriteLine($"Please enter a number between {minimum} and {maximum}.");
    }
}

double ReadDouble(string prompt)
{
    while (true)
    {
        Console.WriteLine(prompt);
        var input = Console.ReadLine();

        if (double.TryParse(input, out var value) && value > 0)
        {
            return value;
        }

        Console.WriteLine("Please enter a positive number.");
    }
}

bool ReadYesNo(string prompt)
{
    while (true)
    {
        Console.WriteLine(prompt);
        var input = Console.ReadLine();

        if (string.Equals(input, "Y", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (string.Equals(input, "N", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        Console.WriteLine("Please enter Y or N.");
    }
}
