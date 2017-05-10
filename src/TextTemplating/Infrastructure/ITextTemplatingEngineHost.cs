using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.Emit;

namespace TextTemplating.Infrastructure
{
    public interface ITextTemplatingEngineHost
    {
        /// <summary>
        /// 默认引用的程序集
        /// </summary>
        IList<string> StandardAssemblyReferences { get; }
        /// <summary>
        /// 默认引用的名称空间
        /// </summary>
        IList<string> StandardImports { get; }

        void LogErrors(EmitResult result);
        void SetFileExtension(string extension);
        void SetOutputEncoding(Encoding encoding, bool fromOutputDirective);
        string TemplateFilePath { get; }
        string LoadIncludeFile(string fileName);
        string ResolvePath(string path);
    }
}
