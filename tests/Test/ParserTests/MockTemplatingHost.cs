using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis.Emit;
using TextTemplating.Infrastructure;

namespace Test.ParserTests
{
    public class MockTemplatingHost : ITextTemplatingEngineHost
    {
        public IList<string> StandardAssemblyReferences { get; } = new List<string>
        {
            "mscorlib",
            "TextTemplating"
        };
        public IList<string> StandardImports { get; } = new List<string>
        {
            "System",
            "TextTemplating",
            "TextTemplating.Infrastructure",
            "TextTemplating.T4.Parsing",
            "TextTemplating.T4.Preprocessing"
        };
        public void LogErrors(EmitResult result)
        {
            throw new NotImplementedException();
        }

        public string FileExtension { get; private set; } = ".cs";

        public Encoding Encoding { get; private set; }

        public void SetFileExtension(string extension)
        {
            FileExtension = extension;
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            Encoding = encoding;
        }

        public string TemplateFilePath { get; } = "./";
        public string LoadIncludeFile(string fileName)
        {
            if (fileName == "Test.tt")
            {
                return @"
<#@ import namespace=""System.Linq"" #>
<#@ assembly name=""MyLib.dll"" #>
";
            }
            throw new FileNotFoundException();
        }

        public string ResolvePath(string path)
        {
            // not now
            throw new NotImplementedException();
        }
    }
}
