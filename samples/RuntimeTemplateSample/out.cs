using System;
using TextTemplating;
using TextTemplating.Infrastructure;
using TextTemplating.T4.Parsing;
using TextTemplating.T4.Preprocessing;
using System.Globalization;

namespace RuntimeTemplateSample
{
    public partial class GeneratedClass : TextTransformationBase
    {
        public override string TransformText()
        {
            Write("\r\n");

            return GenerationEnvironment.ToString();
        }

        private string FixAttributeName(string name)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
        }

        Write("\r\n");
    }
}
