using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.Emit;

namespace Bricelam.TextTemplating.TestUtilities
{
    public class TestEngineHost : ITextTemplatingEngineHost
    {
        public virtual string FileExtension { get; } = ".cs";
        public IList<string> StandardAssemblyReferences { get; } = new List<string>();
        public IList<string> StandardImports { get; } = new List<string>();

        public virtual void LogErrors(EmitResult result)
        {
        }

        public string ResolveAssemblyReference(string assemblyReference) => assemblyReference;

        public void SetFileExtension(string extension)
        {
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
        }
    }
}