using System;
using TextTemplating;
using TextTemplating.Infrastructure;
using TextTemplating.T4.Parsing;
using TextTemplating.T4.Preprocessing;
using TextTemplating.Razor;

namespace RuntimeTemplateSample
{
    public partial class RuntimeTextTemplate : TextTransformationBase
    {
        public override string TransformText()
        {
            Write("Hello, World!");

            return GenerationEnvironment.ToString();
        }

    }
}
