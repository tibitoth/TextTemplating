using System.Text;
using TextTemplating.Infrastructure;

namespace TextTemplating.T4.Preprocessing
{
    public abstract class TextTransformationBase
    {
        public StringBuilder GenerationEnvironment { get; } = new StringBuilder();
        public void Write(string textToAppend) => GenerationEnvironment.Append(textToAppend);
        public void WriteLine(string textToAppend) => GenerationEnvironment.AppendLine(textToAppend);

        public void PushIndent(string indent) { }

        public string PopIndent() => null;
        public ITextTemplatingEngineHost Host { get; set; }

        public abstract string TransformText();
    }
}
