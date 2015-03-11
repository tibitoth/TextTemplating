using Bricelam.TextTemplating.CommandLine;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Infrastructure;

namespace Bricelam.TextTemplating
{
    public class TextTemplatingService : ITextTemplating
    {
        public string ProcessTemplate(string content)
        {
            var engine = new Engine();
            var libraryManager = CallContextServiceLocator.Locator.ServiceProvider.GetService<ILibraryManager>();
            var host = new CommandLineEngineHost(libraryManager);

            return engine.ProcessTemplate(content, host);
        }
    }
}
