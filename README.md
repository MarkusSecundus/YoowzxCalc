# YoowzxCalc

### Knihovna pro zpracování matematických výrazů v prostředí .NET vyvíjená s důrazem na výkon a flexibilitu použití.   
\
Parsuje výrazy v infixní notaci s podporou uživatelsky definovaných pomocných funkcí a rekurzivních volání (viz níže gramatiku).   
Schopná z výrazů za běhu generovat CIL kód - běžící přímo na VM a tedy rychle - volatelný skrze delegáta.  
Rovněž zahrnuje programovatelný kalkulátor pro příkazovou řádku sloužící jako demo.

&nbsp;

Poděkování:
  - Autorům .NET a jazyka C#
  - Autorům knihovny [ReadLine](https://github.com/tonerdo/readline)
  - Autorům knihovny [CommandLineParser](https://github.com/commandlineparser/commandline)
  - Autorům parser-generátoru [ANTLR](https://www.antlr.org/)

-----------------------
&nbsp;
## Začínáme

Pro přímočaré použití základní funkcionality slouží fasáda [YoowzxCalculator](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc/IYoowzxCalculator.cs).  
Ta zaštiťuje celou pipelinu zpracování výrazu - prvotní zparsování textového zápisu na obecný abstraktní syntaktický strom, následnou generaci spustitelného kódu podle AST a zároveň i správu kontextu s funkcemi, které je možné z výrazů volat. Pro každý z těchto segmentů je možné explicitně dodat vlastní implementaci nebo nechat, aby byla použita ta defaultní.  

***Instanci kalkulátoru počítajícího nad typem `double` získáme takto:***
```c#
IYoowzxCalc<double> calc = IYoowzxCalc<double>.Make(); 
```
*V základu lze vytvořit kalkulátor nad typy `double`, `decimal` a `long`. Pro počítání nad jiným typem je třeba explicitní přidání podpory uživatelem (viz níže `NumberOperator`).*  

***Jakmile máme instanci kalkulátoru, můžeme vesele kompilovat výrazy:***
```c#
Func<double> f1 = calc.Compile<Func<double>>("1 + 1");
Console.WriteLine(f1()); //vypíše 3

Func<double, double> f2 = calc.Compile<Func<double, double>>("f(číslo) := číslo * (3 + 4 ** 5e-1)");
Console.WriteLine(f2(0)); //vypíše 0

Func<double, double> fibonacci = calc.Compile<Func<double, double>>("fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)");
for(int t=0;t<10;++t)
    Console.WriteLine(fibonacci(t));    //vypíše prvních 10 fibonacciho čísel
```
Takto výraz pouze zkompilujeme, ale ať už byl pojmenovaný nebo ne, nikdy nebude přidán jako volatelná funkce do kontextu. (Ale jeho jméno pořád má význam když chceme praktikovat rekurzi.)  

***Máme-li naopak několik výrazů, které chceme zkompilovat a rovnou přidat do kontextu, můžeme použít metodu `AddFunctions()`:***
```c#
calc.AddFunctions("fib(x) := x<= 1 ? x : fib(x-1) + fib(x-2)",
                  "Pi := 4",
                  "Fib_10 := fib(10)");
```
Jakmile je funkce součástí kontextu, můžeme ji volat z jiných výrazů.  

***Někdy by se ale hodilo moci zpřístupnit k volání sofistikovanější funkci definovanou přímo v C#:***
```c#
calc.AddFunction<Func<double>>("Pi", () => 4)
    .AddFunction<Func<double, double>>("Sin", Math.Sin)
    .AddFunction<Func<double, double>>("Print", x=> { Console.WriteLine(x); return x; });
```   

***Jakmile se funkce jednou nachází v kontextu, ať již do něj byla přidána odkudkoliv, dokážeme ji z něj získat:***
```c#
Func<double, double> f1 = calc.Get<Func<double, double>>("f");
Func<double, double, double> f2 = calc.Get<Func<double, double, double>>("f");
```
_Pozor - Yoowzx podporuje přetěžování funkcí. V tomto případě pro hodnotu f1 bude hledána funkce s názvem "f" a jedním argumentem, pro f2 jiná funkce "f" s dvěma argumenty. Počet argumentů hledané funkce metoda Get() vykouká z typového parametru._  

### ***Koncová rekurze***
Yoowzx plně podporuje [optimalizaci koncové rekurze](https://en.wikipedia.org/wiki/Tail_call). Zadefinujeme-li tedy např. takto výpočet faktorialu, pro libovolně vysoké hodnoty argumentu nehrozí přetečení volacího zásobníku:
```c#
calc.AddFunctions("fact(x, accumulator) := x <= 1? accumulator : fact(x-1, x*accumulator)",
                  "fact(x) := fact(x, 1)");

calc.Get<Func<double, double>>("fact")(800000); //doběhne bez pádu
```

### ***Kešování výsledků***
Pomocí anotace "cached" lze kompilátoru nařídit, aby volání funkce pro danou hodnotu argumentů provedl jednou, výsledek uložil do keše, a příště ho z ní už jenom tahal.  
Tím pádem např. takto definovaná funkce pro výpočet fibonacciho posloupnosti poběží v lineárním čase (resp. konstantním pro opětovná volání):
```c#
calc.AddFunctions("[cached] fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)");

calc.Get<Func<double, double>>("fib")(1000); //doběhne dříve než skončí vesmír
```
_Kešování je podporováno pro všechny funkce bez ohledu na to, jak mnoho argumentů berou._

&nbsp;
-----------------------------
## Gramatika
Překlad textově zapsaného výrazu do počítačem přímočaře zpracovatelné formy (AST) je úkolem modulu MarkusSecundus.YoowzxCalc.DSL.  
Podmodul ***[MarkusSecundus.YoowzxCalc.DSL.AST](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.DSL.AST)*** obsahuje definici jednotlivých uzlů AST a [mašinérii](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/IYCVisitor.cs) pro jejich zpracování pomocí [visitor patternu](https://en.wikipedia.org/wiki/Visitor_pattern).  
Sestavení AST z textově zapsaného výrazu je úkolem [YCAstBuilder](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.Parser/IYCAstBuilder.cs)u. Jeho kanonická implementace (která respektuje níže popsanou gramatiku) je bezestavový singleton a lze ji získat jako `IYCAstBuilder.Instance`.  
Máme-li tedy textově zapsaný výraz, AST z něj získáme nějak takto:
```c#
string expression;
YCFunctionDefinition root = IYCAstBuilder.Instance.Build(expression);
```
Narazil-li parser na nějakou lexikální či syntaktickou chybu, vyhodí na konci svého běhu [výjimku](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.Parser/ParserExceptions/YCAggregateAstBuilderException.cs), nesoucí informace o všech chybách, ke kterým v překládaném textu došlo.

### Bílé znaky
Za bílé jsou považovány všechny znaky s ASCII kódem od 0 do ord(' ') včetně. Z hlediska gramatiky jsou ignorovány, slouží jako oddělovač.

### Literály a identifikátory
Pro větší flexibilitu nejsou na úrovni gramatiky rozlišovány a jejich definice je velmi volná, s cílem umožnit např. zpracování výrazů nad textovými řetězci apod. bez nutnosti gramatiku přepisovat. 
Jejich validace a rozlišení jsou ponechány na uživateli v rámci pozdějších fází zpracování výrazu (viz níže YCNumberOperator).  

Na úrovni AST je reprezentuje uzel [YCLiteralExpression](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/PrimaryExpression/YCLiteralExpression.cs).

Literál je libovolně dlouhý řetězec literálových prvků. Literálový prvek matchuje na jeden z těchto regulárních výrazů:
  - `libovolný_nespecielní_newhitespace_znak`   //specielní jsou všechny znaky s přiřazenou konkrétní rolí v gramatice - znaky operátorů apod.
  - `"([^"]|\")"`   //textový řetězec v uvozovkách - může obsahovat i specielní a bílé znaky; uvozovky též, pokud jsou odescapované
  - `'([^']|\')'`   //textový řetězec v apostrofech - může obsahovat i specielní a bílé znaky; apostrofy též, pokud jsou odescapované
  - `[0-9]+(\.[0-9]*)?([eE][+-]?[0-9]+)?`    //reálně číslo v exponenciální notaci - může obsahovat specielní znak '+' nebo '-'  

&nbsp;

Příkl. literálů: 
  - `321.092` (řetězec nespeciálních znaků)
  - `"Toto je text: \"qw\"""Jiný řetězec"` (dvojice řetězců hned vedle sebe, neoddělených bílým znakem)
  - `@'Další exemplář textu: "REwqefds"'` (nespeciální znak následovaný řetězcem)
  - `Abc1e+32"rew  "` (řetězec nespeciálních znaků následovaný číslem v exp. notaci následovaný řetězcem)


### Operátory
Yoowzx definuje klasicky používané, unární, binární a ternární, aritmetické a logické operátory s obvyklými prioritami a asociativitou.
Každému operátoru odpovídá uzel AST, pro vyčerpávající výčet podporovaných operátorů náhlédněte tedy prosím zde:
  - [Unární operátory](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.DSL.AST/UnaryExpressions)
  - [Binární operátory](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.DSL.AST/BinaryExpressions)
  - [Ternární operátor](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/OtherExpressions/YCConditionalExpression.cs)



### Volání funkcí
Volání funkcí probíhá klasickým způsobem známým např. z jazyka C:  
Jméno funkce je libovolný literál, za ním následují kulaté závorky, obsahující příp. jednotlivé argumenty (libovolně složité výrazy) oddělené čárkami.  
Na úrovni AST je reprezentováno uzlem [YCFunctioncallExpression](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/OtherExpressions/YCFunctioncallExpression.cs).


## Kompilační jednotka
Výstupem kompilace je objekt typu [YCFunctionDefinition](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/YCFunctionDefinition.cs).  
Jeho zápis vypadá nějak takto:
```c
list_anotací? jméno_funkce '(' seznam_jmen_argumentů ')' ':=' výraz
```
Jméno funkce je libovolný literál, seznam jmen argumentů je (příp. prázdný) list literálů oddělených znakem ',', výraz pak libovolně složitý matematický výraz reprezentující tělo definované funkce.  
V případě funkce s nulovým počtem argumentů lze příp. prázdné závorky vynechat.  
Popř. lze vynechat i jméno funkce s výrazem přiřadítka a zůstat se samotným (volitelně oanotovaným) výrazem. 
V takovém případě bude jako jméno funkce použita (zaručeně non-null) hodnota `YCFunctionDefinition.AnonymousFunctionName`.