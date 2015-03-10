using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Bricelam.TextTemplating.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bricelam.TextTemplating
{
    public class Engine
    {
        public string PreprocessTemplate(
            string content,
            ITextTemplatingEngineHost host,
            string className,
            string classNamespace,
            out string language,
            out string[] references)
        {
            var result = new Parser(host).Parse(content);
            language = result.Language;
            references = result.References.ToArray();

            return new PreprocessTextTransformation(className, classNamespace, result).TransformText();
        }

        public string ProcessTemplate(string content, ITextTemplatingEngineHost host)
        {
            var className = "TempTransformaion";
            var classNamespace = "Temp";
            var assemblyName = "Temp";

            string language;
            string[] references;
            var transformationText = PreprocessTemplate(
                content,
                host,
                className,
                classNamespace,
                out language,
                out references);

            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { SyntaxFactory.ParseSyntaxTree(transformationText) },
                references.Select(host.ResolveAssemblyReference),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var stream = new MemoryStream())
            {
                var result = compilation.Emit(stream);
                host.LogErrors(result);

                var transformationAssembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethods("Load")
                    .First(
                        m =>
                        {
                            var parameters = m.GetParameters();

                            return parameters.Length == 1 && parameters[0].ParameterType == typeof(byte[]);
                        })
                    .Invoke(null, new[] { stream.ToArray() });
                var transformationType = transformationAssembly.GetType(classNamespace + "." + className);
                var transformation = Activator.CreateInstance(transformationType);
                var transformMethod = transformationType.GetTypeInfo().GetDeclaredMethod("TransformText");

                return (string)transformMethod.Invoke(transformation, null);
            }
        }
    }
}
