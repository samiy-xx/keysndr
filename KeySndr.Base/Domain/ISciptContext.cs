namespace KeySndr.Base.Domain
{
    public interface IScriptContext
    {
        InputScript Script { get; }
        bool IsValid { get; }
        bool HasBeenParsed { get; }
        bool HasBeenExecuted { get; }
        void SetTestMode(bool b);
        void Execute();
        void Run();
    }
}
