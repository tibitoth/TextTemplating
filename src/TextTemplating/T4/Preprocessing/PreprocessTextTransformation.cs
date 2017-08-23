using System;
using System.Linq;
using Microsoft.DotNet.InternalAbstractions;
using TextTemplating.Infrastructure;
using TextTemplating.T4.Parsing;

namespace TextTemplating.T4.Preprocessing
{
    /// <summary>
    /// Transform .tt to .cs
    /// </summary>
    internal class PreprocessTextTransformation : TextTransformationBase
    {
        private readonly string _className;
        private readonly string _classNamespace;
        private readonly ParseResult _result;

        public PreprocessTextTransformation(string className, string classNamespace, ParseResult result, ITextTemplatingEngineHost host)
        {
            _className = className;
            _classNamespace = classNamespace;
            _result = result;
            Host = host;
        }

        public override string TransformText()
        {
            foreach (var import in _result.Imports.Distinct())
            {
                WriteLine($"using {import};");
            }

            Write($"{Environment.NewLine}namespace {_classNamespace}");
            Write($"{Environment.NewLine}{{{Environment.NewLine}");
            PushIndent("    ");
            Write($"{_result.Visibility} partial class {_className} : TextTransformationBase{Environment.NewLine}");
            Write($"{Environment.NewLine}{{{Environment.NewLine}");
            PushIndent("    ");
            Write("public override string TransformText()");
            Write($"{Environment.NewLine}{{{Environment.NewLine}");
            PushIndent("    ");

            foreach (var block in _result.ContentBlocks)
            {
                WriteLine(Render(block));
            }

            Write("return GenerationEnvironment.ToString();");
            PopIndent();
            Write($"{Environment.NewLine}}}{Environment.NewLine}");

            // todo add feature block supports
            //foreach (var block in _result.FeatureBlocks)
            //{
            //    WriteLine(Render(block));
            //}

            PopIndent();
            Write($"{Environment.NewLine}}}{Environment.NewLine}");

            return GenerationEnvironment.ToString();
        }
        
        private string Render(Block block)
        {
            switch (block.BlockType)
            {
                case BlockType.TextBlock:
                    return $"Write(\"{block.Content.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n","\\n")}\");";

                case BlockType.StandardControlBlock:
                    return block.Content;

                case BlockType.ExpressionControlBlock:
                    return $"Write(({block.Content}).ToString());";

                default:
                    throw new InvalidOperationException("Unexpected block type.");
            }
        }
    }
}
