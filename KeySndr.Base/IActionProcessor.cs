using System.Threading.Tasks;
using KeySndr.Common;

namespace KeySndr.Base
{
    public interface IActionProcessor
    {
        InputActionExecutionContainer Container { get; set; }
        Task Process();
    }
}
