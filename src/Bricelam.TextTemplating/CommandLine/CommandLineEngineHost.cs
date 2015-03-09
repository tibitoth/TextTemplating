using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.Emit;

namespace Bricelam.TextTemplating.CommandLine
{
    public class CommandLineEngineHost : ITextTemplatingEngineHost
    {
        private string _fileExtension = ".cs";
        private Encoding _encoding;

        public string FileExtension => _fileExtension;

        public IList<string> StandardAssemblyReferences { get; } = new List<string>
        {
#if DNXCORE50
            "System.Runtime",
#else
            "System",
#endif
            "Bricelam.TextTemplating"
        };

        public IList<string> StandardImports { get; } = new List<string>
        {
            "System"
        };

        public void LogErrors(EmitResult result)
        {
        }

        public string ResolveAssemblyReference(string assemblyReference) => assemblyReference;
        public void SetFileExtension(string extension) => _fileExtension = extension;
        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective) => _encoding = encoding;
    }
}