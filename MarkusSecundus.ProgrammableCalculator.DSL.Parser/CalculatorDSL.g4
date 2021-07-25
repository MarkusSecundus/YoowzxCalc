grammar CalculatorDSL;
options
{
	language=CSharp;
}


unit: function_definition EOF #upcast_function_definition_to_unit
	| expression EOF #anonymous_function_definition
	;

function_definition: IDENTIFIER bracketed_args_list ASSIGN expression ;

bracketed_args_list: LPAR args_list RPAR #bracketed_args_list_fillled
	| LPAR RPAR #bracketed_args_list_empty
	;

args_list: IDENTIFIER #args_list_create
	| args_list COMMA IDENTIFIER #args_list_add
	;




literal : IDENTIFIER #identifier_expr
	| function_call #functioncall_to_literal_cast_expr
	| NUMBER #constant_expr
	| LPAR expression RPAR #expr_in_parentheses
	;

expression : add_expression #upcast_add_expr
	;

pow_expression : literal	#upcast_literal_expr
	| literal POW pow_expression #exponent_expr
	| PLUS pow_expression #unary_plus_expr
	| MINUS pow_expression #unary_minus_expr
	;


mult_expression : pow_expression	#upcast_pow_expr
	| mult_expression STAR pow_expression #mult_expr
	| mult_expression DIV pow_expression #div_expr
	| mult_expression MOD pow_expression #mod_expr
	;


add_expression : mult_expression	#upcast_mult_expr
	| add_expression PLUS mult_expression #add_expr
	| add_expression MINUS mult_expression #subtract_expr
	;


function_call: IDENTIFIER bracketed_invoke_list #functioncall_expr
	;

bracketed_invoke_list: LPAR invoke_list RPAR #bracketed_invoke_list_filled
	| LPAR RPAR #bracketed_invoke_list_empty
	;

invoke_list: expression #invoke_list_create
	| invoke_list COMMA expression  #invoke_list_add
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
ASSIGN : ':=' | '=' ;


NUMBER : DIGIT+ ('.' DIGIT*)? (('E' | 'e') '-'? DIGIT+)?;

IDENTIFIER : LETTER (LETTER | DIGIT)* ;

WHITESPACE : ( '\u0000' .. ' ' )+ -> skip;
