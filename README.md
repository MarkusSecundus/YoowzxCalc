# ***YoowzxCalc***

### Knihovna pro zpracování matematických výrazů v prostředí .NET vyvíjená s důrazem na výkon a flexibilitu použití.   
\
Parsuje výrazy v infixní notaci s podporou uživatelsky definovaných pomocných funkcí a rekurzivních volání.   
Schopná z výrazů za běhu generovat CIL kód - běžící přímo na VM a tedy rychle - volatelný skrze delegáta.  
Rovněž zahrnuje programovatelný kalkulátor pro příkazovou řádku sloužící jako demo.


&nbsp;

#### ***Obsah***
  0. [Začínáme](#začínáme)
     - [Koncová rekurze](#koncová-rekurze) 
     - [Kešování výsledků](#kešování-výsledků)
  1. [Gramatka](#gramatika)
     - [Bílé znaky](#bílé-znaky)
     - [Literály a identifikátory](#literály-a-identifikátory)
     - [Operátory](#operátory)
     - [Volání funkcí](#volání-funkcí)
     - [Kompilační jednotka](#kompilační-jednotka)
       - [Anotace](#anotace)
       - [Příklady](#příklady)
  2. [Kompilace](#kompilace)
     - [Jak definovat operace](#jak-definovat-operace)
       - [Rozlišení konstant](#rozlišení-konstant)
       - [Validace identifikátorů](#validace-identifikátorů)
       - [Definice operátorů](#definice-operátorů)
       - [Standardní knihovna](#standardní-knihovna)
       - [Registrace NumberOperatoru](#registrace-numberoperatoru)
     - [Kompilační kontext](#kompilační-kontext)
       - [Signatura funkce](#signatura-funkce)
       - [Správa definic](#správa-definic)
     - [Kompilátor](#kompilátor)
  3. [Demo kalkulačka](#demo-kalkulačka)

&nbsp;

#### ***Poděkování:***
  - Autorům .NET a jazyka C#
  - Autorům knihovny [ReadLine](https://github.com/tonerdo/readline)
  - Autorům knihovny [CommandLineParser](https://github.com/commandlineparser/commandline)
  - Autorům parser-generátoru [ANTLR](https://www.antlr.org/)

#### ***Autor***:
  - Jakub Hroník


-----------------------
&nbsp;
## ***Začínáme***

Pro přímočaré použití základní funkcionality slouží fasáda [YoowzxCalculator](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc/IYoowzxCalculator.cs).  
Ta zaštiťuje celou pipelinu zpracování výrazu - prvotní zparsování textového zápisu na obecný abstraktní syntaktický strom, následnou generaci spustitelného kódu podle AST a zároveň i správu kontextu s funkcemi, které je možné z výrazů volat. Pro každý z těchto segmentů je možné explicitně dodat vlastní implementaci nebo nechat, aby byla použita ta defaultní.  

***Instanci kalkulátoru počítajícího nad typem `double` získáme takto:***
```c#
IYoowzxCalc<double> calc = IYoowzxCalc<double>.Make(); 
```
*V základu lze vytvořit kalkulátor nad typy `double`, `decimal` a `long`. Pro počítání nad jiným typem je třeba explicitní přidání podpory uživatelem (viz níže [`NumberOperator`](#registrace-numberoperatoru)).*  

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
Takto výraz pouze zkompilujeme, ale ať už byl pojmenovaný nebo ne, nikdy nebude přidán jako volatelná funkce do kontextu. (Jeho jméno však pořád má význam když chceme praktikovat rekurzi.)  

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

_Současná implementace kešování na funkcích, jež ji využívají, neumožňuje optimalizaci koncové rekurze. Na řešení se pracuje._

-----------------------------
&nbsp;
## ***Gramatika***
Překlad textově zapsaného výrazu do počítačem přímočaře zpracovatelné formy (AST) je úkolem modulu MarkusSecundus.YoowzxCalc.DSL.  
Podmodul ***[MarkusSecundus.YoowzxCalc.DSL.AST](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.DSL.AST)*** obsahuje definici jednotlivých uzlů AST a [mašinérii](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/IYCVisitor.cs) pro jejich zpracování pomocí [visitor patternu](https://en.wikipedia.org/wiki/Visitor_pattern).  
Sestavení AST z textově zapsaného výrazu je úkolem [YCAstBuilder](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.Parser/IYCAstBuilder.cs)u. Jeho kanonická implementace (která respektuje níže popsanou gramatiku) je bezestavový singleton a lze ji získat jako `IYCAstBuilder.Instance`.  
Máme-li tedy textově zapsaný výraz, AST z něj získáme nějak takto:
```c#
string expression;
YCFunctionDefinition root = IYCAstBuilder.Instance.Build(expression);
```
Narazil-li parser na nějakou lexikální či syntaktickou chybu, vyhodí na konci svého běhu [výjimku](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.Parser/ParserExceptions/YCAggregateAstBuilderException.cs), nesoucí informace o všech chybách, ke kterým v překládaném textu došlo.

### ***Bílé znaky***
Za bílé jsou považovány všechny znaky s ASCII kódem od 0 do ord(' ') včetně. Z hlediska gramatiky jsou ignorovány, slouží jako oddělovač.

### ***Literály a identifikátory***
Pro větší flexibilitu nejsou na úrovni gramatiky rozlišovány a jejich definice je velmi volná, s cílem umožnit např. zpracování výrazů nad textovými řetězci apod. bez nutnosti gramatiku přepisovat. 
Jejich validace a rozlišení jsou ponechány na uživateli v rámci pozdějších fází zpracování výrazu (viz níže YCNumberOperator).  

Na úrovni AST je reprezentuje uzel [YCLiteralExpression](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/PrimaryExpression/YCLiteralExpression.cs).

Literál je libovolně dlouhý řetězec literálových prvků. Literálový prvek matchuje na jeden z těchto regulárních výrazů:
  - `libovolný_nespecielní_newhitespace_znak`   //specielní jsou všechny znaky s přiřazenou konkrétní rolí v gramatice - znaky operátorů apod.
  - `"([^"]|\")"`   //textový řetězec v uvozovkách - může obsahovat i specielní a bílé znaky; uvozovky též, pokud jsou odescapované
  - `'([^']|\')'`   //textový řetězec v apostrofech - může obsahovat i specielní a bílé znaky; apostrofy též, pokud jsou odescapované
  - `[0-9]+(\.[0-9]*)?([eE][+-]?[0-9]+)?`    //reálně číslo v exponenciální notaci - může obsahovat specielní znak `+` nebo `-`  

&nbsp;

Příkl. literálů: 
  - `321.092` (řetězec nespeciálních znaků)
  - `"Toto je text: \"qw\"""Jiný řetězec"` (dvojice řetězců hned vedle sebe, neoddělených bílým znakem)
  - `@'Další exemplář textu: "REwqefds"'` (nespeciální znak následovaný řetězcem)
  - `Abc1e+32"rew  "` (řetězec nespeciálních znaků následovaný číslem v exp. notaci následovaný řetězcem)


### ***Operátory***
Yoowzx definuje klasicky používané, unární, binární a ternární, aritmetické a logické operátory s obvyklými prioritami a asociativitou.
Každému operátoru odpovídá uzel AST, pro vyčerpávající výčet podporovaných operátorů náhlédněte tedy prosím zde:
  - [Unární operátory](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.DSL.AST/UnaryExpressions)
  - [Binární operátory](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.DSL.AST/BinaryExpressions)
  - [Ternární operátor](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/OtherExpressions/YCConditionalExpression.cs)



### ***Volání funkcí***
Volání funkcí probíhá klasickým způsobem známým např. z jazyka C:  
Jméno funkce je libovolný literál, za ním následují kulaté závorky, obsahující příp. jednotlivé argumenty (libovolně složité výrazy) oddělené čárkami.  
Na úrovni AST je reprezentováno uzlem [YCFunctioncallExpression](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/OtherExpressions/YCFunctioncallExpression.cs).


### ***Kompilační jednotka***
Výstupem kompilace je objekt typu [YCFunctionDefinition](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.DSL.AST/YCFunctionDefinition.cs).  
Jeho zápis vypadá nějak takto:
```c
definice_funkce: list_anotací? jméno_funkce '(' seznam_jmen_argumentů ')' ':=' výraz ;
```
"Jméno funkce" je libovolný literál, "seznam jmen argumentů" je (příp. prázdný) list literálů oddělených znakem `,` a "výraz" pak libovolně složitý výraz reprezentující tělo definované funkce.  
V případě funkce s nulovým počtem argumentů lze příp. prázdné závorky vynechat.  
Popř. lze vynechat i jméno funkce s výrazem přiřadítka a zůstat se samotným (volitelně oanotovaným) výrazem - v takovém případě bude jako jméno funkce použita (zaručeně non-null) hodnota `YCFunctionDefinition.AnonymousFunctionName`.

#### ***Anotace***
Někdy se hodí moci k definici funkce přiložit ještě dodatečná data, sloužící např. jako řidicí direktiva pro kompilátor apod. .  
List anotací se zapisuje do hranatých závorek a jednotlivé anotace v něm jsou oddělené čárkami. Anotace může být buď prázdná - samotný literál, nebo může mít hodnotu uvozenou dvojtečkou a danou druhým literálem. Gramatika tedy vypadá takto:
```c
list_anotací: '[' anotace (',' anotace)* ']' ;
anotace: LITERÁL | LITERÁL ':' LITERÁL ;
```


#### ***Příklady***
Validní definice která projde kompilátorem může vypadat např. takto:
  - `f(x) := x*x + 1`
  - `Funkce1(arg1, arg2, arg3, arg4, arg5) := arg1==1? (arg1 + arg2 - (30 - arg1)*arg4)**((arg4)**2.14e-3) : Funkce1(1,1,1,1,arg3)`
  - `f(a, b, a) := a*b*a` //kompilátor netestuje duplicitu funkčních argumentů
  - `[anotace1, anotace2: něco] f() := 1`  
  - `[anotace1, anotace2: něco] f := 1`
  - `[anotace1, anotace2: něco] 1`


&nbsp;
-----------------------------
## ***Kompilace***
Jakmile je postaven abstraktní syntaktický strom, nic už nám nebrání začít se zabývat jeho kompilací na spustitelný kód.  
Mašinérii s tím související obsahuje modul ***[MarkusSecundus.YoowzxCalc.Compilation](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.Compilation)***.

### ***Jak definovat operace***
Chceme-li být schopni přeložit matematický výraz na spustitelný kód, musíme nejprve vědět, co vůbec která v něm zapsaná operace znamená, a také jak rozlišit konstantu od identifikátoru a jak vypadá platný identifikátor. To všechno kompilátoru řekneme skrze instanci rozhranní ***[IYCNumberOperator](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Numerics/IYCNumberOperator.cs)***.  

Pracujeme-li s typem `double`, `decimal` nebo `long`, nemusíme se namáhat - pro ty už je defaultní implementace připravena - jako podtřída statické třídy [YCBasicNumberOperators](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Numerics/YCBasicNumberOperators.cs). Tyto výchozí implementace definují operátory intuitivním způsobem - operátor `+` odpovídá sčítání, `%` modulení, `**` mocnění apod., konstantou je vše, co projde metodou `TryParse` na odpovídajícím číselném typu při invariantní kultuře, validní identifikátor matchuje na regex `[[:alpha:]_][[:alnum:]_]*`, operátor pro typ `double` navíc zahrnuje ve standardní knihovně všechny funkce ze třídy `System.Math`.  

Chcete-li napsat vlastní číselný operátor, doporučuji se podívat pro inspiraci právě na tyto předpřipravené implementace. Celkově to ale je poměrně přímočarý proces.

#### ***Rozlišení konstant***
První metodou, jíž je třeba dodat, je `TryParse`. Jejím úkolem je z textového zápisu určit, zda reprezentuje konstantu, a její případnou hodnotu. Všechny literály jsou nejprve testovány na konstantu a teprve pokud neprojdou, stanou se kandidátem na identifikátor.

#### ***Validace identifikátorů***
Pokud literál není vyhodnocen jako konstanta, stane se kandidátem na identifikátor. Metoda `ValidateIdentifier` má pak za úkol rozhodnout, zda identifikátorem vskutku je, příp. lidsky čitelným způsobem popsat odchylky od identifikátorového formátu, jichž se dopouští. Třída [YCBasicNumberOperators](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Numerics/YCBasicNumberOperators.cs) poskytuje pár statických metod a polí, které by se při její implementaci mohly hodit.

#### ***Definice operátorů***
Nyní zbývá už jen doplnit metody odpovídající jednotlivým operátorům definovaným v gramatice, což by měl být naprosto přímočarý proces.

#### ***Standardní knihovna***
Volitelně ještě můžeme dodat množinu funkcí jakožto standardní knihovnu. Každá funkce v ní definovaná bude kompilátorem automaticky viditelná, aniž by se musela nacházet v kompilačním kontextu. Pokud se v kontextu nachází funkce se stejnou signaturou, zastíní funkci ve standardní knihovně.

#### ***Registrace NumberOperatoru***
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
Přesně takto získává defaultní operátor fasáda [YoowzxCalculator](#začínáme), pokud jí žádný nedodáme explicitně. Nyní ji tedy můžeme bez problémů používat pro náš nový typ. 

&nbsp;

### ***Kompilační kontext***
Z našeho výrazu je možné libovolně volat externí pojmenované funkce. Vzniká tedy problém, jak kompilátoru dodat jejich definice, aby mohl ona volání vytvořit.

#### ***Signatura funkce***
Nejprve si ale musíme rozmyslet, jak vůbec funkci jednoznačně identifikovat. YoowzxCalc pro větší uživatelské pohodlí podporuje přetěžování funkcí se stejným jménem, ale různými argumenty. K jednoznačné identifikaci tedy slouží struktura [YCFunctionSignature&lt;TNumber&gt;](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/YCFunctionSignature.cs) - nese jméno funkce, počet a typ (jako generický parametr) argumentů. Ve třídě [YCCompilerUtils](https://github.com/MarkusSecundus/YoowzxCalc/blob/master/MarkusSecundus.YoowzxCalc.Compilation/Compiler/Util/YCCompilerUtils.cs) najdete extension-metody, kterými lze signaturu jednoduše získat z instance `System.Delegate` nebo z uzlů AST.

#### ***Správa definic***
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
Přesně toto dělá kompilátor pokaždé, když narazí na funkci, jež není k nalezení v hešmapě `Functions` ani ve [standardní knihovně](#standardní-knihovna).  
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

### ***Kompilátor***
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
## ***Demo kalkulačka***
Balíček [MarkusSecundus.YoowzxCalc.Cmd](https://github.com/MarkusSecundus/YoowzxCalc/tree/master/MarkusSecundus.YoowzxCalc.Cmd) obsahuje spustitelnou kalkulačku pro příkazovou řádku operující v klasické REPL smyčce nad typem `double` a sloužící jako ukázka demonstrující některé možnosti této knihovny.  

Pro podrobnější instrukce k jejímu použití kalkulačku spusťte v interaktivním režimu a zavolejte příkaz `help`.