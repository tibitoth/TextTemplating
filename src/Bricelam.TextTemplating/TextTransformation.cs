using System.Text;

namespace Bricelam.TextTemplating
{
    public abstract class TextTransformation
    {
        public StringBuilder GenerationEnvironment { get; } = new StringBuilder();
        public void Write(string textToAppend) => GenerationEnvironment.Append(textToAppend);
        public abstract string TransformText();
    }
}
