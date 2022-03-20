# ***YoowzxCalc***
 
### .NET library for processing of mathematical expressions developed with emphasis on performance and flexibility of use.
\
Parses expressions in infix notation, with support of recursion and user defined functions.  
Works by emitting CIL bytecode at runtime - capable of running natively on the VM - callable via delegate.  
Also includes a programmable calculator for terminal that serves as demo.  


&nbsp;

#### ***Contents***
  0. [First steps](#first-steps)
     - [How to build](#how-to-build)
     - [How to use](#how-to-use)
     - [Tail recursion](#tail-recursion) 
     - [Caching return values](#caching-return-values)
  1. [Performance](#performance)
  2. [Grammar](#grammar)
     - [Whitespace](#whitespace)
     - [Literals and identifiers](#literals-and-identifiers)
     - [Operators](#operators)
     - [Functioncalls](#functioncalls)
     - [Compilation unit](#compilation-unit)
       - [Annotations](#annotations)
       - [Examples](#examples)
  3. [Compilation](#compilation)
     - [How to define an operation](#how-to-define-an-operation)
       - [Recognition of constants](#recognition-of-constants)
       - [Validation of identifiers](#validation-of-identifiers)
       - [Operator definitions](#operator-definitions)
       - [Standard library](#standard-library)
       - [How to register a NumberOperator](#how-to-register-a-numberoperator)
     - [Compilation context](#compilation-context)
       - [Function signature](#function-signature)
       - [Management of definitions](#management-of-definitions)
     - [Compiler](#compiler)
  4. [Demo calculator](#demo-calculator)

&nbsp;

#### ***Credits:***
  - All the contributors to .NET and C# language
  - Creators of the [ReadLine](https://github.com/tonerdo/readline) library
  - Creators of the [CommandLineParser](https://github.com/commandlineparser/commandline) library
  - Creators of the [ANTLR](https://www.antlr.org/) parser-generator

#### ***Author***:
  - Jakub Hroník


-----------------------
&nbsp;
## ***First steps***

### ***How to build***
Open the solution in an up-to-date version of MS Visual Studio 2019 and build it. Throughout the projects, features of C# 9 are used heavily - thus .NET 5.0 is required.  

### ***How to use***

For straightforward use of the base functionality the facade [YoowzxCalculator](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc/IYoowzxCalculator.cs) is provided.  
It encompasses the whole expression processing pipeline - initial parsing of text representation into abstract syntax tree (AST), subsequent generation of executable bytecode from the AST and also management of the context containing other functions callable from the expressions. For each of these segments, the user can provide custom implementation or just let the default one be used.  

***Instance of calculator operating on type `double` can be obtained like this:***
```c#
IYoowzxCalc<double> calc = IYoowzxCalc<double>.Make();  
```
*Basic configuration supports creation of calculators operating on types `double`, `decimal` and `long`. For operating on other types, explicitly adding support by the user is required (see [`NumberOperator`](#how-to-register-a-numberoperator)).*  

***Nothing now stays in our way to compile some expressions:***
```c#
Func<double> f1 = calc.Compile<Func<double>>("1 + 1");
Console.WriteLine(f1()); //prints 3

Func<double, double> f2 = calc.Compile<Func<double, double>>("f(some_number) := some_number * (3 + 4 ** 5e-1)");
Console.WriteLine(f2(0)); //prints 0

Func<double, double> fibonacci = calc.Compile<Func<double, double>>("fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)");
for(int t=0;t<10;++t)
    Console.WriteLine(fibonacci(t));    //prints the first 10 fibonacci numbers  
```
This way the expression gets compiled, but no matter if it was given a name, it won't be implicitly added as a callable function to the context. (Though the name still has value if we want to use recursion.)  
  
***Conversely, if we have some expressions that we want to just compile and add to the context, we can use the method `AddFunctions()`:***  
```c#
calc.AddFunctions("fib(x) := x<= 1 ? x : fib(x-1) + fib(x-2)",
                  "Pi := 4",
                  "Fib_10 := fib(10)");
```
_The grammar defines that for functions with no parameters the parentheses can be omitted. Thus constants are the same as parameterless functions._  
  
As soon as the function becomes member of Context, it can be called from other expressions.  

***This way, we can add any function that exists in the C# environment:***  
```c#
calc.AddFunction<Func<double>>("Pi", () => 4)
    .AddFunction<Func<double, double>>("Sin", Math.Sin)
    .AddFunction<Func<double, double>>("Print", x=> { Console.WriteLine(x); return x; });
```   

***Function present in Context can be obtained by its signature:***  
```c#
Func<double, double> f1 = calc.Get<Func<double, double>>("f");
Func<double, double, double> f2 = calc.Get<Func<double, double, double>>("f");
```
_Achtung! - Yoowzx supports function overloading. In this case, for the variable f1 a function named "f" with one parameter will be sought; completely different function "f" with two parameters for f2. `Get<>()` deduces the number of arguments from its type parameter._  

### ***Tail recursion***
Yoowzx fully supports the [Tail recursion optimization](https://en.wikipedia.org/wiki/Tail_call). Should we thus define e.g. computation of factorial this way, there's no need to worry about stack overflow:  
```c#
calc.AddFunctions("fact(x, accumulator) := x <= 1? accumulator : fact(x-1, x*accumulator)",
                  "fact(x) := fact(x, 1)");

calc.Get<Func<double, double>>("fact")(800000); //finishes without crash
```

### ***Caching return values***
Using the annotation "cached", the compiler can be ordered to cache return values of the function.  
Thus e.g. this function for computing fibonacci numbers will run in linear time (resp. constant for repeated calls):
```c#
calc.AddFunctions("[cached] fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)");

calc.Get<Func<double, double>>("fib")(1000); //doběhne dříve než skončí vesmír
```
_Caching is supported for all functions no matter the number of parameters they have_  

_The current implementation of caching prevents functions using it from taking advantage of the tail recursion optimisation. The solution is being worked on._  

----------------------------
&nbsp;
## ***Performance***

The module [MarkusSecundus.YoowzxCalc.Benchmarks](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.Benchmarks) contains a few simple benchmarks, comparing the products of YC with equivalent lambdas compiled directly as part of C# source code.  
It can be seen from the output that the current version of YC is more than negligibly slower - aprox. 4times in heavily functioncall-dependent scenarios (Fibonacci, Factorial), around 25% in scenarios that depend rather on arithmetics.  
But still we get drastic speedup compared to non-JITted interpreter languages (like CPython etc.).  

|                  Method |       Mean |    Error |   StdDev |
|-------------------------|------------|----------|----------|
|        Fibonacci_Yoowzx | 1,535.2 us | 12.71 us | 10.61 us |
|        Fibonacci_CSharp |   372.8 us |  1.78 us |  1.66 us |
| Fibonacci_Cached_Yoowzx |   593.0 us |  3.87 us |  3.62 us |
| Fibonacci_Cached_CSharp |   552.1 us |  3.05 us |  2.70 us |
|        Factorial_Yoowzx |   715.8 us |  4.70 us |  4.16 us |
|        Factorial_CSharp |   201.1 us |  0.78 us |  0.73 us |
|              Sum_Yoowzx | 2,669.6 us | 20.27 us | 17.97 us |
|              Sum_CSharp | 2,125.0 us | 12.02 us | 11.24 us |

  
_Reference results were measured on a factory-clocked Core i7 9700KF._


-----------------------------
&nbsp;
## ***Grammar***
Translation of text-written expression into machine-processible form (AST) is responsibility of the module `MarkusSecundus.YoowzxCalc.DSL`.  
The submodule ***[MarkusSecundus.YoowzxCalc.DSL.AST](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.DSL.AST)*** carries definitions of individual AST nodes and the [machinery](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/IYCVisitor.cs) necessary to process them using the [Visitor pattern](https://en.wikipedia.org/wiki/Visitor_pattern).  
Building AST from text-written expression is the responsibility of [YCAstBuilder](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.Parser/IYCAstBuilder.cs). Its cannonical implementation (which respects the grammar described below) is a stateless singleton a can be obtained as `IYCAstBuilder.Instance`.  
If we have a text-written expression, AST can be created this way:  
```c#
string expression;  
YCFunctionDefinition root = IYCAstBuilder.Instance.Build(expression);  
```
Should the parser come across a lexical or syntax error, it will throw an [exception](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.Parser/ParserExceptions/YCAggregateAstBuilderException.cs), carrying info about all the errors encountered.  
  
### ***Whitespace***  
All the characters with ASCII code 0 to ord(' ') (inclusive) are considered whitespace. From the grammar point of view they are ignored and serve as token separators.  
  
### ***Literals and identifiers***  
Fore more flexibility, they are considered the same on the grammar level and their definition is very loose so as to enable e.g. implementing expressions on text strings etc. without need to modify the grammar.  
Their validation and distinction are left up to the user in later phases of expression evaluation (see [YCNumberOperator](#how-to-register-a-numberoperator)).  
  
On AST level, they are represented by [YCLiteralExpression](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/PrimaryExpression/YCLiteralExpression.cs) nodes.  
  
Literal is an arbitrarily long string of literal segments. A literal segment matches one of the following regexes:  
  - `any_nonspecial_nonwhitespace_char`   //special are all characters with a concrete role explicitly mentioned somewhere in the grammar definition - operator characters ('+', '-',...) etc.  
  - `"([^"]|\")"`   //text string in quotes - can contain special and whitespace chars; contained quotes must be escaped out  
  - `'([^']|\')'`   //text string in apostrophes - can contain special and whitespace chars; contained apostrophes must be escaped out)  
  - `[0-9]+(\.[0-9]*)?([eE][+-]?[0-9]+)?`    //real number, optionally in exponential notation - may contain special char `+` or `-`  

&nbsp;

Literal examples:  
  - `321.092` (string of nonspecial chars)  
  - `"This is text: \"qw\"""Another string"` (pair of quoted strings right next to each other - not separated by whitespace)  
  - `@'Another example of text: "REwqefds"'` (nonspecial char followed by apostrophe string)  
  - `Abc1e+32"rew  "` (string of nonspecial chars followed by a number in exp. notation followed by a quote string)  


### ***Operators***
Yoowzx defines all the classically used, unary, binary and ternary, arithmetic and logical operators with priorities and associativity as usual.  
Each operator has its corresponding AST node - thus for exhaustive list of supported operators please see:  
  - [Unary operators](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.DSL.AST/UnaryExpressions)  
  - [Binary operators](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.DSL.AST/BinaryExpressions)  
  - [Ternary operators](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/OtherExpressions/YCConditionalExpression.cs)  



### ***Functioncalls***
Function invocation works as usual.  
Function name is an arbitrary literal followed by parentheses, enclosing a comma-separated list of zero or more arguments (arbitrarily complex expressions).  
In AST it's represented by the [YCFunctioncallExpression](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/OtherExpressions/YCFunctioncallExpression.cs) node.  


### ***Compilation unit***  
Result of compilation is an object of type [YCFunctionDefinition](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/YCFunctionDefinition.cs).  
In grammar it's defined as:  
```c
function_definition: annotation_list? function_name '(' argument_names_list ')' ':=' expression ;  
```
`function_name` is an arbitrary literal, `argument_names_list` is (eventl. empty) comma-separated list of literals and `expression` is some expression describing the body of the defined function.  
If the function has zero parameters, the empty parentheses can voluntarily be omitted.  
Eventually one can omit even the function name with ':=' operator and be left with just (optionally annotated) the lone expression - in such case, `YCFunctionDefinition.AnonymousFunctionName` (guaranteed non-null) will be used as function name.  

#### ***Annotations***  
Sometimes it may be handy to attach some additional data serving e.g. as a compiler directive etc. .  
Annotation list is written in square brackets, individual elements comma-separated. An annotation can either be empty - lone literal - or it can have a value (another literal) assigned - marked by the colon char.  
This is how the grammar looks:    
```c  
annotations_list: '[' annotation (',' annotation)* ']' ;  
annotation: LITERAL | LITERAL ':' LITERAL ;  
```
  
  
#### ***Examples***  
A valid definition that gets accepted by the compiler can look e.g. this:  
  - `f(x) := x*x + 1`  
  - `Func1(arg1, arg2, arg3, arg4, arg5) := arg1==1? (arg1 + arg2 - (30 - arg1)*arg4)**((arg4)**2.14e-3) : Func1(1,1,1,1,arg3)`  
  - `f(a, b, a) := a*b*a` //the compiler doesn't test for duplicities within function parameters  
  - `[annotation1, annotation2: something] f() := 1`    
  - `[annotation1, annotation2: something] f := 1`  
  - `[annotation1, annotation2: something] 1`  


&nbsp;  
-----------------------------  
## ***Compilation***  
The AST is finally built and now nothing stays in out way to start dealing with its compilation to executable code.  
The machinery geared towards that matter is placed in the ***[MarkusSecundus.YoowzxCalc.Compilation](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.Compilation)*** module.  

### ***How to define an operation***
Chceme-li být schopni přeložit matematický výraz na spustitelný kód, musíme nejprve vědět, co vůbec která v něm zapsaná operace znamená, a také jak rozlišit konstantu od identifikátoru a jak vypadá platný identifikátor. To všechno kompilátoru řekneme skrze instanci rozhranní ***[IYCNumberOperator](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Numerics/IYCNumberOperator.cs)***.  

Pracujeme-li s typem `double`, `decimal` nebo `long`, nemusíme se namáhat - pro ty už je defaultní implementace připravena - jako podtřída statické třídy [YCBasicNumberOperators](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Numerics/YCBasicNumberOperators.cs). Tyto výchozí implementace definují operátory intuitivním způsobem - operátor `+` odpovídá sčítání, `%` modulení, `**` mocnění apod., konstantou je vše, co projde metodou `TryParse` na odpovídajícím číselném typu při invariantní kultuře, validní identifikátor matchuje na regex `[[:alpha:]_][[:alnum:]_]*`, operátor pro typ `double` navíc zahrnuje ve standardní knihovně všechny funkce ze třídy `System.Math`.  

Chcete-li napsat vlastní číselný operátor, doporučuji se podívat pro inspiraci právě na tyto předpřipravené implementace. Celkově to ale je poměrně přímočarý proces.

#### ***Recognition of constants***
První metodou, jíž je třeba dodat, je `TryParse`. Jejím úkolem je z textového zápisu určit, zda reprezentuje konstantu, a její případnou hodnotu. Všechny literály jsou nejprve testovány na konstantu a teprve pokud neprojdou, stanou se kandidátem na identifikátor.

#### ***Validation of identifiers***
Pokud literál není vyhodnocen jako konstanta, stane se kandidátem na identifikátor. Metoda `ValidateIdentifier` má pak za úkol rozhodnout, zda identifikátorem vskutku je, příp. lidsky čitelným způsobem popsat odchylky od identifikátorového formátu, jichž se dopouští. Třída [YCBasicNumberOperators](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Numerics/YCBasicNumberOperators.cs) poskytuje pár statických metod a polí, které by se při její implementaci mohly hodit.

#### ***Operator definitions***
Nyní zbývá už jen doplnit metody odpovídající jednotlivým operátorům definovaným v gramatice, což by měl být naprosto přímočarý proces.

#### ***Standard library***
Volitelně ještě můžeme dodat množinu funkcí jakožto standardní knihovnu. Každá funkce v ní definovaná bude kompilátorem automaticky viditelná, aniž by se musela nacházet v kompilačním kontextu. Pokud se v kontextu nachází funkce se stejnou signaturou, zastíní funkci ve standardní knihovně.

#### ***How to register a NumberOperator***
Volitelně ještě může mít smysl vytvořený number operator zaregistrovat jako kanonický operátor k použití nad daným číselným typem.  
Jejich seznam vede opět třída [YCBasicNumberOperators](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Numerics/YCBasicNumberOperators.cs) a registrujeme v ní factory dodávající vždy novou instanci. Je-li náš operátor bezestavový singleton, může to vypadat nějak takto:
```c#

struct MyNumberType { }
class MyNumberOperator : IYCNumberOperator<MyNumberType> 
{
    ...
    public static MyNumberOperator Instance { get; } = new();
}

class EntryPoint
{
    public static void Main()
    {
        YCBasicNumberOperators.Register<MyNumberType>(() => MyNumberOperator.Instance);
    }
}
```
Instanci kanonického operátoru nyní získáme takto:
```c#
IYCNumberOperator<MyNumberType> op = YCBasicNumberOperators.Get<MyNumberType>();
```
Přesně takto získává defaultní operátor fasáda [YoowzxCalculator](#how-to-use), pokud jí žádný nedodáme explicitně. Nyní ji tedy můžeme bez problémů používat pro náš nový typ. 

&nbsp;

### ***Compilation context***
Z našeho výrazu je možné libovolně volat externí pojmenované funkce. Vzniká tedy problém, jak kompilátoru dodat jejich definice, aby mohl ona volání vytvořit.

#### ***Function signature***
Nejprve si ale musíme rozmyslet, jak vůbec funkci jednoznačně identifikovat. YoowzxCalc pro větší uživatelské pohodlí podporuje přetěžování funkcí se stejným jménem, ale různými argumenty. K jednoznačné identifikaci tedy slouží struktura [YCFunctionSignature&lt;TNumber&gt;](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/YCFunctionSignature.cs) - nese jméno funkce, počet a typ (jako generický parametr) argumentů. Ve třídě [YCCompilerUtils](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/Util/YCCompilerUtils.cs) najdete extension-metody, kterými lze signaturu jednoduše získat z instance `System.Delegate` nebo z uzlů AST.

#### ***Management of definitions***
Správa seznamu definic je úkolem objektu [IYCFunctioncallContext](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/Contexts/IYCFunctioncallContext.cs).  
Prázdnou instanci jeho kanonické implementace pro funkce nad typem `double` získáme takto:
```c#
IYCFunctioncallContext<double> ctx = IYCFunctioncallContext<double>.Make();
```
Hešmapu funkcí, jež se v něm již napevno nacházejí, získáme skrze property `ctx.Functions`.  

Ne vždy jsme ale schopni pro všechny funkce, jež chceme volat, mít těla již zkompilovaná a připravená. Představte si např., že pracujeme na umělé inteligenci ke hře a pokoušíme se v kalkulátoru naimplementovat klasickým učebnicovým způsobem algoritmus [MiniMax](https://en.wikipedia.org/wiki/Minimax). Máme dvě funkce, které se mají volat vzájemně - první volá druhou, druhá volá první, tedy cyklická závislost.  
Zkompilovat obě funkce najednou, aby jedna o druhé vzájemně věděly, není v silách YC - přineslo by to do něj extrémní a zbytečnou komplexitu. Přímočarý způsob, jak takovou situaci rozřešit, je umožnit volání funkcí, jež ještě nebyly definovány - vytvořit prázdný wrapper, na který odkážeme místo toho, a počítat, že příslušná definice do něj bude dodána později. Přesně tak to YC dělá a k tomu slouží ostatek mašinerie, kterou má [IYCFunctioncallContext](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/Contexts/IYCFunctioncallContext.cs) na starosti.  
Wrapper pro potencielně nerozřešený symbol s danou signaturou získáme takto:
```c#
IYCFunctioncallContext<double> ctx;
YCFunctionSignature<double> signature;
SettableOnce<Delegate> unresolved = ctx.GetUnresolvedFunction(signature);
```
Přesně toto dělá kompilátor pokaždé, když narazí na funkci, jež není k nalezení v hešmapě `Functions` ani ve [standardní knihovně](#standard-library).  
Do `unresolved` nyní, pokud vskutku je nerozřešena (což není garantováno - zjistíme příp. skrze `unresolved.IsSet`), pokud bychom vážně chtěli, můžeme ručně uložit delegáta a rozřešit ji tím, normálně takto:
```c#
Delegate value;
unresolved.Value = value;
```
V praxi to ale skoro jistě dělat nebudeme - místo toho využijeme metody `ResolveSymbols` na kontextu - nějak takto:
 ```c#
ctx = ctx.ResolveSymbols((signature, del));
 ```
 Ta bere jako varargs libovolný počet dvojic `(signatura, delegát)`, všechny najednou rozřeší a vrátí novou instanci kontextu, jež má všechny rozřešené symboly přidány do svých `Functions` (včetně symbolů, jež byly vedeny jako unresolved, ale měly hodnotu už nastavenou odjinud než z argumentů `ResolveSymbols`). Definice předaná sem jako argument bude v pořádku přidána do výsledného kontextu i tehdy, když vůbec nebyla vedena jako unresolved - tímto způsobem tedy jsme schopni do kontextu přímočaře přidávat i úplně nové definice.  

 _Jakmile je symbol jednou rozřešen, pokus o změnu jeho hodnoty vyústí v běhovou chybu - to je záměr. Já, jakožto autor, si jsem plně vědom, že tím zavírám cestu k mnoha zajímavým a zajisté i velmi užitečným trikům, kterých by nebýt toho bylo možné dosáhnout, avšak v důsledku toho, jak je zbytek YC implementován, by to vedlo v některých okrajových případech k velmi komplexnímu chování, které, upřímně, nemám nervy dokumentovat.  
 Pokud to uživatel opravdu nutně potřebuje, neměl by pro něj být velký problém naimplementovat nad YC další vrstvu, jež mu to umožní, příp. obstarat si na vlastní nebezpečí verzi `MarkusSecundus.Util.dll` s odebranými checky v `SettableOnce`, je-li vážně zoufalý._

 Někdy by se mohla hodit metoda `GetUnresolvedSymbolsList` - ta vrací proud těch symbolů, jež jsou vedeny jako unresolved a skutečně ještě rozřešeny nebyly.

_Vedlejším efektem tohoto chování je fakt, že volání neexistující funkce zákonitě nemůže ústit v kompilační chybu, ale vždy až běhovou při pokusu onu neexistující funkci zavolat._

### ***Compiler***
Nyní konečně známe vše, co potřebujeme, abychom mohli přistoupit k vlastní kompilaci.  
Ta je úkolem objektu [IYCCompiler](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/IYCCompiler.cs).  
Máme-li již instanci operátoru, kompilátor získáme následovně:
```c#
IYCNumberOperator<MyNumberType> op;
IYCCompiler<MyNumberType> compiler = IYCCompiler<MyNumberType>.Make(op);
```
Použití je víceméně přímočaré - zavoláme metodu `Compile`, jako argumenty jí předáme kontext a AST výrazu, jenž chceme zkompilovat - takto:
```c#
IYCFunctioncallContext<MyNumberType> ctx;

YCFunctionDefinition toCompile;
IYCCompilationResult<MyNumberType> result = compiler.Compile(ctx, toCompile);
```

Z implementačních důvodů takto ještě nezískáme přímo spustitelného delegáta, ale polotovar, který je nutné finalizovat jedním z těchto způsobů:
```c#
Delegate weaklyTypedResult = result.Finalize();
Func<MyNumberType> stronglyTypedResult = result.Finalize<Func<MyNumberType>>();
```

Nyní jsme konečně získali spustitelného delegáta reprezentujícího náš výraz! Ještě zbývá zkontrolovat a příp. doplnit nevyřešené symboly, které se kompilací objevily v kontextu, a můžeme ho vesele začít používat, jak jen se nám zachce.



-----------------------------
&nbsp;
## ***Demo calculator***
Balíček [MarkusSecundus.YoowzxCalc.Cmd](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.Cmd) obsahuje spustitelnou kalkulačku pro příkazovou řádku operující v klasické REPL smyčce nad typem `double` a sloužící jako ukázka demonstrující některé možnosti této knihovny.  

Pro podrobnější instrukce k jejímu použití kalkulačku spusťte v interaktivním režimu a zavolejte příkaz `help`.
