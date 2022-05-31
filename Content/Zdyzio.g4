grammar Zdyzio;

/*
 * Parser rules
 */

program: (functionDeclaration | statementList)+ EOF;

functionDeclaration: accessModifier functionType IDENTIFIER LEFT_PARENTHESIS parameterList? RIGHT_PARENTHESIS block;

accessModifier: PRIVATE | PUBLIC;

functionType: type | VOID;

parameterList: type IDENTIFIER (COMMA type IDENTIFIER)*;

block: LEFT_CURLY_BRACKET statementList RIGHT_CURLY_BRACKET;

statementList: statement+;

statement:
    assignment AT
	| IF LEFT_PARENTHESIS logicExpression RIGHT_PARENTHESIS block (ELSE block)?
	| WHILE LEFT_PARENTHESIS logicExpression RIGHT_PARENTHESIS block
	| RETURN expression? AT
	| BREAK AT
	| variableDeclaration AT
	| functionCall AT;

assignment: IDENTIFIER ASSIGN_OPERATOR expression;

expression:
	| arithmeticExpression
	| logicExpression
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
    primary logicOperator primary
    | NEGATION_OPERATOR LEFT_PARENTHESIS primary logicOperator primary RIGHT_PARENTHESIS;

logicOperator:
    LESS_THAN_OR_EQUAL_OPERATOR 
    | GRATER_THAN_OR_EQUAL_OPERATOR 
    | GRATER_THAN_OPERATOR 
    | LESS_THAN_OPERATOR 
    | EQUAL_OPERATOR 
    | NOT_EQUAL_OPERATOR 
    | AND_OPERATOR 
    | OR_OPERATOR;

functionCall: IDENTIFIER LEFT_PARENTHESIS argumentList? RIGHT_PARENTHESIS;

argumentList: (IDENTIFIER | literal) (COMMA (IDENTIFIER | literal))*;

variableDeclaration: type IDENTIFIER (ASSIGN_OPERATOR (expression | functionCall))?;

type:
    BOOLEAN
	| INT
	| FLOAT
	| CHAR
	| STRING;

/*
 * Lexer rules
 */

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
PRIVATE: 'private';
PUBLIC: 'public';
TRUE_LITERAL: 'true';
FALSE_LITERAL: 'false';
NULL_LITERAL: 'null';
INT_LITERAL : [0-9]+;
FLOAT_LITERAL : [0-9]+'.'[0-9]+;
CHAR_LITERAL: '\'' (~['\\\r\n]) '\'';
IDENTIFIER: [a-zA-Z][a-zA-Z0-9_]*;
STRING_LITERAL: '"' (~["\\\r\n])* '"';
WHITESPACE: (' ' | '\t' | '\n') -> skip ;