using System.Threading.Tasks;

namespace KeySndr.Base
{
    public interface IActionProcessor
    {
        Task Process();
    }
}
