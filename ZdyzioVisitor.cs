using System.Text;
using ZdyzioToPython.Content;

namespace ZdyzioToPython;

public class ZdyzioVisitor: ZdyzioBaseVisitor<string>
{
    private int numberOfIndentations;
    private bool statementOfIfElse = false;
    public override string VisitProgram(ZdyzioParser.ProgramContext context)
    {
        StringBuilder result = new StringBuilder();

        var processedFunctions = 0;
        var processedStatements = 0;

        foreach (var c in context.children)
        {
            var child = c.GetText();
            if (child.StartsWith("func"))
            {
                result.Append(VisitFunctionDeclaration(context.functionDeclaration(processedFunctions)));
                processedFunctions++;
            }
            else if (!child.Equals("<EOF>"))
            {
                result.Append(VisitStatementList(context.statementList(processedStatements)));
                processedStatements++;
            }
            result.Append("\n");
        }
        return result.ToString();
    }

    public override string VisitStatementList(ZdyzioParser.StatementListContext context)
    {
        StringBuilder statements = new StringBuilder();
        foreach (var s in context.statement())
        {
            statements.Append(VisitStatement(s));
        }
        
        return statements.ToString();
    }

    public override string VisitFunctionDeclaration(ZdyzioParser.FunctionDeclarationContext context)
    {
        numberOfIndentations++;
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

    public override string VisitArgumentList(ZdyzioParser.ArgumentListContext context)
    {
        StringBuilder arguments = new StringBuilder();

        var processedIdentifiers = 0;
        var processedLiterals = 0;

        foreach (var c in context.children)
        {
            var child = c.GetText();

            if(!child.Equals(",")) {
                if (!char.IsDigit(child[0]) && !child.StartsWith("\"") && !child.StartsWith("'")) {
                    arguments.Append(context.IDENTIFIER(processedIdentifiers));
                    processedIdentifiers++;
                } else {
                    arguments.Append(VisitLiteral(context.literal(processedLiterals)));
                    processedLiterals++;
                }
                arguments.Append(", ");
            }
        }
        
        arguments.Remove(arguments.Length - 2, 2);
        return arguments.ToString();
    }

    public override string VisitBlock(ZdyzioParser.BlockContext context)
    {
        StringBuilder block = new StringBuilder();
        foreach (var s in context.statementList().statement())
        {
            string indentation = new string('\t', numberOfIndentations);
            block.Append($"{indentation}{VisitStatement(s)}");
        }
        if (statementOfIfElse)
        {
            statementOfIfElse = false;
            return block.ToString();
        }
        numberOfIndentations--;
        return block.ToString();
    }

    public override string VisitStatement(ZdyzioParser.StatementContext context)
    {
        if (context.assignment() is not null)
            return VisitAssignment(context.assignment());

        if (context.WHILE() is not null)
        {
            numberOfIndentations++;
            if (context.logicExpression() is not null)
                return $"while {VisitLogicExpression(context.logicExpression())}:\n{VisitBlock(context.block(0))}"; 
            
            if(context.primary() is not null)
                return $"while {VisitPrimary(context.primary())}:\n{VisitBlock(context.block(0))}"; 
            
            return $"while {VisitComparationExpression(context.comparationExpression())}:\n{VisitBlock(context.block(0))}";
        }

        if (context.IF() is not null)
        {
            numberOfIndentations++;
            //TODO: IF ELSE
            if (context.ELSE() is not null)
            {
                string indentation = new string('\t', numberOfIndentations-1);
                statementOfIfElse = true;

                if (context.logicExpression() is not null)
                    return $"if {VisitLogicExpression(context.logicExpression())}:\n{VisitBlock(context.block(0))}\n" +
                           $"{indentation}else:\n{VisitBlock(context.block(1))}";
                if (context.primary() is not null)
                    return $"if {VisitPrimary(context.primary())}:\n{VisitBlock(context.block(0))}" +
                           $"{indentation}else:\n{VisitBlock(context.block(1))}";
                return $"if {VisitComparationExpression(context.comparationExpression())}:\n{VisitBlock(context.block(0))}" +
                       $"{indentation}else:\n{VisitBlock(context.block(1))}";
            }
            else
            {
                //numberOfIndentations++;
                if (context.logicExpression() is not null)
                    return $"if {VisitLogicExpression(context.logicExpression())}:\n{VisitBlock(context.block(0))}";
                if (context.primary() is not null)
                    return $"if {VisitPrimary(context.primary())}:\n{VisitBlock(context.block(0))}";
                return $"if {VisitComparationExpression(context.comparationExpression())}:\n{VisitBlock(context.block(0))}";
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

    public override string VisitComparationExpression(ZdyzioParser.ComparationExpressionContext context)
    {
        var negation = context.NEGATION_OPERATOR() is null ? "" : "not ";
        return $"{negation}{VisitPrimary(context.primary(0))} {VisitComparationOperator(context.comparationOperator())} {VisitPrimary(context.primary(1))}";
    }

    public override string VisitLogicExpression(ZdyzioParser.LogicExpressionContext context)
    {
        var negation = context.NEGATION_OPERATOR() is null ? "" : "not ";
        if (context.LEFT_PARENTHESIS() is not null)
        {
            return $"{negation}{VisitComparationExpression(context.logicExpression().comparationExpression(0))} {VisitLogicOperator(context.logicExpression().logicOperator())} {VisitComparationExpression(context.logicExpression().comparationExpression(1))}";
        }
        return $"{negation}{VisitPrimary(context.primary(0))} {VisitLogicOperator(context.logicOperator())} {VisitPrimary(context.primary(1))}";
    }

    public override string VisitLogicOperator(ZdyzioParser.LogicOperatorContext context)
    {
        if (context.AND_OPERATOR() is not null)
            return "and";
        
        return "or";
    }

    public override string VisitComparationOperator(ZdyzioParser.ComparationOperatorContext context)
    {
        if (context.LESS_THAN_OR_EQUAL_OPERATOR() is not null)
            return context.LESS_THAN_OR_EQUAL_OPERATOR().GetText();
        
        if (context.GRATER_THAN_OR_EQUAL_OPERATOR() is not null)
            return context.GRATER_THAN_OR_EQUAL_OPERATOR().GetText();
        
        if (context.GRATER_THAN_OPERATOR() is not null)
            return context.GRATER_THAN_OPERATOR().GetText();
        
        if (context.LESS_THAN_OPERATOR() is not null)
            return context.LESS_THAN_OPERATOR().GetText();
        
        if (context.EQUAL_OPERATOR() is not null)
            return context.EQUAL_OPERATOR().GetText();
        
        return "!=";
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
            return "True";
        if (context.FALSE_LITERAL() is not null)
            return "False";
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
        var arguments = context.argumentList() is not null ? VisitArgumentList(context.argumentList()) : "";
        
        return $"{context.IDENTIFIER()}({arguments})\n";
    }
}
