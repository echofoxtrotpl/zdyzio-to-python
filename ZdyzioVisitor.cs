using System.Text;
using ZdyzioToPython.Content;

namespace ZdyzioToPython;

public class ZdyzioVisitor: ZdyzioBaseVisitor<string>
{
    public override string VisitProgram(ZdyzioParser.ProgramContext context)
    {
        StringBuilder result = new StringBuilder();

        var processedFunctions = 0;
        var processedStatements = 0;

        for (int i = 0; i < context.ChildCount - 1; ++i)
        {
            if (context.GetChild(i).GetText().StartsWith(PytonTokens.FUNC))
                result.Append(VisitFunctionDeclaration(context.functionDeclaration(processedFunctions)));
            else
                result.Append(VisitStatementList(context.statementList(processedStatements)));
        }

        result.Append(PytonTokens.NEWLINE);
        return result.ToString();
    }

    public override string VisitFunctionDeclaration(ZdyzioParser.FunctionDeclarationContext context)
    {
        var parameters = context.parameterList() is not null ? VisitParameterList(context.parameterList()) : "";
        return $"def {context.IDENTIFIER().GetText()}({parameters}):\n{VisitBlock(context.block())}";
    }

    public override string VisitParameterList(ZdyzioParser.ParameterListContext context)
    {
        StringBuilder parameters = new StringBuilder();
        foreach (var i in context.IDENTIFIER())
        {
            parameters.Append($"{i.GetText()}, ");
        }

        parameters.Remove(parameters.Length - 2, 2);
        return parameters.ToString();
    }

    public override string VisitBlock(ZdyzioParser.BlockContext context)
    {
        StringBuilder block = new StringBuilder();
        foreach (var s in context.statementList().statement())
        {
            block.Append($"\t{VisitStatement(s)}");
        }

        return block.ToString();
    }
}