using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;

namespace TextTemplating.Infrastructure
{
    public class RoslynCompilationService
    {
        private readonly ConcurrentDictionary<string, AssemblyMetadata> _metadataFileCache = new ConcurrentDictionary<string, AssemblyMetadata>(StringComparer.OrdinalIgnoreCase);
        
        private readonly ITextTemplatingEngineHost _host;

        public RoslynCompilationService(ITextTemplatingEngineHost host)
        {
            //_libraryExporter = libraryExporter;
            _host = host;
        }

        public Assembly Compile(string assemblyName, PreprocessResult preprocessResult)
        {
            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { SyntaxFactory.ParseSyntaxTree(preprocessResult.PreprocessedContent) },
                preprocessResult.References.SelectMany(ResolveAssemblyReference),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var stream = new MemoryStream())
            {
                var result = compilation.Emit(stream);
                _host.LogErrors(result);

                var transformationAssembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethods("Load").First(m =>
                {
                    var parameters = m.GetParameters();
                    return parameters.Length == 1 && parameters[0].ParameterType == typeof(byte[]);
                }).Invoke(null, new[] { stream.ToArray() });

                return transformationAssembly;
            }
        }

        /// <summary>
        /// Resolve assembly reference by GAC name or .dll file's absolute path
        /// </summary> 
        /// <param name="assemblyReference"></param>
        /// <returns></returns>
        public IList<MetadataReference> ResolveAssemblyReference(string assemblyReference)
        {
            
            var references = new List<MetadataReference>();
            //var libraryExport = _libraryExporter.GetExport(assemblyReference);
            // 首先尝试从
            MetadataReference.CreateFromFile(assemblyReference);
            //if (libraryExport?.MetadataReferences != null && libraryExport.MetadataReferences.Count > 0)
            //{
            //    Debug.Assert(libraryExport.MetadataReferences.Count == 1, "Expected 1 MetadataReferences, found " + libraryExport.MetadataReferences.Count);

            //    var roslynReference = libraryExport.MetadataReferences[0] as IRoslynMetadataReference;
            //    var compilationReference = roslynReference?.MetadataReference as CompilationReference;
            //    if (compilationReference != null)
            //    {
            //        references.AddRange(compilationReference.Compilation.References);
            //        references.Add(roslynReference.MetadataReference);
            //        return references;
            //    }
            //}

            //var export = _libraryExporter.GetAllExports(assemblyReference);
            //if (export != null)
            //{
            //    references.AddRange(export.MetadataReferences.Select(ConvertMetadataReference));
            //}

            return references;
        }

        //private MetadataReference ConvertMetadataReference(IMetadataReference metadataReference)
        //{
        //    var roslynReference = metadataReference as IRoslynMetadataReference;
        //    if (roslynReference != null)
        //    {
        //        return roslynReference.MetadataReference;
        //    }

        //    var embeddedReference = metadataReference as IMetadataEmbeddedReference;
        //    if (embeddedReference != null)
        //    {
        //        return MetadataReference.CreateFromImage(embeddedReference.Contents);
        //    }

        //    var fileMetadataReference = metadataReference as IMetadataFileReference;
        //    if (fileMetadataReference != null)
        //    {
        //        return CreateMetadataFileReference(fileMetadataReference.Path);
        //    }

        //    var projectReference = metadataReference as IMetadataProjectReference;
        //    if (projectReference != null)
        //    {
        //        using (var ms = new MemoryStream())
        //        {
        //            projectReference.EmitReferenceAssembly(ms);

        //            return MetadataReference.CreateFromImage(ms.ToArray());
        //        }
        //    }

        //    throw new NotSupportedException();
        //}

        private MetadataReference CreateMetadataFileReference(string path)
        {
            var metadata = _metadataFileCache.GetOrAdd(path, _ =>
            {
                using (var stream = File.OpenRead(path))
                {
                    var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
                    return AssemblyMetadata.Create(moduleMetadata);
                }
            });

            return metadata.GetReference(filePath: path);
        }
    }
}