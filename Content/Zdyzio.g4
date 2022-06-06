grammar Zdyzio;

/*
 * Parser rules
 */

program: (functionDeclaration | statementList)+ EOF;

functionDeclaration: FUNC IDENTIFIER LEFT_PARENTHESIS parameterList? RIGHT_PARENTHESIS COLON functionType block;

functionType: type | VOID;

parameterList: IDENTIFIER COLON type (COMMA IDENTIFIER COLON type)*;

block: LEFT_CURLY_BRACKET statementList RIGHT_CURLY_BRACKET;

statementList: statement+;

statement:
    assignment AT
	| IF LEFT_PARENTHESIS (logicExpression | comparationExpression) RIGHT_PARENTHESIS block (ELSE block)?
	| WHILE LEFT_PARENTHESIS (logicExpression | comparationExpression) RIGHT_PARENTHESIS block
	| RETURN expression? AT
	| BREAK AT
	| variableDeclaration AT
	| constantDeclaration AT
	| functionCall AT;

assignment: IDENTIFIER ASSIGN_OPERATOR expression;

expression:
	| arithmeticExpression
	| logicExpression
	| comparationExpression
	| primary
	| functionCall;
	
arithmeticExpression:
    primary arithmeticOperator primary;
    
primary:
	literal
	| IDENTIFIER;
    
literal:
    NULL_LITERAL
	| INT_LITERAL
	| FLOAT_LITERAL
	| CHAR_LITERAL
	| TRUE_LITERAL
	| FALSE_LITERAL
	| STRING_LITERAL;
	
arithmeticOperator:
    MULTIPLICATION_OPERATOR 
    | DIVISION_OPERATOR 
    | MODULO_OPERATOR 
    | ADDITION_OPERATOR 
    | SUBTRACTION_OPERATOR 
    | EXPONENTIATION_OPERATOR;

logicExpression:
    LEFT_PARENTHESIS logicExpression RIGHT_PARENTHESIS
    | NEGATION_OPERATOR logicExpression
    | comparationExpression logicOperator comparationExpression
    | primary logicOperator primary;
    
comparationExpression:
    NEGATION_OPERATOR comparationExpression
    | primary comparationOperator primary;

logicOperator:
    AND_OPERATOR 
    | OR_OPERATOR
    | NEGATION_OPERATOR;
    
comparationOperator:
    LESS_THAN_OR_EQUAL_OPERATOR 
    | GRATER_THAN_OR_EQUAL_OPERATOR 
    | GRATER_THAN_OPERATOR 
    | LESS_THAN_OPERATOR 
    | EQUAL_OPERATOR 
    | NOT_EQUAL_OPERATOR;

functionCall: IDENTIFIER LEFT_PARENTHESIS argumentList? RIGHT_PARENTHESIS;

argumentList: (IDENTIFIER | literal) (COMMA (IDENTIFIER | literal))*;

variableDeclaration: VAR IDENTIFIER COLON type (ASSIGN_OPERATOR expression)?;

constantDeclaration: CONST IDENTIFIER COLON type ASSIGN_OPERATOR expression;

type:
    BOOLEAN
	| INT
	| FLOAT
	| CHAR
	| STRING;

/*
 * Lexer rules
 */

COLON: ':';
VAR: 'var';
CONST: 'const';
FUNC: 'func';
LEFT_PARENTHESIS: '(';
RIGHT_PARENTHESIS: ')';
LEFT_CURLY_BRACKET: '{';
RIGHT_CURLY_BRACKET: '}';
AT: '@';
COMMA: ',';
DOT: '.';
ASSIGN_OPERATOR: '=';
GRATER_THAN_OPERATOR: '>';
LESS_THAN_OPERATOR: '<';
NEGATION_OPERATOR: '!';
EQUAL_OPERATOR: '==';
LESS_THAN_OR_EQUAL_OPERATOR: '<=';
GRATER_THAN_OR_EQUAL_OPERATOR: '>=';
NOT_EQUAL_OPERATOR: '!=';
AND_OPERATOR: '&&';
OR_OPERATOR: '||';
ADDITION_OPERATOR: '+';
SUBTRACTION_OPERATOR: '-';
MULTIPLICATION_OPERATOR: '*';
MODULO_OPERATOR: '%';
DIVISION_OPERATOR: '/';
EXPONENTIATION_OPERATOR: '^';
BOOLEAN: 'boolean';
INT: 'int';
FLOAT: 'float';
VOID: 'void';
CHAR: 'char';
STRING: 'string';
ELSE: 'else';
IF: 'if';
WHILE: 'while';
BREAK: 'break';
RETURN: 'return';
TRUE_LITERAL: 'true';
FALSE_LITERAL: 'false';
NULL_LITERAL: 'null';
INT_LITERAL : [0-9]+;
FLOAT_LITERAL : [0-9]+'.'[0-9]+;
CHAR_LITERAL: '\'' (~['\\\r\n]) '\'';
IDENTIFIER: [a-zA-Z][a-zA-Z0-9_]*;
STRING_LITERAL: '"' (~["\\\r\n])* '"';
WHITESPACE: (' ' | '\t' | '\n') -> skip ;