using System;
using System.Linq;
using TextTemplating.Infrastructure;
using TextTemplating.T4.Parsing;

namespace TextTemplating.T4.Preprocessing
{
    internal partial class PreprocessTextTransformation : TextTransformationBase
    {
        public override string TransformText()
        {
            foreach (var import in _result.Imports.Distinct())
            {
                Write("using ");
                Write((import).ToString());
                Write(";\r\n");
            }

            Write("\r\nnamespace ");
            Write((_classNamespace).ToString());
            Write("\r\n{\r\n    ");
            Write((_result.Visibility).ToString());
            Write(" partial class ");
            Write((_className).ToString());
            Write(" : TextTransformationBase\r\n    {\r\n        public override string TransformText()\r\n        {\r\n");

            foreach (var block in _result.ContentBlocks)
            {
                Write("            ");
                Write((Render(block)).ToString());
                Write("\r\n");
            }

            Write("\r\n            return GenerationEnvironment.ToString();\r\n        }\r\n\r\n");

            foreach (var block in _result.FeatureBlocks)
            {
                Write("        ");
                Write((Render(block)).ToString());
                Write("\r\n");
            }

            Write("    }\r\n}\r\n");

            return GenerationEnvironment.ToString();
        }

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

        private string Render(Block block)
        {
            switch (block.BlockType)
            {
                case BlockType.TextBlock:
                    return "Write(\"" +
                        block.Content.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n") +
                        "\");";

                case BlockType.StandardControlBlock:
                    return block.Content;

                case BlockType.ExpressionControlBlock:
                    return "Write((" + block.Content + ").ToString());";

                default:
                    throw new InvalidOperationException("Unexpected block type.");
            }
        }
    }
}
