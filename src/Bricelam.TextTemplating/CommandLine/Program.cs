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

        public Program(IApplicationEnvironment appEnv)
        {
            _appEnv = appEnv;
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
                var host = new TextTemplatingEngineHost();
                var fileName = Path.GetFileNameWithoutExtension(template);
                var content = File.ReadAllText(template);

                string transformedText;
                if (preprocess)
                {
                    var classNamespace = _appEnv.ApplicationName + '.' + Path.GetDirectoryName(template)
                        .Substring(_appEnv.ApplicationBasePath.Length + 1).Replace(Path.DirectorySeparatorChar, '.');

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
                File.WriteAllText(output, transformedText);
            }

            return 0;
        }
    }
}