using System;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using TextTemplating.Infrastructure;

namespace TextTemplating.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            const string helpTemplate = "-h|--help";
            var app = new CommandLineApplication
            {
                Description = "A simple Text Template Transformer for .Net Core"
            };
            var process = app.Command("proc", command =>
            {
                command.Description = "Process template to CSharp class file for runtime transform";
                var fileOption = command.Option("-f|--file", "The texttemplate to be processed", CommandOptionType.SingleValue);
                var outputOption = command.Option("-o|--output", "Output directory path, default: out.cs", CommandOptionType.SingleValue);
                var classNameOption = command.Option("-c|--class", "Generated class name, default: GeneratedClass", CommandOptionType.SingleValue);
                var namespaceNameOption = command.Option("-ns|--namespace", "Generated namespace name, default: GeneratedNamespace",
                    CommandOptionType.SingleValue);
                command.HelpOption(helpTemplate);
                command.OnExecute(() =>
                {
                    var fileName = Path.Combine(Environment.CurrentDirectory, fileOption.Value());
                    var outputName = Path.Combine(Environment.CurrentDirectory, outputOption.Value() ?? "out.cs");
                    string className = classNameOption.Value() ?? "GeneratedClass";
                    string namespaceName = namespaceNameOption.Value() ?? "GeneratedNamespace";
                    try
                    {
                        return PreprocessTemplate(fileName, outputName, className, namespaceName);
                    }
                    catch (IOException e)
                    {
                        switch (e)
                        {
                            case FileNotFoundException fe:
                            case DirectoryNotFoundException de:
                                Console.WriteLine(e.Message);
                                break;
                        }
                        return 1;
                    }
                });
            });
            app.HelpOption(helpTemplate);
            try
            {
                return app.Execute(args);
            }
            catch (CommandParsingException e)
            {
                app.ShowHelp(e.Command.Name);
                return 1;
            }
        }

        /// <summary>
        /// 转换模板
        /// </summary>
        /// <param name="file">模板文件绝对路径</param>
        /// <param name="outPut">输出路径</param>
        /// <param name="className">生成的类名</param>
        /// <param name="namespaceName">生成的名称空间名</param>
        /// <returns></returns>
        static int PreprocessTemplate(string file, string outPut, string className, string namespaceName)
        {
            var templatesRoot = Path.GetDirectoryName(file);
            var host = new CommandLineEngineHost(templatesRoot);
            var engin = new Engine(host);
            var templateContent = File.ReadAllText(file);
            var result = engin.PreprocessT4Template(templateContent, className, namespaceName);
            File.WriteAllText(outPut, result.PreprocessedContent);
            return 0;
        }
    }
}

