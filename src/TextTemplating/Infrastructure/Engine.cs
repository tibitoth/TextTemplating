using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using TextTemplating.Razor;
using TextTemplating.T4.Parsing;
using TextTemplating.T4.Preprocessing;
using Microsoft.AspNetCore.Razor.CodeGenerators;

namespace TextTemplating.Infrastructure
{
    public class Engine
    {
        private readonly ILibraryExporter _libraryExporter;
        private readonly ITextTemplatingEngineHost _host;

        public Engine(ILibraryExporter libraryExporter, ITextTemplatingEngineHost host)
        {
            _libraryExporter = libraryExporter;
            _host = host;
        }

        public PreprocessResult PreprocessT4Template(string content, string className, string classNamespace)
        {
            var result = new Parser(_host).Parse(content);
            var transformation = new PreprocessTextTransformation(className, classNamespace, result, _host);
            var preprocessedContent = transformation.TransformText();

            var preprocessed = new PreprocessResult()
            {
                References = result.References.Distinct().ToArray(),
                PreprocessedContent = preprocessedContent,
            };

            return preprocessed;
        }

        public string ProcessT4Template(string content)
        {
            var className = "GeneratedClass";
            var classNamespace = "Generated";
            var assemblyName = "Generated";

            var preResult = PreprocessT4Template(content, className, classNamespace);

            var compiler = new RoslynCompilationService(_libraryExporter, _host);
            var transformationAssembly = compiler.Compile(assemblyName, preResult);

            var transformationType = transformationAssembly.GetType(classNamespace + "." + className);
            var transformation = (TextTransformationBase)Activator.CreateInstance(transformationType);

            transformation.Host = _host;
            return transformation.TransformText();
        }

        public PreprocessResult PreprocessRazorTemplate(string content, string className, string classNamespace)
        {
            var language = new CSharpRazorCodeLanguage();
            var razorHost = new RazorEngineHost(language)
            {
                DefaultBaseClass = typeof(TemplateBase).FullName,
                DefaultNamespace = classNamespace,
                DefaultClassName = className,
                GeneratedClassContext = new GeneratedClassContext(
                    nameof(TemplateBase.ExecuteAsync),
                    nameof(TemplateBase.Write),
                    nameof(TemplateBase.WriteLiteral),
                    new GeneratedTagHelperContext())
            };
            razorHost.NamespaceImports.Add(classNamespace);

            var razorEngine = new RazorTemplateEngine(razorHost);
            var generatorResults = razorEngine.GenerateCode(new StringReader(content));
            if (!generatorResults.Success)
            {
                throw new Exception(string.Join(Environment.NewLine, generatorResults.ParserErrors.Select(x => x.Message)));
            }

            var preprocessResult = new PreprocessResult()
            {
                PreprocessedContent = generatorResults.GeneratedCode,
                References = _host.StandardImports.ToList(),
            };

            return preprocessResult;
        }

        public async Task<string> ProcessRazorTemplate(string content)
        {
            var className = "GeneratedClass";
            var classNamespace = "Generated";
            var assemblyName = "Generated";

            var preprocessResult = PreprocessRazorTemplate(content, className, classNamespace);

            var compiler = new RoslynCompilationService(_libraryExporter, _host);
            var assembly = compiler.Compile(assemblyName, preprocessResult);

            var instanceType = assembly.GetType(classNamespace + "." + className);
            var instance = (TemplateBase)Activator.CreateInstance(instanceType);
            return await instance.GenerateCodeAsync();
        }
    }
}
