using ZdyzioToPython.Content;

namespace ZdyzioToPython;

public class ZdyzioVisitor: ZdyzioBaseVisitor<object?>
{
    private Dictionary<string, object?> Variables = new();
    public override object? VisitAssignment(ZdyzioParser.AssignmentContext context)
    {
        var varName = context.IDENTIFIER().GetText();

        var value = Visit(context.expression());

        Variables[varName] = value;

        return value;
    }
}