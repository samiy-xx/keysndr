using System.Threading.Tasks;

namespace KeySndr.Base.Commands
{
    public interface ICommand<T>
        where T : class
    {
        T Result { get; }
        void Execute();
    }

    public interface IAsyncCommand<T>
        where T : class
    {
        T Result { get; }
        Task Execute();
    }
}
