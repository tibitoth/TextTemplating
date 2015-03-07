using Microsoft.CodeAnalysis.Emit;

namespace Bricelam.TextTemplating
{
    public interface ITextTemplatingEngineHost
    {
        void LogErrors(EmitResult result);
    }
}
