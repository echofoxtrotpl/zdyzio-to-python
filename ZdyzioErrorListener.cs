using Antlr4.Runtime;

namespace ZdyzioToPython;

public class ZdyzioErrorListener : BaseErrorListener
{
    public static List<string> ErrorMessages { get; set; } = new ();
    public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
        RecognitionException e)
    {
        ErrorMessages.Add($"line:{line} position:{charPositionInLine} - {msg}\n");
    }
}