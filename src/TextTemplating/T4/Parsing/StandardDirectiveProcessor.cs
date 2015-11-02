using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextTemplating.Infrastructure;

namespace TextTemplating.T4.Parsing
{
    internal class StandardDirectiveProcessorBase : DirectiveProcessorBase
    {
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

        public override bool IsDirectiveSupported(string directiveName) =>
            directiveName == "template"
                || directiveName == "output"
                || directiveName == "assembly"
                || directiveName == "import"
                || directiveName == "include";

        public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
        {
            switch (directiveName)
            {
                case "template":
                    string visibility;
                    if (arguments.TryGetValue("visibility", out visibility))
                    {
                        _result.Visibility = visibility;
                    }
                    break;

                case "output":
                    string extension;
                    if (arguments.TryGetValue("extension", out extension))
                    {
                        _host.SetFileExtension(extension);
                    }
                    string encoding;
                    if (arguments.TryGetValue("encoding", out encoding))
                    {
                        int codepage;
                        _host.SetOutputEncoding(
                            /*int.TryParse(encoding, out codepage)
                                ? Encoding.GetEncoding(codepage)
                                : */Encoding.GetEncoding(encoding),
                            fromOutputDirective: true);
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
                    string file;
                    if (arguments.TryGetValue("file", out file))
                    {
                        string onceArgument;
                        arguments.TryGetValue("once", out onceArgument);
                        bool once;
                        bool.TryParse(onceArgument, out once);
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