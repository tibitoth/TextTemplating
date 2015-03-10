using System;

namespace RuntimeTemplateSample
{
    class Program
    {
        static void Main()
        {
            var template = new RuntimeTextTemplate();
            var output = template.TransformText();
            
            Console.WriteLine(output);
        }
    }
}
