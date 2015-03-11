using System;
using Bricelam.TextTemplating;
using Microsoft.Framework.DependencyInjection;

namespace ServiceSample
{
    class Program
    {
        static void Main()
        {
            var services = new ServiceCollection()
                .AddTextTemplating()
                .BuildServiceProvider();
            var textTemplating = services.GetService<ITextTemplating>();
            
            var template = "The current time is: <#= DateTime.Now.ToString(\"HH:mm:ss.ff\") #>";
            var output = textTemplating.ProcessTemplate(template);

            Console.WriteLine(output);
        }
    }
}
