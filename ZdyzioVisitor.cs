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
        if (context.assignment() is not null)
            return VisitAssignment(context.assignment());
        //TODO: if else
        //TODO: while
        if (context.WHILE() is not null)
        {
            Func<object?, bool> condition = context.WHILE().GetText() == "while"
                ? IsTrue
                : IsFalse
            ;

            if(context.LEFT_PARENTHESIS() is not null)
            {
                bool checker = false;
                if(condition(VisitLogicExpression(context.logicExpression()))
                    || condition(VisitComparationExpression(context.comparationExpression())))
                {
                    checker = true;
                }
                if(context.RIGHT_PARENTHESIS() is not null && checker)
                {
                    do
                    {
                        // tu nie wiem co zrobic
                        VisitBlock(context.block());
                    }while(condition(VisitLogicExpression(context.logicExpression())) 
                    || condition(VisitComparationExpression(context.comparationExpression())));
                }
            }
        }

        if(context.IF() is not null)
        {
            Func<object?, bool> condition = context.IF().GetText() == "if"
                ? IsTrue
                : IsFalse
            ;
            //tu chyba juz całkowicie pomieszałem
            if(context.LEFT_PARENTHESIS() is not null)
            {
                bool checker = false;

                if(condition(VisitLogicExpression(context.logicExpression()))
                    || condition(VisitComparationExpression(context.comparationExpression())))
                {
                    checker = true;
                }

                if(context.RIGHT_PARENTHESIS() is not null && checker)
                {
                    VisitBlock(context.block());
                }
                else if(context.RIGHT_PARENTHESIS() is not null && context.ELSE() is not null && !checker)
                {
                    VisitBlock(context.block());
                }
            }
        }

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
    private bool IsTrue(object? value)
    {
        if(value is bool b)
        {
            return b;
        }
        throw new Exception("Value is not boolean");
    }

    private bool IsFalse(object? value) => !IsTrue(value); 

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
    
    public override string VisitArithmeticExpression(ZdyzioParser.ArithmeticExpressionContext context)
    {
        Operators opToReturn = new Operators();
        var left = Visit(context.primary(0));
        var right = Visit(context.primary(1));

        var op = context.arithmeticOperator().GetText();

        return op switch
        {
            "*" => opToReturn.Multiply(left, right),
            "/" => opToReturn.Divide(left, right),
            "%" => opToReturn.Modulo(left, right),
            "+" => opToReturn.Add(left, right),
            "-" => opToReturn.Substract(left, right),
            "^" => opToReturn.Exponent(left, right),
            _ => throw new NotImplementedException()
        };
    }

    public override string VisitComparationExpression(ZdyzioParser.ComparationExpressionContext context)
    {
        Comparators comToReturn = new Comparators();

        var left = Visit(context.primary(0));
        var right = Visit(context.primary(1));

        var com = context.comparationOperator().GetText();

        return com switch
        {
            "<=" => comToReturn.LessThanOrEqual(left, right),
            ">=" => comToReturn.GreaterThanOrEqual(left, right),
            ">" => comToReturn.GreaterThan(left, right),
            "<" => comToReturn.LessThan(left, right),
            "==" => comToReturn.Equal(left, right),
            "!=" => comToReturn.NotEqual(left, right),
            _ => throw new NotImplementedException()
        };
    }
}

