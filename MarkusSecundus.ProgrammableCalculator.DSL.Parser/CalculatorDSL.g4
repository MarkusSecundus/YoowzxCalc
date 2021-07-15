grammar CalculatorDSL;
options
{
	language=CSharp;
}


unit: function_definition EOF;

function_definition: IDENTIFIER bracketed_args_list__opt ASSIGN expression ;

bracketed_args_list__opt: LPAR RPAR
	| LPAR args_list RPAR
	;

args_list: IDENTIFIER
	| args_list COMMA IDENTIFIER
	;




literal : IDENTIFIER
	| function_call
	| NUMBER
	| LPAR expression RPAR
	;

expression : add_expression ;

pow_expression : literal
	| literal POW pow_expression
	| PLUS pow_expression
	| MINUS pow_expression;


mult_expression : pow_expression
	| mult_expression STAR pow_expression
	| mult_expression DIV pow_expression 
	| mult_expression MOD pow_expression ;


add_expression : mult_expression
	| add_expression PLUS mult_expression
	| add_expression MINUS mult_expression ;


function_call: IDENTIFIER bracketed_invoke_list__opt ;

bracketed_invoke_list__opt: LPAR invoke_list RPAR
	| LPAR RPAR
	;

invoke_list: expression
	| expression COMMA invoke_list
	;

	


fragment DIGIT : [0-9] ;
fragment LETTER : [a-zA-Z_] ;
fragment DOT : [.] ;



PLUS : '+' ;
MINUS : '-' ;
STAR : '*' ;
DIV : '/' ;
MOD : '%' ;
POW : '^' | '**';
LPAR : '(' ;
RPAR : ')' ;
COMMA : ',' ;
ASSIGN : ':=' ;


NUMBER : DIGIT+ ('.' DIGIT*)? ;

IDENTIFIER : LETTER (LETTER | DIGIT)* ;

WHITESPACE : ( '\t' | ' ' | '\r' | '\n'| '\u000C' )+ -> skip;
