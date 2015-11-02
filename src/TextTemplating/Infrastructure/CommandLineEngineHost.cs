using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis.Emit;

namespace TextTemplating.Infrastructure
{
    public class CommandLineEngineHost : ITextTemplatingEngineHost
    {
        public CommandLineEngineHost(string templateFilePath)
        {
            TemplateFilePath = templateFilePath;
        }

        public string FileExtension { get; private set; } = ".cs";

        public Encoding Encoding { get; private set; }

        public string TemplateFilePath { get; }

        public void SetFileExtension(string extension) => FileExtension = extension;

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective) => Encoding = encoding;

        public IList<string> StandardAssemblyReferences { get; } = new List<string>
        {
            // TODO dnx
#if DNXCORE50
            "System.Runtime",
#else
            "mscorlib",
#endif
            "TextTemplating",
        };

        public IList<string> StandardImports { get; } = new List<string>
        {
            "System",
            "TextTemplating",
            "TextTemplating.Infrastructure",
            "TextTemplating.T4.Parsing",
            "TextTemplating.T4.Preprocessing",
            "TextTemplating.Razor",
        };

        public void LogErrors(EmitResult result)
        {
            if (!result.Success)
            {
                throw new InvalidOperationException(string.Format("Build failed. Diagnostics: {0}", string.Join(Environment.NewLine, result.Diagnostics)));
            }
        }

        public string LoadIncludeFile(string fileName)
        {
            if (Path.IsPathRooted(fileName))
            {
                return File.ReadAllText(fileName);
            }

            return File.ReadAllText(Path.Combine(Path.GetDirectoryName(TemplateFilePath), fileName));
        }

        public string ResolvePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            return Path.Combine(Path.GetDirectoryName(TemplateFilePath), path);
        }

        
    }
}