using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Framework.FileSystemGlobbing;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Common.CommandLine;

namespace Bricelam.TextTemplating.CommandLine
{
    internal class Program
    {
        private readonly IApplicationEnvironment _appEnv;
        private readonly ILibraryManager _libraryManager;

        public Program(IApplicationEnvironment appEnv, ILibraryManager libraryManager)
        {
            _appEnv = appEnv;
            _libraryManager = libraryManager;
        }

        public int Main(string[] args)
        {
            var app = new CommandLineApplication();
            var preprocess = app.Option(
                "-p|--preprocess",
                "Create a run-time text template",
                CommandOptionType.NoValue);
            app.OnExecute(() => Execute(app.RemainingArguments, preprocess.HasValue()));

            return app.Execute(args);
        }

        private int Execute(IReadOnlyList<string> include, bool preprocess)
        {
            var matcher = new Matcher();
            if (include.Count == 0)
            {
                matcher.AddInclude(@"**\*.tt");
            }
            else
            {
                matcher.AddIncludePatterns(include);
            }

            var engine = new Engine();
            var templates = matcher.GetResultsInFullPath(_appEnv.ApplicationBasePath);
            foreach (var template in templates)
            {
                Console.WriteLine("Processing '{0}'...", template);
                var host = new CommandLineEngineHost(_libraryManager);
                var fileName = Path.GetFileNameWithoutExtension(template);
                var content = File.ReadAllText(template);

                string transformedText;
                if (preprocess)
                {
                    var relativeDir = Path.GetDirectoryName(template).Substring(_appEnv.ApplicationBasePath.Length);
                    var classNamespace = _appEnv.ApplicationName;
                    if (relativeDir.Length != 0)
                    {
                        classNamespace += '.' + relativeDir.Replace(Path.DirectorySeparatorChar, '.');
                    }

                    string language;
                    string[] references;
                    transformedText = engine.PreprocessTemplate(
                        content,
                        host,
                        fileName,
                        classNamespace,
                        out language,
                        out references);
                }
                else
                {
                    transformedText = engine.ProcessTemplate(content, host);
                }

                var output = Path.ChangeExtension(template, host.FileExtension);
                Console.WriteLine("Writing '{0}'...", output);
                
                if (host.Encoding != null)
                {
                    File.WriteAllText(output, transformedText, host.Encoding);
                }
                else
                {
                    File.WriteAllText(output, transformedText);
                }
            }

            return 0;
        }
    }
}