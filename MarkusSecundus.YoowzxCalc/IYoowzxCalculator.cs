using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.Parser;
using MarkusSecundus.YoowzxCalc.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc
{
    //TODO: doplnit dockomenty
    /// <summary>
    /// Fasáda nad Yoowzxí pipeline pro zpracování matematických výrazů.
    /// <para/>
    /// Poskytuje pohodlnou a přímočarou cestu kompilace z textové reprezentace přímo do spustitelného kódu apod.
    /// </summary>
    /// <typeparam name="TNumber">Číselný typ nad kterým probíhají výpočty</typeparam>
    public interface IYoowzxCalculator<TNumber>
    {
        /// <summary>
        /// Objekt zodpovědný za převod výrazu z textové reprezentace na AST
        /// </summary>
        public IYCAstBuilder AstBuilder { get; init; }
        /// <summary>
        /// Objekt zodpovědný za překlad AST na spustitelný kód
        /// </summary>
        public IYCCompiler<TNumber> Compiler { get; init; }
        /// <summary>
        /// Objekt definující funkce, na něž se lze z překládaného výrazu odkazovat
        /// </summary>
        public IYCInterpretationContext<TNumber> Context { get; init; }



        /// <summary>
        /// Získá z kontextu již existující funkci s požadovanou signaturou.
        /// </summary>
        /// <param name="signature">Signatura hledané funkce</param>
        /// <returns>Funkce s požadovanou signaturou definovaná v kontextu kalkulátoru</returns>
        /// <exception cref="KeyNotFoundException">Pokud pro požadovanou signaturu v kontextu žádná funkce není definována</exception>
        public Delegate Get(YCFunctionSignature<TNumber> signature);
        /// <summary>
        /// Získá z kontextu již existující funkci s požadovanou signaturou. 
        /// Argumenty v signatuře jsou vykoukány z typu hledaného delegáta.
        /// </summary>
        /// <typeparam name="TDelegate">Datový typ hledané funkce. Musí být konkrétní - <see cref="Delegate"/> a <see cref="MulticastDelegate"/> nejsou povolené.</typeparam>
        /// <param name="signature">Jméno hledané funkce</param>
        /// <returns>Funkce s požadovanou signaturou definovaná v kontextu kalkulátoru</returns>
        /// <exception cref="KeyNotFoundException">Pokud pro požadovanou signaturu v kontextu žádná funkce není definována</exception>
        /// <exception cref="ArgumentException">Pokud <typeparamref name="TDelegate"/> je typu <see cref="Delegate"/> nebo <see cref="MulticastDelegate"/></exception>
        public TDelegate Get<TDelegate>(string signature) where TDelegate : Delegate;

        

        /// <summary>
        /// Zkompiluje výraz zapsaný v textovém řetězci na spustitelný kód.
        /// </summary>
        /// <typeparam name="TDelegate">Datový typ výsledného delegáta</typeparam>
        /// <param name="function">Výraz ke zkompilování</param>
        /// <returns>Zkompilovaný výraz jako delegát</returns>
        public TDelegate Compile<TDelegate>(string function) where TDelegate : Delegate;

        /// <summary>
        /// Zkompiluje všechny dané výrazy a postupně je přidá do kontextu kalkulátoru.
        /// </summary>
        /// <param name="toAdd">List textově reprezentovaných výrazů</param>
        /// <returns><c>this</c> pro účely řetězení</returns>
        public IYoowzxCalculator<TNumber> AddFunctions(IEnumerable<string> toAdd);

        /// <summary>
        /// Zkompiluje daný výraz a přidá ho do kontextu kalkulátoru.
        /// </summary>
        /// <param name="expression">Textová reprezentace výrazu</param>
        /// <param name="signature">Signatura popisující zkompilovaný výraz, pod kterou byl právě přidán do kontextu</param>
        /// <param name="result">Spustitelný produkt kompilace, jež byl právě přidán do kontextu</param>
        /// <returns><c>this</c> pro účely řetězení</returns>
        public IYoowzxCalculator<TNumber> AddFunction(string expression, out YCFunctionSignature<TNumber> signature, out Delegate result);


        /// <summary>
        /// Přidá delegáta do kalkulátorového kontextu.
        /// </summary>
        /// <typeparam name="TDelegate">Datový typ přidávaného delegáta. Hodí se specifikovat pro pohodlné přidávání inline lambd.</typeparam>
        /// <param name="name">Jméno, pod kterým má funkce být přidána.</param>
        /// <param name="toAdd">Delegát k přidání do kontextu</param>
        /// <param name="signature">Signatura pod níž byl delegát uložen do kontextu. Počet argumentů byl vykoukán z hlavičky funkce, na níž delegát reálně ukazuje</param>
        /// <returns><c>this</c> pro účely řetězení</returns>
        public IYoowzxCalculator<TNumber> AddFunction<TDelegate>(string name, TDelegate toAdd, out YCFunctionSignature<TNumber> signature) where TDelegate: Delegate;

        /// <summary>
        /// Přidá delegáta do kalkulátorového kontextu.
        /// <para/>
        /// Dodatečné přetížení pro větší pohodlí uživatele - alias pro <c>AddFunction(name, toAdd, out _)</c>
        /// </summary>
        /// <typeparam name="TDelegate">Datový typ přidávaného delegáta. Hodí se specifikovat pro pohodlné přidávání inline lambd.</typeparam>
        /// <param name="name">Jméno, pod kterým má funkce být přidána.</param>
        /// <param name="toAdd">Delegát k přidání do kontextu</param>
        /// <returns><c>this</c> pro účely řetězení</returns>
        public IYoowzxCalculator<TNumber> AddFunction<TDelegate>(string name, TDelegate toAdd) where TDelegate : Delegate
            => AddFunction(name, toAdd, out _);



        /// <summary>
        /// Vytvoří novou instanci kanonické implementace kalkulátoru.
        /// </summary>
        /// <param name="astBuilder">Instance AstBuilderu. Pokud je null, bude použita instance kanonické implementace.</param>
        /// <param name="compiler">Instance kompilátoru. Pokud je null, bude použita instance kanonické implementace s operátorem získaným skrze <c><see cref="YCBasicNumberOperators.Get{TNumber}()"/></c>.</param>
        /// <param name="context">Instance kontextu. Pokud je null, bude použita instance kanonické implementace.</param>
        /// <returns>Nová instance kanonické implementace kalkulátoru.</returns>
        public static IYoowzxCalculator<TNumber> Make(IYCAstBuilder astBuilder = null, IYCCompiler<TNumber> compiler = null, IYCInterpretationContext<TNumber> context = null)
            => new YoowzxCalculator<TNumber>() { AstBuilder = astBuilder, Compiler = compiler, Context = context };
    }




    /// <summary>
    /// Statická třída s rozšiřujícími metodami pro pohodlnější práci s <see cref="IYoowzxCalculator{TNumber}"/>
    /// </summary>
    public static class YoowzxCalculatorExtensions
    {
        /// <summary>
        /// Zkompiluje všechny dané výrazy a postupně je přidá do kontextu kalkulátoru.
        /// <para/>
        /// Dodatečné přetížení pro větší pohodlí uživatele.
        /// </summary>
        /// <param name="toAdd">List textově reprezentovaných výrazů</param>
        /// <returns><c>self</c> pro účely řetězení</returns>
        public static IYoowzxCalculator<TNumber> AddFunctions<TNumber>(this IYoowzxCalculator<TNumber> self, params string[] toAdd)
            => self.AddFunctions(toAdd);

        /// <summary>
        /// Přidá delegáta do kalkulátorového kontextu.
        /// <para/>
        /// Dodatečné přetížení pro větší pohodlí uživatele.
        /// </summary>
        /// <param name="name">Jméno, pod kterým má funkce být přidána.</param>
        /// <param name="toAdd">Delegát k přidání do kontextu</param>
        /// <returns><c>self</c> pro účely řetězení</returns>
        public static IYoowzxCalculator<TNumber> AddFunction<TNumber>(this IYoowzxCalculator<TNumber> self, string name, Delegate toAdd)
            => self.AddFunction(name, toAdd);

    }
}
