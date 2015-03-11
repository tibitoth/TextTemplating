using System;
using Bricelam.TextTemplating;
using Bricelam.TextTemplating.CommandLine;
using Microsoft.Framework.Runtime;

namespace CustomHostSample
{
    class Program
    {
        ILibraryManager _libraryManager;
        
        public Program(ILibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
        }
        
        void Main()
        {
            var host = new CustomEngineHost();
            var engine = new Engine();
            var template = "The current date is: <#= DateTime.Today.ToString(\"ddd MM/dd/yyyy\") #>";
            var output = engine.ProcessTemplate(template, host);
            
            Console.WriteLine(output);
        }
    }
}
