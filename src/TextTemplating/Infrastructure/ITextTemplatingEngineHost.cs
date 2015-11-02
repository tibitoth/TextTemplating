using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.Emit;

namespace TextTemplating.Infrastructure
{
    public interface ITextTemplatingEngineHost
    {
        IList<string> StandardAssemblyReferences { get; }
        IList<string> StandardImports { get; }

        void LogErrors(EmitResult result);
        void SetFileExtension(string extension);
        void SetOutputEncoding(Encoding encoding, bool fromOutputDirective);
        string TemplateFilePath { get; }
        string LoadIncludeFile(string fileName);
        string ResolvePath(string path);
    }
}
