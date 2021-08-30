namespace System
{
    /// <summary>
    /// Custom fork of <see href="https://github.com/tonerdo/readline"/>
    /// <para/>
    /// All credits go to the original creator: Toni Solarin-Sodara 
    /// </summary>
    public interface IAutoCompleteHandler
    {
        char[] Separators { get; set; }
        string[] GetSuggestions(string text, int index);
    }
}