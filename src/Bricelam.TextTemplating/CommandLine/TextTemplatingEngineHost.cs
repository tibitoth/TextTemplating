using Microsoft.CodeAnalysis.Emit;

namespace Bricelam.TextTemplating.CommandLine
{
    public class TextTemplatingEngineHost : ITextTemplatingEngineHost
    {
        public virtual string FileExtension { get; } = ".cs";

        public virtual void LogErrors(EmitResult result)
        {
        }
    }
}