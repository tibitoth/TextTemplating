using System;
using Bricelam.TextTemplating.Parsing;

namespace Bricelam.TextTemplating
{
    internal partial class PreprocessTextTransformation : TextTransformation
    {
        public override string TransformText()
        {
            foreach (var import in _result.Imports)
            {
                Write("using ");
                Write((import).ToString());
                Write(";\r\n");
            }

            Write("\r\nnamespace ");
            Write((_classNamespace).ToString());
            Write("\r\n{\r\n    public partial class ");
            Write((_className).ToString());
            Write(" : TextTransformation\r\n    {\r\n        public override string TransformText()\r\n        {\r\n");

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

        public PreprocessTextTransformation(string className, string classNamespace, ParseResult result)
        {
            _className = className;
            _classNamespace = classNamespace;
            _result = result;
        }

        private string Render(Block block)
        {
            switch (block.BlockType)
            {
                case BlockType.TextBlock:
                    return "Write(\"" +
                        block.Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n") +
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
