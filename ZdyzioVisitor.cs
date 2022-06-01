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
            {
                result.Append(VisitFunctionDeclaration(context.functionDeclaration(processedFunctions)));
                processedFunctions++;
            }
            else
            {
                result.Append(VisitStatementList(context.statementList(processedStatements)));
                processedStatements++;
            }
        }

        result.Append(PytonTokens.NEWLINE);
        return result.ToString();
    }

    public override string VisitFunctionDeclaration(ZdyzioParser.FunctionDeclarationContext context)
    {
        var parameters = context.parameterList() is not null ? VisitParameterList(context.parameterList()) : "";
        return $"def {context.IDENTIFIER().GetText()}({parameters}):\n{VisitBlock(context.block())}\n";
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

    public override string VisitArgumentList(ZdyzioParser.ArgumentListContext context)
    {
        //TODO: 
        return "";
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
        if (context.assignment() is not null)
            return VisitAssignment(context.assignment());

        if (context.WHILE() is not null)
        {
            //TODO: while
            return "";
        }
        
        if (context.IF() is not null)
        {
            //TODO: if else
            return "";
        }

        if (context.RETURN() is not null)
        {
            var expression = context.expression();
            if (expression is null)
                return "return\n";
            return $"return {VisitExpression(expression)}\n";
        }
        
        if (context.BREAK() is not null)
            return "break\n";
        
        if (context.variableDeclaration() is not null)
            return VisitVariableDeclaration(context.variableDeclaration());

        if (context.constantDeclaration() is not null)
            return VisitConstantDeclaration(context.constantDeclaration());

        if (context.functionCall() is not null)
            return VisitFunctionCall(context.functionCall());

        return "";
    }

    public override string VisitAssignment(ZdyzioParser.AssignmentContext context)
    {
        return $"{context.IDENTIFIER().GetText()} = {VisitExpression(context.expression())}\n";
    }

    public override string VisitConstantDeclaration(ZdyzioParser.ConstantDeclarationContext context)
    {
        return $"{context.IDENTIFIER().GetText()} = {VisitExpression(context.expression())}\n";
    }

    public override string VisitVariableDeclaration(ZdyzioParser.VariableDeclarationContext context)
    {
        if (context.ASSIGN_OPERATOR() is not null)
        {
            return $"{context.IDENTIFIER().GetText()} = {VisitExpression(context.expression())}\n";
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
    
    public override string VisitArithmeticExpression(ZdyzioParser.ArithmeticExpressionContext context)
    {
        return $"{VisitPrimary(context.primary(0))} {VisitArithmeticOperator(context.arithmeticOperator())} {VisitPrimary(context.primary(1))}";
    }

    public override string VisitArithmeticOperator(ZdyzioParser.ArithmeticOperatorContext context)
    {
        if (context.MULTIPLICATION_OPERATOR() is not null)
            return context.MULTIPLICATION_OPERATOR().GetText();
        if (context.DIVISION_OPERATOR() is not null)
            return context.DIVISION_OPERATOR().GetText();
        if (context.MODULO_OPERATOR() is not null)
            return context.MODULO_OPERATOR().GetText();
        if (context.ADDITION_OPERATOR() is not null)
            return context.ADDITION_OPERATOR().GetText();
        if (context.SUBTRACTION_OPERATOR() is not null)
            return context.SUBTRACTION_OPERATOR().GetText();
        
        return "**";
    }

    public override string VisitType(ZdyzioParser.TypeContext context)
    {
        return "";
    }

    public override string VisitPrimary(ZdyzioParser.PrimaryContext context)
    {
        if (context.literal() is not null)
            return VisitLiteral(context.literal());

        return context.IDENTIFIER().GetText();
    }

    public override string VisitLiteral(ZdyzioParser.LiteralContext context)
    {
        if (context.NULL_LITERAL() is not null)
            return context.NULL_LITERAL().GetText();
        if (context.INT_LITERAL() is not null)
            return context.INT_LITERAL().GetText();
        if (context.FLOAT_LITERAL() is not null)
            return context.FLOAT_LITERAL().GetText();
        if (context.CHAR_LITERAL() is not null)
            return context.CHAR_LITERAL().GetText();
        if (context.TRUE_LITERAL() is not null)
            return context.TRUE_LITERAL().GetText();
        if (context.FALSE_LITERAL() is not null)
            return context.FALSE_LITERAL().GetText();
        return context.STRING_LITERAL().GetText();
    }

    public override string VisitExpression(ZdyzioParser.ExpressionContext context)
    {
        if (context.arithmeticExpression() is not null)
            return VisitArithmeticExpression(context.arithmeticExpression());
        
        if(context.logicExpression() is not null)
            return VisitLogicExpression(context.logicExpression());
        
        if(context.comparationExpression() is not null)
            return VisitComparationExpression(context.comparationExpression());

        if(context.functionCall() is not null)
            return VisitFunctionCall(context.functionCall());

        return VisitPrimary(context.primary());
    }

    public override string VisitFunctionCall(ZdyzioParser.FunctionCallContext context)
    {
        //var arguments = context.argumentList() is not null ? VisitArgumentList(context.argumentList()) : "";
        //TODO: Function call
        return "";
    }
}

