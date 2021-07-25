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

expression : ternary_expression #upcast_ternary_expr
	;

pow_expression : literal	#upcast_literal_expr
	| literal POW pow_expression #exponent_expr
	| PLUS pow_expression #unary_plus_expr
	| MINUS pow_expression #unary_minus_expr
	| EXCLAM pow_expression #logical_not_expr
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

compare_expression : add_expression #upcast_add_expr
	| compare_expression COMPARE_LT add_expression	#lt_expr
	| compare_expression COMPARE_LE add_expression	#le_expr
	| compare_expression COMPARE_GT add_expression	#gt_expr
	| compare_expression COMPARE_GE add_expression  #ge_expr
	| compare_expression COMPARE_EQ add_expression  #eq_expr
	| compare_expression COMPARE_NE add_expression  #ne_expr
	;

and_expression : compare_expression #upcast_compare_expr
	| and_expression AND compare_expression #and_expr
	;

or_expression : and_expression #upcast_and_expr
	| or_expression OR and_expression #or_expr
	;

ternary_expression : or_expression #upcast_or_expr
	| or_expression QUESTION ternary_expression COLON ternary_expression #ternary_expr
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
ASSIGN : ':=' ;

COMPARE_LT : '<';
COMPARE_LE : '<=';
COMPARE_GT : '>';
COMPARE_GE : '>=';
COMPARE_EQ : '=' || '==';
COMPARE_NE : '!=';

EXCLAM : '!' | '¬';

AND : '&' | '∧';
OR : '|' | '∨';

QUESTION : '?';
COLON : ':' ;

NUMBER : DIGIT+ ('.' DIGIT*)? (('E' | 'e') '-'? DIGIT+)?;

IDENTIFIER : LETTER (LETTER | DIGIT)* ;

WHITESPACE : ( '\u0000' .. ' ' )+ -> skip;
