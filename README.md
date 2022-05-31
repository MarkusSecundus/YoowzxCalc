# ***YoowzxCalc***
[![.NET](https://github.com/MarkusSecundus/YoowzxCalc/actions/workflows/dotnet.yml/badge.svg)](https://github.com/MarkusSecundus/YoowzxCalc/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/vpre/MarkusSecundus.YoowzxCalc.svg)](https://www.nuget.org/packages/MarkusSecundus.YoowzxCalc)
[![NuGet](https://img.shields.io/nuget/dt/MarkusSecundus.YoowzxCalc.svg)](https://www.nuget.org/packages/MarkusSecundus.YoowzxCalc) 

### .NET library for processing of mathematical expressions developed with emphasis on performance and flexibility of use.
\
Parses expressions in infix notation, with support of recursion and user defined functions.  
Works by emitting CIL bytecode at runtime - capable of running natively on the VM - callable via delegate.  
Also includes a programmable calculator for terminal that serves as demo.  


 - _[CZ version of Readme](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/README_cs-CZ.md)_
&nbsp;

#### ***Contents***
  0. [First steps](#first-steps)
     - [NuGet package](#nuget-package) 
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

### ***NuGet package***
Current build is available both locally on [Github Releases](https://github.com/MarkusSecundus/YoowzxCalc/releases) and on [nuget.org](https://www.nuget.org/packages/MarkusSecundus.YoowzxCalc/):  
```powershell
PM> Install-Package MarkusSecundus.YoowzxCalc
```

### ***How to build***
Open the solution in an up-to-date version of MS Visual Studio 2019+ and build it. Throughout the projects, features of C# 9 are used heavily - thus .NET 5.0 is required.  

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
Console.WriteLine(f1()); //prints 2

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

calc.Get<Func<double, double>>("fib")(1000); //manages to finish sooner than existence of the universe
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
Building AST from text-written expression is the responsibility of [YCAstBuilder](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.Parser/IYCAstBuilder.cs). Its cannonical implementation (which respects the grammar described below) is a stateless singleton and can be obtained as `IYCAstBuilder.Instance`.  
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
Their validation and distinction is left up to the user in later phases of expression evaluation (see [YCNumberOperator](#how-to-register-a-numberoperator)).  
  
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
A valid definition that gets accepted by the parser can look e.g. like this:  
  - `f(x) := x*x + 1`  
  - `Func1(arg1, arg2, arg3, arg4, arg5) := arg1==1? (arg1 + arg2 - (30 - arg1)*arg4)**((arg4)**2.14e-3) : Func1(1,1,1,1,arg3)`  
  - `f(a, b, a) := a*b*a` //the parser doesn't test for duplicities within function parameters  
  - `[annotation1, annotation2: something] f() := 1`    
  - `[annotation1, annotation2: something] f := 1`  
  - `[annotation1, annotation2: something] 1`  
  - `[cached] ackermann(m,n) := m == 0 ? n + 1 : n == 0 ? ackermann(m - 1, 1) : ackermann(m-1, ackermann(m, n-1))`  


&nbsp;  
-----------------------------  
## ***Compilation***  
The AST is finally built and now nothing stays in out way to start dealing with its compilation to executable code.  
The machinery geared towards that matter is placed in the ***[MarkusSecundus.YoowzxCalc.Compilation](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.Compilation)*** module.  

### ***How to define an operation***
To be capable of translating a mathematical expression to executable code, we first need to define what the individual operations written in it actually mean, as well as how to distinguish a constant from an identifier and what even is a valid identifier.  
All of those things are told to the compiler through an instance of interface ***[IYCNumberOperator](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Numerics/IYCNumberOperator.cs)***.  

When operating on type `double`, `decimal` or `long`, no effort is required - for those there are already default implementations prepared - as subclasses of static class [YCBasicNumberOperators](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Numerics/YCBasicNumberOperators.cs).  
Such default implementations implement all the operators the intuitive way - operator `+` means addition, `%` modulus, `**` power, etc., constant is anything that gets accepted by the `TryParse` method on the corresponding type using invariant culture, valid identifier matches regex `[[:alpha:]_][[:alnum:]_]*`.  
The operator for type `double` also includes all functions from `System.Math` as members of its standard library.  

When implementing your own number operator, it may be a good idea to look at the premade implementations for inspiration. Although it should overall be a very straightforward process.  

#### ***Recognition of constants***
The first method to be supplied is `TryParseConstant`.  
Its task is to resolve whether a text string represents a constant and eventually to determine its value.  
All literals are first tested for being constant and only if they do not pass, they become identifier candidates.  

#### ***Validation of identifiers***
If a literal isn't resolved to be a constant, it becomes identifier candidate.  
The method `ValidateIdentifier` is responsible for determining if it indeed is an identifier, providing human-readable summary of perpetrated violations of the identifier format if it is not.  
The class [YCBasicNumberOperators](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Numerics/YCBasicNumberOperators.cs) provides a few static methods and fields that could come in handy while implementing this method.  

#### ***Operator definitions***
The only thing remaining is to fill out all the methods that correspond to particular grammar-defined operators, which should be a totally straightforward process.  

#### ***Standard library***
Voluntarily, we can also provide a set of functions to serve as standard library.  
The functions defined there will automatically be visible to the compiler without needing to be present in compilation context. When a function with the same signature appears in context, it shadows the one in standard library.  

#### ***How to register a NumberOperator***
It may also make sense to register the just created number operator as canonical operator for the given numeric type.  
Maintaining their list is another responsibility of [YCBasicNumberOperators](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Numerics/YCBasicNumberOperators.cs) and the registration is done by providing a factory creating new operator instances.  
If our operator is a stateless singleton, it may look e.g. like this:  
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
An instance of cannonical operator can now be obtained like this:  
```c#
IYCNumberOperator<MyNumberType> op = YCBasicNumberOperators.Get<MyNumberType>();
```
Which is exactly how the facade [YoowzxCalculator](#how-to-use) gets it. Thus, we can now use it for our new type without worries.  

&nbsp;

### ***Compilation context***
Expression may contain arbitrary calls of external named functions. A question occurs - how do we supply their definitions to the compiler, so that the calls can be created?  
#### ***Function signature***
For greater user convenience, YoowzxCalc is made to support function overloading (same name, different arguments).  
The structure [YCFunctionSignature&lt;TNumber&gt;](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/YCFunctionSignature.cs) serves for unique identification of functions - it contains the function name, number of arguments and their type (as a generic parameter).   
The class [YCCompilerUtils](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/Util/YCCompilerUtils.cs) contains extension methods through which the signature can easily be obtained from both `System.Delegate` and AST nodes.  

#### ***Management of definitions***
Management of definitions is responsibility of [IYCFunctioncallContext](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/Contexts/IYCFunctioncallContext.cs).  
Any empty instance of its cannonical implementation for operating on type `double` can be obtained this way:
```c#
IYCFunctioncallContext<double> ctx = IYCFunctioncallContext<double>.Make();
```
Hashmap of functions contained in there can be accessed through the `ctx.Functions` property.  

Not always are we able to have the bodies of all functions we intend to call compiled and prepared in advance. Take e.g. this situation: we are working on an AI for a game and are trying to implement the classical textbook version of [MiniMax](https://en.wikipedia.org/wiki/Minimax) algorithm inside the calculator. We have two functions that call one another, cyclic dependency.  
Compiling both the functions at once, so that they directly know about each other, is not in power of YC - supporting such scenario would bring extreme and unnecessary complexity. Straightforward approach to solving this problem is to allow calling functions that haven't been defined yet - create an empty wrapper to which the call is directed, and assume the definition will be put there later. Which is exactly how YC does it and what the rest of the machinery, that[IYCFunctioncallContext](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/Contexts/IYCFunctioncallContext.cs) provides, is meant for.  
A wrapper for a potentionally unresolved symbol with given signature can be obtained like this:  
```c#
IYCFunctioncallContext<double> ctx;
YCFunctionSignature<double> signature;
SettableOnce<Delegate> unresolved = ctx.GetUnresolvedFunction(signature);
```
...which is exactly what the compiler does each time it comes across a function that can be found neither in the `Functions` hashmap, nor in the [standard library](#standard-library).  
Now, if `unresolved` indeed is unresolved (not guarranteed - check via `unresolved.IsSet`), we can manually assign a delegate to it, thus resolve it:  
```c#
Delegate value;
unresolved.Value = value;
```
However, in practice we most certainly won't do it this way - instead using the method `ResolveSymbols` on context - like this:  
 ```c#
ctx = ctx.ResolveSymbols((signature, del));
 ```
 It takes an arbitrary number of `(signature, delegate)` pairs as varargs, resolves all of them at once and returns a new instance of context, where all the resolved symbols are added to `Functions` (including the 'unresolved' whose value was already resolved from somewhere else than the arguments of `ResolveSymbols`).   
 A definition provided in the arguments will be added into the result context even when it wasn't established as 'unresolved' - making this a straightforward way of adding brand new definitions into the context.   


_Once a symbol is resolved, any attempt to change it results in an exception - that's intentional.  
I, as the author, am completely aware that I'm closing the gate to many interesting and maybe even some useful tricks that could be achied otherwise, however, in consequence of the way YC is implemented, it would lead in some edge cases to very complex behavior that I honestly don't have the nerve to document.
If the user desperately needs it, it shouldn't be such a big deal to implement another layer above YC that makes it possible, or possibly at his own risk obtain a custom version of `MarkusSecundus.Util.dll` with checks in [SettableOnce](https://github.com/MarkusSecundus/YoowzxCalc/blob/translate_doccoments_to_english/MarkusSecundus.Util/SettableOnce.cs) removed if the despair is really deep._

Also worth mention is the method `GetUnresolvedSymbolsList` - it returns a stream of symbols that are established as unresolved and really havent't been resolved yet.  

Unintentional consequence of this behavior is the fact that calling non-existent function cannot result in a compile error, but always only runtime one on the attempt to call the function._  

### ***Compiler***
Now we finally have everything we need to approach the compilation itself.  
That is the task of [IYCCompiler](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/IYCCompiler.cs).  

If we alredy have an instance of number operator, the compiler can be obtained like this:  
```c#
IYCNumberOperator<MyNumberType> op;
IYCCompiler<MyNumberType> compiler = IYCCompiler<MyNumberType>.Make(op);
```
The usage is quite straightforward - just call the method `Compile`, passing the context and expression AST as arguments:  
```c#
IYCFunctioncallContext<MyNumberType> ctx;

YCFunctionDefinition toCompile;
IYCCompilationResult<MyNumberType> result = compiler.Compile(ctx, toCompile);
```

Because of implementation reasons, we don't directly get the delegate, but another object, that needs to be finalized into the delegate in one of these ways:  
```c#
Delegate weaklyTypedResult = result.Finalize();
Func<MyNumberType> stronglyTypedResult = result.Finalize<Func<MyNumberType>>();
```

We finally obtained a delegate that represents our expression! Now the only thing remaining is to check, evtl. fill in unresolved symbols and we can happily start using it.  



-----------------------------
&nbsp;
## ***Demo calculator***
The package [MarkusSecundus.YoowzxCalc.Cmd](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.Cmd) contains a command line calculator, operating clasically in REPL over the type `double`, that serves as a demo showcasing some abilities of this library.    

For detailed instructions about its usage please run the calculator in interactive mode and call the `help` command.  
