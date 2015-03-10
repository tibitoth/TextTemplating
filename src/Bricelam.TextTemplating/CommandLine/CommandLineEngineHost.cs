using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Roslyn;

namespace Bricelam.TextTemplating.CommandLine
{
    public class CommandLineEngineHost : ITextTemplatingEngineHost
    {
        private readonly ILibraryManager _libraryManager;
        private string _fileExtension = ".cs";
        private Encoding _encoding;
        
        public CommandLineEngineHost(ILibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
        }

        public string FileExtension => _fileExtension;
        public Encoding Encoding => _encoding;

        public IList<string> StandardAssemblyReferences { get; } = new List<string>
        {
#if DNXCORE50
            "System.Runtime",
#else
            "mscorlib",
#endif
            "Bricelam.TextTemplating"
        };

        public IList<string> StandardImports { get; } = new List<string>
        {
            "System",
            "Bricelam.TextTemplating"
        };

        public void LogErrors(EmitResult result)
        {
            if (!result.Success)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Build failed. Diagnostics: {0}",
                        string.Join(Environment.NewLine, result.Diagnostics)));
            }
        }

        public MetadataReference ResolveAssemblyReference(string assemblyReference)
        {
            var metadataReference = _libraryManager.GetLibraryExport(assemblyReference).MetadataReferences.First();
            var roslynMetadataReference = metadataReference as IRoslynMetadataReference;
            if (roslynMetadataReference != null)
            {
                return roslynMetadataReference.MetadataReference;
            }
            var metadataFileReference  = metadataReference as IMetadataFileReference ;
            if (metadataFileReference != null)
            {
                return MetadataReference.CreateFromFile(metadataFileReference.Path);
            }
            
            throw new Exception("Unexpected metadata reference type.");
        }
        
        public void SetFileExtension(string extension) => _fileExtension = extension;        
        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective) => _encoding = encoding;
    }
}