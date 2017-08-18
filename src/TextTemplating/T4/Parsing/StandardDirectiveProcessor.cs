using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextTemplating.Infrastructure;

namespace TextTemplating.T4.Parsing
{
    internal class StandardDirectiveProcessorBase : DirectiveProcessorBase
    {
        private static readonly string[] SupportedDirectives =
        {
            "template","output","assembly","import","include"
        };

        private readonly ParseResult _result;
        private readonly ICollection<string> _references = new List<string>();
        private readonly ICollection<string> _imports = new List<string>();
        private readonly ICollection<IncludeFile> _includeFiles = new List<IncludeFile>();

        private ITextTemplatingEngineHost _host;

        public StandardDirectiveProcessorBase(ParseResult result)
        {
            _result = result;
        }

        public override void Initialize(ITextTemplatingEngineHost host)
        {
            _host = host;
        }

        public override bool IsDirectiveSupported(string directiveName) => SupportedDirectives.Contains(directiveName);

        public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
        {
            switch (directiveName)
            {
                case "template":
                    if (arguments.TryGetValue("visibility", out string visibility))
                    {
                        _result.Visibility = visibility;
                    }
                    break;

                case "output":
                    if (arguments.TryGetValue("extension", out string extension))
                    {
                        _host.SetFileExtension(extension);
                    }
                    string encoding;
                    if (arguments.TryGetValue("encoding", out encoding))
                    {
                        _host.SetOutputEncoding(Encoding.GetEncoding(encoding), fromOutputDirective: true);
                    }
                    break;

                case "assembly":
                    string name;
                    if (arguments.TryGetValue("name", out name))
                    {
                        _references.Add(name);
                    }
                    break;

                case "import":
                    string @namespace;
                    if (arguments.TryGetValue("namespace", out @namespace))
                    {
                        _imports.Add(@namespace);
                    }
                    break;

                case "include":
                    
                    if (arguments.TryGetValue("file", out string file))
                    {
                        
                        arguments.TryGetValue("once", out string onceArgument);
                       
                        bool.TryParse(onceArgument, out bool once);
                        _includeFiles.Add(new IncludeFile(file, once));
                    }
                    break;
            }
        }

        public override string[] GetReferencesForProcessingRun() => _references.ToArray();
        public override string[] GetImportsForProcessingRun() => _imports.ToArray();
        public override IncludeFile[] GetIncludeFilesForProcessingRun() => _includeFiles.ToArray();
    }
}