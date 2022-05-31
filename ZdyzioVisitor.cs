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

    public override string VisitStatement(ZdyzioParser.StatementContext context)
    {
        //TODO: if else
        //TODO: while
        
        if (context.assignment() is not null)
            return VisitAssignment(context.assignment());

        if (context.RETURN() is not null)
        {
            var expression = context.expression();
            if (expression is null)
                return "return\n";
            return $"return {VisitExpression(expression)}\n";
        }
        
        if (context.BREAK() is not null)
            return "break";
        
        if (context.variableDeclaration() is not null)
            return VisitVariableDeclaration(context.variableDeclaration());

        if (context.constantDeclaration() is not null)
            return VisitConstantDeclaration(context.constantDeclaration());

        if (context.functionCall() is not null)
            return VisitFunctionCall(context.functionCall());

        return "";
    }

    public override string VisitConstantDeclaration(ZdyzioParser.ConstantDeclarationContext context)
    {
        if(context.expression() is not null)
            return $"{context.IDENTIFIER().GetText()} = {VisitExpression(context.expression())}\n";
        
        return $"{context.IDENTIFIER().GetText()} = {VisitFunctionCall(context.functionCall())}\n";
    }

    public override string VisitVariableDeclaration(ZdyzioParser.VariableDeclarationContext context)
    {
        if (context.ASSIGN_OPERATOR() is not null)
        {
            if(context.expression() is not null)
                return $"{context.IDENTIFIER().GetText()} = {VisitExpression(context.expression())}\n";
        
            return $"{context.IDENTIFIER().GetText()} = {VisitFunctionCall(context.functionCall())}\n";
        }
        if(context.type().STRING() is not null)
            return $"{context.IDENTIFIER().GetText()} = \"\"\n";
        
        if(context.type().INT() is not null)
            return $"{context.IDENTIFIER().GetText()} = 0\n";
        
        if(context.type().FLOAT() is not null)
            return $"{context.IDENTIFIER().GetText()} = 0.0\n";
        
        if(context.type().CHAR() is not null)
            return $"{context.IDENTIFIER().GetText()} = \'\'\n";
        
        return $"{context.IDENTIFIER().GetText()} = False\n";
    }
}