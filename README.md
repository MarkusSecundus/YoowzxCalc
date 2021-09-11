# YoowzxCalc

### Knihovna pro zpracování matematických výrazů v prostředí .NET vyvíjená s důrazem na výkon a flexibilitu použití.   
\
Parsuje výrazy v infixní notaci s podporou uživatelsky definovaných pomocných funkcí a rekurzivních volání (viz níže gramatiku).   
Schopná z výrazů za běhu generovat CIL kód - běžící přímo na VM a tedy rychle - volatelný skrze delegáta.  
Rovněž zahrnuje programovatelný kalkulátor pro příkazovou řádku sloužící jako demo.

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
Yoowzx plně podporuje optimalizaci koncové rekurze. Zadefinujeme-li tedy např. takto výpočet faktorialu, pro libovolně vysoké hodnoty argumentu nehrozí přetečení volacího zásobníku:
```c#
calc.AddFunctions("fact(x, accumulator) := x <= 1? accumulator : fact(x-1, x*accumulator)",
                  "fact(x) := fact(x, 1)");
```

### ***Kešování výsledků***
Pomocí anotace "cached" lze kompilátoru nařídit, aby volání funkce pro danou hodnotu argumentů provedl jednou, výsledek uložil do keše, a příště ho z ní už jenom tahal.  
Tím pádem např. takto definovaná funkce pro výpočet fibonacciho posloupnosti poběží v lineárním čase (resp. konstantním pro opětovná volání):
```c#
calc.AddFunctions("[cached] fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)");
```
_Kešování je podporováno pro všechny funkce bez ohledu na to, jak moc argumentů berou._

&nbsp;
-----------------------------
## Gramatika