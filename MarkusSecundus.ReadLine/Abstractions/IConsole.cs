namespace Internal.ReadLine.Abstractions
{
    /// <summary>
    /// Custom fork of <see href="https://github.com/tonerdo/readline"/>
    /// 
    /// All credits go to the original creator: Toni Solarin-Sodara 
    /// </summary>
    internal interface IConsole
    {
        int CursorLeft { get; }
        int CursorTop { get; }
        int BufferWidth { get; }
        int BufferHeight { get; }
        void SetCursorPosition(int left, int top);
        void SetBufferSize(int width, int height);
        void Write(string value);
        void WriteLine(string value);
    }
}