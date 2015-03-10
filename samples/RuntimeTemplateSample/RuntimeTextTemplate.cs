using System;
using Bricelam.TextTemplating;

namespace RuntimeTemplateSample
{
    public partial class RuntimeTextTemplate : TextTransformation
    {
        public override string TransformText()
        {
            Write("Hello, World!");

            return GenerationEnvironment.ToString();
        }

    }
}
