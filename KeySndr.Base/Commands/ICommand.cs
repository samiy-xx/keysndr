namespace KeySndr.Base.Commands
{
    public interface ICommand<T>
        where T : class
    {
        T Result { get; }
        void Execute();
    }
}
